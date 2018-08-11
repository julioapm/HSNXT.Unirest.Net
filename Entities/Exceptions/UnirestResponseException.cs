using System;

namespace HSNXT.Unirest.Net.Entities.Exceptions
{
    /// <inheritdoc cref="System.Exception" />
    /// <summary>
    /// Represents an exception thrown when a request completes with a non-success status code, as per
    /// <see cref="BaseHttpResponse"/>.<see cref="BaseHttpResponse.IsSuccess"/>.
    ///
    /// <p>An <see cref="UnirestResponseException{T}"/> object should be disposed after being caught.</p>
    /// </summary>
    /// <typeparam name="T">The original target response type, which a failed response needn't conform to</typeparam>
    public class UnirestResponseException<T> : Exception, IDisposable
    {
        /// <summary>
        /// The partial, untouched (by the library) response object.
        /// </summary>
        public PartialHttpResponse<T> PartialResponse { get; }
        
        internal UnirestResponseException(string message, PartialHttpResponse<T> partialResponse) : base(message)
        {
            PartialResponse = partialResponse;
        }

        public void Dispose() => PartialResponse?.Dispose();
    }
}