using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudCode;
using UnityEngine;

namespace Mayotech.CloudCode
{
    [CreateAssetMenu(fileName = "CloudCodeManager", menuName = "Manager/CloudCodeManager")]
    public class CloudCodeManager : Service
    {
        private ICloudCodeService CloudCode => CloudCodeService.Instance;

        public override void InitService() { }

        public void SendRequest<TRequest, TResponse>(TRequest request, Action<TResponse> onSuccess = null,
            Action<Exception> onFail = null, Action onFinally = null)
            where TRequest : CloudCodeRequest<TResponse> where TResponse : CloudCodeResponse
        {
            request.AddSuccessCallback(onSuccess);
            request.AddFailCallback(onFail);
            request.AddFinallyCallback(onFinally);
            request.Call();
        }

        public async UniTask SendTestRequest()
        {
            await new TestRequest()
                .AddSuccessCallback(response =>
                {
                    Debug.Log($"request Success: {JsonConvert.SerializeObject(response)}");
                })
                .AddFailCallback(exception => { Debug.Log($"request failed: {exception}"); })
                .AddFinallyCallback(() => { Debug.Log($"finally"); })
                .AddJson("data", "{}")
                .Call();
        }
    }
}