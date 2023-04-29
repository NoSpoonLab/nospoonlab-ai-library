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
        /// Value representing the GPT-3.5 Turbo model with 16k vocabulary size.
        /// </summary>
        gpt_35_16k,
        
        /// <summary>
        /// Value representing the GPT-4 model.
        /// </summary>
        gpt_4,
        
        /// <summary>
        /// Value representing the GPT-4 model with 32k vocabulary size.
        /// </summary>
        gpt_4_32k,

        /// <summary>
        /// Value representing the Davinci model.
        /// </summary>
        davinci,
        
        /// <summary>
        /// Value representing the Ada text embedding model version 002.
        /// </summary>
        embedding_ada_002
    }
}