using AsiActionEditor_Ex.RunTime;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace AsiTimeLine.Editor
{
    public partial class DrawInspector
    {
        public static void DrawEvent(EditorActionEvent _actionEvent, bool _isInit)
        {
            IActionEventData _eventData = _actionEvent.EventData;

            EEvenType _evenType = (EEvenType)_eventData.GetEvenType();

            switch (_evenType)
            {
                case EEvenType.EET_CharacterMove:
                    DrawCharacterMove((Event_CharacterMove)_eventData);
                    break;
                
                case EEvenType.EET_CameraChange:
                    DrawCameraChange(_actionEvent);
                    break;
                
                default:
                    //绘制 EditorProperty 
                    DrawEditorAttribute.Draw(_eventData);
                    break;
            }
        }

        #region DrawFuntion

        private static void DrawCameraChange(EditorActionEvent _actionEvent)
        {
            Event_CameraChange _eventCameraChange = (Event_CameraChange)_actionEvent.EventData;
            if (_actionEvent.Duration > 0)
            {
                using (var _check = new EditorGUI.ChangeCheckScope())
                {
                    DrawEditorAttribute.Draw(_eventCameraChange, new[] { "EnterCam", "EnterPoint", "ExitCam", 
                        "ExitCamPoint", "CheckLable" });
                    if (_check.changed)
                    {
                        TimeLineWindow.Instance.UpdateTimeToNow();
                        // SceneView.RepaintAll();
                    }
                }
                if (_eventCameraChange.CheckLable)
                {
                    DrawEditorAttribute.Draw(_eventCameraChange, new[] { "ActionLable", "IsContain"});
                }
            }
            else
            {
                using (var _check = new EditorGUI.ChangeCheckScope())
                {
                    DrawEditorAttribute.Draw(_eventCameraChange, new[] { "EnterCam", "EnterPoint", "CheckLable" });
                    if (_check.changed)
                    {
                        TimeLineWindow.Instance.UpdateTimeToNow();
                        // SceneView.RepaintAll();
                    }
                }

                if (_eventCameraChange.CheckLable)
                {
                    DrawEditorAttribute.Draw(_eventCameraChange, new[] { "ActionLable", "IsContain"});
                }
            }
        }
        private static void DrawCharacterMove(Event_CharacterMove _ecm)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("监测到位移输入时移动: ",GUILayout.Width(130));
                _ecm.IsMoveInput = EditorGUILayout.Toggle(_ecm.IsMoveInput);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("移动速度(m/s): ",GUILayout.Width(100));
                _ecm.MoveSpeed = EditorGUILayout.Vector3Field("",_ecm.MoveSpeed);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("线性过渡速度: ",GUILayout.Width(100));
                _ecm.LerpSpeed = EditorGUILayout.FloatField(_ecm.LerpSpeed);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("移动方向参考: ",GUILayout.Width(100));
                _ecm.MoveDirType = (EMoveDirType)EditorGUILayout.EnumPopup(_ecm.MoveDirType);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("朝向移动方向: ",GUILayout.Width(100));
                _ecm.RotToMoveDir = EditorGUILayout.Toggle(_ecm.RotToMoveDir);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("朝向过渡速度: ",GUILayout.Width(100));
                _ecm.RotLerpSpeed = EditorGUILayout.FloatField(_ecm.RotLerpSpeed);
            }
        }

        #endregion
    }
}