﻿Shader "ThemedUI/Image"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
        [NoScaleOffset]_Palette("Palette", 2D) = "white" {}
        _ColorCount("ColorCount", Float) = 6

        _StencilComp		("Stencil Comparison", Float) = 8
	    _Stencil			("Stencil ID", Float) = 0
	    _StencilOp			("Stencil Operation", Float) = 0
	    _StencilWriteMask	("Stencil Write Mask", Float) = 255
	    _StencilReadMask	("Stencil Read Mask", Float) = 255

        _CullMode			("Cull Mode", Float) = 0
	    _ColorMask			("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Stencil
	    {
		    Ref [_Stencil]
		    Comp [_StencilComp]
		    Pass [_StencilOp]
		    ReadMask [_StencilReadMask]
    		WriteMask [_StencilWriteMask]
	    }
        LOD 100
        Lighting Off Cull Off ZTest Always ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
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
                fixed4 color : COLOR;
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
            float _ColorCount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;                
                o.color = v.color;                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float index = floor(i.uv.x);
                float2 paletteUV = float2((index + .5f) / (float)_ColorCount, .5f);
                float4 Tint = tex2D(_Palette, paletteUV);
                float4 Color = tex2D(_MainTex, i.uv - float2(index, 0));
                return Color * Tint * i.color;
            }
            ENDCG
        }
    }    
}
