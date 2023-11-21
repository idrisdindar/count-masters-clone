using UnityEngine;
using UnityEngine.Serialization;

namespace IdrisDindar.HyperCasual.Managers
{
    public abstract class Manager : MonoBehaviour
    {
        public bool IsInitialized { get; protected set; }
        
        protected virtual void Initialize(){ }
    }
}