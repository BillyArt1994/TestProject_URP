using AsiActionEngine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public partial class TimeLineWindow
    {
        private MouseActionInfo mTimeLineControlCallBack_dwon;
        private MouseActionInfo mTimeLineControlCallBack_move;
        private MouseActionInfo mTimeLineControlCallBack_up;
        
        private bool mTimeControlIsDraw = false;
        private bool mIsPlaying = false;
        private bool mChangeKeep = false;

        private int mTimeLineStartTime;
        private int mNowTime = 0;
                
        private float mUpdateTotalTime;
        private float mLastTime = 0f;
        
        public int NowTime
        {
            get { return mNowTime; }
        }

        public void InitTimeLineControl()
        {
            mTimeLineControlCallBack_dwon = new MouseActionInfo(OnLoadTimeControlEvent, MotionEngineConst.Priority_TimeControl);
            mTimeLineControlCallBack_move = new MouseActionInfo(OnMoveTimeControlHandle, MotionEngineConst.Priority_TimeControl);
            mTimeLineControlCallBack_up = new MouseActionInfo(UnLoadTimeControlEvent, MotionEngineConst.Priority_TimeControl);
            
            // mNowTime = 0;
            if (!mChangeKeep)
            {
                mPreviweState = EPreviweState.Pause;
            }
            mTimeLineStartTime = 0;
            mUpdateTotalTime = 0;
            UpdateTime(0);
        }
        
        private void DrawTimeLineControl(Rect _rect)
        {
            float _timeToPos = GetPosToTime(mNowTime);
            mTimeControlIsDraw = _timeToPos >= TimelineRect.x;
            if (mTimeControlIsDraw) mTimeControlIsDraw = _timeToPos <= TimelineRect.x + TimelineRect.width;
            if (mTimeControlIsDraw)
            {
                OnLoadMouseAction(EMouseEvent.MouseDwonLeft,mTimeLineControlCallBack_dwon);
                Rect _timeControlRect = new Rect(_rect);
                _timeControlRect.y += 3f;
                _timeControlRect.height -= 3f;
                _timeControlRect.width = MotionEngineConst.GUI_TimeLineControlWidth;
                _timeControlRect.x = _timeToPos - MotionEngineConst.GUI_TimeLineControlWidth * 0.5f;
                EditorGUI.DrawRect(_timeControlRect, EngineSetting.colorTimeLineControl);

                Vector2 _starPos = new Vector2(_timeToPos, _timeControlRect.y + _timeControlRect.height);
                Vector2 _endPos = new Vector2(_timeToPos, TimelineRect.height + TimelineRect.y);
                Handles.color = EngineSetting.colorTimeLineControl;
                Handles.DrawLine(_starPos,_endPos);
            }

            UpdateEditorTimeLine();
            UpdateBakeAttackBox();//烘焙攻击盒时
        }

        private void UpdateEditorTimeLine()
        {
            bool isPlaying = mPreviweState == EPreviweState.Play;
            if (mIsPlaying != isPlaying)
            {
                //初始化
                if (isPlaying)
                {
                    mLastTime = Time.realtimeSinceStartup;
                    if (mNowTime >= mActionTotalTime)
                    {
                        mTimeLineStartTime = 0;
                    }
                    else
                    {
                        mTimeLineStartTime = mNowTime;
                    }
                    mUpdateTotalTime = 0;
                }
                mIsPlaying = isPlaying;
            }

            //每帧更新 TimeLine 状态 
            if (isPlaying)
            {
                float _editorDeltaTime = Time.realtimeSinceStartup - mLastTime;
                mUpdateTotalTime += _editorDeltaTime * mPlaySpeed * RunTime.MotionEngineConst.TimeDoubling;
                int _updateNowTime = mTimeLineStartTime + Mathf.RoundToInt(mUpdateTotalTime);

                //播放到末尾了
                if (_updateNowTime > mActionTotalTime)
                {
                    if (mIsLoop)
                    {
                        mTimeLineStartTime -= mActionTotalTime;
                    }
                    else
                    {
                        mPreviweState = EPreviweState.Pause;
                        _updateNowTime = mActionTotalTime;
                    }
                }
                
                UpdateTime(_updateNowTime);
                
                mLastTime = Time.realtimeSinceStartup;
            }
        }
        
        private void UpdateTime(int _time)
        {
            if (needInit)
            {
                return;
            }
            
            mNowTime = _time;
            mTimeViewer.SetTime(_time);
            GUI.changed = true;

            ScenceDraw.Instance.DrawBoxInit();
            
            if (!(SelectActionState.EditorAnimEvent == null || SelectActionState.EditorAnimEvent.EventData == null))
            {
                // EditorEventUpdate(_time, SelectActionState.EditorAnimEvent);
                EventUpdate.Instance.EditorEventUpdate(_time, SelectActionState.EditorAnimEvent);
            }

            if (SelectActionState.AllEventTrackGroup != null)
            {
                EventUpdate.Instance.Init();
                foreach (var _trackGroup in SelectActionState.AllEventTrackGroup)
                {
                    foreach (var _actionTrack in _trackGroup.CurActiontTrack)
                    {
                        if (_actionTrack is EventTrack _eventTrack)
                        {
                            if (_eventTrack.IsPreview)
                            {
                                foreach (var _eventDisplay in _eventTrack.CurEventDisplay)
                                {
                                    EventUpdate.Instance.EditorEventUpdate(_time, _eventDisplay.MainEvent);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateTimeToNow()
        {
            UpdateTime(mNowTime);
        }
        
        //回调
        private bool OnLoadTimeControlEvent(Event _event)
        {
            if (mHeadrInteractRect.Contains(_event.mousePosition))
            {
                mPreviweState = EPreviweState.Pause;
                UpdateTime(GetTimeToPos(_event.mousePosition.x));
                OnLoadMouseAction(EMouseEvent.MouseDrag, mTimeLineControlCallBack_move);
                OnLoadMouseAction(EMouseEvent.MouseUp, mTimeLineControlCallBack_up);
                return true;
            }
            return false;
        }

        private bool OnMoveTimeControlHandle(Event _event)
        {
            int _mNowTime = GetTimeToPos(_event.mousePosition.x);
            _mNowTime = Mathf.Clamp(_mNowTime, 0, mActionTotalTime);
            UpdateTime(_mNowTime);
            // mNowTime = GetTimeToPos(_event.mousePosition.x);
            // mNowTime = Mathf.Clamp(mNowTime, 0, mActionTotalTime);
            // mTimeViewer.SetTime(mNowTime);
            return false;
        }

        private bool UnLoadTimeControlEvent(Event _event)
        {
            UnLoadMouseAction(EMouseEvent.MouseDwonLeft,mTimeLineControlCallBack_dwon);
            UnLoadMouseAction(EMouseEvent.MouseDrag, mTimeLineControlCallBack_move);
            UnLoadMouseAction(EMouseEvent.MouseUp, mTimeLineControlCallBack_up);
            return false;
        }
    }
}