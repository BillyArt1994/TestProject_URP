using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_CharacterOnMove : IActionEventData
    {
        [SerializeField] protected EMoveDirType mMoveDirType = EMoveDirType.Transform;
        [SerializeField] protected Vector3 mMoveDir = Vector3.forward;
        [SerializeField] protected float mLerpSpeed = 0;
        
        
        #region Property

        [EditorProperty("朝向类型: ", EditorPropertyType.EEPT_Enum)]
        public EMoveDirType MoveDirType
        {
            get { return mMoveDirType; }
            set { mMoveDirType = value; }
        }
        [EditorProperty("移动速度: ", EditorPropertyType.EEPT_Vector3)]
        public Vector3 MoveDir
        {
            get { return mMoveDir; }
            set { mMoveDir = value; }
        }
        [EditorProperty("过渡速度: ", EditorPropertyType.EEPT_Float)]
        public float LerpSpeed
        {
            get { return mLerpSpeed; }
            set { mLerpSpeed = value; }
        }

        #endregion
        public int GetEvenType() => (int)EEvenType.EET_CharacterOnMove;

        private Vector3 lerpDir;
        
        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                lerpDir = _stateMachine.CurUnit.transform.forward;
                if (_isSingle)
                {
                    _characterControl.CharacterMove = GetDir(_stateMachine);
                }
            }

        }

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                if (LerpSpeed > 0)
                {
                    lerpDir = Vector3.Lerp(lerpDir, GetDir(_stateMachine), LerpSpeed * _actionTime.Deltatime);
                    _characterControl.CharacterVelocity = lerpDir;
                }
                else
                {
                    _characterControl.CharacterVelocity = GetDir(_stateMachine);
                }
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_CharacterOnMove _event = null;
            if (_eventData == null) _event = new Event_CharacterOnMove();
            else _event = (Event_CharacterOnMove)_eventData;

            _event.MoveDirType = mMoveDirType;
            _event.MoveDir = mMoveDir;
            _event.LerpSpeed = mLerpSpeed;
            
            return _event;
        }

        private Vector3 GetDir(ActionStateMachine _stateMachine)
        {
            if (mMoveDirType == EMoveDirType.Transform)
            {
                return _stateMachine.CurUnit.transform.TransformDirection(mMoveDir);
            }
            else if (mMoveDirType == EMoveDirType.Camera)
            {
                return Quaternion.Euler(0, _stateMachine.GetCamRot().eulerAngles.y, 0) * mMoveDir;
            }
            else if (mMoveDirType == EMoveDirType.inputDir_Cam)
            {
                Quaternion _dir = _stateMachine.CurUnit.transform.rotation;
                if (_stateMachine.PlayerInputMoveDir != Vector3.zero)
                {
                    float _moveDir = Quaternion.LookRotation(_stateMachine.PlayerInputMoveDir).eulerAngles.y;
                    _moveDir = _moveDir - _stateMachine.GetCamRot().eulerAngles.y;
                    if (_moveDir > 180) _moveDir -= 360;
                    if (_moveDir < -180) _moveDir += 360;
                    _dir = Quaternion.Euler(0, _moveDir, 0);
                }

                return _dir * mMoveDir;
            }
            return mMoveDir;
        }
    }
}