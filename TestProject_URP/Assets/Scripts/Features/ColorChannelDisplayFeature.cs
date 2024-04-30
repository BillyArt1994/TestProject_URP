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
        private RenderTargetIdentifier passSource { get; set; }//源图像，目标图像
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
            //从命令缓冲区池获取一个带标签的命令缓冲区，该标签名在帧调试器中可以见到
            CommandBuffer cmd = CommandBufferPool.Get(passTag);
            //获取目标相机的描述信息，创建一个结构体，里面有render texture各中信息，比如尺寸，深度图精度等等
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            //设置深度缓冲区，0表示不需要深度缓冲区
            opaqueDesc.depthBufferBits = 0;
            //申请临时图像
            cmd.GetTemporaryRT(passTempleColorTex.id, opaqueDesc);
            //将源图像放入材质中计算，然后存储到临时缓冲区中
            cmd.Blit(passSource, passTempleColorTex.Identifier(), passMat);
            //将临时缓冲区的结果存回源图像中
            cmd.Blit(passTempleColorTex.Identifier(), passSource);
            //执行命令缓冲区
            context.ExecuteCommandBuffer(cmd);
            //释放命令缓存
            CommandBufferPool.Release(cmd);
            //释放临时render texture
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
        //获取当前相机中的图像
        var src = renderer.cameraColorTarget;
        m_ScriptablePass.SetUp(src);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


