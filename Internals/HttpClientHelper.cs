using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HSNXT.Unirest.Net.Entities;

namespace HSNXT.Unirest.Net.Internals
{
    internal static class HttpClientHelper
    {
        private const string UserAgent = "unirest.net";// TODO allow configure this too

        internal static UnirestClientSettings ClientSettings { get; private set; }
        
        internal static HttpClient SharedClient { get; private set; }

        static HttpClientHelper() => UpdateClientSettings(new UnirestClientSettings
        {
            Timeout = TimeSpan.FromMinutes(10),
            // disable proxies by default, fixes long delay every few requests on Windows
            // for consistency, they are disabled on both windows and linux.
            // https://stackoverflow.com/a/11234260
            // https://developercommunity.visualstudio.com/content/problem/282756/intermittent-and-indefinite-wcf-hang-blocking-requ.html
            // https://community.sophos.com/kb/en-us/120019
            UseProxy = false
        });

        internal static void UpdateClientSettings(UnirestClientSettings set)
        {
            SharedClient?.Dispose();

            // TODO check if this is actually the default HttpClient handler, net core source seems to suggest it is
            var handler = new HttpClientHandler();
            if (set.AllowAutoRedirect.HasValue)
                handler.AllowAutoRedirect = set.AllowAutoRedirect.Value;
            if (set.AutomaticDecompression.HasValue)
                handler.AutomaticDecompression = set.AutomaticDecompression.Value;
            if (set.CheckCertificateRevocationList.HasValue)
                handler.CheckCertificateRevocationList = set.CheckCertificateRevocationList.Value;
            if (set.ClientCertificateOptions.HasValue)
                handler.ClientCertificateOptions = set.ClientCertificateOptions.Value;
            if (set.CookieContainer.HasValue)
                handler.CookieContainer = set.CookieContainer.Value;
            if (set.Credentials.HasValue)
                handler.Credentials = set.Credentials.Value;
            if (set.DefaultProxyCredentials.HasValue)
                handler.DefaultProxyCredentials = set.DefaultProxyCredentials.Value;
            if (set.MaxAutomaticRedirections.HasValue)
                handler.MaxAutomaticRedirections = set.MaxAutomaticRedirections.Value;
            if (set.MaxConnectionsPerServer.HasValue)
                handler.MaxConnectionsPerServer = set.MaxConnectionsPerServer.Value;
            if (set.MaxResponseHeadersLength.HasValue)
                handler.MaxResponseHeadersLength = set.MaxResponseHeadersLength.Value;
            if (set.MaxRequestContentBufferSize.HasValue)
                handler.MaxRequestContentBufferSize = set.MaxRequestContentBufferSize.Value;
            if (set.PreAuthenticate.HasValue)
                handler.PreAuthenticate = set.PreAuthenticate.Value;
            if (set.Proxy.HasValue)
                handler.Proxy = set.Proxy.Value;
            if (set.ServerCertificateCustomValidationCallback.HasValue)
                handler.ServerCertificateCustomValidationCallback = set.ServerCertificateCustomValidationCallback.Value;
            if (set.SslProtocols.HasValue)
                handler.SslProtocols = set.SslProtocols.Value;
            if (set.UseCookies.HasValue)
                handler.UseCookies = set.UseCookies.Value;
            if (set.UseDefaultCredentials.HasValue)
                handler.UseDefaultCredentials = set.UseDefaultCredentials.Value;
            if (set.UseProxy.HasValue)
                handler.UseProxy = set.UseProxy.Value;

            // TODO don't recreate the HttpClient if not necessary
            var client = new HttpClient(handler);
            if (set.BaseAddress.HasValue)
                client.BaseAddress = set.BaseAddress.Value;
            if (set.MaxResponseContentBufferSize.HasValue)
                client.MaxResponseContentBufferSize = set.MaxResponseContentBufferSize.Value;
            if (set.Timeout.HasValue)
                client.Timeout = set.Timeout.Value;

            ClientSettings = set;
            SharedClient = client;
        }

        public static async Task<HttpResponse<T>> RequestAsync<T>(HttpRequest request,
            OnSuccessAsync<T> onSuccess = null, OnFailAsync<T> onFail = null)
        {
            return await HttpResponse<T>.GetAsync(await RequestHelper(request), request.EnsureSuccess, onSuccess, onFail);
        }

        public static async Task<HttpResponse<Stream>> RequestStreamAsync(HttpRequest request,
            OnSuccessAsync<Stream> onSuccess = null, OnFailAsync<Stream> onFail = null)
        {
            return await HttpResponse<Stream>.GetAsync(await RequestStreamHelper(request), request.EnsureSuccess, onSuccess, onFail);
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