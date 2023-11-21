using UnityEngine;

namespace IdrisDindar.HyperCasual
{
    public class Level : MonoBehaviour
    {
        [SerializeField]
        private Transform _plane;

        public Transform Plane => _plane;
    }
}