using System;

namespace AIAgent.Types.Data
{
    /// <summary>
    /// This namespace contains various data models used by an AI agent.
    /// </summary>
    namespace AIAgent.Types.Data
    {
        /// <summary>
        /// This class represents the importance of a specific memory inside the AI agent's memory.
        /// </summary>
        [Serializable]
        public class AgentMemoryImportance
        {
            /// <summary>
            /// Represents a numerical score corresponding to the importance of an agent's memory.
            /// The higher this value, the more important the memory is considered.
            /// </summary>
            public float score;
        }
    }
}