using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_UnitRot : IActionEventData
    {
        [SerializeField] protected bool mIsMove = true;
        [SerializeField] protected ERotType mRotType;
        [SerializeField] protected int mRotLerp = 12;

        #region Property
        [EditorProperty("移动输入时转向: ", EditorPropertyType.EEPT_Bool)]
        public bool IsMove
        {
            get { return mIsMove; }
            set { mIsMove = value; }
        }
        [EditorProperty("朝向类型: ", EditorPropertyType.EEPT_Enum)]
        public ERotType RotType
        {
            get { return mRotType; }
            set { mRotType = value; }
        }
        [EditorProperty("转向速度(负为线性：s): ", EditorPropertyType.EEPT_Int)]
        public int RotLerp
        {
            get { return mRotLerp; }
            set { mRotLerp = value; }
        }
        #endregion
        
        public int GetEvenType() => (int)EEvenType.EET_UnitRot;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            if (_isSingle)
            {
                ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
                _stateMachine.CurUnit.transform.rotation = GetTargetRot(_stateMachine, mRotType);
            }
        }

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            Transform _transform = _stateMachine.CurUnit.transform;
            Quaternion _targetRot= GetTargetRot(_stateMachine, mRotType);
            if (mRotLerp == 0)
            {
                _transform.rotation = _targetRot;
            }
            else if (mRotLerp > 0)
            {
                _transform.rotation = 
                    Quaternion.Lerp(_transform.rotation, _targetRot, RotLerp * _actionTime.Deltatime);
            }
            else
            {
                mRotLerp *= -1;
                _transform.rotation = Quaternion.RotateTowards(_transform.rotation,_targetRot,
                    RotLerp * _actionTime.Deltatime);
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_UnitRot _event = new Event_UnitRot();

            _event.IsMove = mIsMove;
            _event.RotType = mRotType;
            _event.RotLerp = mRotLerp;
            
            return _event;
        }

        private Quaternion GetTargetRot(ActionStateMachine _stateMachine, ERotType _RotType)
        {
            if (_RotType == ERotType.Camera)
            {
                return Quaternion.Euler(0, _stateMachine.CamRot.eulerAngles.y, 0);
            }

            if (_RotType == ERotType.MoveDir)
            {
                Quaternion _moveDir = Quaternion.LookRotation(_stateMachine.PlayerInputMoveDir, Vector3.up);
                _moveDir *= _stateMachine.GetCamRot();
                return Quaternion.Euler(0, _moveDir.eulerAngles.y, 0);
            }

            if (_RotType == ERotType.LockToTargetDir)
            {
                
            }
            
            return Quaternion.identity;
        }
    }
}