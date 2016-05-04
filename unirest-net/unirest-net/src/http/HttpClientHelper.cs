using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System;
using System.IO;

using unirest_net.request;

namespace unirest_net.http
{
    public static class HttpClientHelper
    {
        private const string USER_AGENT = "unirest-net/1.0";

        //singleton access to HttpClient
        private static HttpClient _sharedHttpClient = null;
        private static TimeSpan timeout = TimeSpan.FromMinutes(10);
        private static object syncRoot = new object();
        
        private static HttpClient sharedClient
        {
            get
            {
                lock(syncRoot)
                {
                    if (_sharedHttpClient == null)
                    {
                        _sharedHttpClient = new HttpClient();
                        _sharedHttpClient.Timeout = timeout;
                    }
                    return _sharedHttpClient;
                }
            }
        }

        /// <summary>
        /// Use this timeout value unless request specifies its own value for timeout
        /// Throws System.Threading.Tasks.TaskCanceledException when timeout
        /// </summary>
        public static TimeSpan ConnectionTimeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// Do not dispose HttpClient upon every http request.
        /// This method should be called upon end of application execution.
        /// </summary>
        public static void Shutdown()
        {
            lock (syncRoot)
            {
                if (_sharedHttpClient != null)
                {
                    _sharedHttpClient.Dispose();
                }
            }
        }

        public static HttpResponse<T> Request<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            Task.WaitAll(responseTask);
            var response = responseTask.Result;

            return new HttpResponse<T>(response);
        }
        
        public static Task<HttpResponse<T>> RequestAsync<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            return Task<HttpResponse<T>>.Factory.StartNew(() =>
            {
                Task.WaitAll(responseTask);
                return new HttpResponse<T>(responseTask.Result);
            });
        }

        public static HttpResponse<T> RequestStream<T>(HttpRequest request)
        {
            var responseTask = RequestStreamHelper(request);
            Task.WaitAll(responseTask);
            var response = responseTask.Result;

            return new HttpResponse<T>(response);
        }

        public static Task<HttpResponse<T>> RequestStreamAsync<T>(HttpRequest request)
        {
            var responseTask = RequestStreamHelper(request);
            return Task<HttpResponse<T>>.Factory.StartNew(() =>
            {
                Task.WaitAll(responseTask);
                return new HttpResponse<T>(responseTask.Result);
            });
        }

        private static Task<HttpResponseMessage> RequestHelper(HttpRequest request)
        {
            //create http request
            HttpRequestMessage msg = prepareRequest(request, sharedClient);
            return sharedClient.SendAsync(msg);
        }

        private static Task<HttpResponseMessage> RequestStreamHelper(HttpRequest request)
        {
            //create http request
            HttpRequestMessage msg = prepareRequest(request, sharedClient);
            return sharedClient.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead);
        }

        private static HttpRequestMessage prepareRequest(HttpRequest request, HttpClient client)
        {
            if (!request.Headers.ContainsKey("user-agent"))
            {
                request.Headers.Add("user-agent", USER_AGENT);
            }

            //create http request
            HttpRequestMessage msg = new HttpRequestMessage(request.HttpMethod, request.URL);

            //process basic authentication
            if (request.NetworkCredentials != null)
            {
                string authToken = Convert.ToBase64String(
                                        Encoding.UTF8.GetBytes(string.Format("{0}:{1}",
                                        request.NetworkCredentials.UserName,
                                        request.NetworkCredentials.Password))
                                    );

                string authValue = string.Format("Basic {0}", authToken);

                request.Headers.Add("Authorization", authValue);
            }

            //append body content
            if (request.Body != null)
            {
                if (!(request.Body is MultipartFormDataContent) || (request.Body as MultipartFormDataContent).Any())
                    msg.Content = request.Body;
            }

            //append all headers
            foreach (var header in request.Headers)
            {
                string contentTypeKey = "Content-type";
                if (header.Key.Equals(contentTypeKey, StringComparison.CurrentCultureIgnoreCase))
                {
                    msg.Content.Headers.Remove(contentTypeKey);
                    msg.Content.Headers.Add(contentTypeKey, header.Value);
                }
                else
                {
                    msg.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            //process message with the filter before sending
            request.Filter?.Invoke(msg);

            return msg;
        }
    }
}
