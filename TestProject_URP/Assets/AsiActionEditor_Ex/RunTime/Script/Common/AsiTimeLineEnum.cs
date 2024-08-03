using System.ComponentModel;

namespace AsiTimeLine.RunTime
{
    public enum EConditionType
    {
        [Description("检测玩家是否在地面")] EIT_CheckGround,
        [Description("检测玩家前方障碍")] EIT_CheckBarrier,
        [Description("检查当前找到的交互点")] EIT_CheckFindPoint,
        
        [Description("检测玩家输入方向和角色方向差值")] EIT_CheckInputToTranDir,
        [Description("检测单位行为状态")] EIT_CheckActionState,
        [Description("检测单位行为标签")] EIT_CheckActionLable,
    }
    
    public enum EEvenType 
    { 
        //相机切换
        [Description("相机跳转")] EET_CameraChange,
        
        //动画状态相关
        [Description("设置Animator的Float参数")] EET_SetAnimFloat,
        [Description("设置角色对齐到障碍交互点位")] EET_InteractBarrier,
        [Description("移动角色到钩锁交互点")] EET_MoveToHookPoint,
        
        // 角色行为相关 
        [Description("音效")] EET_Audio,
        [Description("特效")] EET_Effect,
        
        //道具交互
        [Description("武器挂点切换")] EET_WeaponPointChange,
        [Description("交互点类型")] EET_FindPoint,
        
        // 杂项
        [Description("Debug")] EET_ActionDebug,

        //单位相关
        [Description("角色位移")] EET_CharacterMove,
        [Description("角色重力")] EET_CharacterGravity,
        [Description("角色力度施加")] EET_CharacterAddForce,
        [Description("角色仅位移")] EET_CharacterOnMove,
        [Description("单位朝向")] EET_UnitRot,

        //单位状态
        [Description("单位状态标签")] EET_UnitStateLable,
        
        //2D控制器
        [Description("2D角色位移")] EET_2D_Move,
        [Description("2D角色位移")] EET_2D_Gravity,
    }
}