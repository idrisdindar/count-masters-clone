using System;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using UnityEngine.UI;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual.UI
{
    public class LevelCompletedPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _nextLevelButton;

        private void OnEnable()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);  
        }

        private void OnDisable()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);  
        }

        public void Initialize()
        {
            gameObject.SetActive(true);
        }
        
        private void OnNextLevelButtonClicked()
        {
            EventManager.TriggerEvent(Event.GoToNextLevel);
            gameObject.SetActive(false);
        }
    }
}