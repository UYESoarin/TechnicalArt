Shader "Hidden/PostProcessing/CustomPostProcess"{
    Properties{
        _Param0("Param0", Float) = 1
        _Param1("Param1", Range(0, 1)) = 0.5
    }
    SubShader{
        Tags{
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off
        ZWrite Off
        ZTest Always
        
        Pass{
            Name "CustomPostProcessPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _Param0;
            float _Param1;
            CBUFFER_END

            half4 Frag(Varyings input):SV_Target{
                float2 uv = input.texcoord;
                // half3 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).rgb;
                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                half3 inverted = 1.0 - col.rgb;
                half3 result = lerp(col.rgb, inverted, saturate(_Param1));

                return half4(result, col.a);
            }
            ENDHLSL
        }
    }
    Fallback Off
}