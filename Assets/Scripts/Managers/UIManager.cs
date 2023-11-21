using System;
using IdrisDindar.HyperCasual.UI;
using UnityEngine;

namespace IdrisDindar.HyperCasual.Managers
{
    public class UIManager : Manager
    {
        [SerializeField]
        private LobbyUI _lobby;

        [SerializeField]
        private GameUI _gameUI;

        private Delegate _gameStatusChangedHandler;

        private void Awake()
        {
            _gameStatusChangedHandler = new Action<GameStatus>(OnGameStatusChanged);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.GameStatusChanged, _gameStatusChangedHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.GameStatusChanged, _gameStatusChangedHandler);
        }
        
        private void OnGameStatusChanged(GameStatus newStatus)
        {
            _gameUI.SetEnabled(newStatus != GameStatus.OnLobby);
            _lobby.SetEnabled(newStatus == GameStatus.OnLobby);
        }
    }
}