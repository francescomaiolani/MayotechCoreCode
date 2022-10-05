using System;
using System.Reflection;
using Mayotech.CloudCode;
using Mayotech.Missions;
using Mayotech.Navigation;
using Mayotech.Player;
using Mayotech.Resources;
using Mayotech.SafeTime;
using Mayotech.SaveLoad;
using Mayotech.Settings;
using Mayotech.Store;
using Mayotech.Toast;
using Mayotech.UGSAuthentication;
using Mayotech.UGSConfig;
using Mayotech.UGSEconomy.Currency;
using Mayotech.UGSEconomy.Inventory;
using UnityEngine;

[Serializable]
public class ServiceLocator : SingletonPersistent<ServiceLocator>
{
    [SerializeField] protected MainCamera mainCamera;
    [Header("Services")]
    [SerializeField, AutoConnect] protected NavigationManager navigationManager;
    [SerializeField, AutoConnect] protected MissionManager missionManager;
    [SerializeField, AutoConnect] protected ResourceManager resourceManager;
    [SerializeField, AutoConnect] protected PlayerManager playerManager;
    [SerializeField, AutoConnect] protected AuthenticationManager authenticationManager;
    [SerializeField, AutoConnect] protected SaveManager saveManager;
    [SerializeField, AutoConnect] protected CurrencyManager currencyManager;
    [SerializeField, AutoConnect] protected InventoryManager inventoryManager;
    [SerializeField, AutoConnect] protected StoreManager storeManager;
    [SerializeField, AutoConnect] protected SettingsManager settingsManager;
    [SerializeField, AutoConnect] protected ConfigManager configManager;
    [SerializeField, AutoConnect] protected ToastManager toastManager;
    [SerializeField, AutoConnect] protected CloudCodeManager cloudCodeManager;
    [SerializeField, AutoConnect] protected AnalyticsManager analyticsManager;
    [SerializeField, AutoConnect] protected TimeManager timeManager;
    
    public NavigationManager NavigationManager => navigationManager;
    public MissionManager MissionManager => missionManager;
    public ResourceManager ResourceManager => resourceManager;
    public PlayerManager PlayerManager => playerManager;
    public AuthenticationManager AuthenticationManager => authenticationManager;
    public SaveManager SaveManager => saveManager;
    public MainCamera MainCamera => mainCamera;
    public CurrencyManager CurrencyManager => currencyManager;
    public InventoryManager InventoryManager => inventoryManager;
    public StoreManager StoreManager => storeManager;
    public SettingsManager SettingsManager => settingsManager;
    public ConfigManager ConfigManager => configManager;
    public ToastManager ToastManager => toastManager;
    public CloudCodeManager CloudCodeManager => cloudCodeManager;
    public AnalyticsManager AnalyticsManager => analyticsManager;
    public TimeManager TimeManager => timeManager;

    [Header("Unity Events")]
    [SerializeField] protected GameEvent<bool> onApplicationPause;
    [SerializeField] protected GameEvent onApplicationQuit;


    private void Start()
    {
        var type = typeof(ServiceLocator);
        var fields = type.GetFields( BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var fieldType = field.GetValue(this);
            if (fieldType is IService service)
                service.InitService();
        }
    }

    private void OnApplicationPause(bool pauseStatus) => onApplicationPause.RaiseEvent(pauseStatus);

    private void OnApplicationQuit() => onApplicationQuit.RaiseEvent();
}
