using UnityEngine;
using UnityEngine.Serialization;

namespace IdrisDindar.HyperCasual
{
    [CreateAssetMenu(fileName = "GateData", menuName = "Data/Create Gate Data", order = 1)]
    public class GateDataSO : ScriptableObject
    {
        [FormerlySerializedAs("PositiveGateMaterial")]
        public Material PositiveMaterial;
        [FormerlySerializedAs("NegativeGateMaterial")]
        public Material NegativeMaterial;
        
        public int MaxSumAmount = 100;
        public int MinSumAmount = 20;
        
        public int MaxSubtractionAmount = 50;
        public int MinSubtractionAmount = 10;
        
        public int MaxMultiplicationAmount = 5;
        public int MinMultiplicationAmount = 2;
    }
}