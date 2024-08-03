using System;
using AsiActionEngine.RunTime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AsiTimeLine.RunTime
{
    public class UnitCreact :MonoBehaviour
    {
        public string UnitName;
        public bool IsPlayer = false;
        private void Start()
        {
            ActionEngineManager_Input.Instance.CreactGameManager();

            if (string.IsNullOrEmpty(UnitName))
            {
                return;
            }
            
            ActionEngineManager_Unit.Instance.CreactUnit(UnitName, (Unit _unit) =>
            {
                _unit.transform.SetPositionAndRotation(transform.position, transform.rotation);
                if (IsPlayer)
                {
                    ActionEngineManager_Input.Instance.ChangePlayer(_unit);
                }
            });
        }
    }
}