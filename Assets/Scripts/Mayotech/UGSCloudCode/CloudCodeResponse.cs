using System;
using Newtonsoft.Json;

namespace Mayotech.CloudCode
{
    public interface ICloudCodeResponse
    {
        CloudCodeError Error { get; }
        bool HasErrors { get; }
    }
    
    [Serializable]
    public abstract class CloudCodeResponse : ICloudCodeResponse
    {
        [JsonProperty("error")]
        protected CloudCodeError error;

        public bool HasErrors => error != null;
        
        public CloudCodeError Error => error;
    }
}