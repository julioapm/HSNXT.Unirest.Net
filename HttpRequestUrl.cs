using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable MemberCanBeProtected.Global

namespace HSNXT.Unirest.Net
{
    /// <summary>
    /// Represents an HTTP request with an URL query params builder.
    /// </summary>
    public class HttpRequestUrl : HttpRequest
    {
        /// <summary>
        /// Shortcut to <see cref="HttpRequest.SetFields"/> or individual <see cref="SetField(string,object)"/> calls
        /// (use object initializer)
        /// </summary>
        // ReSharper disable once CollectionNeverQueried.Global
        public new FieldsDictUrl Fields { get; }
        
        private bool HasFirstField { get; set; }
        
        private string UrlString { get; set; }
        private string UrlStringCached { get; set; }
        
        private Uri UrlCached { get; set; }

        public bool EncodeSpaceAsPlusSign { get; set; }
        
        public override Uri Url => TryCreateUrl(UrlString);

        private Uri TryCreateUrl(string url)
        {
            if (UrlStringCached == url && UrlCached != null)
            {
                return UrlCached;
            }

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var locurl))
            {
                throw new ArgumentException("The url passed is not a valid HTTP/S URL");
            }

            UrlStringCached = url;
            return UrlCached = locurl;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a new HTTP request with an URL query params builder.
        /// </summary>
        /// <param name="method">The HTTP protocol method to send the request with.</param>
        /// <param name="url">The URI to make the request with.</param>
        /// <param name="encodeSpaceAsPlusSign">Whether to encode the space character when used in
        /// <see cref="SetField"/> as <c>+</c> or <c>%20</c>. The default is <c>true</c> as it's the most common in
        /// query strings.</param>
        public HttpRequestUrl(HttpMethod method, string url, bool encodeSpaceAsPlusSign = true) : base(method, url)
        {
            Fields = new FieldsDictUrl(this);
            UrlString = url;
            UrlStringCached = url;
            EncodeSpaceAsPlusSign = encodeSpaceAsPlusSign;
        }

        public new HttpRequestUrl SetField(string key, object value)
        {
            var val = GetValue(value?.ToString() ?? "null");
            if (!HasFirstField)
            {
                HasFirstField = true;
                UrlString += $"?{key}={val}";
            }
            else
            {
                UrlString += $"&{key}={val}";
            }
            return this;
        }

        private string GetValue(string value)
        {
            return EncodeSpaceAsPlusSign ? HttpUtility.UrlEncode(value) : Uri.EscapeDataString(value);
        }
    }

    public class FieldsDictUrl : Dictionary<string, object>
    {
        private HttpRequestUrl Request { get; }
        
        public new string this[string key]
        {
            get => base[key] as string;
            set
            {
                Request.SetField(key, value);
                base[key] = value;
            }
        }
        
        public FieldsDictUrl(HttpRequestUrl mast)
        {
            Request = mast;
        }
    }
}