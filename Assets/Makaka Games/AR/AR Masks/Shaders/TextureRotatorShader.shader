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

Shader "Custom/Makaka Games/Texture Rotator"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}

        _Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
        
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
        _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5

        [Space]
        _RotationDegrees ("Rotation Degrees", Range(-3.14159, 3.14159)) = 0.0
    }

    SubShader
    {  
        CGPROGRAM

        #pragma surface surf Standard alpha:fade vertex:vert

        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input
        {
            half2 uv_MainTex;
        };
       
		fixed4 _Color;

        half _Metallic;
        half _Glossiness;

        half _RotationDegrees;
     
        void vert (inout appdata_full v)
        {
            // Rotation Animation

            half s = sin(_RotationDegrees);
            half c = cos(_RotationDegrees);
           
            half2x2 rotationMatrix = half2x2 (c, -s, s, c);

            rotationMatrix *= 0.5;
            rotationMatrix += 0.5;
            rotationMatrix = rotationMatrix * 2.0 - 1.0;

            v.texcoord.xy -= 0.5;
            v.texcoord.xy = mul(v.texcoord.xy, rotationMatrix);
            v.texcoord.xy += 0.5;
        }
 
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Color
 
            half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

        ENDCG
    }
    FallBack "Standard"
}