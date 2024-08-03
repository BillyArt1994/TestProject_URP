using System;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using AsiActionEditor_Ex.RunTime;

namespace AsiTimeLine.Editor
{
    public partial class DrawInspector
    {
        public static string GetTitle(EditorActionEvent _actionEvent, bool _isInit)
        {
            string _title = String.Empty;
            
            IActionEventData _eventData = _actionEvent.EventData;
            if (_eventData == null)
            {
                return "Error";
            }
            
            if (_eventData is Event_UnitStateLable _eusl)
            {
                if (_eusl.IsRemoveAll)
                {
                    _title = "状态标签: <color=#FF0000>删除所有标签</color>";
                }
                else
                {
                    _title = string.Format("状态标签：<color=#FFCC00>{0}</color>\n类型: {1}",
                        ResourcesWindow.Instance.ActionLable[_eusl.StateLable],
                        _eusl.IsRemove ? "<color=#FF0000>删除该标签</color>" : "<color=#00FF00>添加该标签</color>"
                    );
                }
            }
            else if (_eventData is Event_WeaponPointChange _ewpc)
            {
                _title = string.Format("武器挂点切换：{0}\n挂点: <color=#FFCC00>{1}</color>",
                    _ewpc.IsRightWeapon ? "右" : "左",
                    ((ECharacteLimbType)_ewpc.LimbPointType).ToString()
                );
            }else if (_eventData is Event_CameraChange _ecc)
            {
                if (ResourcesWindow.Instance.PreCamControl)
                {
                    _title = string.Format(
                        "相机跳转: <color=#ffcc00><b>{0}</b></color>\nExitCam: <color=#ffcc00><b>{1}</b></color>", 
                        ResourcesWindow.Instance.PreCamControl.allCinemachine[_ecc.EnterCam].name, 
                        ResourcesWindow.Instance.PreCamControl.allCinemachine[_ecc.ExitCam].name
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