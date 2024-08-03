using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace AsiActionEngine.RunTime
{
    [System.Serializable]
    public class UnitWarp : IProperty
    {
        [SerializeField] protected int mID;
        [SerializeField] protected string mCameID;
        [SerializeField] protected string mName;
        [SerializeField] protected string mModelPath; //模型路径
        [SerializeField] protected string mAction; //动编数据


        #region Property

        public int ID
        {
            get { return mID; }
            set { mID = value; }
        }
        public string CameID
        {
            get { return mCameID; }
            set { mCameID = value; }
        }
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        [EditorProperty("角色预制体", EditorPropertyType.EEPT_GameObject)]
        public string ModelPath
        {
            get { return mModelPath; }
            set { mModelPath = value; }
        }
        public string Action
        {
            get { return mAction; }
            set { mAction = value; }
        }
        

        #endregion
        public UnitWarp(int _id, string _name)
        {
            ID = _id;
            Name = _name;
        }
    }
}