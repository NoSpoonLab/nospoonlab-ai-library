using System.Threading.Tasks;
using AICore.Infrastructure.AzureAI.Data;
using AICore.Services.Types.Data;

namespace AICore.Services.Interfaces
{
    /// <summary>
    /// Defines the requirements for a service that processes cloud-based voice services like Speech to Text and Text to Speech.
    /// </summary>
    /// <seealso cref="AICore.Services.Interfaces.IService"/>
    public interface ICloudService : IService
    {
        /// <summary>
        /// Asynchronously sends a speech-to-text request.
        /// </summary>
        /// <param name="data">The binary data of the audio file to transcribe.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a VoiceDataResponse object with the transcribed text.</returns>
        Task<VoiceDataResponse> SendSpeechToTextRequest(byte[] data, string locale);
      
        /// <summary>
        /// Asynchronously sends a text-to-speech request.
        /// </summary>
        /// <param name="data">The text that needs to be transformed into speech.</param>
        /// <param name="voiceName">The name of the synthesizer voice to be used for text-to-speech conversion. Default value is empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a VoiceDataResponse object with the synthesized speech data.</returns>
        Task<VoiceDataResponse> SendTextToSpeechRequest(string data, string voiceName, string locale, string style = "");

        /// <summary>
        /// Sets the initial data settings for the Cloud-Based Voice Services.
        /// </summary>
        /// <param name="value">The VoiceServiceInitData object carrying the initial settings data.</param>
        void SetSettings(VoiceServiceInitData value);
    }
}