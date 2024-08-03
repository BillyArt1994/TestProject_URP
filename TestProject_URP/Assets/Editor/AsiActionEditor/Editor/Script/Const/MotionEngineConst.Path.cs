using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public partial class MotionEngineConst
    {
        private static string EditorResourcesPath => 
            (MotionEngineEditorPath.Instance.EditorLocalPath + "Resources/");

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
    }
}