/// <summary>
/// The AgentConstants static class houses constant values related to AI agent's operational parameters.
/// </summary>
namespace AIAgent
{
    public static class AgentConstants
    {
        /// <summary>
        /// The normal number of tokens that AI agent can output in a single completion. Default value is 768.
        /// </summary>
        public const int NORMAL_TOKENS_ON_COMPLETION = 768;
        
        /// <summary>
        /// The maximum number of tokens that AI agent can output in a single completion. Default value is NORMAL_TOKENS_ON_COMPLETION x2.
        /// </summary>
        public const int MAX_TOKENS_ON_COMPLETION = NORMAL_TOKENS_ON_COMPLETION * 2;

        /// <summary>
        /// The maximum number of tokens that AI agent can process in a single request. Default value is 4086.
        /// </summary>
        public const int MAX_TOKENS_ON_REQUEST = 40960;

        /// <summary>
        /// The minimum number of messages that the AI agent retains during conversation cleanup. Default value is 8.
        /// </summary>
        public const int MIN_MESSAGE_ON_CLEAN = 30;

        /// <summary>
        /// The maximum number of past interactions that the AI agent can retrieve from its memory. Default value is 20.
        /// </summary>
        public const int MAX_MEMORIES_RETRIEVAL = 60;
    }
}