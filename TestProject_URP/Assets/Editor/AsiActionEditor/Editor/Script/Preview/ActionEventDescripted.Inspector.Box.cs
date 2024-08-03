using System.Collections.Generic;
using System.Linq;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public partial class ActionEventDescripted
    {
        private readonly string[] _boxType = new string[]
        {
            "胶囊",
            "射线",
            "方块",
            "球体"
        };

        private readonly string[] _boxDrawType = new string[]
        {
            "仅常规攻击盒",
            "仅烘焙攻击盒",
            "同时绘制"
        };

        public int BoxDrawType { get; set; }
        public int BakerBoxLife { get; set; }
        private int mBakeNumber;
        private void DrawAttackBox(Event_AttackBox _eventAttackBox, EditorActionEvent _actionEvent)
        {
            if (mIsInit)
            {
                mBakeNumber = 0;
                DrawEditorAttribute.NeedInit = true;
                // Debug.Log("攻击盒初始化");
            }
            AttackBoxInfo attackBoxInfo = _eventAttackBox.AttackBoxInfo;
            
            //已配置的挂点
            List<string> _pointTypesStr = new List<string>();
            List<ECharacteLimbType> _poinTypes = new List<ECharacteLimbType>();
            int _selectID = 0;
            if (ResourcesWindow.Instance.GetRole().TryGetComponent(out CharacterConfig _config))
            {
                ECharacteLimbType[] _ECLT = _config.HelpPointDic.Keys.ToArray();
                for (int i = 0; i < _ECLT.Length; i++)
                {
                    if (_ECLT[i] == attackBoxInfo.ReferPoint)
                    {
                        _selectID = i;
                    }
                    _pointTypesStr.Add(_ECLT[i].ToString());
                    _poinTypes.Add(_ECLT[i]);
                }
            }
            else
            {
                GUIStyle _style = new GUIStyle();
                _style.normal.textColor = Color.red;
                _style.alignment = TextAnchor.MiddleCenter;
                _style.fontSize = 24;
                GUILayout.Label("角色未挂载CharacterConfig组件，\n无法配置攻击盒");
                return;
            }

            if (_pointTypesStr.Count < 1)
            {
                GUILayout.Label("角色挂载了CharacterConfig组件，\n但是并未配置任何挂点");
                return;
            }
            

            
            //挂点选择
            using (var _check = new EditorGUI.ChangeCheckScope())
            {
                _selectID = EditorGUILayout.Popup(_selectID, _pointTypesStr.ToArray());
                attackBoxInfo.ReferPoint = _poinTypes[_selectID];
                if (_check.changed)
                {
                    TimeLineWindow.Instance.UpdateTimeToNow();
                    SceneView.RepaintAll();
                }
            }

            using (var _check = new EditorGUI.ChangeCheckScope())
            {
                //攻击盒类型
                attackBoxInfo.AttackBoxType = EditorGUILayout.Popup(attackBoxInfo.AttackBoxType, _boxType);

                if (attackBoxInfo.AttackBoxType == 0)
                {
                    attackBoxInfo.OffsetPos = EditorGUILayout.Vector3Field("相对位置: ", attackBoxInfo.OffsetPos);
                    attackBoxInfo.OffsetRot = EditorGUILayout.Vector3Field("相对角度: ", attackBoxInfo.OffsetRot);
                    attackBoxInfo.Scale.x = Mathf.Max(EditorGUILayout.FloatField("长度: ", attackBoxInfo.Scale.x),0);
                    attackBoxInfo.Scale.y = EditorGUILayout.FloatField("半径: ", attackBoxInfo.Scale.y);

                }//胶囊
                else if (attackBoxInfo.AttackBoxType == 1)
                {
                    attackBoxInfo.OffsetPos = EditorGUILayout.Vector3Field("相对位置: ",attackBoxInfo.OffsetPos);
                    attackBoxInfo.OffsetRot = EditorGUILayout.Vector3Field("相对角度: ",attackBoxInfo.OffsetRot);
                    attackBoxInfo.Scale.x = Mathf.Max(EditorGUILayout.FloatField("长度: ", attackBoxInfo.Scale.x),0);

                }//射线
                else if (attackBoxInfo.AttackBoxType == 2)
                {
                    attackBoxInfo.OffsetPos = EditorGUILayout.Vector3Field("相对位置: ",attackBoxInfo.OffsetPos);
                    attackBoxInfo.OffsetRot = EditorGUILayout.Vector3Field("相对角度: ",attackBoxInfo.OffsetRot);
                    attackBoxInfo.Scale.x = Mathf.Max(EditorGUILayout.FloatField("长度: ", attackBoxInfo.Scale.x),0);

                }//方块
                else if (attackBoxInfo.AttackBoxType == 3)
                {

                }//球

                if (_check.changed)
                {
                    if (TimeLineWindow.Instance.mPreviweState != EPreviweState.Play)
                    {
                        TimeLineWindow.Instance.UpdateTimeToNow();
                        SceneView.RepaintAll();
                    }
                }
            }

            GUILayout.Space(20);
            using (new GUIColorScope(Color.cyan))
            {
                GUILayout.Label("烘焙攻击盒");
            }

            GUIStyle _bakestyle = new GUIStyle();
            _bakestyle.normal.textColor = Color.white;
            if (attackBoxInfo.Box.Length > 0)
            {
                GUILayout.Label($"攻击盒已烘焙数量: <color=#FFCC00>{attackBoxInfo.Box.Length}</color>",_bakestyle);
            }
            else
            {
                _bakestyle.normal.textColor = Color.red;
                GUILayout.Label("!!从未烘焙过攻击盒!!", _bakestyle);
            }
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("烘焙", GUILayout.Width(60)))
                {
                    TimeLineWindow.Instance.BekaAttackBoxParts(_actionEvent, mBakeNumber);
                }
                mBakeNumber = EditorGUILayout.IntField(mBakeNumber);
                using (new GUIColorScope(Color.red))
                {
                    if (GUILayout.Button("清空烘焙", GUILayout.Width(80)))
                    {
                        _eventAttackBox.AttackBoxInfo.Box = new AttackBoxPart[0];
                        // _eventAttackBox.AttackBoxInfo = new AttackBoxInfo(
                        //     new AttackBoxPart[0],
                        //     _eventAttackBox.AttackBoxInfo.AttackBoxType,
                        //     _eventAttackBox.AttackBoxInfo.ReferPoint,
                        //     _eventAttackBox.AttackBoxInfo.OffsetPos,
                        //     _eventAttackBox.AttackBoxInfo.OffsetRot,
                        //     _eventAttackBox.AttackBoxInfo.Scale
                        // );
                    }
                }
            }
            
            GUILayout.Space(20);
            using (new GUIColorScope(Color.cyan))
            {
                GUILayout.Label("攻击盒整体显示状态");
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("绘制类型: ",GUILayout.Width(60));
                using (var _check = new EditorGUI.ChangeCheckScope())
                {
                    BoxDrawType = EditorGUILayout.Popup(BoxDrawType, _boxDrawType);
                    if (_check.changed)
                    {
                        TimeLineWindow.Instance.UpdateTimeToNow();
                        SceneView.RepaintAll();
                    }
                }
            }
            if (BoxDrawType == 1 || BoxDrawType == 2)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("烘焙攻击盒显示寿命: ", GUILayout.Width(120));
                    // BakerBoxLife = Mathf.Max(EditorGUILayout.IntField(BakerBoxLife), 0);
                    using (var _check = new EditorGUI.ChangeCheckScope())
                    {
                        BakerBoxLife = EditorGUILayout.IntSlider(BakerBoxLife, 0, 1000);
                        if (_check.changed)
                        {
                            TimeLineWindow.Instance.UpdateTimeToNow();
                            SceneView.RepaintAll();
                        }
                    }
                }
            }
            
            GUILayout.Space(10);
            using (new GUIColorScope(Color.cyan))
            {
                GUILayout.Label("基础参数配置");
            }
            DrawEditorAttribute.Draw(_eventAttackBox);
            
            DrawEditorAttribute.Draw(_eventAttackBox.AttackInfo);
        }
    }
}