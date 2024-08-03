using System;
using System.Collections.Generic;
using System.IO;
using AsiActionEngine.RunTime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace AsiTimeLine.RunTime
{
    public partial class ActionEngineManager_Unit
    {
        #region Instance

        private static ActionEngineManager_Unit _instance;
        public static ActionEngineManager_Unit Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionEngineManager_Unit();
                }

                return _instance;
            }
        }

        #endregion
        private List<Unit> mUnits = new List<Unit>();
        private Dictionary<string, UnitWarp> mUnitWarpInfo = new Dictionary<string, UnitWarp>();//角色列表数据
        private Dictionary<string, ActionStateInfo> mActionInfo = new Dictionary<string, ActionStateInfo>();//角色行为列表数据


        
        public List<Unit> Units => mUnits;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init() => OnStart();
        /// <summary>
        /// 每帧执行
        /// </summary>
        /// <param name="_deltatime">每帧间隔时间</param>
        public void Update(float _deltatime) => OnUpdate(_deltatime);
        /// <summary>
        /// 每帧执行（Late）
        /// </summary>
        /// <param name="_deltatime">每帧间隔时间</param>
        public void LateUpdate(float _deltatime) => OnLateUpdate(_deltatime);
        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="_unit"></param>
        public void AddUnit(Unit _unit) => OnAddUnit(_unit);

        // public void LoadUnitWarpInfo(UnitWarpInfo _unitWarpInfo) => OnLoadUnitWarpInfo(_unitWarpInfo);
        

        #region PublicFunction

        /// <summary>
        /// 获取单位的封装数据
        /// </summary>
        /// <param name="_name">单位名称</param>
        /// <param name="_loadCallback">加载结束后的回调</param>
        /// <returns></returns>
        public void GetUnitWarp(string _name, Action<UnitWarp> _loadCallback) => OnGetUnitWarp(_name, _loadCallback);
        public void GetActionList(string _name, Action<ActionStateInfo> _loadCallback) => OnGetActionList(_name, _loadCallback);

        
        #endregion 

        #region Function
        private void OnStart()
        {
            
        }
        
        private void OnUpdate(float _deltatime)
        {
            foreach (var _unit in mUnits)
            {
                _unit.OnUpdate(_deltatime);
            }
        }
        
        private void OnLateUpdate(float _deltatime)
        {
            foreach (var _unit in mUnits)
            {
                _unit.OnLateUpdate(_deltatime);
            }
        }

        private void OnAddUnit(Unit _unit)
        {
            mUnits.Add(_unit);
        }

        
        private void OnGetUnitWarp(string _name, Action<UnitWarp> _loadCallback)
        {
            if (mUnitWarpInfo.TryGetValue(_name, out var _unitWarp))
            {
                _loadCallback(_unitWarp);
            }
            else
            {
                OnLoadUnitWarpInfo(_name, _loadCallback);
            }
        }

        private void OnGetActionList(string _name, Action<ActionStateInfo> _loadCallback)
        {
            if (mActionInfo.ContainsKey(_name))
            {
                _loadCallback(mActionInfo[_name]);
            }
            else
            {
                //如果字典中不存在这个行为列表，则自行加载
                OnLoadUnitAction(_name, (list =>
                    {
                        mActionInfo.Add(_name,list);
                        _loadCallback(list);
                    })
                );
            }
        }

        private void OnLoadUnitWarpInfo(string _name, Action<UnitWarp> _unitWarpInfo)
        {
            if (mUnitWarpInfo.Count < 1)
            {
                string _path = ActionEngineRuntimePath.Instance.UnitPath("Unit");

                string _str = Resources.Load<TextAsset>(_path).text;

                UnitWarpInfo _info = new UnitWarpInfo(null);
                JsonUtility.FromJsonOverwrite(_str, _info);

                foreach (var _unitWarp in _info.mUnitWarp)
                {
                    mUnitWarpInfo.Add(_unitWarp.Name, _unitWarp);
                }

                if (mUnitWarpInfo.TryGetValue(_name, out UnitWarp _unitWarpv))
                {
                    _unitWarpInfo(_unitWarpv);
                }
                else
                {
                    EngineDebug.Log($"Unit数据加载失败:  {_name}");
                }

            }
            else
            {
                if(mUnitWarpInfo.TryGetValue(_name, out UnitWarp _unitWarp))
                {
                    _unitWarpInfo(_unitWarp);
                }
            }
        }

        private void OnLoadUnitAction(string _name, Action<ActionStateInfo> _loadCallback)
        {
            string _path = ActionEngineRuntimePath.Instance.ActionPath(_name);

            TextAsset _textAsset = Resources.Load<TextAsset>(_path);
            if (_textAsset is null)
            {
                EngineDebug.LogWarning($"单位数据加载出错， \n路径: <color=#FFCC00>{_path}</color>");
            }
            else
            {
                string _str = _textAsset.text;
                ActionStateInfo _info = new ActionStateInfo(null);
                JsonUtility.FromJsonOverwrite(_str, _info);
                _loadCallback(_info);
            }
        }

        #endregion
    }
}