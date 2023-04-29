using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AIAgent.Components;
using AIAgent.Language;
using AIAgent.Types;
using AIAgent.Types.Data;
using AIAgent.Types.Data.AIAgent.Types.Data;
using AIAgent.Types.Data.Prompts;
using AICore.Infrastructure.NoSpoonAI;
using AICore.Infrastructure.NoSpoonAI.Types.Data;
using AICore.Infrastructure.NoSpoonAI.Types.Request;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace AIAgent
{
    /// <summary>
    /// The main class for conversational agents that use NoSpoonAIClient and IAGPTService to be a smart agent.
    /// </summary>
    public class Agent
    { 
        #region Properties

        /// <summary>
        /// Name of the agent.
        /// </summary>
        private string _agentName;
        private string _agentDescription;
        private int _agentVerbosity;
        private int _agentToxicity;
        private int _agentHumor;
        private int _agentCreativity;
        private int _agentHelpfulness;
        private string _lastContext;
        private string _lastConversation;
        

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
        /// The agent's planner, which is responsible for planning the agent's actions.
        /// </summary>
        private readonly AgentPlanner _agentPlanner;
        
        /// <summary>
        /// Client instance to communicate with AI API.
        /// </summary>
        #pragma warning disable CS0649
        private NoSpoonAIClient _noSpoonAIClient;
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
            _noSpoonAIClient = new NoSpoonAIClient();
        }
        
        public Agent(GameObject agentInstance) : this()
        {
            _agentPlanner = new AgentPlanner(agentInstance);
        }

        #endregion

        #region Lifecycle
        
        public void LateUpdate()
        {
            _agentPlanner?.LateUpdate();
        }
        

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Initializes the agent instance with a name and a locale for the language.
        /// </summary>
        /// <param name="agentName">The name of the agent.</param>
        /// <param name="locale">The locale represents the language the agent will use (currently supports "en" for English and "es" for Spanish).</param>
        public void Initialize(string agentName, string locale, string agentDescription, int agentVerbosity, int agentToxicity, int agentHumor, int agentCreativity, int agentHelpfulness)
        {
            _noSpoonAIClient.Initialize();
            _agentName = agentName;
            _agentLanguage = locale == "en" ? (IAgentLanguage) new EnglishAgent() : new SpanishAgent();
            _agentDescription = agentDescription;
            _agentVerbosity = agentVerbosity;
            _agentToxicity = agentToxicity;
            _agentHumor = agentHumor;
            _agentCreativity = agentCreativity;
            _agentHelpfulness = agentHelpfulness;

            if (_agentPlanner == null) return;
            _agentPlanner.AddGoal(new AgentGoal("Eat",1, false), 1);

            if (_agentName.Contains("Ana"))
            {
                _agentPlanner.AddAction(new AgentAction("GoToYard", new State(), new State("VisitYard", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Mansion", Room = "Yard", ObjectName = "Bench 1"}));
                _agentPlanner.AddAction(new AgentAction("VisitGrave", new State("VisitYard", 1), new State("VisitGrave", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Mansion", Room = "Lounge", ObjectName = "Corpse of Carlos Salina"}));
                _agentPlanner.AddAction(new AgentAction("GoToRest", new State("VisitGrave", 1), new State("Rest", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Mansion", Room = "Room of Ana Salinas", ObjectName = "Bed"}));
                _agentPlanner.AddAction(new AgentAction("PickIngredients", new State("Rest", 1), new State("PickIngredients", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Mansion", Room = "Kitchen", ObjectName = "Fridge"}));
                _agentPlanner.AddAction(new AgentAction("Cooking", new State("PickIngredients", 1), new State("Cook", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Mansion", Room = "Kitchen", ObjectName = "Kitchen Stoves"}));
                _agentPlanner.AddAction(new AgentAction("Eating", new State("Cook", 1), new State("Eat", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Mansion", Room = "Kitchen", ObjectName = "Dining Table"}));

            }
            else
            {
                _agentPlanner.AddAction(new AgentAction("GoToYard", new State(), new State("VisitPolice", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Police", Room = "Room", ObjectName = "Point"}));
                _agentPlanner.AddAction(new AgentAction("GoToYard", new State("VisitPolice", 1), new State("Eat", 1), 5, 3, new AgentWorldObjectMemory{ Zone = "Office", Room = "Room", ObjectName = "Point"}));

            }
        }

        public async void CreateAgentPlan()
        {
            // Setup agent prompt
            var messages = new List<DataMessage>();
            var system = string.Empty;
            system += "Tu tarea consiste en crear una lista de actividades para el " + _agentName + ".\n";
            system += "Estas actividades dependerán de las observaciones que el " + _agentName + " ha vivido, de sus conversaciones y de los objetos que están disponibles, los cuales están directamente vinculados con la actividad que se puede hacer, no se puede crear una actividad que no este directamente vinculada a un objeto existente\n";
            system += " Observaciones:  \n";
            system += _agentMemories.Aggregate(_agentLanguage.Context(), (current, memory) => current + (memory.Data + ";\n"));
            system += " \nObjetos:  \n";
            system += WorldObject.FindAllObjectsInWorld().Aggregate(string.Empty, (current, obj) => current + ("Object: " + obj.ObjectName + " location: " + obj.Zone + ", " + obj.Room  + " locationId: " + obj.Id + " ;\n"));
            messages.Add(new DataMessage { role = AIRole.System, content = system });
            
            var userExample = "Genera una lista de actividades para Ana Salinas" + ".";
            var agentExample = "[\n";
            agentExample += "  {\n";
            agentExample += "    \"action\": \"ir a la policia\",\n";
            agentExample += "    \"locationId\": 190032263,\n";
            agentExample += "  },\n";
            agentExample += "  {\n";
            agentExample += "    \"action\": \"Descansar en cama\",\n";
            agentExample += "    \"locationId\": 1285344057,\n";
            agentExample += "  },\n";
            agentExample += "  {\n";
            agentExample += "    \"action\": \"Comer en la cocina\",\n";
            agentExample += "    \"locationId\": -165029824,\n";
            agentExample += "  },\n";
            messages.Add(new DataMessage { role = AIRole.User, content = userExample });
            messages.Add(new DataMessage { role = AIRole.Assistant, content = agentExample });
            messages.Add(new DataMessage { role = AIRole.User, content = "Generar una lista de actividades para " + _agentName + "." });
            
            //Debug.Log(system);
            var request = new AgentPlanningRequestPrompt(messages, _agentName);
            var response = await _noSpoonAIClient.SendTransformerRequest(request);
            //Debug.Log("Agent plan: " + response);
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
            var messageEmbedding = await GetEmbedding(message);

            // Check the agent's memory limit
            await CheckMemoryAgentLimit(message, messageEmbedding);

            // Retrieve the most relevant memories related to the message
            //Debug.LogWarning("Begin MemoryRetrieval");
            var memories = MemoryRetrieval(messageEmbedding);
            //Debug.Log("Memories: " + memories.Count + " Memories Raw: " + _agentMemories.Count);

            // Setup system
            var system = string.Empty;
            
            // Construct the agent's context
            var context = memories.Aggregate(_agentLanguage.Context(), (current, memory) => current + (memory.Data + "\n"));
            system += context;
            
            // Setup the agent description
            var description = _agentLanguage.Description(_agentDescription, _agentToxicity, _agentHumor, _agentHelpfulness);
            system += description;
            
            // Setup the agent's instructions 
            var instructions = _agentLanguage.Instructions(_agentName);
            system += instructions;
            
            // Setup agent prompt
            messages.Add(new DataMessage { role = AIRole.System, name = _agentName.Replace(" ","_"), content = system });
            _conversationMessages.ForEach(it => messages.Add(it));
            messages.Add(new DataMessage { role = AIRole.User, name = user.Replace(" ","_"), content = message });
            
            
            _lastContext = system;
            _lastConversation = JsonConvert.SerializeObject(messages.FindAll(it => it.role != AIRole.System));

            // Send request to AI API
            var request = new AgentFunctionRequestPrompt(messages, _agentName, _agentLanguage, _agentVerbosity, _agentCreativity);
            var response = await _noSpoonAIClient.SendTransformerRequest<AgentResponse, AgentFunctionRequestPrompt>(request);

            // Process the AI's response
            var functionResponse = response ?? throw new ArgumentNullException(nameof(response));

            // Check if the response is empty
            if (string.IsNullOrEmpty(functionResponse.message)) return functionResponse;
            
            // Add the response to the conversation so the agent is aware of it
            //name = "send_message_to_user", arguments = JsonConvert.SerializeObject(new AgentResponse{ message = functionResponse.message, emotion = functionResponse.emotion }) }
            var toolName = "send_message_to_user";
            messages.Add(new DataMessage
            {
                role = AIRole.Assistant, 
                content = null, 
                name = _agentName.Replace(" ","_"), 
                tool_calls = new List<ToolChoice>
                {
                    new ToolChoice
                    {
                        id = request.GetToolId(toolName),
                        type = "function",
                        function = new FunctionToolChoice
                        {
                            name = toolName,
                            arguments = JsonConvert.SerializeObject(new AgentResponse{ message = functionResponse.message, emotion = functionResponse.emotion })
                        }
                    }
                }
            });
            
            messages.Add(new DataMessage
            {
                role = AIRole.Tool,
                tool_call_id = request.GetToolId(toolName),
                content = JsonConvert.SerializeObject(new AgentResponse{ message = functionResponse.message, emotion = functionResponse.emotion })
            });

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
            await AddMemory(observation, _agentMemories.Count, score);
        }

        private async Task<Dictionary<string, float[]>> GetKeywords(string value)
        {
            var matches = Regex.Matches(value, @"\b\w+\b");
            var keywords = (from Match match in matches select match.Value).ToList();
            var keywordsEmbedding = new Dictionary<string, float[]>();
            var task = keywords.Select(it => Task.Run(async () =>
                {
                    var embeddingTask = await GetEmbedding(it);
                    if(keywordsEmbedding.ContainsKey(it)) return;
                    keywordsEmbedding.Add(it, embeddingTask);
                }))
                .ToList();
            await Task.WhenAll(task);
            return keywordsEmbedding;
        }
        
        public async Task AddMemory(string observation, int id, float score)
        {
            // Process the observation into an embedding
            var embedding = await GetEmbedding(observation);
            
            //Get the keywords of the observation
            /*var keywordsEmbedding = await GetKeywords(observation);
            var keywordsCount = keywordsEmbedding.Count;
            keywordsEmbedding = keywordsEmbedding.OrderBy(it => MathUtils.CalculateCosineSimilarity(embedding, it.Value)).ToDictionary(it => it.Key, it => it.Value);
            keywordsEmbedding = keywordsEmbedding.Take(keywordsCount >= 5 ? 5 : keywordsCount).ToDictionary(it => it.Key, it => it.Value);*/
            //var keywordsEmbedding = new Dictionary<string, float[]>();
           /* var task = GetKeywords(observation);
            task.Start(TaskScheduler.FromCurrentSynchronizationContext());
            var keywordsEmbedding = task.Result;
            var keywordsCount = keywordsEmbedding.Count;
            keywordsEmbedding = keywordsEmbedding.OrderBy(it => MathUtils.CalculateCosineSimilarity(embedding, it.Value)).ToDictionary(it => it.Key, it => it.Value);
            keywordsEmbedding = keywordsEmbedding.Take(keywordsCount >= 5 ? 5 : keywordsCount).ToDictionary(it => it.Key, it => it.Value);
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();*/
            
            

            // Create a new memory with this observation and add it to the agent's memory
            _agentMemories.Add(new AgentMemory
            {
                Id = id,
                Data = observation,
                Embedding = embedding,
                LastVisitedDate = DateTime.Now,
                keywordEmbeddings = new Dictionary<string, float[]>(),
                Importance = score
            });
        }
        
        public string GetLastContext()
        {
            return _lastContext;
        }
        
        public string GetLastConversation()
        {
            return _lastConversation;
        }

        public async void RemoveMemory(int id)
        {
            //Check with prompt
            var memory = _agentMemories.Find(it => it.Id == id);
            if (memory == null) return;

            try
            {
                var memoryData = _conversationMessages
                    .FindAll(it => it.role == AIRole.Tool)
                    .Select(it => JsonConvert.DeserializeObject<AgentResponse>(it.content).message)
                    .ToList();


                var request = new AgentCleanHistoryChatRequestPrompt(memory.Data, memoryData);
                var positions =
                    await _noSpoonAIClient.SendTransformerRequest<List<int>, AgentCleanHistoryChatRequestPrompt>(
                        request);

                var memoryToRemove = positions
                    .FindAll(it => it >= 0 && it < _conversationMessages.Count)
                    .Select(it => memoryData[it])
                    .ToList();

                _conversationMessages.RemoveAll(it =>
                {
                    string content = "";
                    if (it.role == AIRole.User) return false;
                    if (it.role == AIRole.System) return false;
                    if (it.role == AIRole.Assistant)
                    {
                        content = JsonConvert.DeserializeObject<AgentResponse>(it.content).message;
                    }
                    else if (it.role == AIRole.Tool)
                    {
                        content = JsonConvert.DeserializeObject<AgentResponse>(it.tool_calls.First().function.arguments)
                            .message;
                    }

                    return memoryToRemove.Contains(content);

                });
            }
            catch (Exception e)
            {
                //Debug.Log(e.Message);
            }
            
            _agentMemories.RemoveAll(it => it.Id == id);
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
            var conversation = _conversationMessages.FindAll(it => it.role != AIRole.System && it.role != AIRole.Assistant);
            var filteredConversation = conversation.Aggregate("", (current, memory) =>
            {
                var role = memory.role == AIRole.User ? _agentLanguage.Interlocutor() : _agentLanguage.Agent();
                var content = memory.role != AIRole.User ? JsonConvert.DeserializeObject<AgentResponse>(memory.content).message : memory.content;
                return current + $"{role}: {content}\n";
            });
            
            var request = new AgentReflectionThoughtsRequestPrompt(
                filteredConversation,
                _agentLanguage.ReflectionInstructions(),
                new NoSpoonAIExampleData
                {
                    Input = _agentLanguage.ReflectionExample().user,
                    Output = _agentLanguage.ReflectionExample().assitant
                });
            
            // Send the reflection request to the AI API and return the result
            var remoteRequest = await _noSpoonAIClient
                .SendTransformerRequest<NoSpoonAIListData<string>, AgentReflectionThoughtsRequestPrompt>(request);
            return remoteRequest.GetData();
        }

        /// <summary>
        /// Reflects the agent's thoughts and adds them to its memory.
        /// </summary>
        public async Task ReflectAgentThoughts()
        {
            // Get the agent's thoughts
            List<string> reflectionMessage = null;
            try
            {
                reflectionMessage = await GetReflectAgentThoughts();
            }
            catch (Exception e)
            {
                reflectionMessage = new List<string>();
            }
            
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
        public async Task<float[]> GetEmbedding(string observation)
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
            //TODO remove the comment on reflect agent thoughts
            //await ReflectAgentThoughts();
            
            //Remove the messages from the conversation messages
            _conversationMessages.RemoveAll(it => conversationMessages.Contains(it));
            
            //Check if there is any tool message in the second message alone

            var toDelete = new List<int>();
            for (var x = 0; x < _conversationMessages.Count; x++)
            {
                var message = _conversationMessages[x];
                if (message.role == AIRole.Tool && x == 0)
                {
                    toDelete.Add(x);
                    continue;
                }
                if (message.role != AIRole.Tool) continue;
                var previousMessage = _conversationMessages[x - 1];
                if(previousMessage.role == AIRole.Assistant) continue;
                toDelete.Add(x);
            }
            
            toDelete.ForEach(it => _conversationMessages.RemoveAt(it));
        }

        public void ClearLastMessage()
        {
            //Remove the last 3 message from _conversationMessages
            _conversationMessages.RemoveRange(_conversationMessages.Count - 3, 3);
        }

        #endregion
    }
}