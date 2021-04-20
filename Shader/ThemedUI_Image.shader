Shader "ThemedUI/Image"
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
                float2 uv0 : TEXCOORD0;                
                float2 uv1 : TEXCOORD1;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv0 : TEXCOORD0;                
                float2 uv1 : TEXCOORD1;
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
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                float2 paletteIndex = float2(((i.uv1.x + .5f) / (float)_ColorCount),.5f);
                float4 Tint = tex2D(_Palette, paletteIndex);
                float4 Color = tex2D(_MainTex, i.uv0);
                return Color * float4(Tint.xyz, 1);
            }
            ENDCG
        }
    }    
}
