﻿using System.IO;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using UnityEngine;
using MotionEngineConst = AsiActionEngine.Editor.MotionEngineConst;

namespace AsiTimeLine.Editor
{
    public class SaveData
    {
        private const string mSavePath = "Assets/Resources/{0}.json";//Resources下Json资源保存路径
        
        #region 加载数据 是Editor下的资源加载 直接同步加载即可
        static public bool LoadActionData(out EditorActionStateInfo _editorActionState, string _actionName)
        {
            string _path = ActionEngineConst.EditorActionSavePath(_actionName);
            if (!File.Exists(_path))
            {
                _editorActionState = null;
                EngineDebug.LogWarning($"客户端加载 Action 数据失败，\n加载路径：{_path}");
                return false;
            }
            

            string _str = File.ReadAllText(_path);
            EditorActionStateInfo _LoadInfo = new EditorActionStateInfo(null);
            JsonUtility.FromJsonOverwrite(_str, _LoadInfo);
            _editorActionState = _LoadInfo;
            return true;
        }
        
        
        static public bool LoadUnitData(out EditorUnitWarp _editorActionState, string _unitName)
        {
            string _path = ActionEngineConst.EditorUnitSavePath(_unitName);
            if (!File.Exists(_path))
            {
                EngineDebug.Log($"客户端加载 Unit 数据失败，\n加载路径：{_path}");
                _editorActionState = null;
                return false;
            }
            
            string _str = File.ReadAllText(_path);
            EditorUnitWarp _LoadInfo = new EditorUnitWarp(0,"null");
            JsonUtility.FromJsonOverwrite(_str, _LoadInfo);
            _editorActionState = _LoadInfo;
            return true;
        }
        
        static public bool LoadCameraWarp(out EditorCameraWarp _editorCameraWarp, string _name)
        {
            string _path = ActionEngineConst.EditorCamSavePath(_name);
            if (!File.Exists(_path))
            {
                EngineDebug.Log($"客户端加载 Camera 数据失败，\n加载路径：{_path}");
                _editorCameraWarp = null;
                return false;
            }
            
            string _str = File.ReadAllText(_path);
            EditorCameraWarp _LoadInfo = new EditorCameraWarp(0,"");
            JsonUtility.FromJsonOverwrite(_str, _LoadInfo);
            _editorCameraWarp = _LoadInfo;
            return true;
        }
        #endregion


        #region 保存数据
        static public bool SaveActionData(EditorActionStateInfo _editorActionState, string _actionName)
        {
            string _str = JsonUtility.ToJson(_editorActionState);
            File.WriteAllText(ActionEngineConst.EditorActionSavePath(_actionName),_str);
            
            //保存Runtime数据
            string _savePath = ActionEngineRuntimePath.Instance.ActionPath(_actionName);
            _savePath = string.Format(mSavePath, _savePath);

            _str = JsonUtility.ToJson(_editorActionState.GetActionStateInfo());
            File.WriteAllText(_savePath, _str);
            EngineDebug.Log($"成功储存  [ <color=#FFF100>{_actionName}</color> ]  Action数据");
            return true;
        }
        static public bool SaveUnitData(EditorUnitWarp _editorUnitWarp, string _unitName)
        {
            string _str = JsonUtility.ToJson(_editorUnitWarp);
            File.WriteAllText(ActionEngineConst.EditorUnitSavePath(_unitName),_str);
            
            //保存Runtime数据
            string _savePath = ActionEngineRuntimePath.Instance.UnitPath(_unitName);
            _savePath = string.Format(mSavePath, _savePath);

            // _str = JsonUtility.ToJson(RunTimeDataManager.UnitWarpInfo(_editorUnitWarp));
            _str = JsonUtility.ToJson(_editorUnitWarp);
            File.WriteAllText(_savePath, _str);
            
            EngineDebug.Log($"成功储存  [ <color=#FFF100>{_unitName}</color> ]  Unit数据 ");
            return true;
        }
        static public bool SaveCameraWarp(EditorCameraWarp _editorCameraWarp, string _name)
        {
            string _str = JsonUtility.ToJson(_editorCameraWarp);
            File.WriteAllText(ActionEngineConst.EditorCamSavePath(_name),_str);
            
            //保存Runtime数据
            string _savePath = ActionEngineRuntimePath.Instance.CameraPath(_name);
            _savePath = string.Format(mSavePath, _savePath);

            _str = JsonUtility.ToJson(_editorCameraWarp.GetCameraWarp());
            File.WriteAllText(_savePath, _str);
            
            EngineDebug.Log($"成功储存  [ <color=#FFF100>{_name}</color> ]  Camera数据 ");
            return true;
        }
        #endregion

    }
}