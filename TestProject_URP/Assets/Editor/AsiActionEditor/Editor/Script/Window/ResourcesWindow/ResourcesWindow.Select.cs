using AsiActionEngine.RunTime;

namespace AsiActionEngine.Editor
{
    public partial class ResourcesWindow
    {
        public int mSelectmenuID { get; private set; }
        public int mOnSelectUnitID { get; private set; }
        public int mOnSelectActionStateID { get; private set; }
        
        public void OnSelectMenu(int _id)
        {
            mSelectmenuID = _id;
            if (_id < 0)
            {
                mSelectmenuID = 0;
                EngineDebug.LogWarning($"不存在负ID的菜单");
            }
            else if (_id >= mMenu.Length)
            {
                mSelectmenuID = mMenu.Length - 1;
                EngineDebug.LogWarning($"不存在ID为【{_id}】 的菜单");
            }

            //在菜单选择时初始化
            if (mSelectmenuID == 3)
            {
                InitCameraGUI();
            }
        }

        public void OnSelectUnit(int _id)
        {
            mOnSelectUnitID = _id;
            InspectorWindow.Instance.SelectProperty(mUnitWarp[_id]);
        }
        
        public void OnSelectActionState(int _id)
        {
            mOnSelectActionStateID = _id;
            InspectorWindow.Instance.SelectProperty(mActionState[_id]);
            TimeLineWindow.Instance.OnChangeAction(mActionState[_id]);
            TimeLineWindow.Instance.Repaint();
        }
        public void OnSelectActionState(EditorActionState _actionState)
        {
            mOnSelectActionStateID = mActionState.IndexOf(_actionState);
            InspectorWindow.Instance.SelectProperty(mActionState[mOnSelectActionStateID]);
            TimeLineWindow.Instance.OnChangeAction(mActionState[mOnSelectActionStateID]);
            TimeLineWindow.Instance.Repaint();
        }

        public void OnSelectCamWarp(EditorCameraWarp _cameraWarp)
        {
            InspectorWindow.Instance.SelectProperty(_cameraWarp);
        }

        public EditorActionState GetEditorActionStateToSelect()
        {
            if(mActionState == null || mOnSelectActionStateID >= mActionState.Count)
            {
                return null;
            }
            return mActionState[mOnSelectActionStateID];
        }

        public string GetActionToCurrName()
        {
            return mActionState[mOnSelectActionStateID].Name;
        }
        
        public string GetUnitToCurrName()
        {
            return mUnitWarp[mOnSelectUnitID].Name;
        }
    }
}