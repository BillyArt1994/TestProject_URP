using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEditor.ShaderData;

public class ColorChannelDisplayFeature : ScriptableRendererFeature
{
    public enum DisplayChannel
    {
        RGBA,
        R,
        G,
        B,
        A,
        Gray
    }

    public RenderPassEvent passEvent = RenderPassEvent.AfterRendering;
    static public ColorChannelDisplayFeature Instance;
    public DisplayChannel displayChannel;
    public Material mat;

    class CustomRenderPass : ScriptableRenderPass
    {
        public Material passMat = null;
        public DisplayChannel displayChannel;
        private RenderTargetIdentifier passSource { get; set; }//Դͼ��Ŀ��ͼ��
        private RenderTargetHandle passTempleColorTex;
        private string passTag = "Color Channel Display";

        public CustomRenderPass(RenderPassEvent passEvent,Material mat ,string tag,DisplayChannel displayChannel)
        {
            this.renderPassEvent = passEvent;
            this.displayChannel = displayChannel;
            this.passMat = mat;
            this.passMat.shaderKeywords = null;
            switch ((int)displayChannel)
            {
                case 0:
                    this.passMat.EnableKeyword("_CHANNELDISPLAY_RGBA");
                    break;
                case 1:
                    this.passMat.EnableKeyword("_CHANNELDISPLAY_R");
                    break;
                case 2:
                    this.passMat.EnableKeyword("_CHANNELDISPLAY_G");
                    break;
                case 3:
                    this.passMat.EnableKeyword("_CHANNELDISPLAY_B");
                    break;
                case 4:
                    this.passMat.EnableKeyword("_CHANNELDISPLAY_A");
                    break;
                case 5:
                    this.passMat.EnableKeyword("_CHANNELDISPLAY_GRAY");
                    break;
            }
            passTag = tag;
        }

        public void SetUp(RenderTargetIdentifier source)
        {
            this.passSource = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //����������ػ�ȡһ������ǩ������������ñ�ǩ����֡�������п��Լ���
            CommandBuffer cmd = CommandBufferPool.Get(passTag);
            //��ȡĿ�������������Ϣ������һ���ṹ�壬������render texture������Ϣ������ߴ磬���ͼ���ȵȵ�
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            //������Ȼ�������0��ʾ����Ҫ��Ȼ�����
            opaqueDesc.depthBufferBits = 0;
            //������ʱͼ��
            cmd.GetTemporaryRT(passTempleColorTex.id, opaqueDesc);
            //��Դͼ���������м��㣬Ȼ��洢����ʱ��������
            cmd.Blit(passSource, passTempleColorTex.Identifier(), passMat);
            //����ʱ�������Ľ�����Դͼ����
            cmd.Blit(passTempleColorTex.Identifier(), passSource);
            //ִ���������
            context.ExecuteCommandBuffer(cmd);
            //�ͷ������
            CommandBufferPool.Release(cmd);
            //�ͷ���ʱrender texture
            cmd.ReleaseTemporaryRT(passTempleColorTex.id);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        m_ScriptablePass = new CustomRenderPass(passEvent, mat, name, displayChannel);
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //��ȡ��ǰ����е�ͼ��
        var src = renderer.cameraColorTarget;
        m_ScriptablePass.SetUp(src);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


