using System;
using System.Collections.Generic;
using AsiActionEngine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public class ActionWindowMain
    {
        private static ActionEngineSetting mEngineSetting;
        
        public static ActionEngineSetting EngineSetting
        {
            get
            {
                if (!mEngineSetting)
                {
                    mEngineSetting =
                        AssetDatabase.LoadAssetAtPath<ActionEngineSetting>(MotionEngineConst.EditorSetting);
                    mEngineSetting?.Init();
                }

                return mEngineSetting;
            }
            set
            {
                if (value != null)
                {
                    mEngineSetting = value;
                    mEngineSetting.Init();
                }
                else
                {
                    mEngineSetting =
                        AssetDatabase.LoadAssetAtPath<ActionEngineSetting>(MotionEngineConst.EditorSetting);
                    mEngineSetting.Init();
                }
            }
        }

        public static AsiActionEditorFuntion ActionEditorFuntion;
        public static string[] EventType;
        public static string[] ConditionType;
        public static string[] EventType_m;
        public static string[] ConditionType_m;
        public static List<string> EventTypes = new List<string>();
        public static List<string> ConditionTypes = new List<string>();

        public static void MainWindow()
        {
            TimeLineWindow.needInit = true;
            ResourcesWindow.needInit = true;
            TimeLineWindow.Instance.Show();
            ResourcesWindow.Instance.Show();
            InspectorWindow.Instance.Show();
        }

        public static void InitWindow(EditorActionData _editorActionData)
        {
            ActionEditorFuntion = _editorActionData.AsiActionEditorFuntion;
            EventType = _editorActionData.EventDataType;
            ConditionType = _editorActionData.ConditionType;
            EventType_m = Enum.GetNames(typeof(EEvenTypeInternal));
            ConditionType_m = Enum.GetNames(typeof(EInterruptTypeInternal));
            
            EventTypes.AddRange(EventType_m);
            EventTypes.AddRange(EventType);
            ConditionTypes.AddRange(ConditionType_m);
            ConditionTypes.AddRange(ConditionType);
        }

        public static void UpdateMainWindow()
        {
            TimeLineWindow.Instance.Repaint();
        }

        public static bool GetConditionType(int _typeID, out EInterruptTypeInternal _interruptType)
        {
            if (_typeID < 0)
            {
                _interruptType = (EInterruptTypeInternal)(-_typeID);
                return true;
            }

            _interruptType = EInterruptTypeInternal.InValid;
            return false;
        }
        
        public static string GetAssetPathToResources(string _ResourcePath, string _suffix = "json")
        {
            if (string.IsNullOrEmpty(_suffix))
            {
                return $"Assets/Resources/{_ResourcePath}";

            }
            return $"Assets/Resources/{_ResourcePath}.{_suffix}";
        }
    }

    public struct EditorActionData
    {
        //传入事件枚举
        public string[] EventDataType;
        
        //传入跳转条件枚举
        public string[] ConditionType;

        //执行函数
        public AsiActionEditorFuntion AsiActionEditorFuntion;

        public EditorActionData(
            string[] _eventDataType,
            string[] _conditionType,
            AsiActionEditorFuntion _asiActionEditorFuntion
        )
        {
            EventDataType = _eventDataType;
            ConditionType = _conditionType;
            AsiActionEditorFuntion = _asiActionEditorFuntion;
        }
    }

    public abstract class AsiActionEditorFuntion
    {
        /// <summary>
        /// Editor下，Timeline更新时初始化调用
        /// </summary>
        public abstract void TimeLineInit();
        
        /// <summary>
        /// Editor下，Timeline更新时调用的函数
        /// </summary>
        /// <param name="_deltaTime"></param>
        /// <param name="_actionEvent"></param>
        public abstract bool TimeLineUpdate(int _time, EditorActionEvent _actionEvent);

        /// <summary>
        /// 绘制事件的属性面板
        /// </summary>
        /// <param name="_actionEvent"></param>
        public abstract void DrawEventDate(EditorActionEvent _actionEvent, bool _isInit);

        /// <summary>
        /// 绘制跳转条件面板
        /// </summary>
        /// <param name="_actionCondition"></param>
        public abstract void DrawCondition(IInterruptCondition _actionCondition, bool _isInit);

        /// <summary>
        /// 绘制事件头部描述
        /// </summary>
        /// <param name="_actionEvent"></param>
        /// <returns></returns>
        public abstract string DrawEventTitle(EditorActionEvent _actionEvent, bool _isInit);

        /// <summary>
        /// 绘制事件参数描述
        /// </summary>
        /// <param name="_actionEvent"></param>
        /// <returns></returns>
        public abstract string DrawEventDetailed(EditorActionEvent _actionEvent, bool _isInit);

        /// <summary>
        /// 实例化事件
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public abstract EditorActionEvent CreactActionEvent(int _id);

        /// <summary>
        /// 实例化条件
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public abstract IInterruptCondition CreactCondition(int _id);

        public abstract void ActionUnitPreview(ActionPreviewMark _previewMark, GameObject _target);

        public abstract string GetActionEditorDataPath(string _name = "");
        public abstract string GetUnitEditorDataPath(string _name = "");
        public abstract string GetItemEditorWarpPath(string _name = "");
        public abstract string GetCamEditorDataPath(string _name = "");
        public abstract string GetActionDataPath(string _name = "");
        public abstract string GetUnitDataPath(string _name = "");
        public abstract string GetItemWarpPath(string _name = "");
        public abstract string GetCamDataPath(string _name = "");
        public abstract string GetInputActionPath();
        public abstract string GetInputModule();

        //攻击信息
        public abstract IAttackInfo AttackInfo();
        // //受击回调
        // public abstract void BeHit(IAttackInfo _attackInfo);
        
        /// <summary>
        /// 保存Action数据
        /// </summary>
        /// <param name="_editorActionState">要储存的Action数据</param>
        /// <returns>储存成功</returns>
        public virtual bool SaveActionData(EditorActionStateInfo _editorActionState, string _name)
        {
            return false;
        }
        public virtual bool SaveCameraData(EditorCameraWarp _cameraWarp, string _name)
        {
            return false;
        }
        public virtual bool SaveUnitData(EditorUnitWarp _saveData, string _name)
        {
            return false;
        }
        public virtual bool SaveItemData(string _name)
        {
            return false;
        }
        
        /// <summary>
        /// 加载Action数据
        /// </summary>
        /// <param name="_editorActionState">加载对象</param>
        /// <returns>加载成功</returns>
        public virtual bool LoadActionData(out EditorActionStateInfo _editorActionState, string _name)
        {
            _editorActionState = null;
            return false;
        }
        public virtual bool LoadCameraData(out EditorCameraWarp _cameraWarp, string _name)
        {
            _cameraWarp = null;
            return false;
        }
        public virtual bool LoadUnitData(out EditorUnitWarp _saveData, string _name)
        {
            _saveData = null;
            return false;
        }
        public virtual bool LoadItemData(string _name)
        {
            return false;
        }



    }
}