using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// execute blit and write params to material

public class CustomPostProcessPass : ScriptableRenderPass
{
    private Material material;
    // Params
    private float param0;
    private float param1;

    // Camera
    private RTHandle source;
    private RTHandle tempTarget;

    private const string ProfilerTag = "CustomPostProcessPass";
    public void SetSource(RTHandle cameraColorTarget)
    {
        source = cameraColorTarget;
    }

    public CustomPostProcessPass(Material material)
    {
        this.material = material;
        ConfigureInput(ScriptableRenderPassInput.Color);
    }

    // impart params per frame
    public void Setup(float param0, float param1)
    {
        this.param0 = param0;
        this.param1 = param1;
    }

    // fetch camera color target and allocate temp RT
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        var descriptor = renderingData.cameraData.cameraTargetDescriptor;
        
        descriptor.depthBufferBits = 0;
        descriptor.msaaSamples = 1;
        descriptor.bindMS = false;

        RenderingUtils.ReAllocateIfNeeded(ref tempTarget, descriptor, name: "_TempCustomPostProcessTarget");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if(material == null || source == null || tempTarget == null)
            return;

        CommandBuffer cmd = CommandBufferPool.Get(ProfilerTag);

        try
        {
            // params injection
            material.SetFloat("_Param0", param0);
            material.SetFloat("_Param1", param1);

            // Blit: source -> temp -> source
            Blitter.BlitCameraTexture(cmd, source, tempTarget, material, 0);
            Blitter.BlitCameraTexture(cmd, tempTarget, source);

            context.ExecuteCommandBuffer(cmd);
            
        }
        finally
        {
            CommandBufferPool.Release(cmd);            
        }
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        source = null;
    }

    // release RT
    public void Dispose()
    {
        tempTarget?.Release();
        tempTarget = null;

        source = null;
    }
}