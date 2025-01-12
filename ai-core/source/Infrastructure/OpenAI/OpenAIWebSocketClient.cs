using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AICore.Infrastructure.OpenAI.Data;
using AICore.Services.Extension;
using AICore.Services.Types;
using AICore.Services.Types.Data;
using AICore.Services.Types.Request;
using AICore.Services.Types.Response;
using Newtonsoft.Json;

namespace AICore.Infrastructure.OpenAI
{
    public partial class OpenAIClient
    {
        #region Variables
        
        private const int BUFFER_SIZE = 1024 * 1024 * 4; // 4MB buffer

        private ClientWebSocket _webSocket;
        private Thread _webSocketThread;
        private SemaphoreSlim _sendLock;
        private CancellationTokenSource _cancellationTokenSource;
        private static int _onSessionCreatedCounter = 0;

        #endregion
        
        #region Methods

        public void ConnectWebSocket()
        {
            _sendLock = new SemaphoreSlim(1, 1);
            _sendLock.Wait();
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            _webSocketThread = new Thread(async () => await ConnectWebSocketAsync(_cancellationTokenSource.Token));
            _webSocketThread.Start();
        }
        
        public bool IsWebSocketConnected()
        {
            return _webSocket != null && _webSocket.State == WebSocketState.Open;
        }

