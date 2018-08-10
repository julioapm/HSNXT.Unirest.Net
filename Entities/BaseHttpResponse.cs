using System.Collections.Generic;
using System.Net;

namespace HSNXT.Unirest.Net.Http
{
    public class BaseHttpResponse
    {
        /// <summary>
        /// The HTTP status code of the response, as an integer.
        /// </summary>
        public int Code => (int) CodeType;

        /// <summary>
        /// The HTTP status code of the response, as an <see cref="HttpStatusCode"/> object.
        /// </summary>
        public HttpStatusCode CodeType { get; }

        /// <summary>
        /// Whether or not the request succeeded.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// A map of the response headers from key to value.
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        protected BaseHttpResponse(Dictionary<string, string> headers, HttpStatusCode codeType, bool isSuccess)
        {
            Headers = headers;
            CodeType = codeType;
            IsSuccess = isSuccess;
        }
    }
}