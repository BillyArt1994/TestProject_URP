using UnityEditor;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public partial class ScenceDraw
    {
        private static ScenceDraw mInstance = null;

        public static ScenceDraw Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ScenceDraw();
                    SceneView.duringSceneGui += mInstance.OnSceneGUI;
                }

                return mInstance;
            }
        }
    }
}