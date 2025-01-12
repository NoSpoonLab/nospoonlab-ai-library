using System.Collections.Generic;
using System.Linq;

namespace AICore.Infrastructure.OpenAI.Data
{
    #region Extension Types

    public static class OpenAIWebSocketEventTypeExtensions
    {
        private static readonly Dictionary<OpenAIClient.WebSocketServerEventType, string> _serverEventTypeToString =
            new Dictionary<OpenAIClient.WebSocketServerEventType, string>
            {
                { OpenAIClient.WebSocketServerEventType.Error, "error" },
                { OpenAIClient.WebSocketServerEventType.SessionCreated, "session.created" },
                { OpenAIClient.WebSocketServerEventType.SessionUpdated, "session.updated" },
                { OpenAIClient.WebSocketServerEventType.ConversationCreated, "conversation.created" },
                { OpenAIClient.WebSocketServerEventType.ConversationItemCreated, "conversation.item.created" },
                { OpenAIClient.WebSocketServerEventType.ConversationItemInputAudioTranscriptionCompleted, "conversation.item.input_audio_transcription.completed" },
                { OpenAIClient.WebSocketServerEventType.ConversationItemInputAudioTranscriptionFailed, "conversation.item.input_audio_transcription.failed" },
                { OpenAIClient.WebSocketServerEventType.ConversationItemTruncated, "conversation.item.truncated" },
                { OpenAIClient.WebSocketServerEventType.ConversationItemDeleted, "conversation.item.deleted" },
                { OpenAIClient.WebSocketServerEventType.InputAudioBufferCommitted, "input_audio_buffer.committed" },
                { OpenAIClient.WebSocketServerEventType.InputAudioBufferCleared, "input_audio_buffer.cleared" },
                { OpenAIClient.WebSocketServerEventType.InputAudioBufferSpeechStarted, "input_audio_buffer.speech_started" },
                { OpenAIClient.WebSocketServerEventType.InputAudioBufferSpeechStopped, "input_audio_buffer.speech_stopped" },
                { OpenAIClient.WebSocketServerEventType.ResponseCreated, "response.created" },
                { OpenAIClient.WebSocketServerEventType.ResponseDone, "response.done" },
                { OpenAIClient.WebSocketServerEventType.ResponseOutputItemAdded, "response.output_item.added" },
                { OpenAIClient.WebSocketServerEventType.ResponseOutputItemDone, "response.output_item.done" },
                { OpenAIClient.WebSocketServerEventType.ResponseContentPartAdded, "response.content_part.added" },
                { OpenAIClient.WebSocketServerEventType.ResponseContentPartDone, "response.content_part.done" },
                { OpenAIClient.WebSocketServerEventType.ResponseTextDelta, "response.text.delta" },
                { OpenAIClient.WebSocketServerEventType.ResponseTextDone, "response.text.done" },
                { OpenAIClient.WebSocketServerEventType.ResponseAudioTranscriptDelta, "response.audio_transcript.delta" },
                { OpenAIClient.WebSocketServerEventType.ResponseAudioTranscriptDone, "response.audio_transcript.done" },
                { OpenAIClient.WebSocketServerEventType.ResponseAudioDelta, "response.audio.delta" },
                { OpenAIClient.WebSocketServerEventType.ResponseAudioDone, "response.audio.done" },
                { OpenAIClient.WebSocketServerEventType.ResponseFunctionCallArgumentsDelta, "response.function_call_arguments.delta" },
                { OpenAIClient.WebSocketServerEventType.ResponseFunctionCallArgumentsDone, "response.function_call_arguments.done" },
                { OpenAIClient.WebSocketServerEventType.RateLimitsUpdated, "rate_limits.updated" }
            };

        private static readonly Dictionary<string, OpenAIClient.WebSocketServerEventType> _stringToEventType = _serverEventTypeToString.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static string ConvertToString(this OpenAIClient.WebSocketServerEventType eventType)
        {
            return _serverEventTypeToString[eventType];
        }

        public static OpenAIClient.WebSocketServerEventType FromStringToWebsocketServerEventType(this string description)
        {
            return _stringToEventType[description];
        }
        
        private static readonly Dictionary<OpenAIClient.WebSocketClientEventType, string> _clientEventTypeToString =
            new Dictionary<OpenAIClient.WebSocketClientEventType, string>
            {
                { OpenAIClient.WebSocketClientEventType.SessionUpdate, "session.update" },
                { OpenAIClient.WebSocketClientEventType.InputAudioBufferAppend, "input_audio_buffer.append" },
                { OpenAIClient.WebSocketClientEventType.InputAudioBufferCommit, "input_audio_buffer.commit" },
                { OpenAIClient.WebSocketClientEventType.InputAudioBufferClear, "input_audio_buffer.clear" },
                { OpenAIClient.WebSocketClientEventType.ConversationItemCreate, "conversation.item.create" },
                { OpenAIClient.WebSocketClientEventType.ConversationItemTruncate, "conversation.item.truncate" },
                { OpenAIClient.WebSocketClientEventType.ConversationItemDelete, "conversation.item.delete" },
                { OpenAIClient.WebSocketClientEventType.ResponseCreate, "response.create" },
                { OpenAIClient.WebSocketClientEventType.ResponseCancel, "response.cancel" }
            };
        
        private static readonly Dictionary<string, OpenAIClient.WebSocketClientEventType> _stringToClientEventType = _clientEventTypeToString.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        
        public static string ConvertToString(this OpenAIClient.WebSocketClientEventType eventType)
        {
            return _clientEventTypeToString[eventType];
        }
        public static OpenAIClient.WebSocketClientEventType FromStringToWebsocketClientEventType(this string description)
        {
            return _stringToClientEventType[description];
        }
        
        
    }

    #endregion
}