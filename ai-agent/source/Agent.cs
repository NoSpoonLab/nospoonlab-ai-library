using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIAgent.Language;
using AIAgent.Types.Data;
using AIAgent.Types.Data.AIAgent.Types.Data;
using AIAgent.Types.Data.Prompts;
using AICore.Infrastructure.NoSpoonAI;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Utils;
using DependencyInjectionCore;
using Newtonsoft.Json;

namespace AIAgent
{
    /// <summary>
    /// The main class for conversational agents that use NoSpoonAIClient and IAGPTService to be a smart agent.
    /// </summary>
    public class Agent : Injectable
    { 
        #region Properties

        /// <summary>
        /// Name of the agent.
        /// </summary>
        private string _agentName;

        /// <summary>
        /// Represents the language that the agent will "speak".
        /// </summary>
        private IAgentLanguage _agentLanguage;

        /// <summary>
        /// A list of messages that the agent has received or sent.
        /// </summary>
        private List<DataMessage> _conversationMessages;

        /// <summary>
        /// A memory bank for the agent, containing instances of previous observations and interactions.
        /// </summary>
        private readonly List<AgentMemory> _agentMemories;
        
        /// <summary>
        /// Client instance to communicate with AI API.
        /// </summary>
        #pragma warning disable CS0649
        [Inject] private NoSpoonAIClient _noSpoonAIClient;
        #pragma warning restore CS0649
        
        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the Agent class.
        /// </summary>
        public Agent()
        {
            _agentMemories = new List<AgentMemory>();
            _conversationMessages = new List<DataMessage>();
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Initializes the agent instance with a name and a locale for the language.
        /// </summary>
        /// <param name="agentName">The name of the agent.</param>
        /// <param name="locale">The locale represents the language the agent will use (currently supports "en" for English and "es" for Spanish).</param>
        public void Initialize(string agentName, string locale)
        {
            _agentName = agentName;
            _agentLanguage = locale == "en" ? (IAgentLanguage) new EnglishAgent() : new SpanishAgent();
        }

        /// <summary>
        /// Processes a message from a client, interacting with the NLP API and the agent's memory.
        /// </summary>
        /// <param name="message">The user's input message to the agent.</param>
        /// <param name="user">The user id, if needed for specific implementations.</param>
        /// <returns>A response from the agent, including its message and emotion state.</returns>
        public IEnumerator<AgentResponse> IEnumeratorRequestAgentMessage(string message, string user = "")
        {
            var task = RequestAgentMessage(message, user);
            while (!task.IsCompleted) yield return null;
            yield return task.Result;
        }

        /// <summary>
        /// Request an agent's response to a given message.
        /// </summary>
        /// <param name="message">The incoming message from the user.</param>
        /// <param name="user">The user ID, useful for agent personalization (default is empty).</param>
        /// <returns>A task producing the agent's response.</returns>
        public async Task<AgentResponse> RequestAgentMessage(string message, string user = "")
        {
            // Initialization
            var messages = new List<DataMessage>();

            // Process the message into an embedding
            var messageEmbedding = await GetObservationEmbedding(message);

            // Check the agent's memory limit
            await CheckMemoryAgentLimit(message, messageEmbedding);

            // Retrieve the most relevant memories related to the message
            var memories = MemoryRetrieval(messageEmbedding);

            // Setup system
            var system = string.Empty;
            
            // Construct the agent's context
            var context = memories.Aggregate(_agentLanguage.Context(), (current, memory) => current + (memory.Data + ";\n"));
            system += context;
            
            // Setup the agent's instructions 
            var instructions = _agentLanguage.Instructions(_agentName);
            system += instructions;
            
            // Setup agent prompt
            messages.Add(new DataMessage { role = AIRole.System, content = system });
            _conversationMessages.ForEach(it => messages.Add(it));
            messages.Add(new DataMessage { role = AIRole.User, content = message });

            // Send request to AI API
            var request = new AgentFunctionRequestPrompt(messages, _agentName, _agentLanguage);
            var response = await _noSpoonAIClient.SendTransformerRequest<AgentResponse, AgentFunctionRequestPrompt>(request);

            // Process the AI's response
            var functionResponse = response;

            // Check if the response is empty
            if (string.IsNullOrEmpty(functionResponse.message)) return functionResponse;
            
            // Add the response to the conversation so the agent is aware of it
            messages.Add(new DataMessage { role = AIRole.Assistant, content = null, function_call = new FunctionCall{ name = "send_message_to_user", arguments = JsonConvert.SerializeObject(new AgentResponse{ message = functionResponse.message, emotion = functionResponse.emotion }) }});

            // Update the conversation
            _conversationMessages = messages.FindAll(it => it.role != AIRole.System);

            // Return agent's response to the input message
            return functionResponse;
        }

        /// <summary>
        /// Add a new memory to the agent.
        /// </summary>
        /// <param name="observation">The observation to be remembered.</param>
        public async Task AddMemory(string observation)
        {
            float importance;

            try
            {
                // Attempt to assign the importance of this observation
                importance = await GetObservationImportance(observation);
            }
            catch
            {
                // If unsuccessful, assign a predefined importance
                importance = 8;
            }

            // Push this memory into the agent's memory
            await AddMemory(observation, importance);
        }

        /// <summary>
        /// Adds a new memory to the agent with a specific importance score.
        /// </summary>
        /// <param name="observation">The observation to be remembered.</param>
        /// <param name="score">The importance score of this observation.</param>
        public async Task AddMemory(string observation, float score)
        {
            // Process the observation into an embedding
            var embedding = await GetObservationEmbedding(observation);
            
            // Create a new memory with this observation and add it to the agent's memory
            _agentMemories.Add(new AgentMemory
            {
                Id = _agentMemories.Count,
                Data = observation,
                Embedding = embedding,
                LastVisitedDate = DateTime.Now,
                Importance = score
            });
        }
        
        /// <summary>
        /// Retrieves the most relevant memories based on a provided observation.
        /// </summary>
        /// <param name="observation">The observation to base the retrieval upon.</param>
        /// <param name="memoryCount">The maximum number of memories to retrieve (default is MAX_MEMORIES_RETRIEVAL).</param>
        /// <returns>A list of the most relevant memories.</returns>
        public List<AgentMemory> MemoryRetrieval(float[] observation, int memoryCount = AgentConstants.MAX_MEMORIES_RETRIEVAL)
        {
            // Create a filtered list of memories
            var filteredMemories = new List<AgentMemory>(_agentMemories);

            // Order memories by relevance to observation, and limit the amount returned
            return filteredMemories.OrderByDescending(x => x.CalculateTotalScore(observation)).Take(memoryCount).ToList();
        }        
        /// <summary>
        /// Clears the agent's recorded conversation messages.
        /// </summary>
        public void ClearConversationMessages() => _conversationMessages.Clear();
        
        /// <summary>
        /// Clears the agent's memory.
        /// </summary>
        public void ClearAgentMemories() => _agentMemories.Clear();
        
        /// <summary>
        /// Resets the agent's state, clearing all messages and memories and resetting the name. 
        /// </summary>
        public void ResetAgent()
        {
            ClearConversationMessages();
            ClearAgentMemories();
            _agentName = string.Empty;
        }

        /// <summary>
        /// Retrieves thoughts of the agent in regard to its conversation messages.
        /// </summary>
        /// <returns>A task that when completed will return the thoughts of the agent.</returns>
        public async Task<List<string>> GetReflectAgentThoughts()
        {
            var conversation = _conversationMessages.FindAll(it => it.role != AIRole.System);
            var filteredConversation = conversation.Aggregate("", (current, memory) => current + $"{memory.role.ToString()}: {memory.content}\n");
            
            var request = new AgentReflectionThoughtsRequestPrompt(
                filteredConversation,
                _agentLanguage.ReflectionInstructions(),
                new NoSpoonAIExampleData
                {
                    Input = _agentLanguage.ReflectionExample().user,
                    Output = _agentLanguage.ReflectionExample().assitant
                });
            
            // Send the reflection request to the AI API and return the result
            return await _noSpoonAIClient.SendTransformerRequest<List<string>, AgentReflectionThoughtsRequestPrompt>(request);
        }

        /// <summary>
        /// Reflects the agent's thoughts and adds them to its memory.
        /// </summary>
        public async Task ReflectAgentThoughts()
        {
            // Get the agent's thoughts
            var reflectionMessage = await GetReflectAgentThoughts();
            
            // Iterate over the thoughts and add them to memory
            foreach (var it in reflectionMessage)
            {
                await AddMemory(it, 9);
            }
        }
        #endregion
        
        #region Getters

        /// <summary>
        /// Gets the name of the agent.
        /// </summary>
        /// <returns>The agent's name.</returns>
        public string GetAgentName() => _agentName;
        
        /// <summary>
        /// Converts an observation into an embedding array.
        /// </summary>
        /// <param name="observation">The observation to be converted.</param>
        /// <returns>A task that when completed will return the embedding for the provided observation.</returns>
        public async Task<float[]> GetObservationEmbedding(string observation)
        {
            return (await _noSpoonAIClient.SendEmbeddingRequest(new NoSpoonAIEmbeddingRequest(observation))).data.First().embedding;
        }
        
        /// <summary>
        /// Gets the importance of an observation.
        /// </summary>
        /// <param name="observation">The observation to get the importance for.</param>
        /// <returns>A task that when completed will return the importance of the observation.</returns>
        public async Task<float> GetObservationImportance(string observation)
        {
            var request = new AgentObservationImportanceRequestPrompt(
                observation,
                _agentLanguage.ObservationImportance(),
                new NoSpoonAIExampleData
                {
                    Input = _agentLanguage.ObservationImportanceExample().user,
                    Output = _agentLanguage.ObservationImportanceExample().assitant
                }
            );
            
            // Send the request to the API and return the response
            var response = await _noSpoonAIClient.SendTransformerRequest<AgentMemoryImportance, AgentObservationImportanceRequestPrompt>(request);
            return response.score;
        }
        #endregion;

        #region Private Methods
        /// <summary>
        /// Checks if the memory limit of the agent has been reached, and if so, manages the way memories are stored.
        /// </summary>
        /// <param name="prompt">The incoming prompt/message from the user.</param>
        /// <param name="messageEmbedding">The processed message embedding.</param>
        private async Task CheckMemoryAgentLimit(string prompt, float [] messageEmbedding)
        {
            var agentMemories = MemoryRetrieval(messageEmbedding);
            var estimatedTokenCostForMemory = agentMemories.Select(it => it.TokenCount).Sum();
            var estimatedTokenCostForConversation = TokenUtils.Tokenizer(_conversationMessages.Select(it => it.content).ToList().Aggregate("", (current, memory) => current + memory));
            var estimatedTokenCostForPrompt = TokenUtils.Tokenizer(prompt);
            var estimatedTokenCost = (int)((estimatedTokenCostForMemory + 
                                       estimatedTokenCostForPrompt +
                                       estimatedTokenCostForConversation + 
                                       AgentConstants.MAX_TOKENS_ON_COMPLETION) * 1.15f);
            
            //In case the estimated token cost is lower than the max tokens on request, we return
            if (estimatedTokenCost <= AgentConstants.MAX_TOKENS_ON_REQUEST) return;
            
            //In case the estimated token cost is higher than the max tokens on request, we need to clean the list
            //Get the conversation messages 
            var conversationMessages = _conversationMessages.Where(it => it.role == AIRole.User || it.role == AIRole.Assistant).ToList();
            
            //Get the last MIN_MESSAGE_ON_CLEAN messages
            var lastMessages = conversationMessages.Skip(Math.Max(0, conversationMessages.Count() - AgentConstants.MIN_MESSAGE_ON_CLEAN)).ToList();
            
            //Remove the last MIN_MESSAGE_ON_CLEAN messages from the conversation messages
            conversationMessages = conversationMessages.Where(it => !lastMessages.Contains(it)).ToList();
            
            //Add to memory the message that we are going to remove)
            foreach (var it in conversationMessages)
            {
                var message = it.role == AIRole.User ? _agentLanguage.InterlocutorMessage() : _agentLanguage.AgentMessage();
                message+= " " + it.content;
                await AddMemory(message);
            }

            await ReflectAgentThoughts();
            //Remove the messages from the conversation messages
            _conversationMessages.RemoveAll(it => conversationMessages.Contains(it));
        }

        #endregion
    }
}