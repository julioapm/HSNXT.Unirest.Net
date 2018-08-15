using System;
using System.Net.Http;
using HSNXT.Unirest.Net.Entities;
using HSNXT.Unirest.Net.Internals;

namespace HSNXT.Unirest.Net
{
    /// <summary>
    /// Unirest.Net request creation utility methods.
    /// </summary>
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

    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP GET request.
    /// </summary>
    [Obsolete("Use GetRequestUrl for more complete GET request query support")]
    public class GetRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP GET request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public GetRequest(string url) : base(HttpMethod.Get, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP GET request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public GetRequest(Uri url) : base(HttpMethod.Get, url)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP PUT request.
    /// </summary>
    public class PutRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP PUT request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public PutRequest(string url) : base(HttpMethod.Put, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP PUT request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public PutRequest(Uri url) : base(HttpMethod.Put, url)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP POST request.
    /// </summary>
    public class PostRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP POST request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public PostRequest(string url) : base(HttpMethod.Post, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP POST request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public PostRequest(Uri url) : base(HttpMethod.Post, url)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP DELETE request.
    /// </summary>
    public class DeleteRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP DELETE request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public DeleteRequest(string url) : base(HttpMethod.Delete, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP DELETE request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public DeleteRequest(Uri url) : base(HttpMethod.Delete, url)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP HEAD request.
    /// </summary>
    public class HeadRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP HEAD request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public HeadRequest(string url) : base(HttpMethod.Head, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP HEAD request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public HeadRequest(Uri url) : base(HttpMethod.Head, url)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP OPTIONS request.
    /// </summary>
    public class OptionsRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP OPTIONS request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public OptionsRequest(string url) : base(HttpMethod.Options, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP OPTIONS request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public OptionsRequest(Uri url) : base(HttpMethod.Options, url)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP TRACE request.
    /// </summary>
    public class TraceRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP TRACE request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public TraceRequest(string url) : base(HttpMethod.Trace, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP TRACE request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public TraceRequest(Uri url) : base(HttpMethod.Trace, url)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP PATCH request.
    /// </summary>
    public class PatchRequest : HttpRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP PATCH request.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <exception cref="ArgumentException">If <paramref name="url"/> is not a valid URL.</exception>
        public PatchRequest(string url) : base(Unirest.PatchMethod, url)
        {
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP PATCH request.
        /// </summary>
        /// <param name="url">The URI to make the request with.</param>
        public PatchRequest(Uri url) : base(Unirest.PatchMethod, url)
        {
        }
    }
    
    // these ones don't have overloads that take in Uri, since the url builder doesn't play well with them.
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP GET request with an URL query params builder.
    /// </summary>
    public class GetRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP GET request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public GetRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Get, url, encodeSpaceAsPlusSign)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP PUT request with an URL query params builder.
    /// </summary>
    public class PutRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP PUT request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public PutRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Put, url, encodeSpaceAsPlusSign)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP POST request with an URL query params builder.
    /// </summary>
    public class PostRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP POST request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public PostRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Post, url, encodeSpaceAsPlusSign)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP DELETE request with an URL query params builder.
    /// </summary>
    public class DeleteRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP DELETE request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public DeleteRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Delete, url, encodeSpaceAsPlusSign)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP HEAD request with an URL query params builder.
    /// </summary>
    public class HeadRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP HEAD request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public HeadRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Head, url, encodeSpaceAsPlusSign)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP OPTIONS request with an URL query params builder.
    /// </summary>
    public class OptionsRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP OPTIONS request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public OptionsRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Options, url, encodeSpaceAsPlusSign)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP TRACE request with an URL query params builder.
    /// </summary>
    public class TraceRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP TRACE request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public TraceRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(HttpMethod.Trace, url, encodeSpaceAsPlusSign)
        {
        }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Represents an HTTP PATCH request with an URL query params builder.
    /// </summary>
    public class PatchRequestUrl : HttpRequestUrl
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP PATCH request with an URL query params builder.
        /// </summary>
        /// <param name="url">The HTTP/HTTPS URL to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="HttpRequestUrl.SetField(string,object)"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c>
        /// as it's the most common in query strings.</param>
        public PatchRequestUrl(string url, bool encodeSpaceAsPlusSign = true) : base(Unirest.PatchMethod, url, encodeSpaceAsPlusSign)
        {
        }
    }

}
