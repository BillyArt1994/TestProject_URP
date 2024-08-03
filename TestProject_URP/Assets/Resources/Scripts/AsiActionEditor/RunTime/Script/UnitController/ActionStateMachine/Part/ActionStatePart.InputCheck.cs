using System.Collections.Generic;
using UnityEngine;

namespace AsiActionEngine.RunTime
{
    public partial class ActionStatePart
    {
        private Dictionary<string, float> mKeyKeepTime = new Dictionary<string, float>();

        private float mHoldKeyIntervalTime => mActionStateMachine.HoldKeyIntervalTime;
        
        public string NowInputClickKey { get; private set; } //当前点击的按键
        public string NowInputDownKey { get; set; } //当前按下的按键
        public string NowInputUpKey { get; private set; } //当前松开的按键
        public string NowInputHoldKey { get; private set; } //当前长按的按键

        // 注册按键按下
        public void SetKeyDown(string _keyName)
        {
            // Debug.Log("注册按键按下: " + _keyName);
            NowInputDownKey = _keyName;
            if (mKeyKeepTime.ContainsKey(_keyName))
            {
                mKeyKeepTime[_keyName] = 0;
            }
            else
            {
                mKeyKeepTime.Add(_keyName, 0f);
            }
        }
        // 注册按键抬起
        public void SetKeyUp(string _keyName)
        {
            // Debug.Log("注册按键抬起: " + _keyName);
            if (mKeyKeepTime.ContainsKey(_keyName))
            {
                if (mKeyKeepTime[_keyName] < mHoldKeyIntervalTime)
                {
                    NowInputClickKey = _keyName;
                }
                //重置长按按钮
                if (_keyName == NowInputHoldKey)
                {
                    NowInputHoldKey = MotionEngineConst.NondKeyName;
                }
                NowInputUpKey = _keyName;
                mKeyKeepTime.Remove(_keyName);
            }
        }

        public void ReSetKeys(bool _resetHoldKey = false)
        {
            NowInputClickKey = MotionEngineConst.NondKeyName;
            NowInputDownKey = MotionEngineConst.NondKeyName;
            NowInputUpKey = MotionEngineConst.NondKeyName;
            if (_resetHoldKey)
            {
                NowInputHoldKey = MotionEngineConst.NondKeyName;
            }
        }

        private void UpdateInputKey(float _deltatime)
        {
            List<string> _keys = new List<string>(mKeyKeepTime.Keys);
            foreach (var _key in _keys)
            {
                float _value = mKeyKeepTime[_key];
                if (_value < mHoldKeyIntervalTime)
                {
                    mKeyKeepTime[_key] += _deltatime;
                    if (mKeyKeepTime[_key] >= mHoldKeyIntervalTime)
                    {
                        NowInputHoldKey = _key;
                    }
                }
            }
        }
        
    }
}