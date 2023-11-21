using System;
using UnityEngine;

namespace IdrisDindar.HyperCasual.Managers
{
    public class LevelManager : Manager
    {
        [SerializeField]
        private Level[] _levels;
        [SerializeField]
        private Transform _levelContainer;
        
        private Delegate _levelStartedHandler;
        private Delegate _retryLevelHandler;
        private Delegate _goToNextLevelHandler;
        public Level CurrentLevel { get; private set; }
        public int CurrentLevelNumber => Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel;
        
        private void Awake()
        {
            _levelStartedHandler = new Action(OnLevelStarted);
            _retryLevelHandler = new Action(OnRetryLevel);
            _goToNextLevelHandler = new Action(OnGoToNextLevel);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.StartLevel, _levelStartedHandler);
            EventManager.RegisterEvent(Event.RetryLevel, _retryLevelHandler);
            EventManager.RegisterEvent(Event.GoToNextLevel, _goToNextLevelHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.StartLevel, _levelStartedHandler);
            EventManager.UnregisterEvent(Event.RetryLevel, _retryLevelHandler);
            EventManager.UnregisterEvent(Event.GoToNextLevel, _goToNextLevelHandler);
        }

        private void Start()
        {
            Initialize();
        }
        
        protected override void Initialize()
        {
            var currentLevelIndex = Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel;
            CreateLevel(currentLevelIndex);
        }

        private void OnLevelStarted()
        {
            var planeWidth = CurrentLevel.Plane.localScale.x;
            Player.Instance.MovementController.SetMaxXPosition(planeWidth);
        }

        private void OnRetryLevel()
        {
            ClearLevel();
            var currentLevelIndex = Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel;
            CreateLevel(currentLevelIndex);
        }
        
        private void OnGoToNextLevel()
        {
            ClearLevel();
            var currentLevelIndex = ++Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel;
            CreateLevel(currentLevelIndex + 1);
        }

        private void CreateLevel(int levelIndex)
        {
            CurrentLevel = Instantiate(_levels[levelIndex % _levels.Length], _levelContainer);
            EventManager.TriggerEvent(Event.LevelCreated);
        }
        private void ClearLevel()
        {
            Destroy(CurrentLevel.gameObject);
        }
    }
}