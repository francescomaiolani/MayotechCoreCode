using Mayotech.UGSConfig;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataConfig", menuName = "Config/PlayerDataConfig")]
public class PlayerDataConfig : JSONConfig<PlayerData>
{
    protected override void DeserializeData(string data) => SetData(data);
}
