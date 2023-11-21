using System;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual
{
    public class EnemyMinion : Minion
    {
        public EnemyController Controller { get; set; }

        [SerializeField]
        private ParticleSystem _deathParticle;

        protected new void Awake()
        {
            base.Awake();
            _collider.tag = Constants.TAG_ENEMY_MINION;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER_MINION))
            {
                Instantiate(_deathParticle, transform.position, Quaternion.identity);
                EventManager.TriggerEvent(Event.KillEnemyMinion, this);
                EventManager.TriggerEvent(Event.KillPlayerMinion, other.GetComponentInParent<PlayerMinion>());
            }
        }
    }
}