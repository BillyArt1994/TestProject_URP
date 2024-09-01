using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections;

namespace BillyToolKit
{
    public class ScreenCaptureHelper : MonoBehaviour
    {
        public void ScreenCaptureFunction()
        {
            StartCoroutine(TakeScreenshot());
        }

        public IEnumerator TakeScreenshot()
        {
            var defaultName = "ScreenCapture" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var savePath = EditorUtility.SaveFilePanel("Save ScreenShot", "Assets", defaultName, "png");
            ScreenCapture.CaptureScreenshot(savePath, 1);
            while (!File.Exists(savePath))
            {
                yield return null; 
            }
            AssetDatabase.Refresh();
        }
    }
}