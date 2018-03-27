using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HSNXT.Unirest.Net.Request;

namespace HSNXT.Unirest.Net.Http
{
    public static class HttpClientHelper
    {
        private const string UserAgent = "unirest.net";

        /// <summary>
        /// Use this timeout value unless request specifies its own value for timeout
        /// Throws System.Threading.Tasks.TaskCanceledException when timeout
        /// </summary>
        public static TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMinutes(10);

        private static HttpClient SharedClient { get; } = new HttpClient {Timeout = ConnectionTimeout};

        public static HttpResponse<T> Request<T>(HttpRequest request)
        {
            return HttpResponse<T>.Get(RequestHelper(request).GetAwaiter().GetResult());
        }

        public static async Task<HttpResponse<T>> RequestAsync<T>(HttpRequest request)
        {
            return await HttpResponse<T>.GetAsync(await RequestHelper(request));
        }

        public static HttpResponse<T> RequestStream<T>(HttpRequest request)
        {
            return HttpResponse<T>.Get(RequestStreamHelper(request).GetAwaiter().GetResult());
        }

        public static async Task<HttpResponse<T>> RequestStreamAsync<T>(HttpRequest request)
        {
            return await HttpResponse<T>.GetAsync(await RequestStreamHelper(request));
        }

        private static Task<HttpResponseMessage> RequestHelper(HttpRequest request)
        {
            //create http request
            var msg = PrepareRequest(request);
            return SharedClient.SendAsync(msg);
        }

        private static Task<HttpResponseMessage> RequestStreamHelper(HttpRequest request)
        {
            //create http request
            var msg = PrepareRequest(request);
            return SharedClient.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead);
        }

        private static HttpRequestMessage PrepareRequest(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("User-Agent"))
            {
                request.Headers.Add("User-Agent", UserAgent);
            }

            //create http request
            var msg = new HttpRequestMessage(request.HttpMethod, request.Url);

            //process basic authentication
            var creds = request.NetworkCredentials;
            if (creds != null)
            {
                var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{creds.UserName}:{creds.Password}"));
                var authValue = $"Basic {authToken}";
                request.Headers.Add("Authorization", authValue);
            }

            //append body content
            if (request.Body != null)
            {
                if (!(request.Body is MultipartFormDataContent) || ((MultipartFormDataContent) request.Body).Any())
                    msg.Content = request.Body;
            }

            //append all headers
            foreach (var header in request.Headers)
            {
                const string contentTypeKey = "Content-Type";
                if (header.Key.Equals(contentTypeKey, StringComparison.CurrentCultureIgnoreCase) && msg.Content != null)
                {
                    msg.Content.Headers.Remove(contentTypeKey);
                    msg.Content.Headers.Add(contentTypeKey, header.Value);
                }
                else
                {
                    msg.Headers.Add(header.Key, header.Value);
                }
            }

            //process message with the filter before sending
            request.Filter?.Invoke(msg);

            return msg;
        }
    }
}