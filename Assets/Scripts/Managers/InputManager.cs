using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace IdrisDindar.HyperCasual.Managers
{
    public class InputManager : Manager
    {
        [SerializeField]
        private float _inputSensitivity = 1.5f;
        
        private bool _hasInput;
        private Vector3 _inputPosition;
        private Vector3 _previousInputPosition;

        void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        void Update()
        {
            if(Singleton.Instance.GameManager.Status != GameStatus.Playing)
                return;
            
#if UNITY_EDITOR
            _inputPosition = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.isPressed)
            {
                if (!_hasInput)
                {
                    _previousInputPosition = _inputPosition;
                }
                _hasInput = true;
            }
            else
            {
                _hasInput = false;
            }
#else
            if (Touch.activeTouches.Count > 0)
            {
                _inputPosition = Touch.activeTouches[0].screenPosition;

                if (!_hasInput)
                {
                    _previousInputPosition = _inputPosition;
                }
                
                _hasInput = true;
            }
            else
            {
               _hasInput = false;
            }
#endif

            if (_hasInput)
            {
                float normalizedDeltaPosition = (_inputPosition.x - _previousInputPosition.x) / Screen.width * _inputSensitivity;
                EventManager.TriggerEvent(Event.SetDeltaPosition, normalizedDeltaPosition);
            }
            else
            {
                EventManager.TriggerEvent(Event.CancelMovement);
            }

            _previousInputPosition = _inputPosition;
        }
    }
}
