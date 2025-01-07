using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICore.Infrastructure.OpenAI;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using NUnit.Framework;

namespace Tests.AICoreTest.InfrastructureTest.OpenAITest
{
    public class OpenAITest
    {
        #region Properties

        private OpenAIClient _openAIClient;

        #endregion

        #region Constructor

        public OpenAITest()
        {
            _openAIClient = new OpenAIClient();
            var credentials = new AIServiceInitData
            {
                APIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"),
                Type = AIServiceType.OpenAI
            };
            _openAIClient.SetSettings(credentials);
        }

        #endregion

        #region Tests
        
        [Test]
        public async Task SimpleTest()
        {
            Console.WriteLine("Starting test of simple transformer request\n");

            var request = new AITransformerRequest
            {
                messages = new List<DataMessage>
                {
                    new () { role = AIRole.User, content = "Hello, how are you? and how are you today?" }
                }
            };

            var response = await _openAIClient.SendTransformerRequest(request);
            
            Console.WriteLine("Response: " + response);
            Assert.IsTrue(response.generated.Count > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(response.generated[0].message.content));
        }
        
        [Test]
        public async Task EmbeddingTest()
        {
            Console.WriteLine("Starting test of embedding request\n");
            var request = new AIEmbeddingRequest
            {
                input = "She sells seashells by the seashore,\nThe shells she sells are surely seashells.\nSo if she sells shells on the seashore,\nI’m sure she sells seashore shells."
            };
            var response = await _openAIClient.SendEmbeddingRequest(request, AIModel.text_embedding_3_large);
            Console.WriteLine("Response: " + response);
            Assert.IsTrue(response.data[0].embedding.Length > 0);
        }

        [Test]
        public async Task ImageGenerateTest()
        {
            Console.WriteLine("Starting test of image generation request\n");
            var request = new AIImageRequest("A painting of a beautiful sunset over the ocean.");
            var response = await _openAIClient.SendImageRequest(request);
            Assert.IsTrue(response.data.Count > 0);
            Assert.IsTrue(response.data[0].url.Length > 0);
        }

        #endregion
    }
}