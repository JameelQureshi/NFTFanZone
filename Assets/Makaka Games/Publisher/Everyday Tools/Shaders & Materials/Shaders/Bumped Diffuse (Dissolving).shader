/*
===================================================================
Unity Assets by MAKAKA GAMES: https://makaka.org/o/all-unity-assets
===================================================================

Online Docs (Latest): https://makaka.org/unity-assets
Offline Docs: You have a PDF file in the package folder.

=======
SUPPORT
=======

First of all, read the docs. If it didn’t help, get the support.

Web: https://makaka.org/support
Email: info@makaka.org

If you find a bug or you can’t use the asset as you need, 
please first send email to info@makaka.org (in English or in Russian) 
before leaving a review to the asset store.

I am here to help you and to improve my products for the best.
*/

Shader "Custom/Makaka Games/Bumped Diffuse (Dissolving)" 
{
  Properties 
  {
    _Color ("Main Color", Color) = (1, 1, 1, 0)
    _MainTex ("Texture (RGB)", 2D) = "white" {}
    _BumpMap ("Bumpmap", 2D) = "bump" {}
    _SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}
    _Cutoff ("Cutoff", Range(0.0, 1.0)) = 0
  }
  
  SubShader 
  {
    Tags { "RenderType" = "Opaque" }
    
    Cull Off
      
    CGPROGRAM
      
    //if you're not planning on using shadows, remove "addshadow" for better performance
    #pragma surface surf Lambert addshadow
      
    struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float2 uv_SliceGuide;
          float _Cutoff;
      };
      
    sampler2D _MainTex;
    sampler2D _BumpMap;
    sampler2D _SliceGuide;
      
    float _Cutoff;

    uniform float3 _Color;
    
    void surf (Input IN, inout SurfaceOutput o) 
    {
        clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgb - _Cutoff);
          
        o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
        o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
    }
      
    ENDCG
  } 

  Fallback "Diffuse"
}