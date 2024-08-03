using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;

namespace AsiTimeLine.Editor
{
    public partial class CreactActionEvent
    {
        public static IInterruptCondition Condition(int _id)
        {
            EConditionType _interruptType = (EConditionType)_id;
            
            switch (_interruptType)
            {
                case EConditionType.EIT_CheckGround:
                    return new CheckGround();
                
                case EConditionType.EIT_CheckBarrier:
                    return new CheckBarrierData();
                
                case EConditionType.EIT_CheckInputToTranDir:
                    return new CheckInputToTranDir();
                
                case EConditionType.EIT_CheckActionLable:
                    return new CheckUnitActionStateLable();
                
                case EConditionType.EIT_CheckActionState:
                    return new CheckUnitActionState();
                
                default:
                    EngineDebug.LogError("客户端未配置实例化!! " + _interruptType);
                    return null;
            }
        }
    }
}