using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AICore.Infrastructure.OpenAI;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;
using NUnit.Framework;
using NAudio.Wave;

namespace Tests.AICoreTest.InfrastructureTest.OpenAITest
{
    [TestFixture]
    public class OpenAIWebsocketTest
    {
        
        #region Properties

        private OpenAIClient _openAIClient;

        #endregion

        #region Constructor

        public OpenAIWebsocketTest()
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

        
        [Test]
        public async Task OpenAIWebSocketTest()
        {
            Stopwatch stopwatchWS = null;
            var sessionRunning = true;
            _openAIClient.ConnectWebSocket();
            _openAIClient.OnSessionCreated += (e) =>
            {
                Console.WriteLine("Session created with ID: " + e.Session.Id);
                stopwatchWS = Stopwatch.StartNew();
            };
            _openAIClient.OnConversationItemCreated += (e) => { Console.WriteLine("Conversation item created with ID: " + e.Item.Id); };
            _openAIClient.OnResponseTextDelta += (e) => { Console.WriteLine("Response text delta: " + e.Delta); };
            _openAIClient.OnResponseTextDone += (e) =>
            {
                Console.WriteLine("Response text done: " + e.Text);
                sessionRunning = false;
            };
            
            //Update the session
            var updateSession = new AITransformerWebSocketSessionUpdate
            {
                Session = new AITransformerWebSocketSession
                {
                    Modalities = new List<string> { "text" }
                }
            };
            _openAIClient.SendWebSocketMessage(updateSession);
            
            //After the session is updated, the event OnSessionUpdated will be triggered
            _openAIClient.OnSessionUpdated += (e) =>
            {
                //Add the conversation item
                var conversation = new AITransformerWebSocketConversationItemCreate
                {
                    Item = new AITransformerWebSocketConversationItem
                    {
                        Role = "user",
                        Content = new List<AITransformerWebSocketContentItem> { new AITransformerWebSocketContentItem { Text = "Hello, how are you?" } }
                    }
                };
                _openAIClient.SendWebSocketMessage(conversation);
                
                //Ask for a response
                var responseCreated = new AITransformerWebSocketResponseCreate{};
                _openAIClient.SendWebSocketMessage(responseCreated);
            };
            while (sessionRunning) { await Task.Yield(); }
            _openAIClient.DisconnectWebSocket();
            
            stopwatchWS.Stop();
            Console.WriteLine($"WebSocket API Runtime: {stopwatchWS.ElapsedMilliseconds} ms");
        }
        
        public static byte[] ConvertAudio(byte[] audioData)
        {
            using (var inputStream = new MemoryStream(audioData))
            using (var reader = new WaveFileReader(inputStream))
            {
                var outFormat = new WaveFormat(24000, 16, 1); // 24kHz, 16-bit, 1 channel
                using (var conversionStream = new WaveFormatConversionStream(outFormat, reader))
                using (var outputStream = new MemoryStream())
                {
                    conversionStream.CopyTo(outputStream);
                    return outputStream.ToArray();
                }
            }
        }
        
        public static string EncodeToBase64(byte[] pcmAudio)
        {
            return Convert.ToBase64String(pcmAudio);
        }
        
        [Test]
        public async Task OpenAIWebSocketVoiceChatTest()
        {
            Stopwatch stopwatchWS = null;
            var finalText = "";
            var path = "../../data/test-english-audio.wav";
            if (!File.Exists(path))
            {
                Console.WriteLine("The file does not exist.");
                return;
            }
            
            byte[] audioData = File.ReadAllBytes(path);
            var sessionRunning = true;
            _openAIClient.ConnectWebSocket();
            _openAIClient.OnSessionCreated += (e) =>
            {
                Console.WriteLine("Session created with ID: " + e.Session.Id);
                stopwatchWS = Stopwatch.StartNew();
            };
            _openAIClient.OnConversationItemCreated += (e) => { Console.WriteLine("Conversation item created with ID: " + e.Item.Id); };
            _openAIClient.OnResponseTextDelta += (e) => { Console.WriteLine("Response text delta: " + e.Delta); };
            _openAIClient.OnResponseTextDone += (e) =>
            {
                Console.WriteLine("Response text done: " + e.Text);
                sessionRunning = false;
                finalText = e.Text;
            };
            
            //Update the session
            var updateSession = new AITransformerWebSocketSessionUpdate
            {
                Session = new AITransformerWebSocketSession
                {
                    Modalities = new List<string> { "text" }
                }
            };
            _openAIClient.SendWebSocketMessage(updateSession);
            
            //After the session is updated, the event OnSessionUpdated will be triggered
            _openAIClient.OnSessionUpdated += (e) =>
            {
                //Add the conversation item
                var conversation = new AITransformerWebSocketConversationItemCreate
                {
                    Item = new AITransformerWebSocketConversationItem
                    {
                        Role = "user",
                        Content = new List<AITransformerWebSocketContentItem>
                        {
                            new AITransformerWebSocketContentItem
                            {
                                Type = "input_audio",
                                Audio = EncodeToBase64(ConvertAudio(audioData))
                            }
                        }
                    }
                };
                _openAIClient.SendWebSocketMessage(conversation);
                
                //Ask for a response
                var responseCreated = new AITransformerWebSocketResponseCreate{};
                _openAIClient.SendWebSocketMessage(responseCreated);
            };
            while (sessionRunning) { await Task.Yield(); }
            Assert.IsTrue(finalText.Length > 0);
            _openAIClient.DisconnectWebSocket();
            stopwatchWS.Stop();
            Console.WriteLine($"WebSocket API Runtime: {stopwatchWS.ElapsedMilliseconds} ms");
        }
    }
}