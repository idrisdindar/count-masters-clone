using System;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;
using Random = UnityEngine.Random;

namespace IdrisDindar.HyperCasual
{
    public abstract class Minion : MonoBehaviour
    {
        protected Collider _collider;
        private Animator _animator;
        public Animator Animator => _animator;
        
        protected void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponentInChildren<Collider>();
        }

        private void OnEnable()
        {
            var speed = Random.Range(0.8f, 1.2f);
            _animator.SetFloat(Constants.ANIM_SPEED_MULTIPLIER, speed);
        }
    }
}