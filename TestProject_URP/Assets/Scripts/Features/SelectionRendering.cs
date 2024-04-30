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
        // ������Ӧ�� ScriptableRenderPass
        selectionPass = new SelectionPass(this);
        // �� ScriptableRenderPass ����Ⱦʱ��ָ��Ϊ����������Ⱦ�������֮��
        selectionPass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // �ڱ༭���в���Ⱦ���
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            // �Ѵ����� ScriptableRenderPass ������Ⱦ����
            renderer.EnqueuePass(selectionPass);
        }
    }
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            // �ѵ�ǰ����� RenderTarget ���������� ScriptableRenderPass
            selectionPass.SetTarget(renderer.cameraColorTargetHandle);
        }
    }
}