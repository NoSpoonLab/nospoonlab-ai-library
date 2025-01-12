using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AICore.Infrastructure.OpenAI.Data;
using AICore.Services.Extension;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;
using Newtonsoft.Json;

namespace AICore.Infrastructure.OpenAI
{
    public partial class OpenAIClient
    {

        #region Public Methods

        public async Task<AITransformerThreadMessageResponse> CreateThread(List<DataMessage> input = null)
        {
            var inputInternal = input != null ? TransformInputMessages(input) : null;
            return await SendRequestAsync<AITransformerThreadMessageResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST,
                HttpMethod.Post,
                new { messages = inputInternal }
            );
        }

        public Task<AITransformerThreadMessageResponse> DeleteThread(string threadId) =>
            SendRequestAsync<AITransformerThreadMessageResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}",
                HttpMethod.Delete
            );

        public Task<AITransformerThreadMessageResponse> AddMessageToThread(string threadId, DataMessage input) =>
            SendRequestAsync<AITransformerThreadMessageResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/messages",
                HttpMethod.Post,
                TransformInputMessages(new List<DataMessage>{input})[0]
            );

        public Task<AITransformerThreadMessageResponse> GetMessageFromThread(string threadId, string messageId) =>
            SendRequestAsync<AITransformerThreadMessageResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/messages/{messageId}",
                HttpMethod.Get
            );

        public Task<AITransformerThreadListMessageResponse> GetMessagesFromThread(string threadId) =>
            SendRequestAsync<AITransformerThreadListMessageResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/messages",
                HttpMethod.Get
            );

        public async Task<bool> DeleteMessageFromThread(string threadId, string messageId)
        {
            var request = await SendRequestAsync<AITransformerThreadDelete>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/messages/{messageId}",
                HttpMethod.Delete
            );
            return request.Deleted;
        }

        public Task<AITransformerThreadRunResponse> RunAssistantsThread(string threadId, string assistantId) =>
            SendRequestAsync<AITransformerThreadRunResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/runs",
                HttpMethod.Post,
                new { assistant_id = assistantId }
            );

        public Task<AITransformerAssistantListResponse> GetAssistants() =>
            SendRequestAsync<AITransformerAssistantListResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_ASSISTANTS_REQUEST + "?order=desc&limit=20",
                HttpMethod.Get
            );

        public Task<AITransformerThreadRunResponse> CreateAThreadAndRun(string assistantId, List<DataMessage> input) =>
            SendRequestAsync<AITransformerThreadRunResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + "/runs",
                HttpMethod.Post,
                new { assistant_id = assistantId, thread = new { messages = TransformInputMessages(input) } }
            );

        public Task<AITransformerThreadRunListResponse> GetListRuns(string threadId) =>
            SendRequestAsync<AITransformerThreadRunListResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/runs",
                HttpMethod.Get
            );

        public Task<AITransformerThreadRunResponse> GetRun(string threadId, string runId) =>
            SendRequestAsync<AITransformerThreadRunResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/runs/{runId}",
                HttpMethod.Get
            );

        public async Task<bool> RunIsCompleted(string threadId, string runId)
        {
            var request = await GetRun(threadId, runId);
            while (request.Status != "completed")
            {
                request = await GetRun(threadId, runId);
                await Task.Delay(100);
            }
            await Task.Delay(50);

            return true;
        }

        public Task<AITransformerThreadRunResponse> CancelRun(string threadId, string runId)
        {
            return SendRequestAsync<AITransformerThreadRunResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/runs/{runId}/cancel",
                HttpMethod.Post
            );
        }

        public Task<AITransformerThreadRunResponse> ModifyRun(string threadId, string runId, Dictionary<string, string> metadata) =>
            SendRequestAsync<AITransformerThreadRunResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/runs/{runId}",
                HttpMethod.Post,
                new { metadata }
            );

        public Task<AITransformerThreadRunResponse> SubmitToolOutputToRun(string threadId, string runId, List<AITransformerToolOutput> toolsOutput) =>
            SendRequestAsync<AITransformerThreadRunResponse>(
                OpenAIConstants.API_URL + OpenAIConstants.API_THREAD_REQUEST + $"/{threadId}/runs/{runId}/submit_tool_outputs",
                HttpMethod.Post,
                new { tools_output = toolsOutput }
            );

        #endregion
        
        #region Private Methods

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}");
            client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");
            return client;
        }

        private async Task<T> SendRequestAsync<T>(string url, HttpMethod method, object body = null)
        {
            using (var client = CreateHttpClient())
            {
                var request = new HttpRequestMessage(method, url);

                if (body != null)
                {
                    var requestBody = JsonConvert.SerializeObject(body);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                }

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(responseString);
                }
                else
                {
                    throw new Exception($"Request failed: {response.StatusCode}\n{responseString}");
                }
            }
        }

        #endregion

        #region Private Methods

        private List<AITransformerMessageInternal> TransformInputMessages(List<DataMessage> input)
        {
            return input.Select(it => new AITransformerMessageInternal
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
            }).ToList();
        }

        #endregion
    }
}
