using UnityEngine;

public interface IService
{
    void InitService();
    bool CheckServiceIntegrity();
}

public abstract class Service : ScriptableObject, IService
{
    public abstract void InitService();

    public abstract bool CheckServiceIntegrity();
}