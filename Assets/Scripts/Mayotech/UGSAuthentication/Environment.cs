using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "Environment")]
[Serializable]
public class Environment : ScriptableObject
{
    [SerializeField] private string environmentName;
    [SerializeField] private string environmentId;
    [SerializeField] private bool isDefault;

    public string EnvironmentName => environmentName;
    public string EnvironmentId => environmentId;
    public bool IsDefault => isDefault;
}
