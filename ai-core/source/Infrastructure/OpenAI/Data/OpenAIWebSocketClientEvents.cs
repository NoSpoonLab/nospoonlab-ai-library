using AICore.Services.Types.Request;
using AICore.Services.Types.Response;

namespace AICore.Infrastructure.OpenAI
{
    public partial class OpenAIClient
    {

        #region Delegates

        public delegate void OnErrorHandler(AITransformerWebSocketWelcome data);
        public delegate void OnSessionCreatedHandler(AITransformerWebSocketSessionCreated data);
        public delegate void OnSessionUpdatedHandler(AITransformerWebSocketSessionUpdated data);
        public delegate void OnConversationCreatedHandler(AITransformerWebSocketConversationCreated data);
        public delegate void OnConversationItemCreatedHandler(AITransformerWebSocketConversationItemCreated data);
        public delegate void OnConversationItemInputAudioTranscriptionCompletedHandler(AITransformerWebSocketConversationItemInputAudioTranscriptionCompleted data);
        public delegate void OnConversationItemInputAudioTranscriptionFailedHandler(AITransformerWebSocketConversationItemInputAudioTranscriptionFailed data);
        public delegate void OnConversationItemTruncatedHandler(AITransformerWebSocketConversationItemTruncated data);
        public delegate void OnConversationItemDeletedHandler(AITransformerWebSocketConversationItemDeleted data);
        public delegate void OnInputAudioBufferCommittedHandler(AITransformerWebSocketInputAudioBufferCommitted data);
        public delegate void OnInputAudioBufferClearedHandler(AITransformerWebSocketInputAudioBufferCleared data);
        public delegate void OnInputAudioBufferSpeechStartedHandler(AITransformerWebSocketInputAudioBufferSpeechStarted data);
        public delegate void OnInputAudioBufferSpeechStoppedHandler(AITransformerWebSocketInputAudioBufferSpeechStopped data);
        public delegate void OnResponseCreatedHandler(AITransformerWebSocketResponseCreated data);
        public delegate void OnResponseDoneHandler(AITransformerWebSocketResponseDone data);
        public delegate void OnResponseOutputItemAddedHandler(AITransformerWebSocketResponseOutputItemAdded data);
        public delegate void OnResponseOutputItemDoneHandler(AITransformerWebSocketResponseOutputItemDone data);
        public delegate void OnResponseContentPartAddedHandler(AITransformerWebSocketResponseContentPartAdded data);
        public delegate void OnResponseContentPartDoneHandler(AITransformerWebSocketResponseContentPartDone data);
        public delegate void OnResponseTextDeltaHandler(AITransformerWebSocketResponseTextDelta data);
        public delegate void OnResponseTextDoneHandler(AITransformerWebSocketResponseTextDone data);
        public delegate void OnResponseAudioTranscriptDeltaHandler(AITransformerWebSocketResponseAudioTranscriptDelta data);
        public delegate void OnResponseAudioTranscriptDoneHandler(AITransformerWebSocketResponseAudioTranscriptDone data);
        public delegate void OnResponseAudioDeltaHandler(AITransformerWebSocketResponseAudioDelta data);
        public delegate void OnResponseAudioDoneHandler(AITransformerWebSocketResponseAudioDone data);
        public delegate void OnResponseFunctionCallArgumentsDeltaHandler(AITransformerWebSocketResponseFunctionCallArgumentsDelta data);
        public delegate void OnResponseFunctionCallArgumentsDoneHandler(AITransformerWebSocketResponseFunctionCallArgumentsDone data);
        public delegate void OnRateLimitsUpdatedHandler(AITransformerWebSocketRateLimitsUpdated data);

        public event OnErrorHandler OnError;
        public event OnSessionCreatedHandler OnSessionCreated;
        public event OnSessionUpdatedHandler OnSessionUpdated;
        public event OnConversationCreatedHandler OnConversationCreated;
        public event OnConversationItemCreatedHandler OnConversationItemCreated;
        public event OnConversationItemInputAudioTranscriptionCompletedHandler OnConversationItemInputAudioTranscriptionCompleted;
        public event OnConversationItemInputAudioTranscriptionFailedHandler OnConversationItemInputAudioTranscriptionFailed;
        public event OnConversationItemTruncatedHandler OnConversationItemTruncated;
        public event OnConversationItemDeletedHandler OnConversationItemDeleted;
        public event OnInputAudioBufferCommittedHandler OnInputAudioBufferCommitted;
        public event OnInputAudioBufferClearedHandler OnInputAudioBufferCleared;
        public event OnInputAudioBufferSpeechStartedHandler OnInputAudioBufferSpeechStarted;
        public event OnInputAudioBufferSpeechStoppedHandler OnInputAudioBufferSpeechStopped;
        public event OnResponseCreatedHandler OnResponseCreated;
        public event OnResponseDoneHandler OnResponseDone;
        public event OnResponseOutputItemAddedHandler OnResponseOutputItemAdded;
        public event OnResponseOutputItemDoneHandler OnResponseOutputItemDone;
        public event OnResponseContentPartAddedHandler OnResponseContentPartAdded;
        public event OnResponseContentPartDoneHandler OnResponseContentPartDone;
        public event OnResponseTextDeltaHandler OnResponseTextDelta;
        public event OnResponseTextDoneHandler OnResponseTextDone;
        public event OnResponseAudioTranscriptDeltaHandler OnResponseAudioTranscriptDelta;
        public event OnResponseAudioTranscriptDoneHandler OnResponseAudioTranscriptDone;
        public event OnResponseAudioDeltaHandler OnResponseAudioDelta;
        public event OnResponseAudioDoneHandler OnResponseAudioDone;
        public event OnResponseFunctionCallArgumentsDeltaHandler OnResponseFunctionCallArgumentsDelta;
        public event OnResponseFunctionCallArgumentsDoneHandler OnResponseFunctionCallArgumentsDone;
        public event OnRateLimitsUpdatedHandler OnRateLimitsUpdated;

        #endregion

        #region Types

        public enum WebSocketServerEventType
        {
            Error,
            SessionCreated,
            SessionUpdated,
            ConversationCreated,
            ConversationItemCreated,
            ConversationItemInputAudioTranscriptionCompleted,
            ConversationItemInputAudioTranscriptionFailed,
            ConversationItemTruncated,
            ConversationItemDeleted,
            InputAudioBufferCommitted,
            InputAudioBufferCleared,
            InputAudioBufferSpeechStarted,
            InputAudioBufferSpeechStopped,
            ResponseCreated,
            ResponseDone,
            ResponseOutputItemAdded,
            ResponseOutputItemDone,
            ResponseContentPartAdded,
            ResponseContentPartDone,
            ResponseTextDelta,
            ResponseTextDone,
            ResponseAudioTranscriptDelta,
            ResponseAudioTranscriptDone,
            ResponseAudioDelta,
            ResponseAudioDone,
            ResponseFunctionCallArgumentsDelta,
            ResponseFunctionCallArgumentsDone,
            RateLimitsUpdated
        }
        public enum WebSocketClientEventType
        {
            SessionUpdate,
            InputAudioBufferAppend,
            InputAudioBufferCommit,
            InputAudioBufferClear,
            ConversationItemCreate,
            ConversationItemTruncate,
            ConversationItemDelete,
            ResponseCreate,
            ResponseCancel
        }
        #endregion
    }
}