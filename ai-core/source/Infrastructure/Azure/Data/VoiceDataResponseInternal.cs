using System;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable 0649
namespace AICore.Infrastructure.Azure.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal class VoiceDataResponseInternal
    {
        public string RecognitionStatus;
        public int Offset;
        public int Duration;
        public string DisplayText;
    }
}
#pragma warning restore 0649