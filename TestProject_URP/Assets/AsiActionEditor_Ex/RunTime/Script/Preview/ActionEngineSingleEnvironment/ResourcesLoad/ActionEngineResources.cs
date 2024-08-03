// using UnityEditor.iOS;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AsiActionEngine.RunTime;
using UnityEditor;
using Object = UnityEngine.Object;

// using Unity.VisualScripting;

namespace AsiTimeLine.RunTime
{
    public class ActionEngineResources
    {
        #region Instance

        private static ActionEngineResources _instance;

        public static ActionEngineResources Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionEngineResources();
                }

                return _instance;
            }
        }
        

        #endregion
     
        private struct LoadCallBack
        {
            public ResourceRequest RQ;
            public Action<UnityEngine.Object> CallBack;
            public LoadCallBack(ResourceRequest _Rq, Action<UnityEngine.Object> _callback)
            {
                RQ = _Rq;
                CallBack = _callback;
            }
        }

        private List<LoadCallBack> mAllCallBacks = new List<LoadCallBack>();
        
        public void LoadAsync(string _path, Action<UnityEngine.Object> _loadCallBack)
        {
            ResourceRequest _rq = Resources.LoadAsync<GameObject>(_path);
            mAllCallBacks.Add(new LoadCallBack(_rq, _loadCallBack));
        }

        public void ChackCallBack()
        {
            for (int i = 0; i < mAllCallBacks.Count; i++)
            {
                if (mAllCallBacks[i].RQ.isDone)
                {
                    if (mAllCallBacks[i].RQ.asset is GameObject _gameObject)
                    {
                        if (mAllCallBacks[i].CallBack is not null)
                            mAllCallBacks[i].CallBack(mAllCallBacks[i].RQ.asset);
                        else
                            EngineDebug.LogWarning($"加载回调是空的");
                    }
                    else
                    {
                        mAllCallBacks[i].CallBack(null);
                    }
                    mAllCallBacks.RemoveAt(i);
                }
            }
        }
    }
}