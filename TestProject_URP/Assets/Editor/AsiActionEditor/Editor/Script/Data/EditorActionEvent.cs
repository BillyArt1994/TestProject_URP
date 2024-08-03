using System;
using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    [System.Serializable]
    public class EditorActionEvent : ActionEvent
    {
        public int EventTrackType = 0;//0默认轨道， 1首尾有虚线
        public EditorActionEvent(IActionEventData _eventData, int _Duration = RunTime.MotionEngineConst.TimeDoubling, 
            int _EventTrackType = 0)
        {
            EventData = _eventData;
            Duration = _Duration;
            EventTrackType = _EventTrackType;
        }

        public EditorActionEvent Clone()
        {
            EditorActionEvent _actionEvent = new EditorActionEvent(EventData.Clone(null), Duration, EventTrackType);
            _actionEvent.TriggerTime = mTriggerTime;
            _actionEvent.EditorInheritable = mEditorInheritable;
            _actionEvent.Inheritable = mInheritable;
            return _actionEvent;
        }

    }
}