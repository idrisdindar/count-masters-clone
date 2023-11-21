using System;
using UnityEngine;

namespace IdrisDindar.HyperCasual
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if(_camera == null)
                return;

            this.transform.rotation = _camera.transform.rotation;
        }
    }
}