using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HSNXT.Unirest.Net.Entities.Exceptions;
using Newtonsoft.Json;

namespace HSNXT.Unirest.Net.Entities
{
    /// <inheritdoc cref="BaseHttpResponse" />
    /// <summary>
    /// Represents an HTTP response with a body.
    /// </summary>
    /// <typeparam name="T">The type of the response body.</typeparam>
    /// <remarks>An <see cref="HttpResponse{T}"/> object only needs to be disposed if <typeparamref name="T"/> is
    /// <see cref="Stream"/> and calls to the respective method aren't using callbacks; in other situations, the
    /// response is either buffered ahead of time and disposed automatically, or disposed after the callback finishes
    /// executing.</remarks>
    public class HttpResponse<T> : BaseHttpResponse, IDisposable
    {
        // response to get rid of (null unless T is Stream)
        private IDisposable _dispose;

        /// <summary>
        /// The response body's converted type.
        /// </summary>
        public T Body { get; private set; }
        
        // clone an HttpResponse, used for HttpRequest.AsAsync (not the overload with onSuccess/onFail)
        internal HttpResponse(HttpResponse<Stream> existing, Func<Stream,T> genFunc)
            : base(existing.Headers, existing.CodeType, existing.IsSuccess)
        {
            Body = genFunc(existing.Body);
        }
        
        // build an HttpResponse from scratch
        private HttpResponse(HttpResponseMessage response)
            : base(new Dictionary<string, string>(), response.StatusCode, response.IsSuccessStatusCode)
        {
            foreach (var header in response.Headers)
            {
                Headers.Add(header.Key, header.Value.First());
            }
        }

        // the value of the Task should always be dropped when onSuccess/onFail are provided!
        internal static async Task<HttpResponse<T>> GetAsync(HttpResponseMessage response,
            bool ensureSuccess, OnSuccessAsync<T> onSuccess, OnFailAsync<T> onFail)
        {
            var genericType = typeof(T);
            HttpResponse<T> res = null;
            try
            {
                // we need to use await to capture the variable and support finally
                res = await InternalGetAsync(response, ensureSuccess, onSuccess, onFail, genericType);
            }
            finally
            {
                // if res == null then either the call threw (and there was no result) or it called onSuccess/onFail
                // (which return null) so we dispose of the response.
                // if res is a type that's not stream, it's already buffered, so we dispose of the response.
                // if res is a stream and it isn't using onSuccess/onFail then we want to give the return value access
                // to the stream, so we don't want to close the response.
                if (genericType == typeof(Stream) && res != null)
                {
                    res._dispose = response;
                }
                // if ensureSuccess is true, we want the partial response content to be available to the exception
                // handler, so don't dispose it in that case
                else if (!ensureSuccess)
                {
                    response.Dispose();
                }
            }

            return res;
        }
        
        private static async Task<HttpResponse<T>> InternalGetAsync(HttpResponseMessage response,
            bool ensureSuccess, OnSuccessAsync<T> onSuccess, OnFailAsync<T> onFail, Type genericType)
        {
            var res = new HttpResponse<T>(response);
            
            var content = response.Content;
            if (content == null) throw new UnirestRequestException("HttpResponseMessage.Content was null!");

            if (!res.IsSuccess)
            {
                if (onFail != null)
                {
                    await onFail(new PartialHttpResponse<T>(res, content));

                    // we return here to avoid misleading exceptions after executing code when onFail is present; since
                    // onFinish must also be present we shouldn't "bubble up" the result to the outside, as that voids
                    // the entire point of using onFail, as it could throw an exception when parsing malformed JSON.
                    // also, this will make GetAsync dispose the response, which is what we want, since we're finished
                    // with it.
                    return null;
                }

                if (ensureSuccess)
                {
                    throw new UnirestResponseException<T>(new PartialHttpResponse<T>(res, content));
                }
            }

            if (genericType == typeof(byte[]))
                res.Body = (T) (object) await content.ReadAsByteArrayAsync();
            else if (genericType == typeof(string))
                res.Body = (T) (object) await content.ReadAsStringAsync();
            else if (genericType == typeof(Stream))
                res.Body = (T) (object) await content.ReadAsStreamAsync();
            else
                res.Body = JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());

            // ReSharper disable once InvertIf
            if (res.IsSuccess && onSuccess != null)
            {
                await onSuccess(res.Body);
                
                // return null here since there's no point keeping the response, so we want GetAsync to dispose it
                return null;
            }

            return res;
        }

        public void Dispose()
        {
            if (Body is IDisposable disposable) disposable.Dispose();
            _dispose?.Dispose();
        }
    }
}
