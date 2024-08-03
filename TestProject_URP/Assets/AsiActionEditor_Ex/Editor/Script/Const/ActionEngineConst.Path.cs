using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace AsiTimeLine.Editor
{
    public partial class ActionEngineConst
    {
        private static string EditorResourcesPath => 
            (ActionEngineEditorPath.Instance.EditorLocalPath + "Resources/");

        public static readonly string EditorSetting = EditorResourcesPath + "Setting/WindowSetting.asset";
        public static readonly string EditorDataPath = EditorResourcesPath + "Data/";

        public static string EditorUnitSavePath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return EditorDataPath + "Unit";
            }
            return EditorDataPath + $"Unit/{_name}.json";
        }
        public static string EditorActionSavePath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return EditorDataPath + "Action";
            }
            return EditorDataPath + $"Action/{_name}.json";
        }
        
        public static string EditorItemSavePath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return EditorDataPath + "Item";
            }
            return EditorDataPath + $"Item/{_name}.json";
        }
        
        public static string EditorCamSavePath(string _name = "")
        {
            if(string.IsNullOrEmpty(_name))
            {
                return EditorDataPath + "Camera";
            }
            return EditorDataPath + $"Camera/{_name}.json";
        }
    }
}