using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual
{
    public class PlayerMinion : Minion
    {
        protected new void Awake()
        {
            base.Awake();
            _collider.tag = Constants.TAG_PLAYER_MINION;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Constants.TAG_FINISH) && Singleton.Instance.GameManager.Status == GameStatus.Playing)
                EventManager.TriggerEvent(Event.CompleteLevel);
        }
    }
}