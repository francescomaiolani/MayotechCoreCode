using UnityEngine;

public interface IService
{
    void InitService();
}

public abstract class Service : ScriptableObject, IService
{
    public abstract void InitService();
}