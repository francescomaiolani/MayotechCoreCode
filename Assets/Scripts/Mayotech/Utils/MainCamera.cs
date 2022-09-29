using UnityEngine;

public class MainCamera : SingletonPersistent<MainCamera>
{
    [SerializeField] private Camera camera;
    public Camera Camera => camera ??= GetComponent<Camera>();
}
