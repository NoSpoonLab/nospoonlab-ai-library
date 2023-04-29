using System;

namespace AICore.Infrastructure.NoSpoonAI.Types.Exceptions
{
    /// <summary>
    /// A custom exception class that is thrown when an input string is empty.
    /// It normally is thrown when the embedding request is passed empty data.
    /// </summary>
    public class NoSpoonAIEmptyStringException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpoonAIEmptyStringException"/> class.
        /// </summary>
        public NoSpoonAIEmptyStringException() : base("The string that has been entered is empty") {}
    }
}