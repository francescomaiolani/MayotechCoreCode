using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Mayotech.CloudCode
{
    [CreateAssetMenu(fileName = "CloudCodeManager", menuName = "Manager/CloudCodeManager")]
    public class CloudCodeManager : Service
    {
        public override void InitService() { }
        public override bool CheckServiceIntegrity() => true;

        /// <summary>
        /// Sends a CloudCode Request with PREFILLED parameters, with callbacks on post actions 
        /// </summary>
        /// <param name="request">the request object with filled parameters</param>
        /// <param name="onSuccess">callback on success</param>
        /// <param name="onFail">callback on fail</param>
        /// <param name="onFinally">callback on finally</param>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <typeparam name="TResponse">Type of the response</typeparam>
        public void SendRequest<TRequest, TResponse>(TRequest request, Action<TResponse> onSuccess = null,
            Action<Exception> onFail = null, Action onFinally = null)
            where TRequest : ICloudCodeRequest<TResponse> where TResponse : CloudCodeResponse
        {
            request.AddSuccessCallback(onSuccess);
            request.AddFailCallback(onFail);
            request.AddFinallyCallback(onFinally);
            request.Call();
        }

        public async UniTask SendTestRequest()
        {
            var testRequest = new TestRequest().AddJson("data", "{}");
            SendRequest<ICloudCodeRequest<TestResponse>, TestResponse>(testRequest,
                response => { Debug.Log($"request Success: {JsonConvert.SerializeObject(response)}"); },
                exception => { Debug.Log($"request failed: {exception}"); });
        }
    }
}