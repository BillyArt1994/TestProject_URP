using AsiActionEngine.RunTime;
using UnityEngine;
namespace AsiTimeLine.RunTime
{
    public class Ex_InputToTransDir : StaticActionLogics
    {
        public float GetInputToTransDir;
        public override void OnUpdate(ActionStateMachine _stateMachine)
        {
            Vector3 _dir = _stateMachine.CurUnit.transform.TransformDirection(_stateMachine.PlayerInputMoveDir);
            float _angleOffset = _stateMachine.CamRot.eulerAngles.y - Quaternion.LookRotation(_dir).eulerAngles.y;
            if (_angleOffset > 180) _angleOffset -= 360;
            else if (_angleOffset < -180) _angleOffset += 360;
            GetInputToTransDir = _angleOffset;
        }
    }
}