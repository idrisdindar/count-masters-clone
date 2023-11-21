using System;
using Cinemachine;
using DG.Tweening;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _vCam;

        private Delegate _encounterEnemyHandler;
        private Delegate _defeatEnemyHandler;
        private Delegate _levelFailedHandler;

        private void Awake()
        {
            _encounterEnemyHandler = new Action<EnemyController>(OnEncounterEnemy);
            _defeatEnemyHandler = new Action(OnDefeatEnemy);
            _levelFailedHandler = new Action(OnDefeatEnemy);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.EncounterEnemy, _encounterEnemyHandler);
            EventManager.RegisterEvent(Event.DefeatEnemy, _defeatEnemyHandler);
            EventManager.RegisterEvent(Event.FailLevel, _levelFailedHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.EncounterEnemy, _encounterEnemyHandler);
            EventManager.UnregisterEvent(Event.DefeatEnemy, _defeatEnemyHandler);
            EventManager.UnregisterEvent(Event.FailLevel, _levelFailedHandler);
        }
        
        private void OnEncounterEnemy(EnemyController enemyController)
        {
            DOVirtual.Float(39, 30, 0.5f, x => _vCam.m_Lens.FieldOfView = x);
        }
        
        private void OnDefeatEnemy()
        {
            DOVirtual.Float(30, 39, 0.5f, x => _vCam.m_Lens.FieldOfView = x);
        }
    }
}