using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fallk.Unirest.Net.request;

namespace Fallk.Unirest.Net.Http
{
    public static class HttpClientHelper
    {
        private const string UserAgent = "unirest.net";

        //singleton access to HttpClient
        private static HttpClient _sharedHttpClient;

        private static readonly object SyncRoot = new object();

        private static HttpClient SharedClient
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_sharedHttpClient != null) return _sharedHttpClient;
                    _sharedHttpClient = new HttpClient {Timeout = ConnectionTimeout};
                    return _sharedHttpClient;
                }
            }
        }

        /// <summary>
        /// Use this timeout value unless request specifies its own value for timeout
        /// Throws System.Threading.Tasks.TaskCanceledException when timeout
        /// </summary>
        public static TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Do not dispose HttpClient upon every http request.
        /// This method should be called upon end of application execution.
        /// </summary>
        public static void Shutdown()
        {
            lock (SyncRoot)
            {
                _sharedHttpClient?.Dispose();
            }
        }

        public static HttpResponse<T> Request<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            return new HttpResponse<T>(responseTask.GetAwaiter().GetResult());
        }

        public static Task<HttpResponse<T>> RequestAsync<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            return Task.Run(() => new HttpResponse<T>(responseTask.GetAwaiter().GetResult()));
        }

        public static HttpResponse<T> RequestStream<T>(HttpRequest request)
        {
            var responseTask = RequestStreamHelper(request);
            return new HttpResponse<T>(responseTask.GetAwaiter().GetResult());
        }

        public static Task<HttpResponse<T>> RequestStreamAsync<T>(HttpRequest request)
        {
            var responseTask = RequestStreamHelper(request);
            return Task.Run(() => new HttpResponse<T>(responseTask.GetAwaiter().GetResult()));
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