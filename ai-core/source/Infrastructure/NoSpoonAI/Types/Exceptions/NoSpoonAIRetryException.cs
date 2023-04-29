using System;

namespace AICore.Infrastructure.NoSpoonAI.Types.Exceptions
{
    /// <summary>
    /// A custom exception class that is thrown when the maximum number of retry attempts for a request is exhausted.
    /// </summary>
    public class NoSpoonAIRetryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAIRetryException"/> class.
        /// </summary>
        /// <param name="message">Passes the specific error message upon failure of the request attempt.</param>
        public NoSpoonAIRetryException(string message) : base("Exhausted the maximum number of attempts, request fail, error message: " + message) {}
    }
}