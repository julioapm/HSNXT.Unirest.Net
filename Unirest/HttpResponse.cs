using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HSNXT.Unirest.Net.Http
{
    public class HttpResponse<T>
    {
        public int Code { get; }

        public Dictionary<string, string> Headers { get; }

        public T Body { get; private set; }

        public Stream Raw { get; private set; }

        internal HttpResponse(HttpResponse<Stream> existing, Func<Stream,T> genFunc)
        {
            Headers = existing.Headers;
            Code = existing.Code;
            Raw = existing.Raw;
            Body = genFunc(existing.Body);
        }
        
        private HttpResponse(HttpResponseMessage response)
        {
            Headers = new Dictionary<string, string>();
            Code = (int)response.StatusCode;

            foreach (var header in response.Headers)
            {
                Headers.Add(header.Key, header.Value.First());
            }
        }

        public static HttpResponse<T> Get(HttpResponseMessage response)
        {
            var res = new HttpResponse<T>(response);
            if (response.Content == null) return res;
            
            var streamTask = response.Content.ReadAsStreamAsync();

            res.Raw = streamTask.GetAwaiter().GetResult();

            var genericType = typeof(T);
            if (genericType == typeof(string))
                res.Body = (T) (object) response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            else if (genericType == typeof(Stream))
                res.Body = (T) (object) res.Raw;
            else
                res.Body = JsonConvert.DeserializeObject<T>(
                    response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            return res;
        }

        public static async Task<HttpResponse<T>> GetAsync(HttpResponseMessage response)
        {
            var res = new HttpResponse<T>(response);
            if (response.Content == null) return res;
            
            var streamTask = response.Content.ReadAsStreamAsync();

            res.Raw = await streamTask;

            var genericType = typeof(T);
            if (genericType == typeof(string))
                res.Body = (T) (object) await response.Content.ReadAsStringAsync();
            else if (genericType == typeof(Stream))
                res.Body = (T) (object) res.Raw;
            else
                res.Body = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

            return res;
        }
    }
}
