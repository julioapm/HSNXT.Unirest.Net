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
    public class HttpResponse<T> : BaseHttpResponse
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
}
