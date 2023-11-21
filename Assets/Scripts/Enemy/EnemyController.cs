using System;
using System.Collections.Generic;
using DG.Tweening;
using IdrisDindar.HyperCasual;
using IdrisDindar.HyperCasual.Managers;
using TMPro;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;

public class EnemyController : MonoBehaviour
{
    [SerializeField] 
    private EnemyMinion _enemyMinionPrefab;
    [SerializeField]
    private Transform _enemyMinionContainer;
    [SerializeField]
    private TextMeshProUGUI _enemyCountText;
    [SerializeField]
    private EnemyDataSO _enemyData;
    
    private ObjectPool<EnemyMinion> _enemyMinionPool;
    private List<EnemyMinion> _enemyMinions = new List<EnemyMinion>();

    private Delegate _enemyMinionKilledHandler;

    private bool _shouldAttack;
    private bool _isEncountered;

    private void Awake()
    {
        _enemyMinionPool = new ObjectPool<EnemyMinion>(_enemyMinionPrefab, 20, _enemyMinionContainer);
        _enemyMinionKilledHandler = new Action<EnemyMinion>(OnEnemyMinionKilled);
    }

    private void OnEnable()
    {
        EventManager.RegisterEvent(Event.KillEnemyMinion, _enemyMinionKilledHandler);
    }

    private void OnDisable()
    {
        EventManager.UnregisterEvent(Event.KillEnemyMinion, _enemyMinionKilledHandler);
    }

    private void Start()
    {
        InitializeMinions();
        UpdateVisuals();
    }

    private void Update()
    {
        if(!_shouldAttack)
            return;

        var targetPos = Player.Instance.transform.position;
        _enemyMinionContainer.transform.position = Vector3.MoveTowards(_enemyMinionContainer.transform.position, targetPos, Time.deltaTime * 10);
    }

    private void InitializeMinions()
    {
        var minionCount = UnityEngine.Random.Range(_enemyData.MinMinionCount, _enemyData.MaxMinionCount);
        for (int i = 0; i < minionCount; i++)
        {
            var minion = _enemyMinionPool.Get();
            minion.Controller = this;
            _enemyMinions.Add(minion);
            minion.transform.localPosition = GetMinionPosition(i);
            minion.enabled = true;
            minion.gameObject.SetActive(true);
        }
    }
    
    private Vector3 GetMinionPosition(int index) 
    {
        float goldenAngle = Mathf.PI * (3 - Mathf.Sqrt(5)); // Golden angle in radians
        float r = Mathf.Sqrt(index) * Player.Instance.Data.DistanceFactor;
        float theta = index * goldenAngle;

        float x = r * Mathf.Cos(theta);
        float z = r * Mathf.Sin(theta);

        Vector3 position = new Vector3(x, 0, z);
        return position;
    }

    private void OnEnemyMinionKilled(EnemyMinion minion)
    {
        if(minion.Controller != this)
            return;
        
        PlaceMinions();
        _enemyMinions.Remove(minion);
        _enemyMinionPool.Return(minion);
        UpdateVisuals();
        
        if(_enemyMinions.Count == 0)
            EventManager.TriggerEvent(Event.DefeatEnemy);
    }
    
    public void PlaceMinions()
    {
        for (int i = 0; i < _enemyMinions.Count; i++)
        {
            var position = GetMinionPosition(i);
            _enemyMinions[i].transform
                .DOLocalMove(position, 0.5f)
                .SetEase(Ease.OutBack);
        }
    }

    private void UpdateVisuals()
    {
        _enemyCountText.text = $"{_enemyMinions.Count}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER_MINION) && !_isEncountered)
        {
            _isEncountered = true;
            EventManager.TriggerEvent(Event.EncounterEnemy, this);
            _shouldAttack = true;
            SetAnimationSpeed(10);
        }
    }
    
    private void SetAnimationSpeed(float speed)
    {
        foreach (var enemyMinion in _enemyMinions)
        {
            enemyMinion.Animator.SetFloat(Constants.ANIM_SPEED, speed);
        }
    }
}
