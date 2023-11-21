using System;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual.UI
{
    public class GameUI : BaseUI
    {
        [SerializeField]
        private LevelCompletedPanel _levelCompletedPanel;
        [SerializeField]
        private LevelFailedPanel _levelFailedPanel;
        
        private Delegate _gameCompletedHandler;
        private Delegate _gameFailedHandler;
        private void Awake()
        {
            _gameCompletedHandler = new Action(OnGameCompleted);
            _gameFailedHandler = new Action(OnGameFailed);
        }
        
        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.CompleteLevel, _gameCompletedHandler);
            EventManager.RegisterEvent(Event.FailLevel, _gameFailedHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.CompleteLevel, _gameCompletedHandler);
            EventManager.UnregisterEvent(Event.FailLevel, _gameFailedHandler);
        }
        
        private void OnGameCompleted()
        {
            _levelCompletedPanel.Initialize();
        }
        
        private void OnGameFailed()
        {
            _levelFailedPanel.Initialize();
        }
    }
}