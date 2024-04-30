using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CustomShadowCasterFeature : ScriptableRendererFeature
{
    CustomShadowCasterPass m_customShadowPass;
    [SerializeField] private RenderPassEvent m_event = RenderPassEvent.AfterRenderingOpaques;
    [SerializeField] private LayerMask m_layerMask;
    public static CustomShadowCasterFeature m_Instance;
    

    public CustomShadowCasterFeature()
    {
        m_Instance = this;
    }

    public void Init(int w, int h)
    {
        if (m_customShadowPass != null)
            m_customShadowPass.Init(w, h);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_customShadowPass);
    }

    public override void Create()
    {
        if (m_customShadowPass == null)
        {
            m_customShadowPass = new CustomShadowCasterPass(m_event);
        }
    }

    public void Projection(float radius, float sceneCaptureDistance, float depthBias) {
        m_customShadowPass.Projection(radius, sceneCaptureDistance, depthBias);
    }

    public void Transform(Vector3 pos ,Quaternion rot)
    {
        m_customShadowPass.Transform(pos, rot);
    }

}