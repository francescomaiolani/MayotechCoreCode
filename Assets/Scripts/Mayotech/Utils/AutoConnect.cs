using UnityEngine;

public class AutoConnect : PropertyAttribute
{
    public string AssetName;
    
    public AutoConnect()
    {
    }

    public AutoConnect(string assetName)
    {
        AssetName = assetName;
    }
}
