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

Shader "Custom/Makaka Games/Particles/Cutout Transparent"
{
	Properties
	{
		[Header (Translucency)]
		_Translucency("Strength", Range (0 , 50)) = 1
		
		_TransNormalDistortion("Normal Distortion", Range (0 , 1)) = 0.1
		
		_TransScattering("Scaterring Falloff", Range (1 , 50)) = 2
		
		_TransDirect ("Direct", Range (0 , 1)) = 1
		
		_TransAmbient ("Ambient", Range (0 , 1)) = 0.2
		
		_TransShadow ("Shadow", Range (0 , 1)) = 0.9
		
		_Cutoff ("Cutoff", Float) = 0.5
		
		_MainTex ("MainTex", 2D) = "white" {}

		_Color ("Main Color", Color) = (.2,.2,.2,0)
		
		[HideInInspector] 
		_texcoord ("", 2D) = "white" {}
		
		[HideInInspector] 
		__dirty ("", Int) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		
		Cull Off
		
		CGPROGRAM
		
		#include "UnityPBSLighting.cginc"
		
		#pragma target 3.0
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred 
		
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		struct SurfaceOutputStandardCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			fixed3 Translucency;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float _Cutoff = 0.5;
		uniform float3 _Color;

		inline half4 LightingStandardCustom (SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi)
		{
			#if !DIRECTIONAL
			
			float3 lightAtten = gi.light.color;
			
			#else
			
			float3 lightAtten = lerp (_LightColor0.rgb, gi.light.color, _TransShadow);
			
			#endif
			
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			
			half transVdotL = pow ( saturate (dot (viewDir, -lightDir)), _TransScattering );
			
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			
			half4 c = half4 (s.Albedo * translucency * _Translucency, 0);

			SurfaceOutputStandard r;
			
			r.Albedo = s.Albedo * _Color;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI (SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi)
		{
			UNITY_GI (gi, s, data);
		}

		void surf (Input i , inout SurfaceOutputStandardCustom o)
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			
			float4 tex2DNode1 = tex2D (_MainTex, uv_MainTex);
			
			float4 temp_output_4_0 = (tex2DNode1 * i.vertexColor);
			
			o.Albedo = temp_output_4_0.rgb;
			
			float temp_output_5_0 = 0.0;
			
			o.Metallic = temp_output_5_0;
			o.Smoothness = temp_output_5_0;
			o.Translucency = temp_output_4_0.rgb;
			o.Alpha = 1;
			
			clip (tex2DNode1.a - _Cutoff);
		}

		ENDCG
	}

	Fallback "Standard"
}