using System.Collections.Generic;

namespace Mayotech.SaveLoad
{
    public interface ILoadable
    {
        string Key { get; }
        void SubscribeLoad();
        void OnDataLoaded(Dictionary<string, string> data);
    }
}