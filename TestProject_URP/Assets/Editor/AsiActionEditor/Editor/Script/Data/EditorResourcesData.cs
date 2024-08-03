using System.Collections.Generic;
using AsiActionEngine.RunTime;
using UnityEngine;
using UnityEngine.Serialization;
using File = System.IO.File;

namespace AsiActionEngine.Editor
{
    [System.Serializable]
    public class EditorUnitWarpInfo
    {
        public List<EditorUnitWarp> mUnitWarp;
        public EditorUnitWarpInfo(List<EditorUnitWarp> _unitWarps)
        {
            mUnitWarp = _unitWarps;
        }
    }
    
    [System.Serializable]
    public class EditorActionStateInfo
    {
        public List<EditorActionState> mActionState;
        public string mDefaultAction;
        public string mHitAction;
        public List<string> mActionType;
        public List<string> mActionLable;

        public EditorActionStateInfo(List<EditorActionState> _actionStates, string _mDefaultAction = "", string _mHitAction = "",
            List<string> _mActionType = null, List<string> _mActionLable = null)
        {
            if (_actionStates == null)
            {
                mActionState = new List<EditorActionState>();
                mDefaultAction = string.Empty;
                mHitAction = string.Empty;
                List<string> _actionType = new List<string>();
                _actionType.Add("Idle");
                _actionType.Add("Move");
                _actionType.Add("Jump");
                _actionType.Add("UseItem");
                _actionType.Add("Interact");
                _actionType.Add("Attack");
                _actionType.Add("Hit");
                _actionType.Add("Defence");
                _actionType.Add("Dead");

                mActionType = _actionType;
                mActionLable = new List<string>();
            }
            else
            {
                mActionState = _actionStates;
                mDefaultAction = _mDefaultAction;
                mHitAction = _mHitAction;
                mActionType = _mActionType;
                mActionLable = _mActionLable;
            }
        }
        
        public ActionStateInfo GetActionStateInfo()
        {
            List<ActionState> _actionStates = new List<ActionState>();
            if (this.mActionState != null)
            {
                foreach (var VARIABLE in this.mActionState)
                {
                    _actionStates.Add(VARIABLE.GetActionState());
                }
            }
            ActionStateInfo _info = new ActionStateInfo(_actionStates);
            _info.mDefaultAction = this.mDefaultAction;
            _info.mHitAction = this.mHitAction;
            _info.mActionType = this.mActionType;
            _info.mActionLable = this.mActionLable;
            return _info;
        }
    }

    public class RunTimeDataManager
    {
        public static void SaveUnit(EditorUnitWarpInfo _editorUnitWarp, string _name)
        {
            string _savePath = MotionEngineRuntimePath.Instance.UnitPath(_name);

            string _str = JsonUtility.ToJson(UnitWarpInfo(_editorUnitWarp));
            File.WriteAllText(_savePath, _str);
        }

        public static void SaveActionState(EditorActionStateInfo _actionState, string _name)
        {
            string _savePath = MotionEngineRuntimePath.Instance.ActionPath(_name);

            string _str = JsonUtility.ToJson(_actionState.GetActionStateInfo());
            File.WriteAllText(_savePath, _str);
        }

        public static UnitWarpInfo UnitWarpInfo(EditorUnitWarpInfo _editorUnitWarp)
        {
            List<UnitWarp> _unitWarps = new List<UnitWarp>();
            if (_editorUnitWarp != null && _editorUnitWarp.mUnitWarp != null)
            {
                foreach (var _unitWarp in _editorUnitWarp.mUnitWarp)
                {
                    _unitWarps.Add(_unitWarp.GetUnitWarp());
                }
            }
            UnitWarpInfo _info = new UnitWarpInfo(_unitWarps);
            return _info;
        }

    }

}