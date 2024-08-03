using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public class Ex_Update_CharacterControl : ActionLogics
    {
        private bool mIsMove = false;
        private bool mChangeGround_last = false;
        private CharacterController mCharacter;

        public Vector3 CharacterVelocity;//玩家位移速度
        public Vector3 CharacterMove;//玩家位移

        public Quaternion[] ChracterRots { get; set; }//玩家旋转，但是不同等级  序列号越大优先级越低
        public float CharacterGravity { get; set; }
        public float PosY { get; set; }

        public bool ChackGround { get; private set; } = false;
        
        public override void Start(ActionStateMachine _actionState)
        {
            if (mCharacter is null)
            {
                if (_actionState.TryGetComponent(out CharacterController _characterController))
                {
                    //数据初始化
                    mCharacter = _characterController;
                    mIsMove = true;
                    // mPosY = CurUnit.transform.position.y;
                    CharacterVelocity = Vector3.zero;
                    ChracterRots = new Quaternion[]
                    {
                        Quaternion.identity, 
                        Quaternion.identity,
                        Quaternion.identity, 
                    };
                }
                else
                {
                    mIsMove = false;
                    EngineDebug.LogError($"角色无法位移 [<color=#FFCC00>CharacterController</color>] 获取失败！！");
                }
            }
        }
        

        public override void Update(ActionStateMachine _actionState, float _delaTime)
        {
            if (mIsMove)
            {
                //检查角色是否在地面
                ChackGround = Physics.Raycast(_actionState.CurUnit.transform.TransformPoint(0, 0.2f, 0), Vector3.down, 0.3f);

                //浮空时重置重力
                if (mChangeGround_last != ChackGround)
                {
                    if (!ChackGround && PosY < 0)
                    {
                        // Debug.Log("触发");
                        PosY = 0f;
                    }
                    mChangeGround_last = ChackGround;
                }
                
                //常规重力计算
                PosY -= CharacterGravity * _delaTime;
                PosY = Mathf.Max(PosY, -CharacterGravity * 3);//限制下落的最大加速度
                CharacterVelocity.y += PosY;

                CharacterVelocity *= _delaTime;
                mCharacter.Move(CharacterVelocity + CharacterMove);
                CharacterVelocity = Vector3.zero;//位移后清空速度数据
                CharacterMove = Vector3.zero;//位移后清空位移数据
                for (int i = 0; i < ChracterRots.Length; i++)
                {
                    if (ChracterRots[i] != Quaternion.identity)
                    {
                        _actionState.CurUnit.transform.rotation = ChracterRots[i];
                        // Debug.Log("旋转？ :" + i);
                    }
                }
            }
        }

        public Vector3 GetV3ToInputMove(ActionStateMachine _actionState, Vector3 _mouveDir, Quaternion _referDir)
        {
            return Quaternion.LookRotation(_actionState.PlayerInputMoveDir) * _referDir * _mouveDir;
        }
        public Vector3 GetV3ToInputMove(ActionStateMachine _actionState, Vector3 _mouveDir)
        {
            if (_actionState.PlayerInputMoveDir == Vector3.zero)
            {
                return _actionState.CurUnit.transform.TransformDirection(_mouveDir);
            }
            Quaternion _rot = Quaternion.LookRotation(_actionState.PlayerInputMoveDir) *
                              Quaternion.Euler(0, _actionState.CamRot.eulerAngles.y, 0);
            _mouveDir = _rot * _mouveDir;
            return  _mouveDir;
        }
    }
}