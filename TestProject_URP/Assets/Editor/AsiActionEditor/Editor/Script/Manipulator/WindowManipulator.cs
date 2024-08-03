using System;
using System.Collections.Generic;
using AsiActionEngine.RunTime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public delegate bool mouseActionEven(Event _event);

    public class MouseActionInfo
    {
        public mouseActionEven mEvent;
        public int mPriority;
        public MouseActionInfo(mouseActionEven _event, int _priority)
        {
            mEvent = _event;
            mPriority = _priority;
        }
    }

    public class WindowManipulator
    {
        private Dictionary<EMouseEvent, List<MouseActionInfo>> mMouseEvents = new Dictionary<EMouseEvent, List<MouseActionInfo>>();
        
        public void Update(Rect _rect, Event _event)
        {
            if (ExecuteCallback(_rect, _event))
            {
                _event.Use();
            }
        }

        public void OnLoadMouseEvent(EMouseEvent _eventType, MouseActionInfo _even)
        {
            if(!mMouseEvents.ContainsKey(_eventType))
                mMouseEvents.Add(_eventType,new List<MouseActionInfo>());
            if (!mMouseEvents[_eventType].Contains(_even))
            {
                mMouseEvents[_eventType].Add(_even);
            }
        }
        public void UnLoadMouseEvent(EMouseEvent _eventType, MouseActionInfo _even)
        {
            if (mMouseEvents.TryGetValue(_eventType, out var _eventList))
            {
                _eventList.Remove(_even);
            }
            else
            {
                EngineDebug.LogWarning($"警告 从未申请过 {_eventType.ToString()} 类型");
            }
        }

        public void Clear()
        {
            mMouseEvents.Clear();
        }

        private bool ExecuteCallback(Rect _rect, Event _event)
        {
            EMouseEvent _mouseEventType = EMouseEvent.Empty;
            EventType _EventType = _event.type;
            if (_EventType == EventType.MouseDown)
            {
                if (_event.button == 0)
                {
                    _mouseEventType = EMouseEvent.MouseDwonLeft;
                }
                else if (_event.button == 1)
                {
                    _mouseEventType = EMouseEvent.MouseDwonRight;
                }
            }
            else if (_EventType == EventType.MouseDrag)
            {
                _mouseEventType = EMouseEvent.MouseDrag;
            }
            else if (_EventType == EventType.MouseUp)
            {
                _mouseEventType = EMouseEvent.MouseUp;
            }

            if (_mouseEventType == EMouseEvent.Empty) return false;
            if (mMouseEvents.TryGetValue(_mouseEventType, out var _list))
            {
                switch (_event.type)
                {
                    case EventType.MouseDown:
                        if (!_rect.Contains(_event.mousePosition))
                            return false;
                        _list.Sort((x, y) => { return -x.mPriority.CompareTo(y.mPriority);});
                        break;
                    case EventType.ContextClick:
                        //Debug.LogWarning("熟读");
                        return true;

                }
                
                for (int i = 0; i < _list.Count; i++)
                {
                    if (_list[i].mEvent.Invoke(_event))
                    {
                        return true;
                    }
                }

                return false;
            }
            return false;
        }
    }
}