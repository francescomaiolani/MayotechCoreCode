using Newtonsoft.Json.Linq;

namespace Mayotech.SaveLoad
{
    public interface ISaveable
    {
        JObject CollectSaveData();
        string Key { get; }
    }
}