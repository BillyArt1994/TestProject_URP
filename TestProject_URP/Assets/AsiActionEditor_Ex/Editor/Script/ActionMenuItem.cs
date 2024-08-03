using System;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using UnityEditor;

namespace AsiTimeLine.Editor
{
    public class ActionMenuItem
    {
        [MenuItem("AsitirTool/ActionEditor %`")]
        private static void OnActionWindowMain()
        {
            ActionWindowMain.MainWindow();//技能编辑器
            // EditorApplication.ExecuteMenuItem("");
        }
    }
    
    //注册行为编辑器的初始化
    public class ActionEngineInit
    {
        private static ActionEngineEvent ActionEngineEvent;
        
        [UnityEditor.Callbacks.DidReloadScripts(0)]
        public static void ActionEngineInit233()
        {
            if (!Init())
            {
                EngineDebug.LogError("编辑器尝试初始化失败");
                return;
            }

            EditorActionData _editorActionData = new EditorActionData();
            
            _editorActionData.EventDataType = Enum.GetNames(typeof(EEvenType));
            _editorActionData.ConditionType = Enum.GetNames(typeof(EConditionType));
            _editorActionData.AsiActionEditorFuntion = ActionEngineEvent;
            
            ActionWindowMain.InitWindow(_editorActionData);
        }
        
        private static bool Init()
        {
            if (ActionEngineEvent is null)
                ActionEngineEvent = new ActionEngineEvent();
            
            return true;
        }
    }
}