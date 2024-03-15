// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Baoyu/Model/Transparent"
{
    Properties
    {
        _ColorAlpha("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "black" {}
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" }

        Pass
        {
            Cull Back
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D	_MainTex;
            fixed4 _ColorAlpha;

            struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
            };

            struct v2f
            {
                half4 pos    : SV_POSITION;
                half2 tc1    : TEXCOORD0;
            };

            v2f vert(VertInput  ad)
            {
                v2f v;

                v.pos = UnityObjectToClipPos(ad.vertex);
                v.tc1 = ad.texcoord;
                return v;
            }

            fixed4 frag(v2f v) :COLOR
            {
                fixed4 fcolor = tex2D(_MainTex, v.tc1);
                return fcolor * _ColorAlpha;
            }

            ENDCG
        }
    }
    //Fallback "VertexLit"
}
