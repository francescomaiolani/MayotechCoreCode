using System;
using Cysharp.Threading.Tasks;

namespace Mayotech.CloudCode
{
    public interface ICloudCodeRequest<TResponse>
    {
        ICloudCodeRequest<TResponse> AddSuccessCallback(Action<TResponse> onSuccess);
        ICloudCodeRequest<TResponse> AddFailCallback(Action<Exception> onFail);
        ICloudCodeRequest<TResponse> AddFinallyCallback(Action onFinally);
        ICloudCodeRequest<TResponse> AddObject(string key, object value);
        ICloudCodeRequest<TResponse> AddFloat(string key, float value);
        ICloudCodeRequest<TResponse> AddInt(string key, int value);
        ICloudCodeRequest<TResponse> AddJson(string key, string value);
        ICloudCodeRequest<TResponse> AddString(string key, string value);
        ICloudCodeRequest<TResponse> AddBool(string key, bool value);
        UniTask Call();
    }
}