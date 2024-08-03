using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_InteractBarrier : IActionEventData
    {
        private bool mAlignAngle = false;
        private int mCharacteLimbType;//ECharacteLimbType

        private Vector3 mInteractPoint;
        private Vector3 mInitPos;
        private Quaternion mInteractDir;
        private Quaternion mInitRot;
        private float lastGravity;
        private float deltatime = 1;

        #region Property

        [EditorProperty("对齐角度 ", EditorPropertyType.EEPT_Bool)]
        public bool AlignAngle
        {
            get { return mAlignAngle; }
            set { mAlignAngle = value; }
        }

        [EditorProperty("对齐对象 ", EditorPropertyType.EEPT_CharacteLimbType)]
        public int CharacteLimbType
        {
            get { return mCharacteLimbType; }
            set { mCharacteLimbType = value; }
        }
        

        #endregion
        
        public int GetEvenType() => (int)EEvenType.EET_InteractBarrier;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;

            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                Transform _referTransform = GetInteractPos(_stateMachine);
                if (_isSingle)
                {
                    {
                        Vector3 _move = mInteractPoint - _referTransform.position;
                        _characterControl.CharacterMove = _move;
                        _characterControl.ChracterRots[2] = mInteractDir;
                    }

                    return;
                }

                mInitPos = _referTransform.position;
                mInitRot = _referTransform.rotation;
                if (_stateMachine.TryGetStaticLogic(out Ex_BarrierData _barrier))
                {
                    mInteractPoint = _barrier.mInteractPoint;
                    mInteractDir = _barrier.mInteractRot;
                }

                lastGravity = _characterControl.CharacterGravity;
                _characterControl.CharacterGravity = 0;
                _characterControl.PosY = 0;
            }
        }

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                Transform _referTransform = GetInteractPos(_stateMachine);

                deltatime = _actionTime.Deltatime;
                float _startTime = _actionTime.CurrentTime - _actionTime.TriggerTime;
                float _proportion = _startTime / _actionTime.Duration;

                Vector3 _targetPoint = Vector3.Lerp(mInitPos, mInteractPoint, _proportion);
                Vector3 _move = _targetPoint - _referTransform.position;
                _characterControl.CharacterMove = _move;
            }
        }

        public void Exit(ActionStatePart _actionState, bool _interruot)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                if (_interruot)
                {
                    Transform _referTransform = GetInteractPos(_stateMachine);
                    Vector3 _move = mInteractPoint - _referTransform.position;
                    _characterControl.CharacterMove = _move;
                }
                _characterControl.CharacterGravity = lastGravity;
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_InteractBarrier _event = new Event_InteractBarrier();

            _event.AlignAngle = mAlignAngle;
            _event.CharacteLimbType = mCharacteLimbType;
            
            return _event;
        }

        public Transform GetInteractPos(ActionStateMachine _stateMachine)
        {
            if (_stateMachine.TryGetComponent(out CharacterConfig _config))
            {
                ECharacteLimbType _interactType = (ECharacteLimbType)mCharacteLimbType;
                if (_config.HelpPointDic.TryGetValue(_interactType, out Transform _target))
                {
                    return _target;
                }
                else
                {
                    EngineDebug.LogError($"获取参考点失败!! [<color=#FFCC00>{_interactType}</color>]");
                }
            }

            return _stateMachine.CurUnit.transform;
        }
    }
}