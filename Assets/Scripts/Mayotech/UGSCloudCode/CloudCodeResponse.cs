using System;
using Newtonsoft.Json;

namespace Mayotech.CloudCode
{
    [Serializable]
    public abstract class CloudCodeResponse
    {
        [JsonProperty("error")]
        protected CloudCodeError error;

        public bool HasErrors => error != null;
        
        public CloudCodeError Error => error;
    }
}