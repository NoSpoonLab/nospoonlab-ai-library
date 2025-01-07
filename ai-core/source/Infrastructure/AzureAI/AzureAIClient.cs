using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AICore.Infrastructure.Azure.Data;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using Newtonsoft.Json;

namespace AICore.Infrastructure.Azure
{
    public class AzureAIClient : ICloudService
    {
        #region Properties

        private string _region;
        private string _apiKey;
        private string _authToken;
        private static readonly HttpClient _httpClient = new HttpClient();

        #endregion

        #region Initialization

        public AzureAIClient() => _authToken = string.Empty;

        #endregion

        #region Methods

        private async Task Authenticate()
        {
            if (_authToken != string.Empty) return;

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_region}.api.cognitive.microsoft.com/sts/v1.0/issuetoken");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception(responseContent);

            _authToken = responseContent;
        }

        async Task<VoiceDataResponse> ICloudService.SendSpeechToTextRequest(byte[] data, string locale)
        {
            if (string.IsNullOrEmpty(_apiKey)) throw new Exception("[Azure] Subscription Key not set");
            if (string.IsNullOrEmpty(_region)) throw new Exception("[Azure] Region not set");

            await Authenticate();

            if (string.IsNullOrEmpty(_authToken)) throw new Exception("[Azure] Authentication failed");

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_region}.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language={locale}")
            {
                Content = new ByteArrayContent(data)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
            request.Content.Headers.TryAddWithoutValidation("codecs", "audio/pcm");
            request.Content.Headers.TryAddWithoutValidation("samplerate", "16000");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception("[Azure] " + responseContent);

            var responseDataInternal = JsonConvert.DeserializeObject<VoiceDataResponseInternal>(responseContent);
            return ResponseDataInternalToResponseData(responseDataInternal);
        }

        async Task<VoiceDataResponse> ICloudService.SendTextToSpeechRequest(string data, string voiceName, string locale, string style = "")
        {
            if (string.IsNullOrEmpty(voiceName)) throw new Exception("[Azure] VoiceName not set");
            if (string.IsNullOrEmpty(_region)) throw new Exception("[Azure] Region not set");
            if (string.IsNullOrEmpty(_apiKey)) throw new Exception("[Azure] Subscription Key not set");

            await Authenticate();

            if (string.IsNullOrEmpty(_authToken)) throw new Exception("[Azure] Authentication failed");

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_region}.tts.speech.microsoft.com/cognitiveservices/v1");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            request.Headers.TryAddWithoutValidation("X-Microsoft-OutputFormat", "riff-8khz-16bit-mono-pcm");
            request.Headers.TryAddWithoutValidation("User-Agent", "NoSpoonLab/1.0");
            request.Content = new StringContent(
                string.IsNullOrEmpty(style)
                    ? $"<speak version='1.0' xml:lang='{locale}'><voice name='{voiceName}'>{data}</voice></speak>"
                    : $"<speak xmlns='http://www.w3.org/2001/10/synthesis' xmlns:mstts='http://www.w3.org/2001/mstts' xmlns:emo='http://www.w3.org/2009/10/emotionml' version='1.0' xml:lang='{locale}'><voice name='{voiceName}'><mstts:express-as style='{style}'>{data}</mstts:express-as></voice></speak>"
            );
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ssml+xml");

            var response = await _httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception("[Azure] " + response.ReasonPhrase);

            // Refresh token and retry in case of authentication failure
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authToken = string.Empty;
                await Authenticate();

                response = await _httpClient.SendAsync(request);
                responseBytes = await response.Content.ReadAsByteArrayAsync();

                if (response.StatusCode >= HttpStatusCode.BadRequest)
                    throw new Exception("[Azure] " + response.ReasonPhrase);
            }

            return new VoiceDataResponse { rawData = responseBytes };
        }

        void ICloudService.SetSettings(VoiceServiceInitData value)
        {
            SetAPIKey(value.APIKey);
            SetRegion(value.Region);
        }

        private VoiceDataResponse ResponseDataInternalToResponseData(VoiceDataResponseInternal data)
        {
            return new VoiceDataResponse { rawData = data.DisplayText };
        }

        private void SetAPIKey(string value) => _apiKey = value;

        private void SetRegion(string value) => _region = value;

        #endregion
    }
}