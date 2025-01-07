using System;

namespace AICore.Infrastructure.Azure.Data
{
    [Serializable]
    public class VoiceDataResponse
    {
        public object rawData;
        public int instanceId;
    
        public T GetData<T>()
        {
            return (T)rawData;
        }
    }
}