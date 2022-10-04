using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using UnityEngine;

namespace Mayotech.Player
{
    [CreateAssetMenu(menuName = "Manager/PlayerManager")]
    public class PlayerManager : Service
    {
        [SerializeField] private GameEvent onPlayerSignedIn;
        [SerializeReference] private List<PlayerData> playerData;
        
        private Dictionary<StringVariable, PlayerData> playerDataDictionary = new();

        public override void InitService()
        {
            playerData.ForEach(item => playerDataDictionary.Add(item.MappedVariable, item));
            onPlayerSignedIn.Subscribe(OnPlayerSignedIn);
        }

        private void OnDestroy() => onPlayerSignedIn.Unsubscribe(OnPlayerSignedIn);

        protected void OnPlayerSignedIn()
        {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            Debug.Log($"PlayerInfo: {JsonConvert.SerializeObject(AuthenticationService.Instance.PlayerInfo)}");
        }

        public T GetPlayerData<T>(StringVariable name)
        {
            var data = playerDataDictionary.TryGetValue(name, out var playerData) ? playerData : null;
            if (data == null)
                throw new Exception($"PlayerData not found: {name}");
            var castedData = (PlayerData<T>) Convert.ChangeType(data, typeof(PlayerData<T>));
            return castedData.Data;
        }
    }

    public class PlayerStats
    {
        public List<PlayerStat> playerStats;
    }

    public abstract class PlayerStat : ScriptableObject { }
}