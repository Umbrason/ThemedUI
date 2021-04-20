﻿Shader "ThemedUI/Font"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
        [NoScaleOffset]_Palette("Palette", 2D) = "white" {}
        _ColorCount("ColorCount", Float) = 6
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Lighting Off Cull Off ZTest Always ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;                
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _Palette;
            float4 _MainTex_ST;
            float1 _ColorCount;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                float2 paletteIndex = float2(((i.color.r * 255 + .5f) / (float)_ColorCount),.5f);
                float4 Tint = tex2D(_Palette, paletteIndex);
                float4 Color = float4(Tint.xyz, tex2D(_MainTex, i.uv).w);                
                return Color;
            }
            ENDCG
        }
    }    
}
