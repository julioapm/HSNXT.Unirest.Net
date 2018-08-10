using System;
using System.Net.Http;

namespace HSNXT.Unirest.Net.Entities.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an exception thrown when <see cref="HttpResponseMessage.Content"/> is null during transport.
    /// </summary>
    public class UnirestRequestException : Exception
    {
        internal UnirestRequestException(string message) : base(message)
        {
        }
    }
}