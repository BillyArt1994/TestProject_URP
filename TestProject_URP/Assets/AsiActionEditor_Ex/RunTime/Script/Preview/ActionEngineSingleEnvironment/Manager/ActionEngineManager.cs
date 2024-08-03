using System;
using System.Collections;
using System.IO;
using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public class ActionEngineManager : MonoBehaviour
    {
        private void Start()
        {
            ActionEngineManager_Input.Instance.Init();
            ActionEngineManager_Unit.Instance.Init();
        
            //加载单位数据
            string _path = ActionEngineRuntimePath.Instance.UnitPath(MotionEngineConst.UnitSaveName);
        
            TextAsset _textAsset = Resources.Load<TextAsset>(_path);
            if (_textAsset is null)
            {
                EngineDebug.LogWarning($"单位数据加载出错， \n路径: <color=#FFCC00>{_path}</color>");
            }
            else
            {
                string _str = _textAsset.text;
                UnitWarpInfo _info = new UnitWarpInfo(null);
                JsonUtility.FromJsonOverwrite(_str, _info);
            }
        }

        private void Update()
        {
            float _deltaTime = Time.deltaTime;
            
            //所有单位和输入系统的主要逻辑
            ActionEngineManager_Unit.Instance.Update(_deltaTime);
            ActionEngineManager_Input.Instance.Update(_deltaTime);

            //异步加载回调
            ActionEngineResources.Instance.ChackCallBack();
        }
    }
}