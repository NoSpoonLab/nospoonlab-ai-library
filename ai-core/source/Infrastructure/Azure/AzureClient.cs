﻿using System;
using System.Net;
using System.Threading.Tasks;
using AICore.Infrastructure.Azure.Data;
using AICore.Services.Interfaces;
using AICore.Services.Types.Data;
using Newtonsoft.Json;
using RestSharp;

namespace AICore.Infrastructure.Azure
{
    public class AzureClient : ICloudService
    {
        #region Properties

        private string _region;
        private string _apiKey;
        private string _authToken;

        #endregion

        #region Initialization

        public AzureClient() => _authToken = String.Empty;

        #endregion

        #region Methods

        private async Task Authenticate()
        {
            if(_authToken != string.Empty) return;
            
            var client = new RestClient("https://"+ _region + ".api.cognitive.microsoft.com/sts/v1.0/");
            var request = new RestRequest("issuetoken", Method.Post);
            request.AddHeader("Ocp-Apim-Subscription-Key", _apiKey);
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode >= HttpStatusCode.BadRequest) throw new Exception(response.Content);
            
            _authToken = response.Content;
        }

        async Task<VoiceDataResponse> ICloudService.SendSpeechToTextRequest(byte[] data, string locale)
        {
            if(_apiKey == string.Empty) throw new Exception("[Azure] Subscription Key not set");
            if(_region == string.Empty) throw new Exception("[Azure] Region not set");
            await Authenticate();
            if(_authToken == string.Empty) throw new Exception("[Azure] Authentication failed");
            
            
            var client = new RestClient("https://" + _region + ".stt.speech.microsoft.com");
            var request = new RestRequest("speech/recognition/conversation/cognitiveservices/v1?language=" + locale, Method.Post);
            request.AddHeader("Authorization", "Bearer " + _authToken);
            request.AddHeader("Content-Type", "audio/wav; codecs=audio/pcm; samplerate=16000");
            request.AddParameter("application/octet-stream", data, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);
            
            if(response.StatusCode >= HttpStatusCode.BadRequest) throw new Exception("[Azure] " + response.Content);
            var responseDataInternal = JsonConvert.DeserializeObject<VoiceDataResponseInternal>(response.Content);
            return ResponseDataInternalToResponseData(responseDataInternal); 
        }

#pragma warning disable CS1066
        async Task<VoiceDataResponse> ICloudService.SendTextToSpeechRequest(string data, string voiceName, string locale, string style = "")
#pragma warning restore CS1066
        {
            var currentVoiceName = voiceName;
            if(voiceName == string.Empty) throw new Exception("[Azure] VoiceName not set");
            if(_region == string.Empty) throw new Exception("[Azure] Region not set");
            if(_apiKey == string.Empty) throw new Exception("[Azure] Subscription Key not set");
            await Authenticate();
            if(_authToken == string.Empty) throw new Exception("[Azure] Authentication failed");
            
            var client = new RestClient("https://" + _region + ".tts.speech.microsoft.com");
            var request = new RestRequest("cognitiveservices/v1", Method.Post);
            request.AddHeader("Authorization", "Bearer " + _authToken);
            request.AddHeader("Content-Type", "application/ssml+xml");
            request.AddHeader("X-Microsoft-OutputFormat", "riff-8khz-16bit-mono-pcm");
            
            var withoutStyle = "<speak version='1.0' xml:lang='"+ locale + "'><voice name='" + currentVoiceName + "'>"+ data + "</voice></speak>";
            var withStyle = "<speak xmlns='http://www.w3.org/2001/10/synthesis' xmlns:mstts='http://www.w3.org/2001/mstts' xmlns:emo='http://www.w3.org/2009/10/emotionml' version='1.0' xml:lang='"+ locale + "'><voice name='" + currentVoiceName + "'><mstts:express-as style='" + style + "'>" + data + "</mstts:express-as></voice></speak>";
            
            request.AddParameter("application/ssml+xml", string.IsNullOrEmpty(style) ? withoutStyle : withStyle , ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);
            
            try
            {
                if(response.StatusCode >= HttpStatusCode.BadRequest) throw new Exception("[Azure] " + response.Content);
            }
            catch
            {
                _authToken = string.Empty;
                await Authenticate();
                response = await client.ExecuteAsync(request);
                if(response.StatusCode >= HttpStatusCode.BadRequest) throw new Exception("[Azure] " + response.Content);
            }

            return new VoiceDataResponse
            {
                rawData = response.RawBytes
            };
        }

        void ICloudService.SetSettings(VoiceServiceInitData value)
        {
            SetAPIKey(value.APIKey);
            SetRegion(value.Region);
        }

        private VoiceDataResponse ResponseDataInternalToResponseData(VoiceDataResponseInternal data)
        {
            return new VoiceDataResponse
            {
                rawData = data.DisplayText
            };
        }

        private void SetAPIKey(string value) => _apiKey = value;
        private void SetRegion(string value) => _region = value;
        
        #endregion
    }
}