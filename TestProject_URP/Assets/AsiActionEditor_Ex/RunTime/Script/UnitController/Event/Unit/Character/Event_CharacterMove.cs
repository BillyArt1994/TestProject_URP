using AsiActionEngine.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_CharacterMove : IActionEventData
    {
        private const int mRotPriority = 2;//旋转的优先级
        
        [SerializeField] private bool mIsMoveInput = true;
        [SerializeField] private Vector3 mMoveSpeed;
        [SerializeField] private float mLerpSpeed;
        [SerializeField] private EMoveDirType mMoveDirType = EMoveDirType.Transform;
        [SerializeField] private bool mRotToMoveDir = true;
        [SerializeField] private float mRotLerpSpeed;

        private Vector3 mMoveSpeed_lerp;
        private Quaternion mRot_Lerp;
        #region property
        public bool IsMoveInput
        {
            get { return mIsMoveInput; }
            set { mIsMoveInput = value; }
        }
        public Vector3 MoveSpeed
        {
            get { return mMoveSpeed; }
            set { mMoveSpeed = value; }
        }
        public float LerpSpeed
        {
            get { return mLerpSpeed; }
            set { mLerpSpeed = value; }
        }
        public EMoveDirType MoveDirType
        {
            get { return mMoveDirType; }
            set { mMoveDirType = value; }
        }
        public bool RotToMoveDir
        {
            get { return mRotToMoveDir; }
            set { mRotToMoveDir = value; }
        }
        public float RotLerpSpeed
        {
            get { return mRotLerpSpeed; }
            set { mRotLerpSpeed = value; }
        }
        public Vector3 MoveSpeed_lerp
        {
            get { return mMoveSpeed_lerp; }
            set { mMoveSpeed_lerp = value; }
        }
        
        #endregion
        int IActionEventData.GetEvenType() => (int)EEvenType.EET_CharacterMove;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            // EngineDebug.Log($"初始化了: C_Move\nmMoveSpeed: {mMoveSpeed.z}");
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                _characterControl.ChracterRots[mRotPriority] = 
                    _stateMachine.CurUnit.transform.rotation;
            }

            mMoveSpeed_lerp = Vector3.zero;
            mRot_Lerp = _stateMachine.CurUnit.transform.rotation;
        }

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {

                Vector3 _moveSpeed = mMoveSpeed;
                if (mMoveDirType == EMoveDirType.Transform)
                {
                    _moveSpeed = _stateMachine.CurUnit.transform.TransformDirection(mMoveSpeed);

                    RotCharacter(_characterControl.GetV3ToInputMove(_stateMachine, mMoveSpeed), _stateMachine, _actionTime.Deltatime);
                }
                else if (mMoveDirType == EMoveDirType.Camera)
                {
                    if (_stateMachine.IsMoveInput)
                    {
                        _moveSpeed = _characterControl.GetV3ToInputMove(_stateMachine, mMoveSpeed);
                        RotCharacter(_moveSpeed, _stateMachine, _actionTime.Deltatime);
                    }
                }
                else if (mMoveDirType == EMoveDirType.World)
                {
                    if (_stateMachine.IsMoveInput)
                    {
                        _moveSpeed = Quaternion.LookRotation(_stateMachine.PlayerInputMoveDir) * mMoveSpeed;
                        RotCharacter(_moveSpeed, _stateMachine, _actionTime.Deltatime);
                    }
                }



                _moveSpeed = mIsMoveInput && !_stateMachine.IsMoveInput ? Vector3.zero : _moveSpeed;
                if (mLerpSpeed > 0)
                {
                    // if (mMoveDirType == EMoveDirType.Transform)
                    // {
                    //     mMoveSpeed_lerp = Vector3.Lerp(mMoveSpeed_lerp, mMoveSpeed, _actionTime.Deltatime * LerpSpeed);
                    //     Vector3 _moveDir = _stateMachine.CurUnit.transform.TransformDirection(mMoveSpeed_lerp);
                    //     _actionState.ActionStateMachine.CharacterMove += _moveDir;
                    // }
                    // else
                    {
                        
                        mMoveSpeed_lerp = Vector3.Lerp(mMoveSpeed_lerp, _moveSpeed, _actionTime.Deltatime * LerpSpeed);
                        // EngineDebug.Log($"mMoveSpeed_lerp: {mMoveSpeed_lerp.sqrMagnitude}");
                        _characterControl.CharacterVelocity += mMoveSpeed_lerp;
                    }
                }
                else if (mLerpSpeed < 0)
                {
                    _characterControl.CharacterVelocity += mMoveSpeed_lerp;
                }
                else
                {
                    _characterControl.CharacterVelocity += _moveSpeed;
                }
            }
        }

        public void Exit(ActionStatePart _actionState, bool _interruot)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                _characterControl.ChracterRots[mRotPriority] = Quaternion.identity;
            }
            // EngineDebug.Log("退出");
        }

        private void RotCharacter(Vector3 _dir, ActionStateMachine _stateMachine, float _deltatime)
        {
            if (mRotToMoveDir)
            {
                if(_dir==Vector3.zero)return;

                if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
                {
                    if (RotLerpSpeed > 0)
                    {
                        _characterControl.ChracterRots[mRotPriority] = Quaternion.Lerp(_stateMachine.CurUnit.transform.rotation,
                            Quaternion.LookRotation(_dir), _deltatime * RotLerpSpeed);
                    }
                    else
                    {
                        _characterControl.ChracterRots[mRotPriority] = Quaternion.LookRotation(_dir);
                    }
                }
            }
        }
        
        IActionEventData IActionEventData.Clone(IActionEventData _eventData)
        {
            Event_CharacterMove _eventCharacterMove = null;
            if (_eventData != null) _eventCharacterMove = _eventData as Event_CharacterMove;
            else _eventCharacterMove =  new Event_CharacterMove();
            
            _eventCharacterMove.IsMoveInput = mIsMoveInput;
            _eventCharacterMove.MoveSpeed = mMoveSpeed;
            _eventCharacterMove.LerpSpeed = mLerpSpeed;
            _eventCharacterMove.MoveDirType = mMoveDirType;
            _eventCharacterMove.RotToMoveDir = mRotToMoveDir;
            _eventCharacterMove.RotLerpSpeed = mRotLerpSpeed;

            return _eventCharacterMove;
        }
    }
}