using UnityEngine;

namespace IdrisDindar.HyperCasual
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy/Create Enemy Data")]
    public class EnemyDataSO : ScriptableObject
    {
        public int MinMinionCount = 2;
        public int MaxMinionCount = 20;
    }
}