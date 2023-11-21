using System;
using IdrisDindar.HyperCasual.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual.UI
{
    public class LobbyUI : BaseUI
    {
        [SerializeField]
        private Button _startGameButton;

        [SerializeField]
        private TextMeshProUGUI _levelText;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
        }

        private void OnStartGameButtonClicked()
        {
            EventManager.TriggerEvent(Event.StartLevel);
        }
    }
}