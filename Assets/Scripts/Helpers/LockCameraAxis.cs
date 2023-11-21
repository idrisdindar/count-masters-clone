using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdrisDindar.HyperCasual
{
    [ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")]
    public class LockCameraAxis : CinemachineExtension
    {
        public bool _lockX;
        public bool _lockY;
        public bool _lockZ;
        
        public float _yPosition = 0;
        public float _xPosition = 0;
        public float _zPosition = 0;
 
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                if(_lockX)
                    pos.x = _xPosition;
                if(_lockY)
                    pos.y = _yPosition;
                if(_lockZ)
                    pos.z = _zPosition;
                state.RawPosition = pos;
            }
        }
    }
}