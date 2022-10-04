using Mayotech.UGSConfig;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataConfig", menuName = "Config/PlayerDataConfig")]
public class PlayerDataConfig : JSONConfig<PlayerData>
{
    protected override void DeserializeData(JToken data) => SetData(data.ToString());
}
