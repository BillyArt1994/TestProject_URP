using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

class SelectionPass : ScriptableRenderPass
{
    public static readonly int SampleDistanceShaderId = Shader.PropertyToID("_SampleDistance");
    public static readonly int BlitTextureSizeShaderId = Shader.PropertyToID("_BlitTextureSize");
    public static readonly int SelectionColorShaderId = Shader.PropertyToID("_SelectionColor");
    public static readonly int OutlineShaderId = Shader.PropertyToID("_OutlineColor");
    public SelectionRendering feature;
    private RTHandle selection;
    private RTHandle selectionDepth;
    private RTHandle cameraColor;
    private List<ShaderTagId> shaderTagIdList = new List<ShaderTagId> {
        new ShaderTagId("UniversalForward"),
        new ShaderTagId("UniversalForwardOnly"),
        new ShaderTagId("LightweightForward"),
        new ShaderTagId("SRPDefaultUnlit")
    };

    public Material selectionBlitMaterial;

    private static readonly ShaderTagId SelectionShaderTagId = new ShaderTagId("Selection");

    public SelectionPass(SelectionRendering feature)
    {
        this.feature = feature;
        // 在构造函数中创建描边时使用的材质
        this.selectionBlitMaterial = CoreUtils.CreateEngineMaterial(feature.SelectionBlitShader);
        profilingSampler = new ProfilingSampler("SelectionPass");
    }

    public void SetTarget(RTHandle cameraColor)
    {
        this.cameraColor = cameraColor;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.colorFormat = RenderTextureFormat.ARGB32;
        // 分配深度 RenderTexture
        RenderingUtils.ReAllocateIfNeeded(ref selectionDepth, descriptor, wrapMode: TextureWrapMode.Clamp, name: "_selection");
        // 分配颜色 RenderTexture
        descriptor.depthBufferBits = 0;
        RenderingUtils.ReAllocateIfNeeded(ref selection, descriptor, wrapMode: TextureWrapMode.Clamp, name: "_selection");

        // 设置描边材质的信息，这里我希望描边宽度与分辨率成正比所以做了一些计算
        selectionBlitMaterial.SetFloat(SampleDistanceShaderId, feature.SampleDistance * ((float)descriptor.width) / feature.SampleDistanceReferenceWidth);
        selectionBlitMaterial.SetVector(BlitTextureSizeShaderId, new Vector4(descriptor.width, descriptor.height));
        selectionBlitMaterial.SetColor(SelectionColorShaderId, feature.SelectionColor);
        selectionBlitMaterial.SetColor(OutlineShaderId, feature.OutlineColor);

        // 指定 Render Target 和渲染前 RenderTexture 的清空方式
        // 这里需要注意的是，不能只使用颜色 RenderTexture 而不使用深度 RenderTexture，否则会报错
        ConfigureTarget(selection, selectionDepth);
        ConfigureClear(ClearFlag.All, new Color(0, 0, 0, 0));
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // 在编辑器中不渲染描边
        if (renderingData.cameraData.cameraType != CameraType.Game)
        {
            return;
        }
        // 分配渲染用的 CommandBuffer
        CommandBuffer cmd = CommandBufferPool.Get(name: "Selection");
        // ProfilingScope 仅仅是为了方便在 Profiler 中显示
        using (new ProfilingScope(cmd,profilingSampler))
        {
            // 指定 DrawingSettings，这里使用了 URP 默认的 Shader Pass
            DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            RendererListParams rendererListParams = new RendererListParams(
                renderingData.cullResults,
                drawingSettings,
                // 指定 FilteringSettings，这里我们只关心渲染层
                new FilteringSettings(RenderQueueRange.all, -1, feature.RenderingLayerMask)
            );

            // 构建 RendererList
            RendererList rendererList = context.CreateRendererList(ref rendererListParams);

            // 渲染 RendererList
            cmd.DrawRendererList(rendererList);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            // 准备绘制描边
            Blitter.BlitCameraTexture(cmd, selection, cameraColor, selectionBlitMaterial, 0);

            // 执行
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
        // 回收 CommandBuffer
        CommandBufferPool.Release(cmd);
    }
}