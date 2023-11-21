using System;
using DG.Tweening;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual
{
    public class Player : MonoBehaviour
    {
        [SerializeField] 
        private PlayerDataSO data;
        
        private static Player _instance;
        public static Player Instance => _instance;
        public PlayerDataSO Data => data;
        public PlayerMovementController MovementController { get; private set; }
        
        public PlayerMinionController MinionController { get; private set; }
        
        private Delegate _levelStartedHandler;
        private Delegate _levelFailedHandler;
        private Delegate _levelCompletedHandler;
        private Delegate _resetPlayerHandler;
        private Delegate _encounterEnemyHandler;
        private Delegate _defeatEnemyHandler;
        
        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            MovementController = GetComponent<PlayerMovementController>();
            MinionController = GetComponent<PlayerMinionController>();

            _levelStartedHandler = new Action(OnLevelStarted);
            _resetPlayerHandler = new Action(OnResetPlayer);
            _levelFailedHandler = new Action(OnLevelFailed);
            _levelCompletedHandler = new Action(OnLevelCompleted);
            _encounterEnemyHandler = new Action<EnemyController>(OnEncounterEnemy);
            _defeatEnemyHandler = new Action(OnDefeatEnemy);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.StartLevel, _levelStartedHandler);
            EventManager.RegisterEvent(Event.LevelCreated, _resetPlayerHandler);
            EventManager.RegisterEvent(Event.FailLevel, _levelFailedHandler);
            EventManager.RegisterEvent(Event.CompleteLevel, _levelCompletedHandler);
            EventManager.RegisterEvent(Event.EncounterEnemy, _encounterEnemyHandler);
            EventManager.RegisterEvent(Event.DefeatEnemy, _defeatEnemyHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.StartLevel, _levelStartedHandler);
            EventManager.UnregisterEvent(Event.LevelCreated, _resetPlayerHandler);
            EventManager.UnregisterEvent(Event.FailLevel, _levelFailedHandler);
            EventManager.UnregisterEvent(Event.CompleteLevel, _levelCompletedHandler);
            EventManager.UnregisterEvent(Event.EncounterEnemy, _encounterEnemyHandler);
            EventManager.UnregisterEvent(Event.DefeatEnemy, _defeatEnemyHandler);
        }

        private void OnLevelStarted()
        {
            MovementController.EnableMovement(true);
            MinionController.PlaceMinions();
        }

        private void OnLevelFailed()
        {
            MovementController.EnableMovement(false);
        }

        private void OnLevelCompleted()
        {
            MovementController.EnableMovement(false);
        }

        private void OnResetPlayer()
        {
            MinionController.ResetMinions();
            MovementController.ResetPlayer();
        }
        
        private void OnDefeatEnemy()
        {
            MovementController.EnableMovement(true);
            MovementController.OnEnemyDefeated();
        }

        private void OnEncounterEnemy(EnemyController controller)
        {
            MovementController.EnableMovement(false);
            MovementController.Attack(controller);
        }
    }
}
