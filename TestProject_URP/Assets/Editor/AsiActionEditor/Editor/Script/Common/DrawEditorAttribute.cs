using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AsiActionEngine.Editor;
using AsiActionEngine.RunTime;
using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public class DrawEditorAttribute
    {
        public static bool NeedInit = false;
        public static Dictionary<string, GameObject> LoadObjectDic = new Dictionary<string, GameObject>();
        public static void Draw(object obj, string[] filter = null, bool isContain = true)
        {
            PropertyInfo[] pis = obj.GetType().GetProperties().OrderBy(p => p.MetadataToken).ToArray();
            for (int i = 0; i < pis.Length; ++i)
            {
                bool isDraw = true;
                if (filter is not null)
                {
                    isDraw = filter.Contains(pis[i].Name) == isContain;
                }
                object[] attrs = pis[i].GetCustomAttributes(typeof(EditorPropertyAttribute), false);
                if (attrs.Length == 1 && isDraw)
                {
                    EditorPropertyAttribute epa = (EditorPropertyAttribute)attrs[0];
                    if (!string.IsNullOrEmpty(epa.Deprecated)) continue;

                    GUILayout.BeginHorizontal();
                    EditorGUI.BeginChangeCheck();
                    GUILayout.Label(epa.PropertyName, GUILayout.Width(epa.LabelWidth));

                    object val = GetProperty(obj, pis[i].Name);
                    // GUILayout.Label("pis[i].Name: " + pis[i].Name);

                    if (epa.Edit)
                    {
                        switch (epa.PropertyType)
                        {
                            case EditorPropertyType.EEPT_Bool:
                                SetProperty(obj, pis[i].Name, GUILayout.Toggle((bool)val, ""));
                                break;
                            case EditorPropertyType.EEPT_Int:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.IntField((int)val));
                                break;
                            case EditorPropertyType.EEPT_Float:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.FloatField((float)val));
                                break;
                            case EditorPropertyType.EEPT_String:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.TextField((string)val));
                                break;
                            case EditorPropertyType.EEPT_Vector2:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.Vector2Field("", (Vector2)val));
                                break;
                            case EditorPropertyType.EEPT_Vector3:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.Vector3Field("", (Vector3)val));
                                break;
                            case EditorPropertyType.EEPT_Vector4:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.Vector4Field("", (Vector4)val));
                                break;
                            case EditorPropertyType.EEPT_Color:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.ColorField((Color)val));
                                break;
                            case EditorPropertyType.EEPT_Quaternion:
                                {
                                    Quaternion q = (Quaternion)val;
                                    q.eulerAngles = EditorGUILayout.Vector3Field("", q.eulerAngles);
                                    SetProperty(obj, pis[i].Name, q);
                                }
                                break;
                            case EditorPropertyType.EEPT_Enum:
                                SetProperty(obj, pis[i].Name, EditorGUILayout.EnumPopup((Enum)val));
                                break;
                            case EditorPropertyType.EEPT_GameObject:
                                    GameObjectField(obj, pis[i].Name, val);
                                break;

                            //单位状态标签
                            case EditorPropertyType.EEPT_ActionLable:
                                ActionLableField(obj, pis[i].Name, val);
                                break;
                            case EditorPropertyType.EEPT_CharacteLimbType:
                                LimbPointType(obj, pis[i].Name, val);
                                break;
                            case EditorPropertyType.EEPT_Camera:
                                CameraList(obj, pis[i].Name, val);
                                break;
                        }
                    }
                    else
                    {
                        string sz = (val == null ? string.Empty : val.ToString());
                        GUILayout.Label(sz, GUILayout.Width(epa.LabelWidth));
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        TimeLineWindow.Instance.Repaint();
                    }
                    GUILayout.EndHorizontal();
                }

            }

            NeedInit = false;
        }
        
        private static object GetProperty(object obj, string propertyName)
        {
            return obj.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, obj, null);
        }

        private static void SetProperty(object obj, string propertyName, object newValue)
        {
            obj.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, obj, new object[] { newValue });
        }
        
        private static void GameObjectField(object obj, string propertyName, object val)
        {
            GameObject go = null;
            var szPath = (string)val;

            if (!string.IsNullOrEmpty(szPath))
            {
                if (LoadObjectDic.ContainsKey(szPath))
                {
                    go = LoadObjectDic[szPath];
                }
                else
                {
                    // EngineDebug.LogWarning("加载数据");
                    // go = AssetDatabase.LoadAssetAtPath<GameObject>(szPath);
                    go = Resources.Load<GameObject>(szPath);
                    LoadObjectDic.Add(szPath, go);
                }
            }
            else
            {

            }

            using (var _check = new EditorGUI.ChangeCheckScope())
            {
                go = EditorGUILayout.ObjectField(go, typeof(GameObject), false) as GameObject;
                if (_check.changed)
                {
                    string name = AssetDatabase.GetAssetPath(go);
                    // EngineDebug.Log($"预制体路径: {name}");
                    string[] _nameChills = name.Split('/');
                    if (_nameChills.Length < 2 || _nameChills[1] != "Resources")
                    {
                        EditorUtility.DisplayDialog("对象错误", "该资源必须位于 Resources 路径下", "我知道了");
                        return;
                    }

                    name = string.Empty;
                    for (int i = 2; i < _nameChills.Length - 1; i++)
                    {
                        name += _nameChills[i] + "/";
                    }
                    name += _nameChills[^1].Split('.')[0];
                    SetProperty(obj, propertyName, (go is null ? string.Empty : name));
                }
            }
        }
        private static void ActionLableField(object obj, string propertyName, object val)
        {
            int _selectId = (int)val;
            using (var _check = new EditorGUI.ChangeCheckScope())
            {
                int _popup = EditorGUILayout.Popup(_selectId, ResourcesWindow.Instance.ActionLable.ToArray());
                if (_check.changed)
                {
                    SetProperty(obj, propertyName, _popup);
                }
            }
        }

        private static void CameraList(object obj, string propertyName, object val)
        {
            int _id = (int)val;

            GameObject _preCam = ResourcesWindow.Instance.PreCamera;
            if (_preCam)
            {
                Behaviour[] _behaviours = ResourcesWindow.Instance.PreCamControl.allCinemachine;
                string[] _cameraList = new string[_behaviours.Length];
                for (int i = 0; i < _behaviours.Length; i++)
                {
                    Transform _transform = _behaviours[i].transform;
                    _cameraList[i] = $"{_transform.parent.name}/{_transform.name}";
                }

                _id = EditorGUILayout.Popup(_id, _cameraList);
                
                SetProperty(obj, propertyName, _id);
            }
            else
            {
                using (new GUIColorScope(Color.red))
                {
                    GUILayout.Label("未创建相机实例，无法预览");
                }
            }
        }

        private static void LimbPointType(object obj, string propertyName, object val)
        {
            int _selectId = (int)val;

            //已配置的挂点
            List<string> _pointTypesStr = new List<string>();
            List<ECharacteLimbType> _poinTypes = new List<ECharacteLimbType>();
            int _selectID = 0;
            if (ResourcesWindow.Instance.GetRole().TryGetComponent(out CharacterConfig _config))
            {
                ECharacteLimbType[] _ECLT = _config.HelpPointDic.Keys.ToArray();
                for (int i = 0; i < _ECLT.Length; i++)
                {
                    if ((int)_ECLT[i] == _selectId)
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
            _selectID = EditorGUILayout.Popup(_selectID, _pointTypesStr.ToArray());
            SetProperty(obj, propertyName, (int)_poinTypes[_selectID]);
        }
    }
}