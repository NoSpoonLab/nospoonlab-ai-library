using System;
using AICore.Services.Types.Data;

namespace AICore.Services.Extension
{
    /// <summary>
    /// Provides extension methods for the enum 'AIRole' to convert its members to and from their string equivalents.
    /// This static class plays an important role in serializing and deserializing the role of the chat entity in the conversation.
    /// </summary>
    public static class RoleExtension
    {
        /// <summary>
        /// Converts the 'AIRole' enum value to its string equivalent.
        /// </summary>
        /// <param name="value">The role of the chat entity as per the 'AIRole' enum.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception if the input enum value does not match any predefined roles.</exception>
        /// <returns>Returns the string representation of the input 'AIRole' value.</returns>
        internal static string GetString(AIRole value)
        {
            switch (value)
            {
                case AIRole.System:
                    return "system";
                case AIRole.User:
                    return "user";
                case AIRole.Assistant:
                    return "assistant";
                case AIRole.Function:
                    return "function";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
        
        /// <summary>
        /// Converts the string representation of a chat role to its 'AIRole' enum equivalent.
        /// </summary>
        /// <param name="value">The string representation of the chat entity's role.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception if the input string value does not match any predefined roles.</exception>
        /// <returns>Returns the 'AIRole' enum value equivalent to the input string.</returns>
        internal static AIRole GetRole(string value)
        {
            switch (value)
            {
                case "system":
                    return AIRole.System;
                case "user":
                    return AIRole.User;
                case "assistant":
                    return AIRole.Assistant;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}