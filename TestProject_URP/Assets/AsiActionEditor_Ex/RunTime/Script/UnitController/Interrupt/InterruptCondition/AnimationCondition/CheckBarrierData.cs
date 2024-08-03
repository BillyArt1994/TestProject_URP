using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class CheckBarrierData : IInterruptCondition
    {
        [SerializeField] private float mCheckHeight_G;
        [SerializeField] private float mCheckDis_L;
        
        #region property
        [EditorProperty("检查高度（大于）", EditorPropertyType.EEPT_Float)]
        public float CheckHeight_G
        {
            get { return mCheckHeight_G; }
            set { mCheckHeight_G = value; }
        }
        [EditorProperty("检查距离（小于）", EditorPropertyType.EEPT_Float)]
        public float CheckDis_L
        {
            get { return mCheckDis_L; }
            set { mCheckDis_L = value; }
        }
        #endregion

        public int InterruptType => (int)EConditionType.EIT_CheckBarrier;
        public bool CheckInterrupt(Unit unit, ActionStatePart actionStatePart)
        {
            if (actionStatePart.ActionStateMachine.TryGetStaticLogic(out Ex_BarrierData _barrier))
            {
                if (_barrier.mGetDistance < mCheckDis_L)
                {
                    if (_barrier.mGetHeight > mCheckHeight_G)
                    {
                        _barrier.SetInteractPoint();
                        return true;
                    }
                }
            }
            return false;
        }

        public IInterruptCondition Clone()
        {
            CheckBarrierData _check = new CheckBarrierData();

            _check.CheckHeight_G = mCheckHeight_G;
            _check.CheckDis_L = mCheckDis_L;
            
            return _check;
            // throw new System.NotImplementedException();
        }
    }
}