using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DensityVisualizationUtil : MonoBehaviour
{
    public Shader visualizationShader;
    public Texture2D checkerTex;
    public Texture2D mipmapDebugTex;
    public bool mipmapVisualization = false;
    public bool overlayMode = true;

   

    [ContextMenu("Enable Visualization")]
    void EnableVisualization()
    {

        Shader.SetGlobalTexture("_CheckTex", mipmapVisualization? mipmapDebugTex : checkerTex);
        SetKeywordEnable("MIPMAP_MODE", mipmapVisualization);
        SetKeywordEnable("OVERLAY_MODE", overlayMode);        
       

        foreach (UnityEditor.SceneView view in UnityEditor.SceneView.sceneViews)
        {
            view.SetSceneViewShaderReplace(visualizationShader, "RenderType");
        }
    }

    [ContextMenu("Disable Visualization")]
    void DisableVisualization()
    {
        Shader.SetGlobalTexture("_CheckTex", null);
        foreach (UnityEditor.SceneView view in UnityEditor.SceneView.sceneViews)
        {
            view.SetSceneViewShaderReplace(null, "RenderType");
        }
    }

    [ContextMenu("Generate Mipmap Check Texture")]
    void GenerateMipmapCheckTexture()
    {
        int size = 64;
        Color[] mipCols = new Color[] {Color.red, Color.yellow, Color.green, Color.cyan, Color.blue};
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32,true);
        int mipmapCount = (int)Mathf.Log(size, 2);
        int mipmapSize = size;
        for (int i = 0; i <= mipmapCount; i++)
        {    
            Color mipCol = mipCols[Mathf.Min(i, mipCols.Length-1)];
            Color[] cols = tex.GetPixels(i);
            for (int j = 0; j < cols.Length; j++)
            {
                cols[j] = mipCol;
            }
            tex.SetPixels(cols, i);
            /*
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    tex.SetPixel(j,k,mipCols[i],i);
                }
            }*/
            //mipmapSize /= 2;
        }
        tex.Apply(false);
        //byte[] bytes = tex.EncodeToPNG();
        //System.IO.File.WriteAllBytes(Application.dataPath + "/Test/DensityVisualization/mipmapVis.png", bytes);
        UnityEditor.AssetDatabase.CreateAsset(tex, "Assets/DensityVisualization/mipmap_debug.asset");
        UnityEditor.AssetDatabase.Refresh();


    }

    [ContextMenu("Show SceneView Size")]
    void ShowSceneViewSize()
    {
        foreach (UnityEditor.SceneView view in UnityEditor.SceneView.sceneViews)
        {
            Debug.Log("Size:" + view.camera.pixelWidth + ":" + view.camera.pixelHeight);
        }
    }

    private void SetKeywordEnable(string keyword, bool enabled)
    {
        if (enabled)
        {
            Shader.EnableKeyword(keyword);
        }
        else
        {
            Shader.DisableKeyword(keyword);
        }
    }
}
