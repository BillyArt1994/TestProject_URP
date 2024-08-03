﻿using System;
using System.Collections.Generic;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using AsiActionEditor_Ex.RunTime;
using UnityEngine;


namespace AsiTimeLine.Editor
{
    public partial class DrawInspector
    {
        public static string GetDetailed(EditorActionEvent _actionEvent, bool _isInit)
        {
            string _title = String.Empty;
            
            IActionEventData _eventData = _actionEvent.EventData;
            if (_eventData == null)
            {
                return "";
            }
            
            if (_eventData is Event_PlayAnim _epa)
            {
                _title = string.Format("动画：{0}\n总长{1}",
                    _epa.AnimName,
                    (float)_actionEvent.Duration / AsiActionEngine.RunTime.MotionEngineConst.TimeDoubling
                );
            }
            else if (_eventData is Event_CameraChange _ecc)
            {
                // CameraControl _cameraControl = ResourcesWindow.Instance.PreCamControl;
                if (ResourcesWindow.Instance.PreCamera)
                {
                    Behaviour[] _allCinemachine = ResourcesWindow.Instance.PreCamControl.allCinemachine;
                    List<string> _ActionLable = ResourcesWindow.Instance.ActionLable;
                    bool _CinemachineSelf = _ecc.EnterCam < _allCinemachine.Length && _ecc.EnterCam > -1;
                    bool _ActionLableSelf = _ecc.ActionLable < _ActionLable.Count && _ecc.ActionLable > -1;
                    _title = string.Format(
                        "Enter: <color=#ffcc00>{0}</color>\nLable: <color=#00aa00>{1}</color>",
                        _CinemachineSelf ? _allCinemachine[_ecc.EnterCam].name : _ecc.EnterCam,
                        _ActionLableSelf ? _ActionLable[_ecc.ActionLable] : _ecc.ActionLable
                    );
                }
                else
                {
                    _title = "<color=#FF0000>未创建预览相机，无法显示</color>";
                }
            }
            
            return _title;
        }
    }
}