using System;
using System.Collections;
using System.Collections.Generic;
using Mayotech.Missions;
using Mayotech.Navigation;
using Mayotech.Player;
using Mayotech.Resources;
using Mayotech.SaveLoad;
using UnityEngine;

[Serializable]
public class ServiceLocator : SingletonPersistent<ServiceLocator>
{
    [SerializeField] protected MainCamera mainCamera;
    [SerializeField, AutoConnect] protected NavigationManager navigationManager;
    [SerializeField, AutoConnect] protected MissionManager missionManager;
    [SerializeField, AutoConnect] protected ResourceManager resourceManager;
    [SerializeField, AutoConnect] protected PlayerManager playerManager;
    [SerializeField, AutoConnect] protected AuthenticationManager authenticationManager;
    [SerializeField, AutoConnect] protected SaveManager saveManager;

    public NavigationManager NavigationManager => navigationManager;
    public MissionManager MissionManager => missionManager;
    public ResourceManager ResourceManager => resourceManager;
    public PlayerManager PlayerManager => playerManager;
    public AuthenticationManager AuthenticationManager => authenticationManager;
    public SaveManager SaveManager => saveManager;
    public MainCamera MainCamera => mainCamera;
}
