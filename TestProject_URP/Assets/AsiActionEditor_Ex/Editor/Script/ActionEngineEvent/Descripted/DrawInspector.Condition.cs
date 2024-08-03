using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiTimeLine.Editor
{
    public partial class DrawInspector
    {
        public static void DrawCondition(IInterruptCondition _interruptCondition, bool _isInit)
        {
            
            switch ((EConditionType)_interruptCondition.InterruptType)
            {
                // case EInterruptType.EIT_CheckInput:
                //     DrawCheckCostomKey((CheckCostomKey)_interruptCondition);
                //     break;
                default:
                    DrawEditorAttribute.Draw(_interruptCondition);
                    break;
            }
        }
    }
}