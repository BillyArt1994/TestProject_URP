﻿using System;
using AsiActionEngine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public partial class ResourcesWindow : EditorWindow
    {
        #region Instance
        private static ResourcesWindow _instance;
        public static ResourcesWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GetWindow<ResourcesWindow>();
                    _instance.titleContent = new GUIContent("Resources");
                }

                return _instance;
            }
        }

        #endregion

        public static bool needInit = true;
        
        private void OnGUI()
        {
            if (!Init())
            {
                return;
            }

            DrawPreviewGUI();
        }

        private bool Init()
        {
            if (!needInit) return true;

            DrawEditorAttribute.LoadObjectDic.Clear();
            InitUnitGUI();
            InitActionStateGUI();
            
            needInit = false;
            return true;
        }

        public GameObject GetRole()
        {
            if (mRole != null)
                return mRole;

            EditorUtility.DisplayDialog("警告", "请先创建预览角色", "我知道了");
            return null;
        }

        public Unit GetUnit()
        {
            if (GetRole() != null)
            {
                return mUnit;
            }
            return null;
        }
        
        public bool GetPorjectPathTo(out string _porjectPath, string _inPath)
        {
            string[] _allPath = _inPath.Split('/');
            int _isFindAssets = -1;
            for (int i = 0; i < _allPath.Length; i++)
            {
                if (_allPath[i] == "Assets")
                {
                    _isFindAssets = i;
                }
            }

            string _curPath = "";
            if (_isFindAssets > -1)
            {
                for (int i = _isFindAssets; i < _allPath.Length-1; i++)
                {
                    _curPath += _allPath[i] + "/";
                }
                _curPath += _allPath[^1];
                _porjectPath = _curPath;
                return true;
            }

            _porjectPath = "";
            return false;
        }
    }
}