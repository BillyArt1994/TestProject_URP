using AsiActionEditor_Ex.RunTime;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;

namespace AsiTimeLine.Editor
{
    public partial class CreactActionEvent
    {
        public static EditorActionEvent Events(int _id)
        {
            EEvenType _eventType = (EEvenType)_id;
            switch (_eventType)
            {
                //Camera
                case EEvenType.EET_CameraChange:
                    return CreatAction(new Event_CameraChange(),true);//单帧
                
                //Misc
                case EEvenType.EET_ActionDebug:
                    return CreatAction(new Event_ActionDebug(),true);//单帧
                
                //Anima
                case EEvenType.EET_SetAnimFloat:
                    return CreatActionInheritable(new Event_SetAnimFloat(),true);//单帧, 可继承
                case EEvenType.EET_InteractBarrier: 
                    return CreatAction(new Event_InteractBarrier());
                
                //ItemInteract
                case EEvenType.EET_WeaponPointChange:
                    return CreatAction(new Event_WeaponPointChange(),true);//单帧
                case EEvenType.EET_FindPoint:
                    return CreatAction(new Event_FindPoint());
                
                //Unit
                case EEvenType.EET_CharacterMove:
                    return CreatActionInheritable(new Event_CharacterMove());//可继承
                case EEvenType.EET_CharacterGravity:
                    return CreatAction(new Event_CharacterGravity(),true);//单帧
                case EEvenType.EET_CharacterAddForce:
                    return CreatAction(new Event_CharacterAddForce(),true);//单帧
                case EEvenType.EET_CharacterOnMove:
                    return CreatActionInheritable(new Event_CharacterOnMove());//可继承
                case EEvenType.EET_UnitRot:
                    return CreatActionInheritable(new Event_UnitRot());//可继承
                case EEvenType.EET_UnitStateLable:
                    return CreatActionInheritable(new Event_UnitStateLable());//可继承
                
                //2D控制器
                case EEvenType.EET_2D_Move:
                    return CreatActionInheritable(new Event_2D_Move());
                case EEvenType.EET_2D_Gravity:
                    return CreatAction(new Event_2D_Gravity());
            }
            
            EngineDebug.LogError($"客户端下，未写明 [<color=#FFCC00>{_eventType}</color>] 的实例化，请告知客户端程序");
            return null;
        }
        
        private static EditorActionEvent CreatAction(IActionEventData _event, bool _Single = false)
        {
            return new EditorActionEvent(_event, _Single ? 0 : AsiActionEngine.RunTime.MotionEngineConst.TimeDoubling);
        }
        private static EditorActionEvent CreatAction(IActionEventData _event, int _Duration)
        {
            return new EditorActionEvent(_event, _Duration);
        }
        
        //创建可上下继承内部参数的事件轨道
        private static EditorActionEvent CreatActionInheritable(IActionEventData _event, bool _Single = false)
        {
            EditorActionEvent _actionEvent = CreatAction(_event, _Single);
            _actionEvent.EditorInheritable = true;
            _actionEvent.Inheritable = true;
            return _actionEvent;
        }
        private static EditorActionEvent CreatActionInheritable(IActionEventData _event, int _Duration)
        {
            EditorActionEvent _actionEvent = CreatAction(_event, _Duration);
            _actionEvent.EditorInheritable = true;
            _actionEvent.Inheritable = true;
            return _actionEvent;
        }
    }
}