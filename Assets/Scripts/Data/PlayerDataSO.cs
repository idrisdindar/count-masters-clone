using UnityEngine;

namespace IdrisDindar.HyperCasual
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player/Create Player Data", order = 0)]
    public class PlayerDataSO : ScriptableObject
    {
        public float CustomPlayerSpeed = 10.0f;
        public float AccelerationSpeed = 10.0f;
        public float DecelerationSpeed = 20.0f;
        public float HorizontalSpeedFactor = 0.5f;
        public bool AutoMoveForward = true;
        public float DistanceFactor = 0.33f;
    }
}