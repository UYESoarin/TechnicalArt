using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// create material and pass, impart params per frame to pass
public class CustomPostProcessFeature : ScriptableRendererFeature
{
    // Inspector Setting
    [System.Serializable]
    public class Settings
    {
        public Shader shader;
        public float param0 = 1f;
        [Range(0f, 1f)] public float param1 = 0.5f;
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public Settings settings = new Settings();

    private Material material;

    private CustomPostProcessPass pass;

    // once to create material and pass
    public override void Create()
    {
        // create material
        material = URPPostEffectsBase.CheckShaderAndCreateMaterial(settings.shader, material);
        
        if(material == null)
        {
            pass?.Dispose();
            pass = null;
            return;
        }

        // release old pass RTHandle id recreate feature
        pass?.Dispose();

        // create pass
        pass = new CustomPostProcessPass(material)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }

    // camera call per frame to impart params and register
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // create material failed
        if(material == null || pass == null)
            return;

        // skip preview camera
        if(renderingData.cameraData.cameraType == CameraType.Preview)
            return;

        // impart params and register pass
        pass.Setup(settings.param0, settings.param1);
        renderer.EnqueuePass(pass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if(material == null || pass == null)
            return;
        if(renderingData.cameraData.cameraType == CameraType.Preview)
            return;
        
        pass.SetSource(renderer.cameraColorTargetHandle);
    }

    // release material and RT
    protected override void Dispose(bool disposing)
    {
        pass?.Dispose();
        pass = null;

        URPPostEffectsBase.ReleaseMaterial(material);
        material = null;
    }
}
