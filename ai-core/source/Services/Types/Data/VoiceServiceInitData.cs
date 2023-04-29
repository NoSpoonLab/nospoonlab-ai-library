using System;

namespace AICore.Services.Types.Data
{
    /// <summary>
    /// Represents the required configuration data to initialize a voice service.
    /// This configuration includes API Key, geographic region, locale, and choice of voice.
    /// </summary>
    [Serializable]
    public class VoiceServiceInitData
    {
        /// <summary>
        /// The API key to authenticate requests to the voice service.
        /// It is a unique identifier used for authorizing and tracking requests made to the service.
        /// </summary>
        public string APIKey;

        /// <summary>
        /// The geographic region where the voice service is hosted.
        /// This information ensures that requests are sent to the correct server which can affect latency and communication speed.
        /// </summary>
        public string Region;
    }
}