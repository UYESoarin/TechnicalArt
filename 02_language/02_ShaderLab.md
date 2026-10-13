## ShaderLab

---

### 1. Frame

```shaderlab
Shader "[Path/Name]"{
    Properties{
        [Property Definition]
    }
    SubShader{
        [Tags]
        [PublicRenderSetup]
        Pass{
            [Name]
            [Tags]
            [RenderSetup]
            [CG/HLSL]
        }
    }
}
```

---

### 2. Properties

#### 2.1 Definition `属性定义`

|Type|Example|Note|
|:-:|:-:|:-:|
||**`[Attribute] propertyName("InspectText", Type) = Default`**|`uniform`|
|**valueType**|
|Float|`_Name("Text", Float) = 0.0`|32bit float/16bit half/8bit fixed (CG)|
|Range|`_Name("Text", Range(0, 1)) = 0.0`|float slider|
|Int|`_Name("Text", Int) = 1`|float|
|Integer|`_Name("Text", Integer) = 1`|int|
|**struct**|
|Vector|`_Name("Text", Vector) = (1, 1, 1, 1)`|float4|
|Color|`_Name("Text", Color) = (1, 1, 1, 1)`|float4 Color Picker|
|**ReferrenceType**|
|2D|`_Name("Text", 2D) = "white"{}`|sampler2D (SAMPLER + TEXTRUE2D)<br>`"color"{texture}` color: white/black/bump/red|
|Cube|`_Name("Text", Cube) = ""{}`|samplerCUBE|
|3D|`_Name("Text", 3D) = ""{}`|sampler3D|
|2DArray|`_Name("Text", 2DArray) = ""{}`|UNITY_DECLARE_TEX2DARRAY(_MyArr)|
|CubeArray|`_Name("Text", CubeArray)` = ""{}|UNITY_DECLARE_TEXCUBEARRAY(_MyArr)|

>* Serialization 
>* RenderSetup:
` Cull [_CullMode]`

#### 2.2 Property Drawer `属性绘制器`

|Attribute|Note|
|:-:|:-:|
|**`[Attribute]`**|
|`[Space()]`|Spacing|
|`[Header(lable)]`|Title|
|`[HideInInspector]`||
|`[NoScaleOffset]`|fixed RT|
|`[Normal]`|check NormalMap|
|`[HDR]`|Emiss/Effect|
|`[Gamma]`|ColorSpace|
|`[PowerSlider(value)]`|Range drag adjust|
|`[Toggle]`|Float|
|`[Toggle(ENABLE)]`|`#ifdef ENABLE #else #endif`|
|`[IntRange]`|Range|
|`[KeyWordEnum(StateOff, State0, etc)]`|`#if _Name_State0 #elif _Name_State1 #else #endif`|
|`[Enum(UnityEngine.Rendering)]`|`CullMode`, `BlendMode`, `BlendOp`|
|`[Enum(Off, 0, Front, 1, Back, 2)]`|`Cull [_Name]`|
|`[PerRendererData]`||

### 2.3. Variant

|Pragma|Variant|Compile|
|:-:|:-:|:-:|
|`#pragma shader_feature`|`Var`|UsingVariant|
|`#pragma multi_compile`|`__ Var1 Var2 ...`|AllVariant & DynamicShift|

* `_local` control single material

---

### 3. SubShader

#### 3.1 LOD

* ON **first** under `Shader.Find([Path/Name]).maximumLOD`
* `Fallback "Path/Name"/Off` when **none**

#### 3.2 SubShader Tags

|`Queue`|Index (RenderSequence)|Note|
|:-:|:-:|:-:|
|`Background`|1000|Skybox|
|`Geometry`|2000|Opaque(<=2500) near2far `ZWrite On`!!!|
|`AlphaTest`|2450|Alpha|
|`Transparent`|3000|far2near `ZWrite Off`!!!|
|`Overlay`|4000|UI|

|`RenderType` (for CPU)|Note|
|:-:|:-:|
|`Background`||
|`Opaque`||
|`Transparent`||
|`TransparentCutout`||
|`Overlay`||


|OtherTagKey|Value|Note|
|:-:|:-:|:-:|
|`RenderPipeline`|`UniversalPipeline`/`HDRenderPipeline`||
|`DisableBatching`|`True`||
|`ForceNoShadowCasting`|`True`||
|`IgnoreProjector`|`True`|
|`CanUseSpriteAtlas`|`True`||
|`PreivewType`|`Shpere`/`Plane`/`Skybox`||

---

### 4. Pass `drawcall`

#### 4.1 Pass Tags

`"LightMode" = "ForwardBase"/"ForwardAdd"/"UniversalForward"/"ShadowCaster"/SRPDefaultUnlit`
* ###### Note: Unlike SubShader-level "Queue", which controls draw order for the material (only ONE queue), Pass-level "LightMode" govern whether this pass is used by the render pipeline

#### 4.2 RenderState

|Command|Type|Note|
|:-:|:-:|:-:|
|`Cull`|`Back`/`Front`/`Off`||
|`ZTest`|`Less`/`Greater`/`LEqual`/`GEqual`/`Equal`/`NotEqual`/`Always`||
|`ZWrite`|`On`/`Off`|Off when Alpha|
|`Offset`|offsetFactor, offsetUnits|Z-Fighting|
|`BlendOp`|`Add`/`Sub`/`RevSub`/`Min`(`Darken`)/`Max`(`Lighten`)/...|`op SrcS, DstD`|
|`ColorMask`|R G B A 0||
|`AlphaToMask`|`On`/`Off`|MSAA Alpha Test|

|`Blend`|Note|
|:-:|:-:|
|`[SrcFactor] [DstFactor]`|`[One, Zero, OneMinus, Src, Dst, Color(RGB), Alpha]`<br>`SrcCol * SrcFactor [BlendOp] DstCol * DstFactor`|
|`SrcAlpha OneMinusSrcAlpha`|`Normal`: Transparent when SrcAlpha = 0|
|`One OneMinusSrcAlpha`|Transparent when SrcCol = 0 (refer to SrcAlpha)|
|`DstColor  Zero`|`Multiply`|
|`OneMinusDstColor One`|`Soft Additive`|
|`DstColor SrcColor`|`2x Multiply`|
|`One One`|`Linear Dodge`: `Min`to`Darken`, `Max`to`Lighten`|
|`One OneMinusSrcColor`|`Screen`|
|`SrcAlpha One`|particleEffect|
|`One Zero`|Default|

#### 4.3 multiPass Problem

* ##### basic structure

```shaderlab
Shader
 └── SubShader
      ├── SubShader Tags
      │    ├── Queue
      │    └── RenderType
      │
      ├── Pass A
      │    ├── Pass Tags
      │    │    └── LightMode
      │    └── Render State
      │         ├── ZWrite
      │         ├── ZTest
      │         ├── Blend
      │         └── Cull
      │
      └── Pass B
           ├── Pass Tags
           │    └── LightMode
           └── Render State
                ├── ZWrite
                ├── ZTest
                ├── Blend
                └── Cull
```

* ##### usual match

|Queue|RenderType|ZWrite|Blend|
|:-:|:-:|:-:|:-:|
|Geometry|Opaque|On|Off|
|Transparent|Transparent|Off|On|

* ###### special pass: UsePass, GrabPass (CG)