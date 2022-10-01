using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/ParameterlessEvent")]
public class GameEvent : ScriptableObject
{
    protected List<Action> subscribedActions = new List<Action>();
    
    public void Subscribe(Action callback) => subscribedActions.Add(callback);
    
    public void Unsubscribe(Action callback) => subscribedActions.Remove(callback);

    public void RaiseEvent()
    {
        for (var i = subscribedActions.Count - 1; i >= 0; i--) 
            subscribedActions[i]?.Invoke();
    }
}

public abstract class GameEvent<T> : ScriptableObject
{
    protected List<Action<T>> subscribedActions = new();

    public void Subscribe(Action<T> callback) => subscribedActions.Add(callback);
    
    public void Unsubscribe(Action<T> callback) => subscribedActions.Remove(callback);

    public void RaiseEvent(T data)
    {
        for (var i = subscribedActions.Count - 1; i >= 0; i--) 
            subscribedActions[i]?.Invoke(data);
    }
    
}

public abstract class GameEvent<T1, T2> : ScriptableObject
{
    protected List<Action<T1, T2>> subscribedActions = new List<Action<T1, T2>>();

    public void Subscribe(Action<T1, T2> callback) => subscribedActions.Add(callback);
    
    public void Unsubscribe(Action<T1, T2> callback) => subscribedActions.Remove(callback);

    public void RaiseEvent(T1 data1, T2 data2)
    {
        for (var i = subscribedActions.Count - 1; i >= 0; i--) 
            subscribedActions[i]?.Invoke(data1, data2);
    }
}