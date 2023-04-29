using AIAgent.Types.Data;
using AIAgent.Types.Data.AIAgent.Types.Data;

namespace AIAgent.Language
{
    /// <summary>
    /// This interface defines the language and communication protocols associated with an AI agent.
    /// </summary>
    public interface IAgentLanguage
    {
        /// <summary>
        /// Defines the AI agent's context in a given environment or usage.
        /// </summary>
        /// <returns>A string containing the context text.</returns>
        string Context();

        /// <summary>
        /// Provides instructions to the agent using the agent's name.
        /// </summary>
        /// <param name="agentName">Name of the AI agent.</param>
        /// <returns>A string containing the instruction text.</returns>
        string Instructions(string agentName);

        /// <summary>
        /// Returns a message from the AI agent.
        /// </summary>
        /// <returns>A string containing the message text.</returns>
        string AgentMessage();

        /// <summary>
        /// Returns a message from the interlocutor, or the user interfacing with the agent.
        /// </summary>
        /// <returns>A string containing the message text.</returns>
        string InterlocutorMessage();

        /// <summary>
        /// Analyzes and returns the importance of an observation made by the AI agent.
        /// </summary>
        /// <returns>A string containing the observation importance data.</returns>
        string ObservationImportance();

        /// <summary>
        /// Provides an example of an observation importance.
        /// </summary>
        /// <returns>An instance of ExampleData, containing the observation importance example.</returns>
        AgentExampleData.ExampleData ObservationImportanceExample();

        /// <summary>
        /// Gives instructions for reflection, an internal process where the agent assesses its decision-making and behavior.
        /// </summary>
        /// <returns>A string containing the reflection instruction text.</returns>
        string ReflectionInstructions();

        /// <summary>
        /// Provides an example of reflection by the AI agent.
        /// </summary>
        /// <returns>An instance of ExampleData, containing the reflection example.</returns>
        AgentExampleData.ExampleData ReflectionExample();

        /// <summary>
        /// Represents a function or operation performed by the agent.
        /// </summary>
        /// <returns>A dynamic object representing the agent's function.</returns>
        dynamic AgentFunction();

        /// <summary>
        /// Returns a string representation of the AI agent's current emotion state.
        /// </summary>
        /// <param name="emotion">An enum of AgentEmotion representing the agent's current emotion.</param>
        /// <returns>A string representing the current emotional state of the agent.</returns>
        string AgentEmotion(AgentEmotion emotion);
    }
}