using System.Collections.Generic;
using AsiActionEngine.RunTime;

namespace AsiActionEngine.Editor
{
    [System.Serializable]
    public class EditorUnitWarp : UnitWarp
    {
        public string SettingPath = string.Empty;//引擎设置路径
        public bool IsPlayer = false;
        
        public EditorUnitWarp(int _id, string _name) : base(_id, _name)
        {
            
        }

        public UnitWarp GetUnitWarp()
        {
            UnitWarp _unitWarp = new UnitWarp(ID, Name);

            _unitWarp.CameID = CameID;
            _unitWarp.ModelPath = ModelPath;
            _unitWarp.Action = Action;
            
            return _unitWarp;
        }
    }
}