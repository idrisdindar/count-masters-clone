using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameAnalyticsSDK;
using IdrisDindar.HyperCasual.Managers;
using TMPro;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;
using Random = UnityEngine.Random;

namespace IdrisDindar.HyperCasual
{
    public class PlayerMinionController : MonoBehaviour
    {
        [SerializeField]
        private PlayerMinion _minionPrefab;

        [SerializeField]
        private Transform _minionContainer;
        
        [SerializeField]
        private ParticleSystem _deathParticle;

        [SerializeField]
        private TextMeshProUGUI _minionCountText;
        
        private List<PlayerMinion> _minions = new List<PlayerMinion>();
        private ObjectPool<PlayerMinion> _minionPool;
        private Delegate _gateTriggeredHandler;
        private Delegate _playerMinionKilledHandler;
        

        private void Awake()
        {
            _gateTriggeredHandler = new Action<Gate>(OnGateTriggered);
            _minionPool = new ObjectPool<PlayerMinion>(_minionPrefab, 100, _minionContainer);
            var minion = _minionPool.Get();
            minion.gameObject.SetActive(true);
            _minions.Add(minion);
            _playerMinionKilledHandler = new Action<PlayerMinion>(KillMinion);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.TriggerGate, _gateTriggeredHandler);
            EventManager.RegisterEvent(Event.KillPlayerMinion, _playerMinionKilledHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.TriggerGate, _gateTriggeredHandler);
            EventManager.UnregisterEvent(Event.KillPlayerMinion, _playerMinionKilledHandler);
        }

        public void PlaceMinions()
        {
            for (int i = 0; i < _minions.Count; i++)
            {
                var position = GetMinionPosition(i);
                _minions[i].transform
                    .DOLocalMove(position, 0.5f)
                    .SetEase(Ease.OutBack);
            }
        }

        public void ResetMinions()
        {
            _minionPool.ReturnAll();
            _minions.Clear();
            
            var minion = _minionPool.Get();
            minion.gameObject.SetActive(true);
            _minions.Add(minion);
            UpdateVisuals();
        }
        
        public void SetAnimationSpeed(float speed)
        {
            foreach (var minion in _minions)
            {
                minion.Animator.SetFloat(Constants.ANIM_SPEED, speed);
            }
        }

        private Vector3 GetMinionPosition(int index) 
        {
            float goldenAngle = Mathf.PI * (3 - Mathf.Sqrt(5)); // Golden angle in radians
            float r = Mathf.Sqrt(index) * Player.Instance.Data.DistanceFactor;
            float theta = index * goldenAngle;

            float x = r * Mathf.Cos(theta);
            float z = r * Mathf.Sin(theta);

            Vector3 position = new Vector3(x, 0, z);
            return position;
        }

        private void OnGateTriggered(Gate gate)
        {
            var amount = gate.Amount;
            switch (gate.Operation)
            {
                case GateOperation.Sum:
                    AddMinions(amount);
                    break;
                case GateOperation.Subtraction:
                    KillMinions(amount);
                    break;
                case GateOperation.Multiplication:
                    amount = _minions.Count * (amount - 1);
                    AddMinions(amount);
                    break;
            }
            
            PlaceMinions();
        }

        private void AddMinions(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var minion = _minionPool.Get();
                minion.transform.localPosition = Vector3.zero;
                minion.gameObject.SetActive(true);
                _minions.Add(minion);
            }
            
            UpdateVisuals();
            GameAnalytics.NewDesignEvent($"Level_{Singleton.Instance.LevelManager.CurrentLevelNumber}:Minions:Added", amount);
        }

        private void KillMinions(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (_minions.Count <= 0)
                {
                    EventManager.TriggerEvent(Event.FailLevel);
                    return;
                }

                var randomMinion = _minions[Random.Range(0, _minions.Count)];
                Instantiate(_deathParticle, randomMinion.transform.position, Quaternion.identity);
                _minions.Remove(randomMinion);
                _minionPool.Return(randomMinion);
            }
            
            UpdateVisuals();
            GameAnalytics.NewDesignEvent($"Level_{Singleton.Instance.LevelManager.CurrentLevelNumber}:Minions:Removed", amount);
        }

        private void KillMinion(PlayerMinion minion)
        {
            Instantiate(_deathParticle, minion.transform.position, Quaternion.identity);
            _minions.Remove(minion);
            _minionPool.Return(minion);
            UpdateVisuals();
            PlaceMinions();
            
            if (_minions.Count <= 0)
            {
                EventManager.TriggerEvent(Event.FailLevel);
            }
            
        }

        private void UpdateVisuals()
        {
            _minionCountText.text = $"{_minions.Count}";
        }
        
    }
}