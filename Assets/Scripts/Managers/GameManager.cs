using System;
using GameAnalyticsSDK;
using UnityEngine;

namespace IdrisDindar.HyperCasual.Managers
{
    public enum GameStatus
    {
        OnLobby,
        Playing,
        Failed,
        Completed
    }
    public class GameManager : Manager
    {
        public GameStatus Status { get; private set; }

        private Delegate _levelStartedHandler;
        private Delegate _levelCompletedHandler;
        private Delegate _levelFailedHandler;
        private Delegate _retryLevelHandler;
        private Delegate _goToNextLevelHandler;

        private void Awake()
        {
            _levelStartedHandler = new Action(OnLevelStarted);
            _levelCompletedHandler = new Action(OnLevelCompleted);
            _levelFailedHandler = new Action(OnLevelFailed);
            _retryLevelHandler = new Action(OnRetryLevel);
            _goToNextLevelHandler = new Action(OnGoToNextLevel);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.StartLevel, _levelStartedHandler);
            EventManager.RegisterEvent(Event.GoToNextLevel, _goToNextLevelHandler);
            EventManager.RegisterEvent(Event.RetryLevel, _retryLevelHandler);
            EventManager.RegisterEvent(Event.CompleteLevel, _levelCompletedHandler);
            EventManager.RegisterEvent(Event.FailLevel, _levelFailedHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.StartLevel, _levelStartedHandler);
            EventManager.UnregisterEvent(Event.GoToNextLevel, _goToNextLevelHandler);
            EventManager.UnregisterEvent(Event.RetryLevel, _retryLevelHandler);
            EventManager.UnregisterEvent(Event.CompleteLevel, _levelCompletedHandler);
            EventManager.UnregisterEvent(Event.FailLevel, _levelFailedHandler);
        }

        private void Start()
        {
            GameAnalytics.Initialize();
            SetStatus(GameStatus.OnLobby);
        }

        private void OnLevelStarted()
        {
            SetStatus(GameStatus.Playing);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_" + Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel);
        }
        
        private void OnLevelCompleted()
        {
            SetStatus(GameStatus.Completed);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level_" + Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel);
            Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel++;
        }
        private void OnLevelFailed()
        {
            SetStatus(GameStatus.Failed);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level_" + Singleton.Instance.SaveManager.SaveData.Level.CurrentLevel);
        }
        private void OnRetryLevel()
        {
            SetStatus(GameStatus.OnLobby);
        }
        
        private void OnGoToNextLevel()
        {
            SetStatus(GameStatus.OnLobby);
        }
        
        private void SetStatus(GameStatus status)
        {
            this.Status = status;
            EventManager.TriggerEvent(Event.GameStatusChanged, status);
        }
    }
}