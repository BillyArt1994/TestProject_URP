using AsiActionEngine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public class ActionEngineRuntimePath
    {
        #region Instance
        private static ActionEngineRuntimePath _instance = null;
        public static ActionEngineRuntimePath Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionEngineRuntimePath();
                }
                return _instance;
            }
        }
        #endregion

        private const string ReLocalPath = "Script/AsiActionEditor/";
        
        public string DataPath
        {
            get { return ReLocalPath + "Data/"; }
        } 
        
        //单位路径
        public string UnitPath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return DataPath + "Unit";
            }
            return DataPath + $"Unit/{_name}";//.json
        }
        
        //Action保存路径
        public string ActionPath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return DataPath + "Action";
            }
            return DataPath + $"Action/{_name}";//.json
        }
        
        //Item保存路径
        public string ItemPath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return DataPath + "Item";
            }
            return DataPath + $"Item/{_name}";//.json
        }
        
        //Camera保存路径
        public string CameraPath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return DataPath + "Camera";
            }
            return DataPath + $"Camera/{_name}";//.json
        }
        
        //输入系统设置保存路径
        public string InputModulePath()
        {
            return DataPath + $"InputSetting/InputModule";//.json
        }
        
        //输入系统的行为列表保存路径
        public string InputActionListPath()
        {
            return DataPath + $"InputSetting/InputAction";//.json
        }
        
        //
    }
}