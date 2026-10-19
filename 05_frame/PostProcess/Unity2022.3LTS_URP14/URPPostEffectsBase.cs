using UnityEngine;
using UnityEngine.Rendering;

// Tool: material create and release

public static class URPPostEffectsBase
{
    public static Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if(shader == null)
        {
            Debug.LogWarning("[URPPostEffectsBase] Shader is null.");
            ReleaseMaterial(material);
            return null;
        }
        if (!shader.isSupported)
        {
            Debug.LogWarning($"[URPPostEffectsBase] Shader '{shader.name}' is not supported on current platform.");
            ReleaseMaterial(material);
            return null;
        }
        if(material != null && material.shader == shader)
            return material;

        ReleaseMaterial(material);

        var newMaterial = new Material(shader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };

        return newMaterial;
    }
    public static void ReleaseMaterial(Material material)
    {
        if(material != null)
            CoreUtils.Destroy(material);
    }
}