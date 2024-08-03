using UnityEngine;

namespace AsiActionEngine.RunTime
{
    [System.Serializable]
    public class CheckMoveState : IInterruptCondition
    {
        [SerializeField]private bool mIsMove = true;

        #region Property
        [EditorProperty("移动输入中", EditorPropertyType.EEPT_Bool)]
        public bool IsMove
        {
            get { return mIsMove; }
            set { mIsMove = value; }
        }
        #endregion

        public int InterruptType => -(int)EInterruptTypeInternal.EIT_CheckMove;

        public bool CheckInterrupt(Unit unit, ActionStatePart actionStatePart)
        {
            return actionStatePart.ActionStateMachine.IsMoveInput == mIsMove;
        }
        
        public IInterruptCondition Clone()
        {
            CheckMoveState _CheckMoveState = new CheckMoveState();
            _CheckMoveState.IsMove = IsMove;
            return _CheckMoveState;
        }
    }
}