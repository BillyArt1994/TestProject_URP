using System;
using System.Diagnostics;

namespace AsiActionEngine.RunTime
{
    public enum EditorPropertyType
    {
        EEPT_Bool = 1,
        EEPT_Int,
        EEPT_Float,
        EEPT_String,
        EEPT_Vector2,
        EEPT_Vector3,
        EEPT_Vector4,
        EEPT_Color,
        EEPT_Quaternion,
        EEPT_Enum,
        EEPT_Object,

        EEPT_GameObject,

        //下拉列表
        EEPT_AnimatorState,
        EEPT_AnimatorParam,
        EEPT_CustomProperty,
        EEPT_Action,
        EEPT_ActionLable,
        EEPT_CharacteLimbType,
        EEPT_Camera,

        //列表
        EEPT_List,
        EEPT_GameObjectList,
    }
    
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    [Conditional("UNITY_EDITOR")]
    public class EditorPropertyAttribute : Attribute
    {
        private string mPropertyName;
        private EditorPropertyType mPropertyType;
        private bool mEdit;
        private string mDescription;
        private string mDeprecated;
        private float mLabelWidth;

        public EditorPropertyAttribute(string name, EditorPropertyType type)
        {
            mPropertyName = name;
            mPropertyType = type;
            mEdit = true;
            mDeprecated = null;
            mLabelWidth = 100;
        }
        
        #region Property
        public string PropertyName
        {
            get { return mPropertyName; }
            set { mPropertyName = value; }
        }
        public EditorPropertyType PropertyType
        {
            get { return mPropertyType; }
            set { mPropertyType = value; }
        }
        public bool Edit
        {
            get { return mEdit; }
            set { mEdit = value; }
        }
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }
        public string Deprecated
        {
            get { return mDeprecated; }
            set { mDeprecated = value; }
        }
        public float LabelWidth
        {
            get { return mLabelWidth; }
            set { mLabelWidth = value; }
        }
        #endregion
    }
}