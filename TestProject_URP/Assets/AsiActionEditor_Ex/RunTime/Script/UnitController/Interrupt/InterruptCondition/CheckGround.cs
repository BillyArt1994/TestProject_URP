using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class CheckGround : IInterruptCondition
    {
        [SerializeField] private bool mIsGround = true;

        #region Property
        [EditorProperty("角色在地面上", EditorPropertyType.EEPT_Bool)]
        public bool IsGround
        {
            get { return mIsGround; }
            set { mIsGround = value; }
        }
        #endregion
        public int InterruptType => (int)EConditionType.EIT_CheckGround;
        
        public bool CheckInterrupt(Unit unit, ActionStatePart actionStatePart)
        {
            ActionStateMachine _stateMachine = actionStatePart.ActionStateMachine;
            if (_stateMachine.TryGetStaticLogic(out Ex_UnitGroundState _groundState))
            {
                return _groundState.IsGround == mIsGround;
            }
            return false;
        }
        public IInterruptCondition Clone()
        {
            CheckGround _checkGround = new CheckGround();

            _checkGround.IsGround = mIsGround;

            return _checkGround;
        }
    }
}