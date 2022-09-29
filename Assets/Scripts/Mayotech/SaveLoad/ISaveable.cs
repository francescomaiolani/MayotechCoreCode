using System.Collections.Generic;

namespace Mayotech.SaveLoad
{
    public interface ISaveable
    {
        Dictionary<string, object> CollectSaveData();
    }
}