using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using AsiActionEditor_Ex.RunTime;
using UnityEngine;

namespace AsiTimeLine.Editor
{
    public class EditorEventUpdate
    {
        public static void OnInit()
        {
            mEventWeaponPointTime = int.MinValue;
            mEventCameraChangeTime = int.MinValue;
        }
        
        public static bool OnUpdate(int _time, EditorActionEvent _actionEvent)
        {
            IActionEventData _eventData = _actionEvent.EventData;

            if(_eventData is Event_WeaponPointChange _eventWeaponPoint)
            {
                Update_EventWeaponPoint(_time, _eventWeaponPoint, _actionEvent);
            }//武器挂点切换
            else if (_eventData is Event_CameraChange _eventCameraChange)
            {
                Update_CameraChange(_time, _eventCameraChange, _actionEvent);
            }
            else
            {
                return false;
            }

            return true;
        }

        #region UpdateEditorFuntion

        private static int mEventCameraChangeTime;
        private static CharacterConfig _characterConfig;
        private static void Update_CameraChange(int _time, 
            Event_CameraChange _eventPlayAnim, EditorActionEvent _actionEvent)
        {
            if (_time >= _actionEvent.TriggerTime)
            {
                if (_actionEvent.TriggerTime > mEventCameraChangeTime)
                {
                    mEventCameraChangeTime = _actionEvent.TriggerTime;
                }
                else
                {
                    return;
                }

                if (ResourcesWindow.Instance.PreCamera is not null)
                {
                    CameraControl _cameraControl = ResourcesWindow.Instance.PreCamControl;
                    ResourcesWindow.Instance.GetRole().TryGetComponent(out CharacterConfig _characterConfig);

                    if (_characterConfig.HelpPointDic.TryGetValue((ECharacteLimbType)_eventPlayAnim.EnterPoint,
                            out Transform _camPoint))
                    {
                        _cameraControl.ChangeCam(_eventPlayAnim.EnterCam, _camPoint);
                    }
                    else
                    {
                        EngineDebug.LogWarning($"相机切换失败, 未配置 [{(ECharacteLimbType)_eventPlayAnim.EnterPoint}]");
                    }
                }
                else
                {
                    EngineDebug.LogError(
                        "Event_CameraChange 事件未执行!!\n" +
                        "<color=#FF0000>未创建相机预览</color> !!"
                    );
                }
            }
        }
        
        private static int mEventWeaponPointTime;
        private static void Update_EventWeaponPoint(int _time, 
            Event_WeaponPointChange _eventPlayAnim, EditorActionEvent _actionEvent)
        {
            if (_time > _actionEvent.TriggerTime)
            {
                if (_actionEvent.TriggerTime > mEventWeaponPointTime)
                {
                    mEventWeaponPointTime = _actionEvent.TriggerTime;
                }
                else
                {
                    return;
                }
                if (ResourcesWindow.Instance.GetRole().TryGetComponent(out CharacterConfig _config))
                {
                    Transform _weapon = _eventPlayAnim.IsRightWeapon ? _config.WeaponR : _config.WeaponL;
                    if (_weapon is null)
                    {
                        EngineDebug.LogError(
                            "Event_WeaponPointChange 事件未执行!!\n" +
                            $"<color=#FF0000>CharacterConfig</color> 的" +
                            $"{(_eventPlayAnim.IsRightWeapon ? "右": "左")}手武器配置为空!!"
                        );
                        return;
                    }

                    ECharacteLimbType _characteLimb = (ECharacteLimbType)_eventPlayAnim.LimbPointType;
                    if (_config.HelpPointDic.TryGetValue(_characteLimb, out var _target))
                    {
                        _weapon.parent = _target;
                        if (_eventPlayAnim.AlignToPoint)
                        {
                            _weapon.localPosition = Vector3.zero;
                            _weapon.rotation = _target.rotation;
                        }
                    }
                }
                else
                {
                    EngineDebug.LogError(
                        "Event_WeaponPointChange 事件未执行!!\n" +
                        "未挂载 <color=#FF0000>CharacterConfig</color> 组件!!"
                    );
                }
            }
        }
        

        #endregion

    }
}