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

namespace AICore.Infrastructure.OpenAI
{
    public class OpenAIClient : IAIGPTService, IAIImageService
    {
        #region Properties

        private string _apiKey;
        private AIModel _model = AIModel.none;

        #endregion

        #region Methods

        public async Task<AIEmbeddingResponse> SendEmbeddingRequest(AIEmbeddingRequest value)
        {
            if (value == null) throw new Exception("[OpenAI] Data is null");
            if(string.IsNullOrEmpty(value.input)) throw new Exception("[OpenAI] Input is null or empty");

            var data = new AIEmbeddingRequestInternal
            {
                input = value.input,
                model = ModelsExtension.GetString(value.model == AIModel.none ? _model : value.model)
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
                model = ModelsExtension.GetString(value.model == AIModel.none ? _model : value.model),
                function_call = value.function_call,
                functions = value.functions?.Select(it => new FunctionInternal
                    {
                        name = it.name,
                        description = it.description,
                        parameters = new FunctionParameterInternal
                        {
                            type = it.parameters.type,
                            required = it.parameters.required,
                            properties = it.parameters.properties
                        }
                    }
                ).ToList(),
                messages = value.messages.Select(it => new AITransformerMessageInternal
                {
                    role = RoleExtension.GetString(it.role),
                    content = it.content,
                    function_call = it.function_call != null ? new FunctionCallInternal
                    {
                        name = it.function_call.name,
                        arguments = it.function_call.arguments
                    } : null
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
                        functionCall = it.message.function_call != null ? new FunctionContent
                        {
                            name = it.message.function_call.name,
                            arguments = it.message.function_call.arguments
                        } : null
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
            SetModel(value.Model);
        }
        
        internal void SetAPIKey(string apiKey) => _apiKey = apiKey;
        
        internal void SetModel(AIModel model) => _model = model;

        #endregion
}
}

