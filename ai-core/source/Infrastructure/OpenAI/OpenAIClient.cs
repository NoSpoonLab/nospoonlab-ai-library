using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AICore.Services.Extension;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.Json; 
using UnityEngine;

namespace AICore.Infrastructure.OpenAI
{
    public class OpenAIClient : IAIGPTService, IAIImageService, IEmbedding
    {
        #region Properties

        private string _apiKey;

        #endregion

        #region Methods

        public async Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value)
        {
            if (value == null) throw new Exception("[OpenAI] Data is null");
            if(string.IsNullOrEmpty(value.input)) throw new Exception("[OpenAI] Input is null or empty");

            var data = new AIEmbeddingRequestInternal
            {
                input = value.input,
                model = ModelsExtension.GetString(AIModel.embedding_ada_002)
            };
            
            var parameter = new List<string> { JsonConvert.SerializeObject(data) };
            var response = await CreateRequest(OpenAIConstants.API_EMBENDDING_REQUEST, parameter);
            
            if (response.StatusCode >= HttpStatusCode.BadRequest) throw new Exception("[OpenAI] " + response.Content);
            try
            {
                return JsonConvert.DeserializeObject<AIEmbeddingResponse>(response.Content ?? throw new Exception("[OpenAI] Content is null in the response: " + response));
            }
            catch(Exception e)
            { 
                throw new Exception("Error deserializing the response: " + e.Message);
            }
        }


        public async Task<AIImageResponse> SendImageRequest(AIImageRequest value)
        {
            if (value == null) throw new Exception("[OpenAI] Data is null");
            var parameter = new List<string>();
            parameter.Add( JsonConvert.SerializeObject(value));
            var response = await CreateRequest(OpenAIConstants.API_IMAGE_REQUEST, parameter);
            if (response.StatusCode >= HttpStatusCode.BadRequest) throw new Exception("[OpenAI] " + response.Content);
            return JsonConvert.DeserializeObject<AIImageResponse>(response.Content);
        }

        public async Task<AITransformerResponse> SendTransformerRequest(AITransformerRequest value)
        {
            if (value == null) throw new Exception("[OpenAI] Data is null");
            var data = RequestToAPIInternal(value);
            var response = await CreateRequest(OpenAIConstants.API_CHAT_REQUEST, new List<string>(), data);
            if (response.StatusCode >= HttpStatusCode.BadRequest) throw new Exception("[OpenAI] " + response.Content);
            Debug.Log("Request: " + response.Request.Parameters.Aggregate("", (current, it) => current + it.Value + "\n"));
            Debug.Log("Response: " + response.Content);
            var responseDataInternal = JsonConvert.DeserializeObject<AITransformerResponseInternal>(response.Content);
            return ResponseDataInternalToResponseData(responseDataInternal);
        }

        private async Task<RestResponse> CreateRequest(string requestPath, List<string> parameter, object data = null)
        {
            var client = new RestClient(OpenAIConstants.API_URL);
            client.UseSystemTextJson();
            var request = new RestRequest(requestPath, Method.Post);
            request.AddHeader("Authorization", "Bearer " + _apiKey);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            if (data != null)
            {
                var jsonBody = JsonConvert.SerializeObject(data);
                Debug.Log("Body: " + jsonBody);
                request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            }
            
            parameter.ForEach( it => request.AddParameter("application/json", it, ParameterType.RequestBody));
            var result = await client.ExecuteAsync(request);
            try
            {
                if (result.Content == null || result.Content.Contains("\"type\": \"server_error\"") || result.StatusCode == HttpStatusCode.InternalServerError)
                {
                    await Task.Delay(150);
                    return await CreateRequest(requestPath, parameter, data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ErrorCode: " + result.StatusCode + " " + e.Message);
                throw;
            }
            
            return result;
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
                response_format = value.response_format != null ? new ResponseFormatInternal{type = value.response_format.type } : null,
                tools = value.tools?.Select(it => new ToolInternal
                    {
                        type = it.type,
                        function = new FuntionToolInternal
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
                    }
                ).ToList(),
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
                }
                ).ToList()
                
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
}
}

