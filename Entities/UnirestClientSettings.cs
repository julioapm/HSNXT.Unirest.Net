using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace HSNXT.Unirest.Net.Entities
{
    // TODO Optional<>
    // TODO Unirest.UpdateClient
    // TODO put properties from HttpClient here as well
    public class UnirestClientSettings
    {
        #region HttpClientHandler
        /// <summary>Gets or sets a value that indicates whether the handler should follow redirection responses.</summary>
        /// <returns>true if the if the handler should follow redirection responses; otherwise false. The default value is true.</returns>
        public UOptional<bool> AllowAutoRedirect { internal get; set; }

        /// <summary>Gets or sets the type of decompression method used by the handler for automatic decompression of the HTTP content response.</summary>
        /// <returns>The automatic decompression method used by the handler.</returns>
        public UOptional<DecompressionMethods> AutomaticDecompression { internal get; set; }

        /// <summary>Gets or sets a value that indicates whether the certificate is checked against the certificate authority revocation list.</summary>
        /// <returns>true if the certificate revocation list is checked; otherwise, false.</returns>
        public UOptional<bool> CheckCertificateRevocationList { internal get; set; }

        /// <summary>Gets or sets a value that indicates if the certificate is automatically picked from the certificate store or if the caller is allowed to pass in a specific client certificate.</summary>
        /// <returns>The collection of security certificates associated with this handler.</returns>
        public UOptional<ClientCertificateOption> ClientCertificateOptions { internal get; set; }

        /// <summary>Gets or sets the cookie container used to store server cookies by the handler.</summary>
        /// <returns>The cookie container used to store server cookies by the handler.</returns>
        public UOptional<CookieContainer> CookieContainer { internal get; set; }

        /// <summary>Gets or sets authentication information used by this handler.</summary>
        /// <returns>The authentication credentials associated with the handler. The default is null.</returns>
        public UOptional<ICredentials> Credentials { internal get; set; }

        /// <summary>Gets or sets the credentials to submit to the proxy server for authentication.</summary>
        /// <returns>The credentials needed to authenticate a request to the proxy server.</returns>
        public UOptional<ICredentials> DefaultProxyCredentials { internal get; set; }

        /// <summary>Gets or sets the maximum number of redirects that the handler follows.</summary>
        /// <returns>The maximum number of redirection responses that the handler follows. The default value is 50.</returns>
        public UOptional<int> MaxAutomaticRedirections { internal get; set; }

        /// <summary>Gets or sets the maximum number of concurrent connections allowed by an <see cref="HttpClient"/> object.</summary>
        /// <returns>The maximum number of concurrent connections allowed by an <see cref="HttpClient"/> object.</returns>
        public UOptional<int> MaxConnectionsPerServer { internal get; set; }

        /// <summary>Gets or sets the maxiumum response header length.</summary>
        /// <returns>The maximum response header length.</returns>
        public UOptional<int> MaxResponseHeadersLength { internal get; set; }

        /// <summary>Gets or sets the maximum request content buffer size used by the handler.</summary>
        /// <returns>The maximum request content buffer size in bytes. The default value is 2 gigabytes.</returns>
        public UOptional<long> MaxRequestContentBufferSize { internal get; set; }

        /// <summary>Gets or sets a value that indicates whether the handler sends an Authorization header with the request.</summary>
        /// <returns>true for the handler to send an HTTP Authorization header with requests after authentication has taken place; otherwise, false. The default is false.</returns>
        public UOptional<bool> PreAuthenticate { internal get; set; }

        /// <summary>Gets or sets proxy information used by the handler.</summary>
        /// <returns>The proxy information used by the handler. The default value is null.</returns>
        public UOptional<IWebProxy> Proxy { internal get; set; }

        /// <summary>Gets or sets a callback method to validate the server certificate.</summary>
        /// <returns>A callback method to validate the server certificate.</returns>
        public UOptional<Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>>
            ServerCertificateCustomValidationCallback { internal get; set; }

        /// <summary>Gets or sets the SSL protocol used by the HttpClient objects managed by the HttpClientHandler object.</summary>
        /// <returns>One of the values defined in the SecurityProtocolType enumeration.</returns>
        public UOptional<SslProtocols> SslProtocols { internal get; set; }

        /// <summary>Gets or sets a value that indicates whether the handler uses the  <see cref="HttpClientHandler.CookieContainer"></see> property  to store server cookies and uses these cookies when sending requests.</summary>
        /// <returns>true if the if the handler supports uses the  <see cref="HttpClientHandler.CookieContainer"></see> property  to store server cookies and uses these cookies when sending requests; otherwise false. The default value is true.</returns>
        public UOptional<bool> UseCookies { internal get; set; }

        /// <summary>Gets or sets a value that controls whether default credentials are sent with requests by the handler.</summary>
        /// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
        public UOptional<bool> UseDefaultCredentials { internal get; set; }

        /// <summary>Gets or sets a value that indicates whether the handler uses a proxy for requests.</summary>
        /// <returns>true if the handler should use a proxy for requests; otherwise false. The default value is true.</returns>
        public UOptional<bool> UseProxy { internal get; set; }
        #endregion

        #region HttpClient
        /// <summary>Gets or sets the base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.</summary>
        /// <returns>The base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.</returns>
        public UOptional<Uri> BaseAddress { internal get; set; }

        /// <summary>Gets or sets the maximum number of bytes to buffer when reading the response content.</summary>
        /// <returns>The maximum number of bytes to buffer when reading the response content. The default value for this property is 2 gigabytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The size specified is less than or equal to zero.</exception>
        /// <exception cref="InvalidOperationException">An operation has already been started on the current instance.</exception>
        /// <exception cref="ObjectDisposedException">The current instance has been disposed.</exception>
        public UOptional<long> MaxResponseContentBufferSize { internal get; set; }

        /// <summary>Gets or sets the timespan to wait before the request times out.</summary>
        /// <returns>The timespan to wait before the request times out.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The timeout specified is less than or equal to zero and is not <see cref="System.Threading.Timeout.InfiniteTimeSpan"></see>.</exception>
        /// <exception cref="InvalidOperationException">An operation has already been started on the current instance.</exception>
        /// <exception cref="ObjectDisposedException">The current instance has been disposed.</exception>
        public UOptional<TimeSpan> Timeout { internal get; set; }
        #endregion
    }
}