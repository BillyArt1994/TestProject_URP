using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class CheckFindPoint :  IInterruptCondition
    {
        [SerializeField] protected EInteraetPointType mInteraetPointType = EInteraetPointType.HookLock;

        #region Property
        public EInteraetPointType InteraetPointType
        {
            get { return InteraetPointType; }
            set { InteraetPointType = value;}
        }

        #endregion

        public int InterruptType => (int)EConditionType.EIT_CheckFindPoint;
        public bool CheckInterrupt(Unit unit, ActionStatePart actionStatePart)
        {
            ActionStateMachine _stateMachine = actionStatePart.ActionStateMachine;
            if (_stateMachine.TryGetStaticLogic(out Ex_FindHookPoint _hookPoint))
            {
                if (_hookPoint.IsFindPoint)
                {
                    _hookPoint.SetFinishPoint();
                    return true;
                }
            }

            return false;
        }

        public IInterruptCondition Clone()
        {
            CheckFindPoint _check = new CheckFindPoint();

            _check.InteraetPointType = mInteraetPointType;
            
            return _check;
        }
    }
}