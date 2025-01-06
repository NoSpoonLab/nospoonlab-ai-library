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
                    return ModelsExtension.GetString(AIModel.gpt_4o_mini);
                case AIModel.gpt_35_turbo:
                    return "ggpt-3.5-turbo";
                case AIModel.gpt_4:
                    return "gpt-4";
                case AIModel.gpt_4_turbo:
                    return "gpt-4-turbo";
                case AIModel.gpt_4o:
                    return "gpt-4o";
                case AIModel.gpt_4o_mini:
                    return "gpt-4o-mini";
                case AIModel.o1:
                    return "o1";
                case AIModel.o1_mini:
                    return "o1-mini";
                case AIModel.gpt_4o_realtime:
                    return "gpt-4o-realtime-preview";
                case AIModel.gpt_4o_audio:
                    return "gpt-4o-audio-preview";
                case AIModel.davinci:
                    return "davinci";
                case AIModel.embedding_ada_002:
                    return "text-embedding-ada-002";
                case AIModel.none:
                    throw new ArgumentOutOfRangeException(nameof(model), model, null);
                case AIModel.text_embedding_3_small:
                    return "text-embedding-3-small";
                case AIModel.text_embedding_3_large:
                    return "text-embedding-3-large";
                case AIModel.embedding_bert_base_uncased:
                case AIModel.embedding_bert_base_large_uncased:
                case AIModel.embedding_bert_base_cased:
                case AIModel.embedding_bert_base_large_cased:
                case AIModel.embedding_bert_base_multilingual_cased:
                case AIModel.embedding_bert_base_german_cased:
                case AIModel.embedding_bert_custom:
                case AIModel.local_model:
                default:
                    throw new ArgumentOutOfRangeException(nameof(model), model, null);
            }
        }
    }
}