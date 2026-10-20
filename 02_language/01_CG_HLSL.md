## CG (Build-in) & URP HLSL

---

### 1. API

#### 1.1 Spatial Transformation

|Func|CG|HLSL|Note|
|:-:|:-:|:-:|:-:|
|O2C|`UnityObjectClipPos(posOS)`|`TransformObjectHClip(posOS.xyz)`|MVP|
|O2W|`mul(unity_ObjectToWorld, posOS)`|`TransformObjectToWorld(posOS.xyz)`|float3 in World(Light)|
|W2V|`mul(UNITY_MATRIX__V, posWS)`|`TransformWorldToView(posWS)`|`TransformWorldToViewDir(nDirWS)` for matcap|
|W2C|`mul(UNITY_MATRIX_VP, posWS)`|`TransformWorldToHClip(posWS)`||
|nDirWS|`UnityObjectToworldNormal(nDirOS)`|`TransformObjectToWorldNormal(nDirOS)`|Non-uniform Scaling (auto Inverse Transpose Matrix)|
|vDirWS|`_WorldSpaceCameraPos - posWS`|`GetWorldSpaceViewDir(posWS)`|ToNormalized<br>`GetWorldSpaceNormalizedViewDir(posWS)`|
|cameraPosWS|`_WorldSpaceCameraPos`|`GetCameraPositionWS()`|`float3 CameraPosOS = mul(GetWorldToObjectMatrix(), float4(GetCameraPositionWS(), 1)).xyz`|
|C2S(NDC)|`posCS.xy/posCS.w * 0.5 + 0.5`|`GetNormalizedScreenSpace(posCS)`|platform YFlip|
|ScreenResolution|`_ScreenParams.xy`|`GetScaledScreenParams().xy`||

#### 1.2 Light

|Func|CG|HLSL|Note|
|:-:|:-:|:-:|:-:|
|**Data**|
|shadowCoord|`SHADOW_COORDS(idx)` (v2f i.texcoord[idx], i.pos)<br>`TRANSFER_SHADOW(v2f o)`(vert)|`TransfromWorldToShadowCoord(posWS)`|`float4` W2Shadow|
|lDir/Col/shadow|`_WoldSpaceLightPos0.xyz`/`_LightColor0.rgb`/`SHADOW_ATTENUATION(v2f i)`(frag)|`GetMainLight([shadowCoord])`|HLSL: `struct Light{half3 direction, color, distanceAttenuation, shadowAttenuation};`|
|extraLights|`#pragma multi_compile_fwdadd`|`#pragma multi_compile _ _ADDITIONAL_LIGHTS` <br> `#if defined(_ADDITIONAL_LIGHTS) #endif` <br> `GetAdditionalLight(uint idx, float3 posWS)`|HLSL: Return `Light`<br> Cycle with `GetAdditionalLightsCount()`|
|**Model**|
|Lambert|`max(0.0, dot(n, l)) * lCol`|`LightingLambert(lCol, lDir, nDir)`||
|Blinn-Phong|`pos(max(0.0, dot(n, h)), gloss)`|`LightingSpecular(lCol, lDir, nDir, vDir, specular, smoothness)`||
|Ambient|`UNITY_LIGHTMODEL_AMBIENT`|`SampleSH(nDir)`|float3|

#### 1.3 AO

|Func|CG|HLSL|Note|
|:-:|:-:|:-:|:-:|
|SSAO||`SampleAmbientOcclusion(screenUV)`|(half)0~1|
|(in)directAO||`GetScreenSpaceAmbientOcclusion(screenUV)`|`struct AmbientOcclusionFactor{half indirect,direct};`|

#### 1.4 DepthTexture

|Func|CG|HLSL|Note|
|:-:|:-:|:-:|:-:|
|SampleDepth|`tex2D(_CameraDepthTexture, uv)`|`SampleSceneDepth(uv)`||
|DirectLoad|`tex2Dlod`|`LoadSceneDepth(pixelCoord)`||
|EyeDepth|`LinearEyeDepth(depth)`|`LinearEyeDepth(depth, _ZBufferParams)`||
|01Depth|`Linear01Depth(depth)`|`Linear01Depth(depth, _ZBufferParams)`||

