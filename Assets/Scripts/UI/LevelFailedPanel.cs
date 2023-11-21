using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual.UI
{
    public class LevelFailedPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _retryLevelButton;

        private void OnEnable()
        {
            _retryLevelButton.onClick.AddListener(OnRetryLevelButtonClicked);  
        }

        private void OnDisable()
        {
            _retryLevelButton.onClick.RemoveListener(OnRetryLevelButtonClicked);  
        }

        public void Initialize()
        {
            gameObject.SetActive(true);
        }
        
        private void OnRetryLevelButtonClicked()
        {
            EventManager.TriggerEvent(Event.GoToNextLevel);
            gameObject.SetActive(false);
        }
    }
}