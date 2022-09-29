using System;
using System.Collections.Generic;
using UnityEngine;

public class ServicesManager : SingletonPersistent<ServicesManager>
{
    [SerializeField] private List<Service> services;

    private void Start()
    {
        InitServices();
    }

    public void InitServices()
    {
        foreach (var service in services)
        {
            service.InitService();
        }
    }
}