#### 1.5 Sampler & Texture (Macros)

|Func|CG|HLSL|Note|
|:-:|:-:|:-:|:-:|
|**Declare**|
|2D|`sampler2D _MainTex;`|`TEXTURE2D(_MainTex);`<br>`SAMPLER(sampler_MainTex);`||
|CUBE|`samplerCUBE _MainTex`|`TEXTURECUBE(_Env);`<br>`SAMPLER(sampler_Env);`||
|**Sample**|
|2D|`tex2D(_MainTex, uv)`|`SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv)`||
|2DLOD|`tex2Dlod(_MainTex, float4(uv, 0, lod))`|`SAMPLE_TEXTURE2D_LOD(_Tex, sampler_Tex, uv, lod)`||
|CUBE|`texCUBE(_Env, dir)`|`SAMPLE_TEXTURECUBE(_Env, sampler_Env, dir)`||
|CUBELOD|`texCUBElod(_Cubemap, dir, lod)`|`SAMPLE_TEXTURECUBE_LOD(TEXTURECUBE(cubemapName), SAMPLER(samplerName), float3 direction, float lod)`|LOD = 0: smoothest; blur when higher|
|Tilling&offset|`TRANSFORM_TEX(uv, _MainTex)`|`TRANSFORM_TEX(uv, _MainTex)`|`float4 _MainTex_ST;`<br>`o.uv = i.uv * _MainTex_ST.xy + _MainTex_ST.zw`|

#### 1.6 Global Variables

|Variable|Value|
|:-:|:-:|
|`_Time`|`float4(t/20, t, 2t, 3t)`|
|`_SinTime`|`float4(sin(t/8), sin(t/4), sin(t/2), sin(t))`|
|`_CosTime`|`float4(cos(t/8), cos(t/4), cos(t/2), cos(t))`|
|`unity_DeltaTime`|`float4(dt, 1/dt, smootDt, 1/smoothDt)`|
|`_ProjectionParams`|`float4([FlipOrNot], [NearPlane], [FarPlane], 1/[FarPlane])`|
|`_ZBufferParams`|`float4`|
|`_ScreenParams`|`float4(width, height, 1/width, 1/height)`|
|`PI`|`3.1415926535897932384243...`|

#### 1.7 PostProcessing - URP

|Content|Code|Note|
|:-:|:-:|:-:|
|vertex shader|`#pragma vertex Vert`|auto|
|fragment shader|`#pragme fragment [FragName]` <br> `half4 [FragName](Varyings input)`|`float2 uv = input.texcoord`|
|ScreenTexture|`_BlitTexture`|`Blit.hlsl`|
|TexelSize|`_BlitTexture_TexelSize`|`float4(texelWidth, texelHeight, texWidth, texHeight)` <br> `texelSize = 1 / texSize` <br> global declaration (out of CBUFFER)|
|Sample|`SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv)`||

---

### 2. 数学函数

|类型|函数|提示|
|:-:|:-:|:-:|
|三角|`sin`, `cos`, `tan`, `atan2`, `atan`, `radians`, `degrees`, `sincos`, `sincos_half`|`atan` 单参返回 `[-PI/2, PI/2]` ， `atan2` 双惨返回 `[-PI, PI]` ， `sincos(float rad, inout sin, inout cos)` 性能较优|
|指数/幂|`pow`, `exp`, `sqrt`, `rsqrt`|非线性调制|
|取整|`floor`, `ceil`, `round`|`round(x)` 即 `step(0.5, x)`|
|取余|`frac`, `fmod` 用于浮点数|`%` 用于 `int/uint`；`frac(x)` 即 `fmod(x, 1)`；对 n 取模返回 `[0, n)`|
|边界限制|`clamp`, `saturate`, `max`, `min`, `abs`|风格化、离散选区|
|插值/阶跃|`lerp`, `step`, `smoothstep`|`step(A, B)`(`B>=A?1:0`)<br>`smoothstep(edge - threshold, edge + threshold, mask)` 二值间扩展边界插值 <br> `smoothstep(A, B, X)` X 在 (A, B) 之间则平滑过渡 <br>遮罩与条件二值，对目标区间变量插值实现动态变化|
|线代|`dot`, `cross`, `normalize`, `length`, `distance`,`mul`, `reflect`, `refract(i, n, eta)`|`mul`兼容矩阵左/右乘列/行向量|
|导数|`ddx`, `ddy`, `fwidth`|
|丢弃|`clip`, `discard`|
---

