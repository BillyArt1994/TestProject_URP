using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace AsiActionEngine.RunTime
{
    public enum EEvenTypeInternal 
    { 
        InValid,
        
        //动画状态相关

        
        // 角色行为相关 


        // 动画跳转事件
        [Description("动画跳转")] EET_Interrupt,
        [Description("动画跳转[仅结束时跳转]")] EET_Interrupt_E,
        
        //道具交互

        
        // 杂项


        //单位相关
        [Description("攻击盒")] EET_AttackBox,

        //单位状态

        
        //不希望能在轨道配置的事件
        [Description("动画播放")] EET_DTD_PlayAnim,
    }

    public enum EInputKeyType
    {
        [Description("按下")] OnDown,
        [Description("松开")] OnUp,
        [Description("点击")] OnClick,
        [Description("长按")] OnHold,
    }

    //Animator临时解决方案
    public enum EAnimLayerType
    {
        [Description("动画主要层级")] BaseLayer,
        [Description("单个肢体层级")] LimbLayer,
        [Description("瞄准偏移层级")] UpperLayer,
        [Description("抖动叠加层级")] NoiseLayer,
        [Description("程序逻辑层级")] ScriptLayer
    }

    public enum EInterruptTypeInternal 
    {
        InValid,
        [Description("玩家按键检测")] EIT_CheckInput,
        [Description("玩家移动输入检测")] EIT_CheckMove,
        [Description("被命中")] EIT_CheckBeHit,
        [Description("命中对象")] EIT_CheckOnHit,
    }

    public enum ECharacteLimbType
    {
        //角色肢体
        Root,
        Head,
        Neck,
        Chest,
        Spine2,
        Spine,
        Hips,
        Left_Upper_Leg,
        Left_Lower_Leg,
        Left_Foot,
        Right_Upper_Leg,
        Right_Lower_Leg,
        Right_Foot,
        Left_Shoulder,
        Left_Upper_Arm,
        Left_Lower_Arm,
        Left_Hand,
        Right_Shoulder,
        Right_Upper_Arm,
        Right_Lower_Arm,
        Right_Hand,
        
        //常规道具挂点
        HelpPoint_HUD,
        HelpPoint_WeaponL,
        HelpPoint_WeaponR,
        HelpPoint_WorldL,
        helpPoint_World,
        HelpPoint_WorldR,
        HelpPoint_BehindL,
        HelpPoint_BehindR,
        HelpPoint_WaistL,
        HelpPoint_WaistR,
        
        //相机挂点
        Cam_Main,
        Cam_Look,
        Cam_Ani_A,
        Cam_Ani_B,
    }
}
