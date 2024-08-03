using System.Collections.Generic;
using UnityEngine.Serialization;

namespace AsiActionEngine.RunTime
{
    [System.Serializable]
    public class UnitWarpInfo
    {
        public List<UnitWarp> mUnitWarp;
        public UnitWarpInfo(List<UnitWarp> _unitWarps)
        {
            mUnitWarp = _unitWarps;
        }
    }
    
    [System.Serializable]
    public class ActionStateInfo
    {
        public List<ActionState> mActionState;
        public string mDefaultAction;
        public string mHitAction;
        public List<string> mActionType;
        public List<string> mActionLable;
        public ActionStateInfo(List<ActionState> _actionStates)
        {
            mActionState = _actionStates;
        }
    }

}