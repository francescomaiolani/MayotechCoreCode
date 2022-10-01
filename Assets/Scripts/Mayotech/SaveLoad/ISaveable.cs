using System.Collections.Generic;

namespace Mayotech.SaveLoad
{
    public interface ISaveable
    {
        Dictionary<string, object> CollectSaveData();
    }

    public interface ILoadable
    {
        Dictionary<string, object> LoadedObject { get; set; }
        HashSet<string> CollectLoadData();
    }
}