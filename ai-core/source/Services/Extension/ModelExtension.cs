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
        public static string GetString(AIModel model)
        {
            switch (model)
            {
                case AIModel.gpt:
                    return "gpt-4-turbo-preview";
                case AIModel.gpt_35_16k:
                    return "gpt-3.5-turbo-16k";
                case AIModel.gpt_4:
                    return "gpt-4-turbo-preview";
                case AIModel.davinci:
                    return "davinci";
                case AIModel.embedding_ada_002:
                    return "text-embedding-ada-002";
                case AIModel.detective_model:
                    return "gpt-4-turbo-preview";
                default:
                    throw new ArgumentOutOfRangeException(nameof(model), model, null);
            }
        }
    }
}