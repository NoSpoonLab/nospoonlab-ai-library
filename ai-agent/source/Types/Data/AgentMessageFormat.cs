using System;

namespace AIAgent.Types.Data
{
    /// <summary>
    /// This class represents the format of a message within the AI agent's communication protocols.
    /// </summary>
    [Serializable]
    public class AgentMessageFormat
    {
        /// <summary>
        /// Represents the actual content of a message that the AI agent communicates.
        /// </summary>
        public string message;
    }
}