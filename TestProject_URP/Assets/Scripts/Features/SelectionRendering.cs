using UnityEngine.Rendering.Universal;
using UnityEngine;

public class SelectionRendering : ScriptableRendererFeature
{
    SelectionPass selectionPass;
    public Shader SelectionBlitShader;
    public Color OutlineColor;
    public Color SelectionColor;
    public float SampleDistance;
    public float SampleDistanceReferenceWidth;

    public uint RenderingLayerMask = uint.MaxValue;

    public override void Create()
    {
        if (SelectionBlitShader == null)
        {
            SelectionBlitShader = Shader.Find("SelectionRendering/SelectionBlit");
        }
        // 建立对应的 ScriptableRenderPass
        selectionPass = new SelectionPass(this);
        // 将 ScriptableRenderPass 的渲染时机指定为所有其他渲染操作完成之后
        selectionPass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // 在编辑器中不渲染描边
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            // 把创建的 ScriptableRenderPass 放入渲染队列
            renderer.EnqueuePass(selectionPass);
        }
    }
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            // 把当前相机的 RenderTarget 传给创建的 ScriptableRenderPass
            selectionPass.SetTarget(renderer.cameraColorTargetHandle);
        }
    }
}