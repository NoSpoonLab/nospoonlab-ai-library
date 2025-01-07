using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AICore.Infrastructure.OpenAI;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using NUnit.Framework;

namespace Tests.AICoreTest.InfrastructureTest.OpenAITest
{
    public class OpenAIAssistantsTest
    {
        #region Properties
        
        private OpenAIClient _openAIClient;
        private const string ASSISTANT_ID = "asst_B0AKpc1j9MqFf2KVCd2q03PB";
        
        #endregion

        #region Constructor

        public OpenAIAssistantsTest()
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
        public async Task CreateThreadRequest()
        {
            var thread = await _openAIClient.CreateThread();
            Assert.IsTrue(!string.IsNullOrEmpty(thread.Id)); 
            await _openAIClient.DeleteThread(thread.Id);
        }

        [Test]
        public async Task CreateThreadWithMessageRequest()
        {
            var message = new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" };
            var thread = await _openAIClient.CreateThread(new List<DataMessage>{message});
            Assert.IsTrue(!string.IsNullOrEmpty(thread.Id));
            await _openAIClient.DeleteThread(thread.Id);
        }
        
        [Test]
        public async Task AddMessageToThreadRequest()
        {
            var thread = await _openAIClient.CreateThread();
            var message = new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" };
            var response = await _openAIClient.AddMessageToThread(thread.Id, message);
            Assert.IsTrue(!string.IsNullOrEmpty(response.Content.First().Text.value));
            await _openAIClient.DeleteThread(thread.Id);
        }
        
        [Test]
        public async Task DeleteThreadRequest()
        {
            var thread = await _openAIClient.CreateThread();
            var response = await _openAIClient.DeleteThread(thread.Id);
            Assert.IsTrue(response.Deleted);
        }
        
        [Test]
        public async Task GetMessageFromThreadRequest()
        {
            var thread = await _openAIClient.CreateThread();
            var message = new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" };
            var messageResponse = await _openAIClient.AddMessageToThread(thread.Id, message);
            var response = await _openAIClient.GetMessageFromThread(thread.Id,messageResponse.Id);
            Assert.IsTrue(!string.IsNullOrEmpty(response.Content.First().Text.value));
            await _openAIClient.DeleteThread(thread.Id);
        }
        
        [Test]
        public async Task GetMessagesFromThreadRequest()
        {
            var thread = await _openAIClient.CreateThread();
            var message = new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" };
            await _openAIClient.AddMessageToThread(thread.Id, message);
            var response = await _openAIClient.GetMessagesFromThread(thread.Id);
            Assert.IsTrue(response.Data.Count > 0);
            await _openAIClient.DeleteThread(thread.Id);
        }
        
        [Test]
        public async Task DeleteMessageFromThreadRequest()
        {
            var thread = await _openAIClient.CreateThread();
            var message = new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" };
            var messageResponse = await _openAIClient.AddMessageToThread(thread.Id, message);
            var response = await _openAIClient.DeleteMessageFromThread(thread.Id,messageResponse.Id);
            Assert.IsTrue(response);
            await _openAIClient.DeleteThread(thread.Id);
        }

        [Test]
        public async Task RunAssistantsThread()
        {
            var thread = await _openAIClient.CreateThread();
            var message = new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" };
            await _openAIClient.AddMessageToThread(thread.Id, message);
            var response = await _openAIClient.RunAssistantsThread(thread.Id, ASSISTANT_ID);
            Assert.IsNotEmpty(response.Id);
            await _openAIClient.DeleteThread(thread.Id);
        }
        
        [Test]
        public async Task GetAssistantsRequest()
        {
            var response = await _openAIClient.GetAssistants();
            Assert.IsTrue(response.Data.Count > 0);
        }

        [Test]
        public async Task CreateAThreadAndRun()
        {
            var message = new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" };
            var response = await _openAIClient.CreateAThreadAndRun(ASSISTANT_ID, new List<DataMessage>{ message });
            Assert.IsNotEmpty(response.Id);
            await _openAIClient.DeleteThread(response.ThreadId);
        }

        [Test]
        public async Task GetListRuns()
        {
            var thread1 = await _openAIClient.CreateThread();
            var run1 = await _openAIClient.RunAssistantsThread(thread1.Id, ASSISTANT_ID);
            await Task.Delay(10000);
            var response = await _openAIClient.GetListRuns(thread1.Id);
            Assert.IsTrue(response.Data.Count > 0);
        }
        
        [Test]
        public async Task GetRun()
        {
            var thread1 = await _openAIClient.CreateThread();
            var run1 = await _openAIClient.RunAssistantsThread(thread1.Id, ASSISTANT_ID);
            await Task.Delay(1000);
            var response = await _openAIClient.GetRun(thread1.Id, run1.Id);
            Assert.IsNotEmpty(response.Id);
        }
        
        [Test]
        public async Task CancelRun()
        {
            var thread1 = await _openAIClient.CreateThread();
            var run1 = await _openAIClient.RunAssistantsThread(thread1.Id, ASSISTANT_ID);
            
            //Added a try because in some case the run is already completed
            try
            {
                await _openAIClient.CancelRun(thread1.Id, run1.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        [Test]
        public async Task ModifyRun()
        {
            var thread1 = await _openAIClient.CreateThread();
            var run1 = await _openAIClient.RunAssistantsThread(thread1.Id, ASSISTANT_ID);
            await Task.Delay(15000);
            var response = await _openAIClient.ModifyRun(thread1.Id, run1.Id, new Dictionary<string, string>
            {
                {"status", "completed"},
                {"completed_at", "1630000000"}
            });
            Assert.IsNotEmpty(response.Id);
        }

        [Test]
        public async Task SimpleChat()
        {
            var thread = await _openAIClient.CreateThread();
            
            await _openAIClient.AddMessageToThread(thread.Id, new DataMessage{role = AIRole.User, content = "Hola ¿quien eres?" });
            var run = await _openAIClient.RunAssistantsThread(thread.Id, ASSISTANT_ID);
            await _openAIClient.RunIsCompleted(thread.Id, run.Id);
            var response = await _openAIClient.GetMessagesFromThread(thread.Id);
            Console.WriteLine(response.Data.First().Content.First().Text.value);
            await _openAIClient.AddMessageToThread(thread.Id, new DataMessage{role = AIRole.User, content = "Puedes ayudarme a dividir 10 entre 2?" });
            await _openAIClient.RunAssistantsThread(thread.Id, ASSISTANT_ID);
            await _openAIClient.RunIsCompleted(thread.Id, run.Id);
            response = await _openAIClient.GetMessagesFromThread(thread.Id);
            Console.WriteLine(response.Data.First().Content.First().Text.value);
            
            await _openAIClient.DeleteThread(thread.Id);
        }

        #endregion
    }
}