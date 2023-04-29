using System;

namespace AICore.Infrastructure.NoSpoonAI.Types.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the execution of the NoSpoonAI system for which no specific information is available.
    /// </summary>
    public class NoSpoonAIUnknownErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAIUnknownErrorException"/> class.
        /// </summary>
        public NoSpoonAIUnknownErrorException() : base("Unknown error") {}
    }
}