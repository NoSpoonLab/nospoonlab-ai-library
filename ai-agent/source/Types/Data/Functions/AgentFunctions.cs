using System.Collections.Generic;
using Newtonsoft.Json;

namespace AIAgent.Types.Data.Functions
{
    /// <summary>
    /// This class represents functionalities provided by the AI agent, specifically related to handling messages and emotions.
    /// </summary>
    public class AgentFunctions
    {
        /// <summary>
        /// Represents the properties of various functionalities that an AI agent can perform.
        /// </summary>
        public class FunctionProperties
        {
            /// <summary>
            /// Representing properties related to message handling function performed by the AI agent.
            /// </summary>
            public MessageFunctionProperties message{ get; set; }

            /// <summary>
            /// Representing properties related to emotion handling function performed by the AI agent.
            /// </summary>
            public EmotionFunctionProperties emotion{ get; set; }
        }

        /// <summary>
        /// This class represents properties pertaining to the AI agent's message handling function.
        /// </summary>
        public class MessageFunctionProperties
        {
            /// <summary>
            /// The type of the function, which gives an indication of what kind of operation the function performs.
            /// </summary>
            public string type{ get; set; }

            /// <summary>
            /// A description of what the function does.
            /// </summary>
            public string description{ get; set; }
        }

        /// <summary>
        /// This class represents properties pertaining to the AI agent's emotion handling function.
        /// </summary>
        public class EmotionFunctionProperties
        {
            /// <summary>
            /// The type of the function, typically indicating the operational purpose of the function.
            /// </summary>
            public string type{ get; set; }

            /// <summary>
            /// A description of what the function does.
            /// </summary>
            public string description{ get; set; }
        
            /// <summary>
            /// Enumeration representing the possible values associated with the function.
            /// </summary>
            [JsonProperty("enum")]
            public List<string> enumerator { get; set; }
        }
    }
}