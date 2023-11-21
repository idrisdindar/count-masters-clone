using System;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual
{
    public class PlayerMovementController : MonoBehaviour
    {
        private Player _player;
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        private Vector3 _lastPosition;
        private Vector3 _startPosition = Vector3.zero;
        private bool _hasInput;
        private bool _shouldMove = true;
        private bool _shouldAttack;
        private float _maxXPosition = 5;
        private float _XPos;
        private float _ZPos;
        private float _targetPosition;
        private float _speed;
        private float _targetSpeed;

        private EnemyController _target;

        private Delegate _setDeltaPositionHandler;
        private Delegate _resetSpeedHandler;
        private Delegate _adjustSpeedHandler;
        private Delegate _cancelMovementHandler;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
            _setDeltaPositionHandler = new Action<float>(SetDeltaPosition);
            _adjustSpeedHandler = new Action<float>(AdjustSpeed);
            _resetSpeedHandler = new Action(ResetSpeed);
            _cancelMovementHandler = new Action(CancelMovement);
            
            Initialize();
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.SetDeltaPosition, _setDeltaPositionHandler);
            EventManager.RegisterEvent(Event.CancelMovement, _cancelMovementHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.SetDeltaPosition, _setDeltaPositionHandler);
            EventManager.UnregisterEvent(Event.CancelMovement, _cancelMovementHandler);
        }

        private void Initialize()
        {
            _startPosition = transform.position;
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            ResetSpeed();
        }

        private void AdjustSpeed(float speed)
        {
            _targetSpeed = speed;
            _targetSpeed = Mathf.Max(0.0f, _targetSpeed);
        }
        
        private void ResetSpeed()
        {
            _speed = 0.0f;
            _targetSpeed = _player.Data.CustomPlayerSpeed;
        }

        private void SetDeltaPosition(float normalizedDeltaPosition)
        {
            float fullWidth = _maxXPosition * 2.0f;
            _targetPosition += normalizedDeltaPosition * fullWidth;
            _targetPosition = Mathf.Clamp(_targetPosition, -_maxXPosition, _maxXPosition);
            _hasInput = true;

        }
        
        public void SetMaxXPosition(float levelWidth)
        {
            _maxXPosition = levelWidth * 5;
        }

        public void CancelMovement()
        {
            _hasInput = false;
        }

        public void EnableMovement(bool isEnabled)
        {
            _shouldMove = isEnabled;
        }
        
        public void Attack(EnemyController target)
        {
            _target = target;
            _shouldAttack = true;
        }

        public void OnEnemyDefeated()
        {
            _shouldAttack = false;
        }

        public void ResetPlayer()
        {
            transform.position = _startPosition;
            _XPos = 0.0f;
            _ZPos = _startPosition.z;
            _targetPosition = 0.0f;
            _lastPosition = transform.position;
            _hasInput = false;
            _shouldAttack = false;
            
            EnableMovement(true);
            ResetSpeed();
        }

        private void Update()
        {
            if (Singleton.Instance.GameManager.Status != GameStatus.Playing)
                return;
            
            if(!_shouldMove)
                return;

            if (_shouldAttack)
            {
                var targetPos = _target.transform.position;
                _target.transform.position = Vector3.MoveTowards(targetPos, transform.position, Time.deltaTime * _speed * _player.Data.HorizontalSpeedFactor);
                return;
            }
            
            float deltaTime = Time.deltaTime;

            if (!_player.Data.AutoMoveForward && !_hasInput)
            {
                Decelerate(deltaTime, 0.0f);
            }
            else if(_targetSpeed < _speed)
            {
                Decelerate(deltaTime, _targetSpeed);
            }
            else if(_targetSpeed > _speed)
            {
                Accelerate(deltaTime, _targetSpeed);
            }
            
            float speed = _speed * deltaTime;
            
            _ZPos += speed;

            if (_hasInput)
            {
                var horizontalSpeed = speed * _player.Data.HorizontalSpeedFactor;
                var newPositionTarget = Mathf.Lerp(_XPos, _targetPosition, horizontalSpeed);
                var newPositionDifference = newPositionTarget - _XPos;

                newPositionDifference = Mathf.Clamp(newPositionDifference, -horizontalSpeed, horizontalSpeed);
                _XPos += newPositionDifference;
            }

            _player.MinionController.SetAnimationSpeed(_speed);
            
            transform.position = new Vector3(_XPos, transform.position.y, _ZPos);

            _lastPosition = transform.position;
        }

        private void Accelerate(float deltaTime, float targetSpeed)
        {
            _speed += deltaTime * _player.Data.AccelerationSpeed;
            _speed = Mathf.Min(_speed, targetSpeed);
        }
        
        private void Decelerate(float deltaTime, float targetSpeed)
        {
            _speed -= deltaTime * _player.Data.DecelerationSpeed;
            _speed = Mathf.Max(_speed, targetSpeed);
        }

        private bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x,b.x) && Mathf.Approximately(a.y,b.y) && Mathf.Approximately(a.z,b.z);
        }
    }
}