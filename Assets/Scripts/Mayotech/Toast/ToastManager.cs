using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mayotech.Toast
{
    [CreateAssetMenu(fileName = "ToastManager", menuName = "Manager/ToastManager")]
    public class ToastManager : Service
    {
        [SerializeField] private ToastUI toastUI;
        [SerializeField] private int toastMaxQueue;
        
        protected Queue<ToastRequest> toastRequests = new();

        public override void InitService() { }

        public void EnqueueToastRequest(ToastRequest request)
        {
            if (toastRequests.Count > toastMaxQueue)
                return;
            toastRequests.Enqueue(request);
            CheckNextRequest();
        }

        private void CheckNextRequest()
        {
            var request = toastRequests.Peek();
            if (request != null)
            {
                ShowToast(request);
            }
            
        }

        public void ShowToast(ToastRequest toastRequest)
        {
            
        }
    }
}


