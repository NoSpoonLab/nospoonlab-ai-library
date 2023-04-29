using System;
using AICore.Services.Types.Data;

namespace AICore.Services.Extension
{
    /// <summary>
    /// Provides methods to extend the functionality of the <see cref="AIModel"/> enum.
    /// </summary>
    public static class ModelsExtension
    {
        /// <summary>
        /// Converts an <see cref="AIModel"/> enum value to its corresponding string representation.
        /// </summary>
        /// <param name="model">The AIModel to get the string representation of.</param>
        /// <returns>The string representation of the provided AIModel.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unsupported AIModel is provided.</exception>
        internal static string GetString(AIModel model)
        {
            switch (model)
            {
                case AIModel.gpt:
                    return "gpt-3.5-turbo-0613";
                case AIModel.gpt_35_16k:
                    return "gpt-3.5-turbo-16k";
                case AIModel.gpt_4:
                    return "gpt-4";
                case AIModel.gpt_4_32k:
                    return "gpt-4-32k";
                case AIModel.davinci:
                    return "davinci";
                case AIModel.embedding_ada_002:
                    return "text-embedding-ada-002";
                default:
                    throw new ArgumentOutOfRangeException(nameof(model), model, null);
            }
        }
    }
}