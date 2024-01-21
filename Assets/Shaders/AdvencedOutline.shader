﻿Shader "RPG.Shaders/AdvenceOutline"
{
    Properties
    {
		_Tex("Texture", 2D) = "white" { }
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline Width", Range (0, 0.15)) = 0
    }

    SubShader
    {
		CGPROGRAM
        #pragma surface surf Lambert 
		
        struct Input
        {
			float2 uv_Tex;
        };

		sampler2D _Tex;

        void surf (Input IN, inout SurfaceOutput o)
        {
			o.Albedo = tex2D(_Tex, IN.uv_Tex).rgb;
        }
        ENDCG

		Pass
		{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
            {
                float4 vertex : POSITION;
                float2 normal : NORMAL;
            };

			struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

			float _Outline;
			float4 _OutlineColor;

			v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
               
			   float3 norm = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));

			   float2 offset = TransformViewToProjection(norm.xy);

			   o.pos.xy += offset * o.pos.z * _Outline;
			   float4 standerColor = (1, 1, 1, 0);
			   o.color = _Outline > 0 ? _OutlineColor : standerColor;
               return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}
    }
    FallBack "Diffuse"
}
