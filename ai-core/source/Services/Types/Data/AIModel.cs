namespace AICore.Services.Types.Data
{
    /// <summary>
    /// Enum representing all the various AI models currently supported by the AICore.
    /// Each member of this enum corresponds to a particular AI model.
    /// </summary>
    public enum AIModel
    {
        /// <summary>
        /// Value representing no model.
        /// </summary>
        none,
        
        /// <summary>
        /// Value representing the GPT-3.5 Turbo model.
        /// </summary>
        gpt,
        
        /// <summary>
        /// Value representing the GPT-3.5 Turbo model.
        /// </summary>
        gpt_35_turbo,
        
        /// <summary>
        /// Value representing the GPT-4 model.
        /// </summary>
        gpt_4,
        
        /// <summary>
        /// Value representing the GPT-4 turbo model.
        /// </summary>
        gpt_4_turbo,
        gpt_4o,
        gpt_4o_mini,
        o1,
        o1_mini,
        gpt_4o_realtime,
        gpt_4o_audio,
        
        
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Value representing the Davinci model.
        /// </summary>
        davinci,
        
        /// <summary>
        /// Value representing the Ada text embedding model version 002.
        /// </summary>
        text_embedding_3_small,
        text_embedding_3_large,
        embedding_ada_002,
        embedding_bert_base_uncased,
        embedding_bert_base_large_uncased,
        embedding_bert_base_cased,
        embedding_bert_base_large_cased,
        embedding_bert_base_multilingual_cased,
        embedding_bert_base_german_cased,
        embedding_bert_custom,
        local_model
    }
}