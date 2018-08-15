using System;
using System.Net.Http;
using HSNXT.Unirest.Net.Entities;
using HSNXT.Unirest.Net.Internals;

namespace HSNXT.Unirest.Net
{
    public static class Unirest
    {
        internal static readonly HttpMethod PatchMethod = new HttpMethod("PATCH");

        /// <summary>
        /// Creates a new HTTP GET request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Get(string url) => new HttpRequest(HttpMethod.Get, url);

        /// <summary>
        /// Creates a new HTTP POST request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Post(string url) => new HttpRequest(HttpMethod.Post, url);

        /// <summary>
        /// Creates a new HTTP DELETE request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Delete(string url) => new HttpRequest(HttpMethod.Delete, url);

        /// <summary>
        /// Creates a new HTTP PATCH request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Patch(string url) => new HttpRequest(PatchMethod, url);

        /// <summary>
        /// Creates a new HTTP PUT request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Put(string url) => new HttpRequest(HttpMethod.Put, url);

        /// <summary>
        /// Creates a new HTTP OPTIONS request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Options(string url) => new HttpRequest(HttpMethod.Options, url);

        /// <summary>
        /// Creates a new HTTP HEAD request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Head(string url) => new HttpRequest(HttpMethod.Head, url);

        /// <summary>
        /// Creates a new HTTP TRACE request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public static HttpRequest Trace(string url) => new HttpRequest(HttpMethod.Trace, url);

        /// <summary>
        /// Creates a new HTTP GET request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Get(Uri url) => new HttpRequest(HttpMethod.Get, url);

        /// <summary>
        /// Creates a new HTTP POST request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Post(Uri url) => new HttpRequest(HttpMethod.Post, url);

        /// <summary>
        /// Creates a new HTTP DELETE request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Delete(Uri url) => new HttpRequest(HttpMethod.Delete, url);

        /// <summary>
        /// Creates a new HTTP PATCH request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Patch(Uri url) => new HttpRequest(PatchMethod, url);

        /// <summary>
        /// Creates a new HTTP PUT request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Put(Uri url) => new HttpRequest(HttpMethod.Put, url);

        /// <summary>
        /// Creates a new HTTP OPTIONS request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Options(Uri url) => new HttpRequest(HttpMethod.Options, url);

        /// <summary>
        /// Creates a new HTTP HEAD request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Head(Uri url) => new HttpRequest(HttpMethod.Head, url);

        /// <summary>
        /// Creates a new HTTP TRACE request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public static HttpRequest Trace(Uri url) => new HttpRequest(HttpMethod.Trace, url);

        /// <summary>
        /// Modifies the Unirest backing <see cref="HttpClient"/>'s settings in-place. The changes will affect
        /// </summary>
        /// <param name="settingsMutator"><see cref="Action{T}"/> that takes in the existing client's settings and
        /// changes them.</param>
        public static void UpdateClient(Action<UnirestClientSettings> settingsMutator)
        {
            settingsMutator(HttpClientHelper.ClientSettings);
            HttpClientHelper.UpdateClientSettings(HttpClientHelper.ClientSettings);
        }

        /// <summary>
        /// Sets the Unirest backing <see cref="HttpClient"/>'s settings.
        /// </summary>
        /// <param name="newSettings">The new settings to set.</param>
        public static void UpdateClient(UnirestClientSettings newSettings)
        {
            HttpClientHelper.UpdateClientSettings(newSettings);
        }
    }

    [Obsolete("Use GetRequestUrl for more complete GET request query support")]
    public class GetRequest : HttpRequest
    {
        public GetRequest(string url) : base(HttpMethod.Get, url)
        {
        }
    }
    public class PutRequest : HttpRequest
    {
        public PutRequest(string url) : base(HttpMethod.Put, url)
        {
        }
    }
    public class PostRequest : HttpRequest
    {
        public PostRequest(string url) : base(HttpMethod.Post, url)
        {
        }
    }
    public class DeleteRequest : HttpRequest
    {
        public DeleteRequest(string url) : base(HttpMethod.Delete, url)
        {
        }
    }
    public class HeadRequest : HttpRequest
    {
        public HeadRequest(string url) : base(HttpMethod.Head, url)
        {
        }
    }
    public class OptionsRequest : HttpRequest
    {
        public OptionsRequest(string url) : base(HttpMethod.Options, url)
        {
        }
    }
    public class TraceRequest : HttpRequest
    {
        public TraceRequest(string url) : base(HttpMethod.Trace, url)
        {
        }
    }
    public class PatchRequest : HttpRequest
    {
        public PatchRequest(string url) : base(Unirest.PatchMethod, url)
        {
        }
    }
    
    public class GetRequestUrl : HttpRequestUrl
    {
        public GetRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Get, url, encodeSpaceAsPlusSign)
        {
        }
    }
    public class PutRequestUrl : HttpRequestUrl
    {
        public PutRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Put, url, encodeSpaceAsPlusSign)
        {
        }
    }
    public class PostRequestUrl : HttpRequestUrl
    {
        public PostRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Post, url, encodeSpaceAsPlusSign)
        {
        }
    }
    public class DeleteRequestUrl : HttpRequestUrl
    {
        public DeleteRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Delete, url, encodeSpaceAsPlusSign)
        {
        }
    }
    public class HeadRequestUrl : HttpRequestUrl
    {
        public HeadRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Head, url, encodeSpaceAsPlusSign)
        {
        }
    }
    public class OptionsRequestUrl : HttpRequestUrl
    {
        public OptionsRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Options, url, encodeSpaceAsPlusSign)
        {
        }
    }
    public class TraceRequestUrl : HttpRequestUrl
    {
        public TraceRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Trace, url, encodeSpaceAsPlusSign)
        {
        }
    }
    public class PatchRequestUrl : HttpRequestUrl
    {
        public PatchRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(Unirest.PatchMethod, url, encodeSpaceAsPlusSign)
        {
        }
    }

}
