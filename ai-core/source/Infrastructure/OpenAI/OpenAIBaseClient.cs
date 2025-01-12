using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AICore.Infrastructure.OpenAI.Data;
using Newtonsoft.Json;
using AICore.Services.Extension;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;

namespace AICore.Infrastructure.OpenAI
{
    public partial class OpenAIClient : IAIGPTService, IAIImageService, IEmbeddingService
    {
        #region Properties

        private string _apiKey;
        private static readonly HttpClient _httpClient = new HttpClient();

        #endregion

        #region Methods

        public async Task<AIImageResponse> SendImageRequest(AIImageRequest value)
        {
            if (value == null) throw new Exception("[OpenAI] Data is null");
            var parameter = JsonConvert.SerializeObject(value);
            var response = await CreateRequest(OpenAIConstants.API_IMAGE_REQUEST, parameter);

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception("[OpenAI] " + response.Content);

            return JsonConvert.DeserializeObject<AIImageResponse>(response.Content);
        }

        public async Task<AITransformerResponse> SendTransformerRequest(AITransformerRequest value)
        {
            if (value == null) throw new Exception("[OpenAI] Data is null");

            var data = RequestToAPIInternal(value);
            var response = await CreateRequest(OpenAIConstants.API_CHAT_REQUEST, JsonConvert.SerializeObject(data));
            
            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception("[OpenAI] " + response.Content);

            var responseDataInternal = JsonConvert.DeserializeObject<AITransformerResponseInternal>(response.Content);

            return ResponseDataInternalToResponseData(responseDataInternal);
        }

        private async Task<(string Content, HttpStatusCode StatusCode)> CreateRequest(string requestPath, string jsonContent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, OpenAIConstants.API_URL + requestPath)
            {
                Content = new StringContent(jsonContent)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            
            try
            {
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                return (responseContent, response.StatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during HTTP request: " + e.Message);
                throw;
            }
        }

        private AITransformerRequestInternal RequestToAPIInternal(AITransformerRequest value)
        {
            var data = new AITransformerRequestInternal
            {
                max_tokens = value.max_tokens,
                temperature = value.temperature,
                top_p = value.top_p,
                n = value.n,
                stream = value.stream,
                presence_penalty = value.presence_penalty,
                frequency_penalty = value.frequency_penalty,
                user = value.user,
                model = ModelsExtension.GetString(value.model),
                tool_choice = value.tool_choice,
                response_format = value.response_format != null ? new ResponseFormatInternal { type = value.response_format.type, json_schema = value.response_format.json_schema } : null,
                tools = value.tools?.Select(it => new ToolInternal
                {
                    type = it.type,
                    function = new FunctionToolInternal
                    {
                        name = it.function.name,
                        description = it.function.description,
                        parameters = new ToolParameterInternal
                        {
                            type = it.function.parameters.type,
                            required = it.function.parameters.required,
                            properties = it.function.parameters.properties
                        }
                    }
                }).ToList(),
                messages = value.messages.Select(it => new AITransformerMessageInternal
                {
                    name = it.name,
                    role = RoleExtension.GetString(it.role),
                    content = it.content,
                    tool_call_id = string.IsNullOrEmpty(it.tool_call_id) ? null : it.tool_call_id,
                    tool_calls = it.tool_calls?.Select(subIt => new ToolChoiceInternal
                    {
                        id = subIt.id,
                        type = subIt.type,
                        function = new FunctionToolChoiceInternal
                        {
                            name = subIt.function.name,
                            arguments = subIt.function.arguments
                        }
                    }).ToList()
                }).ToList()
            };
            return data;
        }

        private AITransformerResponse ResponseDataInternalToResponseData(AITransformerResponseInternal value)
        {
            var data = new AITransformerResponse
            {
                usage = new UsageData
                {
                    completionTokens = value.usage.completion_tokens,
                    promptTokens = value.usage.prompt_tokens,
                    totalTokens = value.usage.total_tokens
                },
                generated = value.choices.Select(it => new GeneratedData
                {
                    message = new DataContent
                    {
                        role = RoleExtension.GetRole(it.message.role),
                        content = it.message.content,
                        toolCalls = it.message.tool_calls?.Select(tool => new ToolContent
                        {
                            id = tool.id,
                            type = tool.type,
                            function = tool.function != null ? new FuntionToolContent
                            {
                                name = tool.function.name,
                                arguments = tool.function.arguments
                            } : null
                        }).ToList()
                    },
                    finishReason = it.finish_reason
                }).ToList()
            };
            return data;
        }

        #endregion

        #region Setters

        public void SetSettings(AIServiceInitData value)
        {
            SetAPIKey(value.APIKey);
        }

        internal void SetAPIKey(string apiKey) => _apiKey = apiKey;

        #endregion

        public async Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value, AIModel model)
        {
            if (value == null) throw new Exception("[OpenAI] Data is null");
            if (string.IsNullOrEmpty(value.input)) throw new Exception("[OpenAI] Input is null or empty");

            var data = new AIEmbeddingRequestInternal
            {
                input = value.input,
                model = ModelsExtension.GetString(AIModel.embedding_ada_002)
            };

            var response = await CreateRequest(OpenAIConstants.API_EMBENDDING_REQUEST, JsonConvert.SerializeObject(data));

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception("[OpenAI] " + response.Content);

            try
            {
                return JsonConvert.DeserializeObject<AIEmbeddingResponse>(response.Content ?? throw new Exception("[OpenAI] Content is null in the response"));
            }
            catch (Exception e)
            {
                throw new Exception("Error deserializing the response: " + e.Message);
            }
        }
    }
}