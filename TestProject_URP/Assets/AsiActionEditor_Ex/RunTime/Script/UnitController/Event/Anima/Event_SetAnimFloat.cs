using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_SetAnimFloat : IActionEventData
    {
        [SerializeField] private string mAnimaFloatName = string.Empty;
        [SerializeField] private float mAnimaFloatSpeed = 7;
        [SerializeField] private EAnimFloatFor mAnimFloatFor = EAnimFloatFor.InputToTransDir;

        #region property

        [EditorProperty("浮点名称", EditorPropertyType.EEPT_String)]
        public string AnimaFloatName
        {
            get { return mAnimaFloatName; }
            set { mAnimaFloatName = value; }
        }
        
        [EditorProperty("参数来源", EditorPropertyType.EEPT_Enum)]
        public EAnimFloatFor AnimFloatFor
        {
            get { return mAnimFloatFor; }
            set { mAnimFloatFor = value; }
        }

        [EditorProperty("参数过渡速度", EditorPropertyType.EEPT_Float)]
        public float AnimaFloatSpeed
        {
            get { return mAnimaFloatSpeed; }
            set { mAnimaFloatSpeed = value; }
        }
        #endregion
        public int GetEvenType() => (int)EEvenType.EET_SetAnimFloat;

        private float lastAnimFloat = 0;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            lastAnimFloat = 0;
            if (_isSingle)
            {
                ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
                _stateMachine.SetAnimatorFloat(mAnimaFloatName, GetValue(_stateMachine));
            }
        }

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (mAnimaFloatSpeed > 0)
            {
                lastAnimFloat = Mathf.Lerp(lastAnimFloat, GetValue(_stateMachine),
                    mAnimaFloatSpeed * _actionTime.Deltatime);
                _stateMachine.SetAnimatorFloat(mAnimaFloatName, lastAnimFloat);
            }
            else
            {
                _stateMachine.SetAnimatorFloat(mAnimaFloatName, GetValue(_stateMachine));
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_SetAnimFloat _event = null;
            if (_eventData is null) _event = new Event_SetAnimFloat();
            else _event = (Event_SetAnimFloat)_eventData;

            _event.AnimaFloatName = mAnimaFloatName;
            _event.AnimFloatFor   = mAnimFloatFor;
            _event.AnimaFloatSpeed = mAnimaFloatSpeed;
            
            return _event;
        }

        private float GetValue(ActionStateMachine _stateMachine)
        {
            if (mAnimFloatFor == EAnimFloatFor.InputToTransDir)
            {
                if (_stateMachine.TryGetStaticLogic(out Ex_InputToTransDir _dir))
                {
                    return _dir.GetInputToTransDir;
                }
            }else if (mAnimFloatFor == EAnimFloatFor.GroundDir)
            {
                Transform _unitTrans = _stateMachine.CurUnit.transform;
                if (Physics.Raycast(_unitTrans.TransformPoint(0, 0.2f, 0), Vector3.down,
                        out RaycastHit _hit, 1))
                {
                    float _offset = Vector3.SignedAngle(_hit.normal, _unitTrans.forward, _unitTrans.right);
                    _offset -= 90;
                    return _offset;
                }
            }
            return 0;
        }
    }
}