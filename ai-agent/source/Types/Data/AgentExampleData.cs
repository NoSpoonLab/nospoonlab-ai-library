namespace AIAgent.Types.Data
{
    /// <summary>
    /// This namespace contains data models used by an AI agent.
    /// </summary>
    namespace AIAgent.Types.Data
    {
        /// <summary>
        /// This static class contains a nested class representing the example data used by the AI agent for learning and development purposes.
        /// </summary>
        public static class AgentExampleData
        {
            /// <summary>
            /// This class represents the example data between the user and the AI assistant. This data is typically used to train, test, or evaluate the AI agent.
            /// </summary>
            public class ExampleData
            {
                /// <summary>
                /// Represents a message or input from the user.
                /// </summary>
                public string user;

                /// <summary>
                /// Represents a message or response from the AI assistant.
                /// </summary>
                public string assitant;
            }
        }
    }
}