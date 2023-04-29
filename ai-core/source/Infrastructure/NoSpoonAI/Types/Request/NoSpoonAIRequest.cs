using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AICore.Infrastructure.NoSpoonAI.Types.Exceptions;
using AICore.Services.Types.Response;
using Newtonsoft.Json;

namespace AICore.Infrastructure.NoSpoonAI.Types.Request
{
    /// <summary>
    /// This abstract base class contains methods to handle AI responses.
    /// For successful responses, transformation rules can be customized based on the type of the content, message or function.
    /// If the a successful transformation of the result is not possible, the class provides a retry mechanism.
    ///
    /// </summary>
    
    public class NoSpoonToolContent
    {
        public string name { get; set; }
        public string arguments { get; set; }
        public string id { get; set; }
    }
    
    public abstract class NoSpoonAIRequest
    {
        #region Properties

        // Retry count for the AI service calls
        private int _retryCount = 0;

        // The delay between retries in milliseconds
        private int _retryDelay = 0;

        // The maximum limit for attempting retries
        private int _retryLimit = 3;
        
        private List<NoSpoonToolContent> _toolCalls;

        #endregion
        
        #region Methods

        /// <summary>
        /// Returns retry limit.
        /// </summary>
        protected int RetryLimit() => _retryLimit;

        /// <summary>
        /// Returns retry delay.
        /// </summary>
        protected int RetryDelay() => _retryDelay;

        /// <summary>
        /// Sets retry limit.
        /// </summary>
        protected void RetryLimit(int value) => _retryCount = value;
        
        /// <summary>
        /// Sets retry delay.
        /// </summary>
        protected void RetryDelay(int value) => _retryCount = value;

        public string GetToolId(string toolName)
        {
            if (_toolCalls == null) throw new Exception("No tool calls found");
            var tool = _toolCalls.Find(it => it.name == toolName);
            if(tool == null) throw new Exception("Tool not found with name: " + toolName);
            return tool.id;
        }

        /// <summary>
        /// Transforms the content if the successful AI request response is a message.
        /// This class can be override to provide custom transformation rules or to create rules when a retry should be performed.
        /// </summary>
        /// <param name="value">The response content as string.</param>
        protected virtual T OnSuccessMessage<T>(string value) where T : class => JsonConvert.DeserializeObject<T>(value);

        /// <summary>
        /// Transforms the content if the successful AI request response is a function.
        /// This class can be override to provide custom transformation rules or to create rules when a retry should be performed.
        /// </summary>
        /// <param name="value">The response content as string.</param>
        protected virtual List<T> OnSuccessFunctions<T>(List<NoSpoonToolContent> value) where T : class
        {
            var result = value.Select(it => it.arguments);
            return result.Select(it => JsonConvert.DeserializeObject<T>(it)).ToList();
        }

        /// <summary>
        /// Transforms the content if the successful AI request response is a message with given usage data.
        /// This class can be override to provide custom transformation rules or to create rules when a retry should be performed.
        /// </summary>
        /// <param name="value">The response content as string.</param>
        /// <param name="usageData">The usage data from the response.</param>
        protected virtual T OnSuccessMessage<T>(string value, UsageData usageData) where T : class => OnSuccessMessage<T>(value);

        /// <summary>
        /// Transforms the content if the successful AI request response is a function with a given usage data.
        /// This class can be override to provide custom transformation rules or to create rules when a retry should be performed.
        /// </summary>
        /// <param name="value">The response content as string.</param>
        /// <param name="usageData">The usage data from the response.</param>
        protected virtual List<T> OnSuccessFunctions<T>(List<NoSpoonToolContent> value, UsageData usageData)
            where T : class
        {
            _toolCalls = value;
            return OnSuccessFunctions<T>(value);
        }

        /// <summary>
        /// Executes an asynchronous function with retry mechanism. If function execution fails, it retries until the retry limit is met.
        /// This class can be override to provide custom transformation rules or to create rules when a retry should be performed.
        /// </summary>
        /// <param name="function">The function to be executed.</param>
        /// <returns>Returns a task that represents the asynchronous operation. The task result is of type T.</returns>
        public async Task<T> Execute<T>(Func<Task<object>> function) where T : class
        {
            object rawResult;
            try
            {
                rawResult = await function();
                if(rawResult.GetType() == typeof(AIEmbeddingResponse)) return rawResult as T;
                if(rawResult.GetType() == typeof(AIImageResponse)) return rawResult as T;
                var transformerResponse = (AITransformerResponse) rawResult;
                var lastMessage = transformerResponse.generated.First().message;
                if(typeof(T) == typeof(string) && lastMessage.toolCalls != null &&  lastMessage.toolCalls.Count > 0) return JsonConvert.SerializeObject(lastMessage.toolCalls) as T;
                if(typeof(T) == typeof(string)) return transformerResponse.GetRawData() as T;
                if (!string.IsNullOrEmpty(lastMessage.content)) return OnSuccessMessage<T>(lastMessage.content, transformerResponse.usage);
                if(lastMessage.toolCalls != null) return OnSuccessFunctions<T>(
                    lastMessage.toolCalls.Select(it => new NoSpoonToolContent
                    {
                        name = it.function.name,
                        arguments = it.function.arguments,
                        id = it.id
                    }).ToList(), 
                    transformerResponse.usage).First();
                throw new NoSpoonAIUnknownErrorException();
            }
            catch (Exception e)
            {
                if(_retryCount < RetryLimit())
                {
                    _retryCount++;
                    await Task.Delay(RetryDelay());
                    return await Execute<T>(function);
                }
                Console.WriteLine(e);
                throw new NoSpoonAIRetryException(e.Message);
            }
        }

        #endregion
    }
}