using System;
using System.Collections.Generic;
using UnityEngine;

public class ServicesManager : SingletonPersistent<ServicesManager>
{
    [SerializeField] private List<Service> services;

    private void Start() => InitServices();

    private void InitServices()
    {
        foreach (var service in services)
        {
            try 
            {      
                service.InitService();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
