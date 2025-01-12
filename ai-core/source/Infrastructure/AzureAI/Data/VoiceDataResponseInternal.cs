using System;

#pragma warning disable 0649
namespace AICore.Infrastructure.AzureAI.Data
{
    [Serializable]
    internal class VoiceDataResponseInternal
    {
        public string RecognitionStatus;
        public int Offset;
        public int Duration;
        public string DisplayText;
    }
}
#pragma warning restore 0649