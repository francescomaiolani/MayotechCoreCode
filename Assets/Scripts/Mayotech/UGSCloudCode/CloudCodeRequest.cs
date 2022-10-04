using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudCode;
using UnityEngine;

namespace Mayotech.CloudCode
{
    [Serializable]
    public abstract class CloudCodeRequest<TResponse> : ICloudCodeRequest<TResponse> where TResponse : CloudCodeResponse
    {
        protected ICloudCodeService CloudCode => CloudCodeService.Instance;

        protected Action<TResponse> onSuccess;
        protected Action<Exception> onFail;
        protected Action onFinally;

        public virtual string RpcName { get; }
        protected Dictionary<string, object> Parameters { get; } = new();
        
        public ICloudCodeRequest<TResponse> AddSuccessCallback(Action<TResponse> onSuccess)
        {
            this.onSuccess = onSuccess;
            return this;
        }
        
        public ICloudCodeRequest<TResponse> AddFailCallback(Action<Exception> onFail)
        {
            this.onFail = onFail;
            return this;
        }
        
        public ICloudCodeRequest<TResponse> AddFinallyCallback(Action onFinally)
        {
            this.onFinally = onFinally;
            return this;
        }
 
        public ICloudCodeRequest<TResponse> AddObject(string key, object value) =>
            AddJson(key, JsonConvert.SerializeObject(value));

        public ICloudCodeRequest<TResponse> AddFloat(string key, float value) => AddValue(key, value);

        public ICloudCodeRequest<TResponse> AddInt(string key, int value) => AddValue(key, value);

        public ICloudCodeRequest<TResponse> AddJson(string key, string value) => AddValue(key, value);

        public ICloudCodeRequest<TResponse> AddString(string key, string value) => AddValue(key, value);

        public ICloudCodeRequest<TResponse> AddBool(string key, bool value) => AddValue(key, value);

        private ICloudCodeRequest<TResponse> AddValue<T>(string key, T value)
        {
            Parameters.Add(key, value);
            return this;
        }

        public async UniTask Call()
        {
            try
            {
                var response = await CloudCode.CallEndpointAsync<TResponse>(RpcName, Parameters);
                var hasError = response.HasErrors;
                if (hasError)
                {
                    Debug.LogError($"{GetType()} has errors. Code: {response.Error.ErrorCode}, Message: {response.Error.ErrorMessage}");
                    onFail?.Invoke(new Exception($"Code: {response.Error.ErrorCode}, Message: {response.Error.ErrorMessage}"));
                }
                else
                    onSuccess?.Invoke(response);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
            finally
            {
                onFinally?.Invoke();
            }
        }
    }
}