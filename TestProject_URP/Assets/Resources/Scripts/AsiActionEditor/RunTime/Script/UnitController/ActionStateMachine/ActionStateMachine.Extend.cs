﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsiActionEngine.RunTime
{
    public partial class ActionStateMachine
    {
        #region Delegate
        private delegate bool actionCheck();
        private delegate float actionRetrunF();
        private delegate float[] actionRetrunAF();
        #endregion

        private Dictionary<string, Component> MonoComponent = new Dictionary<string, Component>();
        private Dictionary<string, ActionLogics>  ActionLogics  = new Dictionary<string, ActionLogics>();
        private Dictionary<string, StaticActionLogics>  StaticActionLogics  = new Dictionary<string, StaticActionLogics>();

        private List<string> VisitedComponent = new List<string>();
        private List<int> ActionLable = new List<int>();

        public List<int> GetActionLableList => ActionLable;

        private void Update_Extend(float _deltaTime)
        {
            foreach (var _actionLogic in ActionLogics)
            {
                _actionLogic.Value.Update(this, _deltaTime);
            }//每帧都跑的逻辑器
            foreach (var _actionLogic in StaticActionLogics)
            {
                _actionLogic.Value.IsEnble = false;
            }//关闭静态的逻辑器
        }
        
        private void LateUpdate_Extend(float _deltaTime)
        {
            foreach (var _actionLogic in ActionLogics)
            {
                _actionLogic.Value.LateUpdate(this, _deltaTime);
            }//每帧都跑的逻辑器

        }
        
        /// <summary>
        /// 消耗极低，如果使用RD看到高消耗警告，只是因为此函数有Debug
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_funtionName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetComponent<T>(out T _value, string _funtionName = "") where T : Component
        {
            string _typeName = _funtionName;
            if (string.IsNullOrEmpty(_funtionName))
            {
                _typeName = typeof(T).Name;
            }

            if (MonoComponent.ContainsKey(_typeName))
            {
                _value = MonoComponent[_typeName] as T;
                return true;
            } //如果字典已经有了这个类型，就直接OUT
            else
            {
                if (!VisitedComponent.Contains(_typeName))
                {
                    VisitedComponent.Add(_typeName);
                    if (CurUnit.TryGetComponent(out T _componet))
                    {
                        MonoComponent.Add(_typeName, _componet);
                        _value = _componet;
                        return true;
                    }
                    else
                    {
                        EngineDebug.LogError($"[<color=#FFCC00>{_typeName}</color>] 组件获取失败!! ");
                    }
                } //检查是否尝试获取过，如果尝试过就不再进行获取。【主要为了避免多次调用 TryGetComponent 】
            } //如果字典没有这个类型，就尝试添加

            _value = null;
            return false;
        }

        public bool TryGetLogic<T>(out T _value, string _funtionName = "") where T : ActionLogics, new()
        {
            string _typeName = _funtionName;
            if (string.IsNullOrEmpty(_funtionName))
            {
                _typeName = typeof(T).Name;
            }

            if (ActionLogics.ContainsKey(_typeName))
            {
                _value = ActionLogics[_typeName] as T;
                return true;
            }
            else
            {
                _value = new T();
                ActionLogics.Add(_typeName, _value);
                _value.Start(this);
                return true;
            }

            _value = null;
            return false;
        }

        public bool TryGetStaticLogic<T>(out T _value, string _funtionName = "") where T : StaticActionLogics, new()
        {
            string _typeName = _funtionName;
            if (string.IsNullOrEmpty(_funtionName))
            {
                _typeName = typeof(T).Name;
            }

            if (StaticActionLogics.ContainsKey(_typeName))
            {
                _value = StaticActionLogics[_typeName] as T;
                if (!_value.IsEnble)
                {
                    _value.OnUpdate(this);
                    _value.IsEnble = true;
                }
                return true;
            }
            else
            {
                _value = new T();
                StaticActionLogics.Add(_typeName, _value);
                _value.OnUpdate(this);
                _value.IsEnble = true;
                return true;
            }

            _value = null;
            return false;
        }
        public void AddActionLable(int _lable)
        {
            if (!ActionLable.Contains(_lable))
            {
                ActionLable.Add(_lable);
            }
        }
        public void RemoveActionLable(int _lable)
        {
            if (ActionLable.Contains(_lable))
            {
                ActionLable.Remove(_lable);
            }
        }
        public void RemoveAllActionLable()
        {
            ActionLable.Clear();
        }
        public bool CheckActionLable(int _lable)
        {
            return ActionLable.Contains(_lable);
        }

        public string GetActionLable(int _lable)
        {
            return mActionStateInfo.mActionLable[_lable];
        }
    }

    public class ActionLogics
    {
        public virtual void Start(ActionStateMachine _actionState) { }

        public virtual void Update(ActionStateMachine _actionState, float _deltaTime) { }
        
        public virtual void LateUpdate(ActionStateMachine _actionState, float _deltaTime) { }
    }

    public abstract class StaticActionLogics
    {
        public bool IsEnble = false;

        public abstract void OnUpdate(ActionStateMachine _actionState);
    }
}