### 3. Shader Language Features

#### 3.1 Semantics `语义`

* **Semantic Binding Register** `[Type] Name [: Semantic]`

|Semantic|Var|Note|
|:-:|:-:|:-:|
|**VertIn (appdata from CPU)**|
|`: POSITION`|`float4 posOS`||
|`: NORMAL`|`float3 nDirOS`|normalizeAfterInterpolation|
|`: TANGENT`|`float4 tDirOS`|normalTexture|
|**FragOut (VertIn)**|
|`: SV_POSITION`|`float4 posCS`||
|**FragOut**|
|`: SV_Target`|`float4 finalCol`||
|**VertIn/VertOut**||for Data Interpolation|
|`TEXCOORDn`|`float~float4`|`floats2 uv[0~3] : TEXCOORD[0~3]` 4 for uv<br>`TEXCOORD[0~15]` 16 for interpolation|
|`COLOR`|`float4 vertexCol`||

#### 3.2 Interpolation Modifiers `插值修饰符`

|Modifier|Note|
|:-:|:-:|
|`nointerpolation`||
|`centroid`|MSAA|
|`linear`|default|

#### 3.3 Preprocessor Directive - Pragmas`预编译指令`

|Directive|Note|
|:-:|:-:|
|**# pragma [shader] [AssignshaderFunc]**|
|`#pragma vertex vert`|vertexShaderFuncMain|
|`#pragma fragment frag`|fragmentShaderFuncMain|
|`#pragma target 3.0/4.0/5.0`|shaderVersion|
|**# pragma multi_compile**|
|`#pragma multi_compile _ _MAIN_LIGHT_SHADOWS`||
|`#pragma multi_compile _ _ADDITIONAL_LIGHTS`||

 
```cpp
c/c++
#include "MyFunc.hlsl" 
#define myGray(x) float4(x,x,x,1);
#ifdef X/#if define(X) #ifndef X/#if !define(X)
#if [cond] #elif #else #endif
#pragma
```

#### 3.4 SRP Batcher CBUFFER (Macros) `可编程渲染管线批处理器常量缓冲区宏`
  
```hlsl
//URP variables declare
CBUFFER_START(UnityPerMaterial)
    // Build-in RP uniform declare from Properties
    // [Type] [PropertyForPerMaterial];
    float4 _MainTex_ST;
CBUFFER_END
```

#### 3.5 Data Type & Modifier

* `float 32bit, half 16bit, fixed 11bit (onlyCG)` 
* `float3x3 matrix = float3x3()` **constructed by elements/rowVector**
* `sampler2D, sampler3D, samplerCUBE, int, bool`
* `float4[.xyzw/.rgba/.xxx/.rg]`
* `in (default) out inout` **valueType**

---

### 4. Library Files

#### 4.1 CG

> `UnityCG.cginc`
> `UnityShaderVariables.cginc` **(autoIncluded)**
> `Lighting.cginc`
> `AutoLight.cginc`
> `UnityIndirect.cginc`
> `UnityInstancing.cginc`

#### 4.2 HLSL

**Include**
`#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/..."`
> `Core.hlsl`
> `Lighting.hlsl`
> `Shadows.hlsl`
> `DeclareDepthTexture.hlsl`
> `SurfaceInput.hlsl` **(PBR)**
> `Particles.hlsl`

**PostProcessing**
> `#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"`
> `#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"`
> `#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"`

**TempHeader**
> `HLSLINCLUDE ...(#include/CBUFFER) ENDHLSL`