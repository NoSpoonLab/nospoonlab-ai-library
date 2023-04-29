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

        /// <summary>
        /// The locale representing the target language and region for the voice service. 
        /// It determines the language, accent, and other regional specificities in the voice output.
        /// </summary>
        public string Locale;

        /// <summary>
        /// The name of the voice to be used by the voice service.
        /// Each voice has a unique name and represents different characteristics such as gender, age, language, and tone.
        /// </summary>
        public string VoiceName;
    }
}