using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

public class CustomShadowCasterPass : ScriptableRenderPass
{
    RTHandle m_customshadowmap;
    int m_ShadowmapWidth = 1024;
    int m_ShadowmapHeight = 1024;
    
    private static class CustomShadowConstantBuffer
    {
        public static int _CustomShadowMatrix;

        public static int _CustomShadowFilterWidth;
        public static int _CustomShadowTexture;
    }

    const string CUSTOM_SHADOW_KW = "_CUSTOM_SHADOW";
    Matrix4x4 m_ProjectionMatrix;
    Matrix4x4 m_WorldToCameraMatrix;

    FilteringSettings m_FilteringSettings = new FilteringSettings(RenderQueueRange.all);
    ShaderTagId m_ShaderTagId = new ShaderTagId("ShadowCaster");
    private bool m_On = false;


    public CustomShadowCasterPass(RenderPassEvent Event)
    {
        base.profilingSampler = new ProfilingSampler("CustomShadowCaster");
        CustomShadowConstantBuffer._CustomShadowMatrix = Shader.PropertyToID("_CustomShadowMatrix");
        CustomShadowConstantBuffer._CustomShadowFilterWidth = Shader.PropertyToID("_CustomShadowFilterWidth");
        CustomShadowConstantBuffer._CustomShadowTexture = Shader.PropertyToID("_CustomShadowTexture");
        this.renderPassEvent = Event;
        //m_FilteringSettings = new FilteringSettings();
    }

    public void Init(int w, int h)
    {
        m_ShadowmapWidth = w;
        m_ShadowmapHeight = h;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {

        // Normals
        RenderTextureDescriptor textureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        textureDescriptor.height = m_ShadowmapHeight;
        textureDescriptor.width = m_ShadowmapWidth;
        textureDescriptor.colorFormat = RenderTextureFormat.Shadowmap;
        textureDescriptor.depthBufferBits = 32 ;
        textureDescriptor.msaaSamples = 1 ;
        ShadowUtils.ShadowRTReAllocateIfNeeded(ref m_customshadowmap, m_ShadowmapWidth, m_ShadowmapHeight, 32, 1, 0, "CustomShadowMap");
        
        ConfigureTarget(m_customshadowmap);
        ConfigureClear(ClearFlag.Depth, Color.black);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, base.profilingSampler))
        {
            
            ref CameraData cameraData = ref renderingData.cameraData;
            Matrix4x4 projectionMatrix = GL.GetGPUProjectionMatrix(m_ProjectionMatrix, SystemInfo.graphicsUVStartsAtTop);
            Matrix4x4 viewMatrix = m_WorldToCameraMatrix;
            RenderingUtils.SetViewAndProjectionMatrices(cmd, viewMatrix, projectionMatrix, false);
            // update camera cull results 
            renderingData.cameraData.camera.TryGetCullingParameters(out var cullingParameters);
            cullingParameters.cullingMatrix = projectionMatrix * viewMatrix;
            var planes = GeometryUtility.CalculateFrustumPlanes(cullingParameters.cullingMatrix);
            for (int i = 0; i < 6; i++)
            {
                cullingParameters.SetCullingPlane(i, planes[i]);
            }
            var cullResults = context.Cull(ref cullingParameters);

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
            var drawSettings = CreateDrawingSettings(m_ShaderTagId, ref renderingData, sortFlags);
            drawSettings.perObjectData = PerObjectData.None;

            context.DrawRenderers(cullResults, ref drawSettings, ref m_FilteringSettings);
            RenderingUtils.SetViewAndProjectionMatrices(cmd, cameraData.GetViewMatrix(), cameraData.GetGPUProjectionMatrix(), false);
        }

        // Set shadowmap`
       // CoreUtils.SetKeyword(cmd, CUSTOM_SHADOW_KW, true);
        cmd.SetGlobalTexture(CustomShadowConstantBuffer._CustomShadowTexture, m_customshadowmap.rt);
        cmd.SetGlobalMatrix(CustomShadowConstantBuffer._CustomShadowMatrix, m_ProjectionMatrix*m_WorldToCameraMatrix);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        CoreUtils.SetKeyword(cmd, CUSTOM_SHADOW_KW, false);
    }

    public void Release()
    {
        //m_customshadowmap?.Release();
    }

    public void Projection(float radius, float sceneCaptureDistance)
    {
        float nearClipPlane = 0f;
        float farClipPlane = radius * 2f + sceneCaptureDistance;
        m_ProjectionMatrix = Matrix4x4.Ortho(-radius, radius, -radius, radius, nearClipPlane, farClipPlane);
    }

    public void Transform(Vector3 pos, Quaternion rot)
    {
        m_WorldToCameraMatrix = (Matrix4x4.TRS(pos, rot, Vector3.one)*Matrix4x4.Scale(new Vector3(1.0f, 1.0f, -1.0f))).inverse;
        if (SystemInfo.usesReversedZBuffer)
        {
            m_WorldToCameraMatrix[2, 0] = -m_WorldToCameraMatrix[2, 0];
            m_WorldToCameraMatrix[2, 1] = -m_WorldToCameraMatrix[2, 1];
            m_WorldToCameraMatrix[2, 2] = -m_WorldToCameraMatrix[2, 2];
            m_WorldToCameraMatrix[2, 3] = -m_WorldToCameraMatrix[2, 3];
        }



    }


}
