using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HSNXT.Unirest.Net.Request;
using HSNXT.Unirest.Net.Unirest;
using Newtonsoft.Json;

namespace HSNXT.Unirest.Net.Http
{
    public class HttpResponseBase
    {
        /// <summary>
        /// The HTTP status code of the response, as an integer.
        /// </summary>
        public int Code => (int) CodeType;

        /// <summary>
        /// The HTTP status code of the response, as an <see cref="HttpStatusCode"/> object.
        /// </summary>
        public HttpStatusCode CodeType { get; }

        /// <summary>
        /// Whether or not the request succeeded.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// A map of the response headers from key to value.
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        protected HttpResponseBase(Dictionary<string, string> headers, HttpStatusCode codeType, bool isSuccess)
        {
            Headers = headers;
            CodeType = codeType;
            IsSuccess = isSuccess;
        }
    }

    public class HttpResponse<T> : HttpResponseBase
    {
        /// <summary>
        /// The response body's converted type.
        /// </summary>
        public T Body { get; private set; }
        
        /// <summary>
        /// The response's raw byte stream. This is a cached clone of <see cref="HttpResponse{T}.Body"/> and can be
        /// freely read, except when using <see cref="HttpRequest.AsBinaryAsync()"/>.
        /// </summary>
        public Stream Raw { get; private set; }

        // clone an HttpResponse, used for HttpRequest.AsAsync (not the overload with onSuccess/onFail)
        internal HttpResponse(HttpResponse<Stream> existing, Func<Stream,T> genFunc)
            : base(existing.Headers, existing.CodeType, existing.IsSuccess)
        {
            Raw = existing.Raw;
            
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
                    response.Dispose();
                    return default;
                }

                if (ensureSuccess)
                {
                    throw new UnirestResponseException<T>(new PartialHttpResponse<T>(res, content));
                }
            }

            res.Raw = await content.ReadAsStreamAsync();

            var genericType = typeof(T);
            if (genericType == typeof(byte[]))
                res.Body = (T) (object) await content.ReadAsByteArrayAsync();
            else if (genericType == typeof(string))
                res.Body = (T) (object) await content.ReadAsStringAsync();
            else if (genericType == typeof(Stream))
                res.Body = (T) (object) res.Raw;
            else
                res.Body = JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());

            // ReSharper disable once InvertIf
            if (res.IsSuccess && onSuccess != null)
            {
                await onSuccess(res.Body);
                response.Dispose();
                return default;
            }
            
            // TODO when to dispose when doing it this way?
            return res;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents an exception thrown when <see cref="HttpResponseMessage.Content"/> is null during transport.
    /// </summary>
    public class UnirestRequestException : Exception
    {
        internal UnirestRequestException(string message) : base(message)
        {
        }
    }

    /// <inheritdoc cref="System.Exception" />
    /// <summary>
    /// Represents an exception thrown when a request completes with a non-success status code, as per
    /// <see cref="HttpResponseBase"/>.<see cref="HttpResponseBase.IsSuccess"/>.
    ///
    /// <p>An <see cref="UnirestResponseException{T}"/> object should be disposed after being caught.</p>
    /// </summary>
    /// <typeparam name="T">The original target response type, which a failed response needn't conform to</typeparam>
    public class UnirestResponseException<T> : Exception, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public PartialHttpResponse<T> PartialResponse { get; }
        
        internal UnirestResponseException(PartialHttpResponse<T> partialResponse)
        {
            PartialResponse = partialResponse;
        }

        public void Dispose() => PartialResponse?.Dispose();
    }

    /// <inheritdoc cref="System.IDisposable" />
    /// <summary>
    /// Represents an unsuccessful HTTP response. The response body can be read in a format different to the intended
    /// one.
    ///
    /// <p>A <see cref="PartialHttpResponse{T}"/> object only needs to be disposed when it's contained in an
    /// <see cref="UnirestResponseException{T}"/>.</p>
    /// </summary>
    /// <typeparam name="T">The original target response type, which a failed response needn't conform to</typeparam>
    public class PartialHttpResponse<T> : HttpResponseBase, IDisposable
    {
        public HttpContent Content { get; }

        internal PartialHttpResponse(HttpResponseBase existing, HttpContent content)
            : base(existing.Headers, existing.CodeType, false)
        {
            Content = content;
        }

        public async Task<T> AsJsonAsync()
        {
            return JsonConvert.DeserializeObject<T>(await Content.ReadAsStringAsync());
        }

        public async Task<TTarget> AsJsonAsync<TTarget>()
        {
            return JsonConvert.DeserializeObject<TTarget>(await Content.ReadAsStringAsync());
        }

        public Task<string> AsStringAsync() => Content.ReadAsStringAsync();
        
        public Task<byte[]> AsByteArrayAsync() => Content.ReadAsByteArrayAsync();
        
        public Task<Stream> AsStreamAsync() => Content.ReadAsStreamAsync();

        public void Dispose() => Content?.Dispose();
    }
}