        private async Task ConnectWebSocketAsync(CancellationToken cancellationToken)
        {
            try
            {
                var token = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

                // Add headers for authorization and other purposes
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {token}");
                _webSocket.Options.SetRequestHeader("OpenAI-Beta", "realtime=v1");

                var uri = new Uri(
                    $"{OpenAIConstants.WEB_SOCKET_URL}" +
                    $"{OpenAIConstants.WEB_SOCKET_REALTIME_REQUEST}" +
                    $"?model={ModelsExtension.GetString(AIModel.gpt_4o_realtime)}");

                // Connect to the WebSocket server
                await _webSocket.ConnectAsync(uri, cancellationToken);
                if (_webSocket.State == WebSocketState.Open)
                {
                    Console.WriteLine("WebSocket Connected");
                    await ReceiveMessagesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket Error: {ex.Message} {_onSessionCreatedCounter}");
                _onSessionCreatedCounter++;
            }
        }

        public void SendWebSocketMessage<T>(T message) where T : WebsocketBase
        {
            SendWebSocketMessageAsync(message).Wait();
        }
        public async Task SendWebSocketMessageAsync<T>(T message) where T : WebsocketBase
        {
            await _sendLock.WaitAsync();
            try
            {
                await Task.Yield();
                if (_webSocket == null) throw new Exception("WebSocket is not connected");
                if (_webSocket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                    await Task.Yield();
                    //Console.WriteLine("SendMessage Start " + DateTime.Now);
                    await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true,
                        _cancellationTokenSource.Token);
                    //Console.WriteLine("SendMessage End " + DateTime.Now);
                }
            }
            catch (Exception e)
            {
                DisconnectWebSocket();
                throw new Exception(e.Message);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public void DisconnectWebSocket()
        {
            if (_webSocket != null && _webSocket.State == WebSocketState.Open)
            {
                //Console.WriteLine("WebSocket closed by client");
                _webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();
                //_webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();
                _cancellationTokenSource.Cancel();
                Console.WriteLine("WebSocket Disconnected");
                if(_sendLock.CurrentCount != 1)_sendLock.Release();
            }
            _webSocketThread?.Join();
        }

        private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            _sendLock.Release();
            var buffer = new byte[BUFFER_SIZE];
            while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
                    //Console.WriteLine("WebSocket closed by server");
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    
                    // Transform the message into a JSON object
                    var baseType = JsonConvert.DeserializeObject<WebsocketBase>(message);
                    var type = baseType.Type.FromStringToWebsocketServerEventType();
                    //Also print timestamp and type of message
                    //Console.WriteLine(baseType.Type + " " + DateTime.Now);
                    //Console.WriteLine(message + "\n");
                    switch (type)
                    {
                        case WebSocketServerEventType.Error:
                            OnError?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketWelcome>(message));
                            break;
                        case WebSocketServerEventType.SessionCreated:
                            OnSessionCreated?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketSessionCreated>(message));
                            break;
                        case WebSocketServerEventType.SessionUpdated:
                            OnSessionUpdated?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketSessionUpdated>(message));
                            break;
                        case WebSocketServerEventType.ConversationCreated:
                            OnConversationCreated?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketConversationCreated>(message));
                            break;
                        case WebSocketServerEventType.ConversationItemCreated:
                            OnConversationItemCreated?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketConversationItemCreated>(message));
                            break;
                        case WebSocketServerEventType.ConversationItemInputAudioTranscriptionCompleted:
                            OnConversationItemInputAudioTranscriptionCompleted?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketConversationItemInputAudioTranscriptionCompleted>(message));
                            break;
                        case WebSocketServerEventType.ConversationItemInputAudioTranscriptionFailed:
                            OnConversationItemInputAudioTranscriptionFailed?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketConversationItemInputAudioTranscriptionFailed>(message));
                            break;
                        case WebSocketServerEventType.ConversationItemTruncated:
                            OnConversationItemTruncated?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketConversationItemTruncated>(message));
                            break;
                        case WebSocketServerEventType.ConversationItemDeleted:
                            OnConversationItemDeleted?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketConversationItemDeleted>(message));
                            break;
                        case WebSocketServerEventType.InputAudioBufferCommitted:
                            OnInputAudioBufferCommitted?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketInputAudioBufferCommitted>(message));
                            break;
                        case WebSocketServerEventType.InputAudioBufferCleared:
                            OnInputAudioBufferCleared?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketInputAudioBufferCleared>(message));
                            break;
                        case WebSocketServerEventType.InputAudioBufferSpeechStarted:
                            OnInputAudioBufferSpeechStarted?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketInputAudioBufferSpeechStarted>(message));
                            break;
                        case WebSocketServerEventType.InputAudioBufferSpeechStopped:
                            OnInputAudioBufferSpeechStopped?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketInputAudioBufferSpeechStopped>(message));
                            break;
                        case WebSocketServerEventType.ResponseCreated:
                            OnResponseCreated?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseCreated>(message));
                            break;
                        case WebSocketServerEventType.ResponseDone:
                            OnResponseDone?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseDone>(message));
                            break;
                        case WebSocketServerEventType.ResponseOutputItemAdded:
                            OnResponseOutputItemAdded?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseOutputItemAdded>(message));
                            break;
                        case WebSocketServerEventType.ResponseOutputItemDone:
                            OnResponseOutputItemDone?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseOutputItemDone>(message));
                            break;
                        case WebSocketServerEventType.ResponseContentPartAdded:
                            OnResponseContentPartAdded?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseContentPartAdded>(message));
                            break;
                        case WebSocketServerEventType.ResponseContentPartDone:
                            OnResponseContentPartDone?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseContentPartDone>(message));
                            break;
                        case WebSocketServerEventType.ResponseTextDelta:
                            OnResponseTextDelta?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseTextDelta>(message));
                            break;
                        case WebSocketServerEventType.ResponseTextDone:
                            OnResponseTextDone?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseTextDone>(message));
                            break;
                        case WebSocketServerEventType.ResponseAudioTranscriptDelta:
                            OnResponseAudioTranscriptDelta?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseAudioTranscriptDelta>(message));
                            break;
                        case WebSocketServerEventType.ResponseAudioTranscriptDone:
                            OnResponseAudioTranscriptDone?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseAudioTranscriptDone>(message));
                            break;
                        case WebSocketServerEventType.ResponseAudioDelta:
                            OnResponseAudioDelta?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseAudioDelta>(message));
                            break;
                        case WebSocketServerEventType.ResponseAudioDone:
                            OnResponseAudioDone?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseAudioDone>(message));
                            break;
                        case WebSocketServerEventType.ResponseFunctionCallArgumentsDelta:
                            OnResponseFunctionCallArgumentsDelta?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseFunctionCallArgumentsDelta>(message));
                            break;
                        case WebSocketServerEventType.ResponseFunctionCallArgumentsDone:
                            OnResponseFunctionCallArgumentsDone?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketResponseFunctionCallArgumentsDone>(message));
                            break;
                        case WebSocketServerEventType.RateLimitsUpdated:
                            OnRateLimitsUpdated?.Invoke(JsonConvert.DeserializeObject<AITransformerWebSocketRateLimitsUpdated>(message));
                            break;
                        default:
                            DisconnectWebSocket();
                            throw new Exception(message);
                    }
                }
            }
        }

        #endregion
    }
}