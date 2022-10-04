using System;
using TMPro;
using UnityEngine;

namespace Mayotech.Toast
{
    [Serializable] 
    public class ToastUI
    {
        [SerializeField] private TextMeshProUGUI toastText;
        
        public void Init(ToastRequest toastRequest)
        {
            toastText.text = toastRequest.TextKey;
            AnimateEnter();
        }

        private void AnimateEnter()
        {
        }
    }
}