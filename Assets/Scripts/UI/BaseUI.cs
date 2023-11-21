using System;
using UnityEngine;

namespace IdrisDindar.HyperCasual.UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        public Canvas Canvas { get; private set; }

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void SetEnabled(bool isEnabled)
        {
            if(Canvas == null)
                Canvas = GetComponent<Canvas>();
            
            Canvas.enabled = isEnabled;
        }
    }
}