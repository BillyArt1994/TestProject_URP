using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AsiActionEngine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public partial class ResourcesWindow
    {
        private int mSelectActionType_filter;
        private string mSelectActionState = string.Empty;
        private string mActionGroupName = string.Empty;
        private string mStartAction;
        private string mHitAction;
        private List<int> mActionIDList = new List<int>();
        private List<string> mActionType = new List<string>();
        private List<string> mActionLable = new List<string>();
        private List<EditorActionState> mActionState;
        private List<EditorActionState> mActionState_filter = new List<EditorActionState>();
        private Dictionary<int, EditorActionState> mActionStateDic = new Dictionary<int, EditorActionState>();

        public string ActionGroupName => mActionGroupName;
        public string StartAction => mStartAction;
        public string HitAction => mHitAction;
        public List<string> ActionType => mActionType;
        public List<string> ActionLable => mActionLable;
        public List<EditorActionState> AllActionState => mActionState;
        public Dictionary<int, EditorActionState> ActionStateDic => mActionStateDic;
        private void InitActionStateGUI()
        {
            mActionState = null;
            mSelectActionState = string.Empty;
            mActionGroupName = "null";
            mSelectActionType_filter = -2;
            mActionStateDic.Clear();
        }
        private void DrwaActionStateGUI()
        {
            //头部绘制
            using (new GUILayout.HorizontalScope(GUILayout.Width(position.width - 60)))
            {
                if (GUILayout.Button("复制 ", EditorStyles.toolbarButton))
                {
                    if (mActionState == null)
                    {
                        return;
                    }
                    
                    CreactNewState(GetEditorActionStateToSelect());
                }
                if (GUILayout.Button("新建", EditorStyles.toolbarButton))
                {
                    if (mActionState == null)
                    {
                        return;
                    }

                    CreactNewState();
                }
                if (GUILayout.Button("删除", EditorStyles.toolbarButton))
                {
                    if (mActionState == null)
                    {
                        return;
                    }
                    if (mOnSelectActionStateID < mActionState.Count)
                    {
                        if (mSelectActionType_filter > -1)
                        {
                            mActionState_filter.Remove(GetEditorActionStateToSelect());
                        }
                        mActionState.Remove(GetEditorActionStateToSelect());
                    }
                }
            }
            GUILayout.Space(2);
            using (new GUILayout.HorizontalScope(GUILayout.Width(position.width - 60)))
            {
                if (GUILayout.Button("排序 ", EditorStyles.toolbarButton))
                {
                    ActionListSort();
                }

                //筛选
                using (var _check = new EditorGUI.ChangeCheckScope())
                {
                    mSelectActionType_filter = EditorGUILayout.Popup(mSelectActionType_filter,
                        mActionType.ToArray(), EditorStyles.toolbarPopup);
                    if (_check.changed)
                    {
                        mActionState_filter.Clear();
                        foreach (var _actionState in mActionState)
                        {
                            if (_actionState.EditorActionType.Contains(mActionType[mSelectActionType_filter]))
                            {
                                mActionState_filter.Add(_actionState);
                            }
                        }
                    }
                }

                if (mSelectActionType_filter > -1)
                {
                    if (GUILayout.Button("取消过滤", EditorStyles.toolbarButton))
                    {
                        mSelectActionType_filter = -1;
                    }
                }

            }

            Rect _saveRect = position;
            _saveRect.width = 60;
            _saveRect.x = position.width - _saveRect.width;
            _saveRect.y = EditorGUIUtility.singleLineHeight;
            _saveRect.height = 46;
            if (GUI.Button(_saveRect,"保存"))
            {
                if (!string.IsNullOrEmpty(mSelectActionState))
                {
                    SaveActionStateInfo(mSelectActionState, true);
                    UpdateActionStateDic();
                }
            }

            if (mActionState == null)
            {
                return;
            }
            //Action列表

            Rect _rect = new Rect(position);
            float _HeadHeight = 65;
            _rect.y = _HeadHeight;
            _rect.x = 0;
            _rect.height -= _HeadHeight;
            List<EditorActionState> _actionStates = mSelectActionType_filter < 0 ? mActionState : mActionState_filter;
            ActionButtonList.DrawAction(_rect, _actionStates, (EditorActionState _actionState) =>
            {
                OnSelectActionState(_actionState);
            }, mOnSelectActionStateID);
        }
        private Vector2 mActionScroll;

        private void ActionListSort()
        {
            if (mActionState == null)
            {
                return;
            }
            mActionState.Sort((x, y) => { return x.ID.CompareTo(y.ID); });
            if (mSelectActionType_filter > -1)
            {
                mActionState_filter.Sort((x, y) => { return x.ID.CompareTo(y.ID); });
            }
        }
        
        public void SetActionStateName(string _actionName)
        {
            mOnSelectActionStateID = 0;
            mSelectActionState = _actionName;
            InitActionStateGUI();
        }

        public void SetStartAction(string _name)
        {
            mStartAction = _name;
        }
        public void SetHitAction(string _name)
        {
            mHitAction = _name;
        }

        public int GetIDToActionType(string _actonType)
        {
            if (mActionType.Contains(_actonType))
            {
                return mActionType.IndexOf(_actonType);
            }

            return -1;
        }
        
        private void UpdateActionStateDic()
        {
            foreach (var item in mActionState)
            {
                if (!mActionStateDic.TryAdd(item.ID, item))
                {
                    //EngineDebug.LogError
                }
            }
        }

        private int GetNewActionID()
        {
            if (mActionState.Count < 1)
            {
                return 0;
            }
            mActionIDList.Clear();
            foreach (var _actionState in mActionState)
            {
                mActionIDList.Add(_actionState.ID);
            }
            
            for (int i = mOnSelectActionStateID; i < mActionState.Count; i++)
            {
                int _id = mActionState[i].ID + 1;
                if (!mActionIDList.Contains(_id))
                {
                    return _id;
                }
            }
            ActionListSort();
            return mActionState[^1].ID + 1;
        }

        private void CreactNewState(EditorActionState _actionState = null)
        {
            int _id = GetNewActionID();
            EditorActionState _newState = null;
            if (_actionState is null)
            {
                _newState = new EditorActionState(_id, $"Action {_id}");
            }
            else
            {
                _newState = _actionState.Clone(_id);
                mActionState.Add(_newState);
                mActionState_filter.Add(_newState);
                return;
            }

            if (mSelectActionType_filter > -1)
            {
                _newState.EditorActionType = mActionType[mSelectActionType_filter];
                string[] _names = mActionType[mSelectActionType_filter].Split('/');
                string _myName = _names[0];
                for (int i = 1; i < _names.Length; i++)
                {
                    _myName += $"_{_names[i]}";
                }

                _newState.Name = $"{_myName} {_id}";
                mActionState_filter.Add(_newState);
            }

            mActionState.Add(_newState);
        }

        #region 数据存取
        public void LoadActionStateInfo(string _actionName)
        {
            mActionState = new List<EditorActionState>();

            //尝试加载客户端序列化的数据
            if (ActionWindowMain.ActionEditorFuntion.LoadActionData(out EditorActionStateInfo _actionInfo, _actionName))
            {
                if (_actionInfo.mActionState != null)
                {
                    mActionGroupName = _actionName;
                    mActionState = _actionInfo.mActionState;
                    mStartAction = _actionInfo.mDefaultAction;
                    mHitAction = _actionInfo.mHitAction;
                    mActionType = _actionInfo.mActionType;
                    mActionLable = _actionInfo.mActionLable;
                    UpdateActionStateDic();
                }
                return;
            }
            
            string _path = MotionEngineConst.EditorActionSavePath(_actionName);
            if (!File.Exists(_path))
            {
                _actionName = string.Empty;
                return;
            }
            
            if (!string.IsNullOrEmpty(_actionName))
            {
                string _str = File.ReadAllText(_path);
                EditorActionStateInfo _LoadInfo = new EditorActionStateInfo(null);
                JsonUtility.FromJsonOverwrite(_str, _LoadInfo);
                if (_LoadInfo.mActionState != null)
                {
                    mActionGroupName = _actionName;
                    mActionState = _LoadInfo.mActionState;
                    mStartAction = _LoadInfo.mDefaultAction;
                    mHitAction = _LoadInfo.mHitAction;
                    mActionType = _LoadInfo.mActionType;
                    mActionLable = _LoadInfo.mActionLable;
                    UpdateActionStateDic();
                }
            }
        }
        public void SaveActionStateInfo(string _actionName, bool isCopy = false)
        {
            if (isCopy)
            {
                CheckDuplicateName _checkDuplicateName = new CheckDuplicateName();
                foreach (var VARIABLE in mActionState)
                {
                    _checkDuplicateName.OnCheck(VARIABLE.Name);
                }

                if (_checkDuplicateName.IsDuplicate())
                {
                    EditorUtility.DisplayDialog("保存出错！！！",_checkDuplicateName.GetDuplicateName(),"我知道了");
                }
                else
                {
                    EditorActionStateInfo _saveData = new EditorActionStateInfo(mActionState, mStartAction, mHitAction,
                        mActionType, mActionLable);
                    
                    //客户端保存序列化参数
                    if (ActionWindowMain.ActionEditorFuntion.SaveActionData(_saveData, _actionName))
                    {
                        AssetDatabase.Refresh();
                        return;
                    }
                    
                    string _str = JsonUtility.ToJson(_saveData);
                    File.WriteAllText(MotionEngineConst.EditorActionSavePath(_actionName),_str);
                    RunTimeDataManager.SaveActionState(_saveData, _actionName);
                    EngineDebug.Log($"成功储存  [ <color=#FFF100>{_actionName}</color> ]  Action数据");
                }
            }
            else
            {
                EditorActionStateInfo _saveData = new EditorActionStateInfo(null);
                
                //客户端保存序列化参数
                if (ActionWindowMain.ActionEditorFuntion.SaveActionData(_saveData, _actionName))
                {
                    AssetDatabase.Refresh();
                    return;
                }
                
                string _str = JsonUtility.ToJson(_saveData);
                File.WriteAllText(MotionEngineConst.EditorActionSavePath(_actionName),_str);
                RunTimeDataManager.SaveActionState(_saveData, _actionName);
                EngineDebug.Log($"成功新建  [ <color=#00BBFF>{_actionName}</color> ]  Action数据 ");
            }
        }
        #endregion

    }
}