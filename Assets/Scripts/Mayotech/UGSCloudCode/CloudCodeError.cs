using System;
using Newtonsoft.Json;

namespace Mayotech.CloudCode
{
    [Serializable]
    public class CloudCodeError
    {
        [JsonProperty("errorCode")]
        protected int errorCode;
        [JsonProperty("errorMessage")]
        protected string errorMessage;
        
        public int ErrorCode => errorCode;
        public string ErrorMessage => errorMessage;
    }
}