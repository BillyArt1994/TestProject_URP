using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


namespace UnityEngine.Experimental.Rendering.Universal
{
    /// <summary>
    /// Renders a UniqueShadow for the main Light.
    /// </summary>
    public class UniqueShadowCasterPass : ScriptableRenderPass
    {
        private static class UniqueShadowConstantBuffer
        {
            public static int _UniqueShadowMatrix;

            public static int _UniqueShadowFilterWidth;
            public static int _UniqueShadowTexture;
        }

        const string UNIQUE_SHADOW = "_UNIQUE_SHADOW";

        RenderTargetHandle m_UniqueShadowmap;
        RenderTexture m_UniqueShadowTexture;

        Matrix4x4 m_UniqueShadowMatrix;

        Matrix4x4 m_ProjectionMatrix;

        
        public Camera camera { get; set; }

        //Matrix4x4 m_shadowMatrix;
        Matrix4x4 m_shadowSpaceMatrix;

        int m_ShadowmapWidth=1024;
        int m_ShadowmapHeight=1024;
        const int k_ShadowmapBufferBits = 16;


        public int m_CullingMask;

        private bool m_On = false;

        ProfilingSampler m_ProfilingSetupSampler = new ProfilingSampler("Setup Unique Shadowmap");

        ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Unique Shadowmap");

        FilteringSettings m_FilteringSettings;
        ShaderTagId m_ShaderTagId = new ShaderTagId("DepthOnly");

        public UniqueShadowCasterPass(RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask)
        {
            UniqueShadowConstantBuffer._UniqueShadowMatrix = Shader.PropertyToID("_UniqueShadowMatrix");
            UniqueShadowConstantBuffer._UniqueShadowTexture = Shader.PropertyToID("_UniqueShadowTexture");
            UniqueShadowConstantBuffer._UniqueShadowFilterWidth = Shader.PropertyToID("_UniqueShadowFilterWidth");

            base.profilingSampler = new ProfilingSampler(nameof(UniqueShadowCasterPass));
            m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
            renderPassEvent = evt;

            m_UniqueShadowMatrix = new Matrix4x4();

            m_UniqueShadowmap.Init("_UniqueShadowTexture");
        }

        public void SetOn(bool on)
        {
            m_On = on;
        }

        public bool Setup(ref RenderingData renderingData)
        {
            using var profScope = new ProfilingScope(null, m_ProfilingSampler);

            if (renderingData.cameraData.camera.depth != 0)
                return false;

            if (!m_On)
                return false;
            //if (!renderingData.shadowData.supportsMainLightShadows)
            //    return false;

            //Clear();
            int shadowLightIndex = renderingData.lightData.mainLightIndex;
            if (shadowLightIndex == -1)
                return false;

            VisibleLight shadowLight = renderingData.lightData.visibleLights[shadowLightIndex];
            Light light = shadowLight.light;
            //if (light.shadows == LightShadows.None)
            //    return false;

            if (shadowLight.lightType != LightType.Directional)
            {
                Debug.LogWarning("Only directional lights are supported as main light.");
            }

            //Bounds bounds;
            //if (!renderingData.cullResults.GetShadowCasterBounds(shadowLightIndex, out bounds))
            //    return false;


            return true;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            m_UniqueShadowTexture = ShadowUtils.GetTemporaryShadowTexture(m_ShadowmapWidth,
                    m_ShadowmapHeight, k_ShadowmapBufferBits);
            ConfigureTarget(new RenderTargetIdentifier(m_UniqueShadowTexture));
            ConfigureClear(ClearFlag.All, Color.black);
        }

        public void Init(int w, int h, int cullingMask)
        {
            m_ShadowmapWidth = w;
            m_ShadowmapHeight = h;
            m_FilteringSettings.layerMask = cullingMask;
        }
        /// <inheritdoc/>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            RenderUniqueShadowmap(ref context, ref renderingData);
        }

