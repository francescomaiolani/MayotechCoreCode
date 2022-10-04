using Newtonsoft.Json;
using Unity.Services.Authentication;
using UnityEngine;

namespace Mayotech.Player
{
    /// <summary>
    /// Class that contains all the information related to the player, like ids, stats, ecc...
    /// </summary>
    [CreateAssetMenu(menuName = "Manager/PlayerManager")]
    public class PlayerManager : Service
    {
        [SerializeField] protected GameEvent onPlayerSignedIn;

        [Header("Fields")]
        [SerializeField] protected StringVariable playerId;
        [SerializeField] protected StringVariable sessionToken;

        public string PlayerId => playerId.Value;
        public string SessionToken => sessionToken.Value;

        public override void InitService()
        {
            onPlayerSignedIn.Subscribe(OnPlayerSignedIn);
        }

        protected void OnDestroy() => onPlayerSignedIn.Unsubscribe(OnPlayerSignedIn);

        protected void OnPlayerSignedIn()
        {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            Debug.Log($"PlayerInfo: {JsonConvert.SerializeObject(AuthenticationService.Instance.PlayerInfo)}");
            playerId.Value = AuthenticationService.Instance.PlayerId;
            sessionToken.Value = AuthenticationService.Instance.AccessToken;
        }
    }
}