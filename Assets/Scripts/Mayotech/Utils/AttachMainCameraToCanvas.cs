using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WhatWapp.SceneDirector
{
    public class AttachMainCameraToCanvas : MonoBehaviour
    {
        private MainCamera MainCamera => ServiceLocator.Instance.MainCamera;

        private Canvas canvas;
        private Canvas Canvas => canvas ??= GetComponent<Canvas>();
        
        protected virtual void OnEnable()
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;   
            Canvas.worldCamera = MainCamera.Camera;
        }
    }
}