        /// <inheritdoc/>
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            if (m_UniqueShadowTexture)
            {
                RenderTexture.ReleaseTemporary(m_UniqueShadowTexture);
                m_UniqueShadowTexture = null;
            }
            CoreUtils.SetKeyword(cmd, UNIQUE_SHADOW, false);
        }

        public void Clear()
        {
            camera = null;
        }

        public void Projection(float radius, float sceneCaptureDistance, float depthBias)
        {
            float nearClipPlane = 0f;
            float farClipPlane = radius * 2f + sceneCaptureDistance;
            m_ProjectionMatrix = Matrix4x4.Ortho(-radius, radius, -radius, radius, nearClipPlane, farClipPlane);


            var db = SystemInfo.usesReversedZBuffer ? depthBias : -depthBias;
            m_shadowSpaceMatrix.SetRow(0, new Vector4(0.5f, 0.0f, 0.0f, 0.5f));
            m_shadowSpaceMatrix.SetRow(1, new Vector4(0.0f, 0.5f, 0.0f, 0.5f));
            m_shadowSpaceMatrix.SetRow(2, new Vector4(0.0f, 0.0f, 0.5f, 0.5f + db));
            m_shadowSpaceMatrix.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        }
        Matrix4x4 m_WorldToCameraMatrix;


        public void Transform(Vector3 pos, Quaternion rot)
        {
            //Matrix4x4 worldToCameraMatrix = Matrix4x4.Scale(new Vector3(1.0f, 1.0f, -1.0f));
            //worldToCameraMatrix = worldToCameraMatrix * Matrix4x4.TRS(pos, rot, Vector3.one).inverse;

            m_WorldToCameraMatrix = Matrix4x4.TRS(pos, rot, Vector3.one).inverse;
            {
                m_WorldToCameraMatrix[2, 0] = -m_WorldToCameraMatrix[2, 0];
                m_WorldToCameraMatrix[2, 1] = -m_WorldToCameraMatrix[2, 1];
                m_WorldToCameraMatrix[2, 2] = -m_WorldToCameraMatrix[2, 2];
                m_WorldToCameraMatrix[2, 3] = -m_WorldToCameraMatrix[2, 3];
            }
            //TODO: Texel snap? (probably doesn't matter too much since the targets are always animated)
            var shadowViewMat = m_WorldToCameraMatrix;
			var shadowProjection = m_ProjectionMatrix;
            if (SystemInfo.usesReversedZBuffer)
            {
                shadowProjection[2, 0] = -shadowProjection[2, 0];
                shadowProjection[2, 1] = -shadowProjection[2, 1];
                shadowProjection[2, 2] = -shadowProjection[2, 2];
                shadowProjection[2, 3] = -shadowProjection[2, 3];
            }
            m_UniqueShadowMatrix = m_shadowSpaceMatrix * shadowProjection * shadowViewMat;
        }


        void RenderUniqueShadowmap(ref ScriptableRenderContext context, ref RenderingData renderingData)
        {

            // NOTE: Do NOT mix ProfilingScope with named CommandBuffers i.e. CommandBufferPool.Get("name").
            // Currently there's an issue which results in mismatched markers.
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSetupSampler))
            {
                ref CameraData cameraData = ref renderingData.cameraData;

                Matrix4x4 projectionMatrix = GL.GetGPUProjectionMatrix(m_ProjectionMatrix, SystemInfo.graphicsUVStartsAtTop);
                Matrix4x4 viewMatrix = m_WorldToCameraMatrix;
                RenderingUtils.SetViewAndProjectionMatrices(cmd, viewMatrix, projectionMatrix, false);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
                var drawSettings = CreateDrawingSettings(m_ShaderTagId, ref renderingData, sortFlags);
                drawSettings.perObjectData = PerObjectData.None;

                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings);

                RenderingUtils.SetViewAndProjectionMatrices(cmd, cameraData.GetViewMatrix(), cameraData.GetGPUProjectionMatrix(), false);

            }
            SetupUniqueReceiverConstants(cmd);

            context.ExecuteCommandBuffer(cmd);
           // cmd.EndSample("Unique Shadow map");
            CommandBufferPool.Release(cmd);

        }

        void SetupUniqueReceiverConstants(CommandBuffer cmd)           
        {
            CoreUtils.SetKeyword(cmd, UNIQUE_SHADOW, true);
            cmd.SetGlobalTexture(m_UniqueShadowmap.id, m_UniqueShadowTexture);
            cmd.SetGlobalMatrix(UniqueShadowConstantBuffer._UniqueShadowMatrix, m_UniqueShadowMatrix);
            cmd.SetGlobalFloat(UniqueShadowConstantBuffer._UniqueShadowFilterWidth, 1.0f/m_ShadowmapWidth);
        }

    };


    //[DisallowMultipleRendererFeature]
    public class UniqueShadowCaster : ScriptableRendererFeature
    {

        private UniqueShadowCasterPass m_UniqueShadowPass = null;

        static public UniqueShadowCaster s_Instacne;

        UniqueShadowCaster()
        {
            s_Instacne = this;
        }
        /// <inheritdoc/>
        public override void Create()
        {
            // Create the pass...
            if (m_UniqueShadowPass == null)
            {
                m_UniqueShadowPass = new UniqueShadowCasterPass(RenderPassEvent.AfterRenderingShadows, RenderQueueRange.opaque, -1);
            }

            //m_UniqueShadowPass.profilerTag = name;
            //m_UniqueShadowPass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
        }

        /// <inheritdoc/>
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            bool shouldAdd = m_UniqueShadowPass.Setup(ref renderingData);
            if (shouldAdd)
            {
                renderer.EnqueuePass(m_UniqueShadowPass);
                //Debug.Log("ÒÑÖ´ÐÐ UniqueShadowPass");
            }
            else
            {
                
            }
        }

        public void Projection(float radius, float sceneCaptureDistance, float depthBias)
        {
            m_UniqueShadowPass.Projection(radius, sceneCaptureDistance, depthBias);
        }

        public void Transform(Vector3 pos, Quaternion rot)
        {
            m_UniqueShadowPass.Transform(pos, rot);
        }

        public void Init(int w, int h, int cullingMask)
        {
            if(m_UniqueShadowPass != null)
                m_UniqueShadowPass.Init(w, h, cullingMask);
        }
        void OnDestroy()
        {
            s_Instacne = null;
        }

        public void Clear()
        {
            m_UniqueShadowPass.Clear();
        }

        public void SetOn(bool on)
        {
            if (m_UniqueShadowPass != null)
                m_UniqueShadowPass.SetOn(on);
        }

    }

}
