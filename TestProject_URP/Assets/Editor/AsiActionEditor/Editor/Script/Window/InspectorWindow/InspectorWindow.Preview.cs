using AsiActionEngine.RunTime;

namespace AsiActionEngine.Editor
{
    public partial class InspectorWindow
    {
        public IProperty CurSelectProperty => mSelectProperty;
        private IProperty mSelectProperty = null;
        private void DrawPerviewGUI()
        {
            if(mSelectProperty == null)return;
            if (mSelectProperty is EditorUnitWarp _unitWarp)
            {
                DrawUnitWarp(_unitWarp);
            }
            else if (mSelectProperty is ItemWarp _weaponWarp)
            {
                
            }            
            else if (mSelectProperty is EditorActionState _editorActionState)
            {
                DrawActionState(_editorActionState);
            }
            else if (mSelectProperty is EditorActionEvent _editorActionEvent)
            {
                DrawActionEventGUI(_editorActionEvent);
            }
            else if (mSelectProperty is EditorActionInterrupt _editorActionInterrupt)
            {
                DrawActionInterruptGUI(_editorActionInterrupt);
            }
            else if (mSelectProperty is EditorCameraWarp _cameraWarp)
            {
                DrawCameraWarpGUI(_cameraWarp);
            }
        }


    }
}