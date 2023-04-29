using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AICore.Infrastructure.GeminiAI.Data;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;
using Newtonsoft.Json;

namespace AICore.Infrastructure.GeminiAI
{
    public class GoogleAIClient : IAIGPTService
    {
        #region Properties

        private string _clientId;
        private string _clientSecret;
        private string _refreshToken;
        private string _accessToken;
        private string _region = "us-east4";
        private string _project = "aerial-day-185810";
        private static readonly HttpClient _httpClient = new HttpClient();

        #endregion

        #region Methods

        private async Task Authenticate()
        {
            if (string.IsNullOrEmpty(_clientId))
                _clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
            if (string.IsNullOrEmpty(_clientSecret))
                _clientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
            if (string.IsNullOrEmpty(_refreshToken))
                _refreshToken = Environment.GetEnvironmentVariable("GOOGLE_REFRESH_TOKEN");
            if (!string.IsNullOrEmpty(_accessToken))
                return;

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.google.com/o/oauth2/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("refresh_token", _refreshToken),
            });

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode >= System.Net.HttpStatusCode.BadRequest)
                throw new Exception(responseContent);

            _accessToken = JsonConvert.DeserializeObject<GoogleAIAuthTokenRequest>(responseContent).access_token;
        }

        public async Task<AITransformerResponse> SendTransformerRequest(AITransformerRequest value)
        {
            await Authenticate();

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_region}-aiplatform.googleapis.com/v1/projects/{_project}/locations/{_region}/publishers/google/models/gemini-pro:streamGenerateContent")
            {
                Content = new StringContent(JsonConvert.SerializeObject(TransformBaseRequest(value)))
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode >= System.Net.HttpStatusCode.BadRequest)
                throw new Exception(responseContent);

            var resultData = JsonConvert.DeserializeObject<GoogleAIDataTransformResponse[]>(responseContent);
            return TransformBaseResponse(resultData[0]);
        }

        private GoogleAIDataTransformRequest TransformBaseRequest(AITransformerRequest value)
        {
            return new GoogleAIDataTransformRequest
            {
                contents = value.messages.ConvertAll(it => new GoogleAIDataContent
                {
                    role = GetRole(it.role),
                    parts = new[] { new GoogleAIDataParts { text = it.content } }
                }).ToArray(),
                generation_config = new GoogleAIDataGenerationConfig
                {
                    temperature = value.temperature,
                    topP = value.top_p,
                    topK = 40,
                    maxOutputTokens = value.max_tokens,
                    stopSequences = new[] { ".", "?", "!" }
                },
                safety_settings = new GoogleAIDataSafetySettings
                {
                    category = "HARM_CATEGORY_SEXUALLY_EXPLICIT",
                    threshold = "BLOCK_LOW_AND_ABOVE"
                }
            };
        }

        private AITransformerResponse TransformBaseResponse(GoogleAIDataTransformResponse value)
        {
            return new AITransformerResponse
            {
                usage = new UsageData
                {
                    promptTokens = value.usageMetadata.promptTokenCount,
                    completionTokens = value.usageMetadata.candidatesTokenCount,
                    totalTokens = value.usageMetadata.totalTokenCount
                },
                data = value.candidates[0].content,
                generated = value.candidates.ConvertAll(it => new GeneratedData
                {
                    message = new DataContent
                    {
                        content = it.content.parts[0].text,
                        role = GetRole(it.content.role)
                    },
                    finishReason = it.finishReason
                })
            };
        }

        private string GetRole(AIRole role)
        {
            switch (role)
            {
                case AIRole.User:
                    return "user";
                case AIRole.System:
                    return "user";
                case AIRole.Assistant:
                    return "model";
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        private AIRole GetRole(string role)
        {
            switch (role)
            {
                case "user":
                    return AIRole.User;
                case "system":
                    return AIRole.System;
                case "tool":
                    return AIRole.Tool;
                case "model":
                    return AIRole.Assistant;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        public void SetSettings(AIServiceInitData value)
        {
            _refreshToken = value.APIKey;
        }

        #endregion
    }
}