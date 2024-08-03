using AsiActionEngine.RunTime;

namespace AsiActionEditor_Ex.RunTime
{
    //角色相机点位控制
    public class PlayerCamera : CameraControl
    {
        public override void OnUpdate(float _deltaTime)
        {
            
            lookTarget.rotation = stateMachine.GetCamRot();
        }
    }
}