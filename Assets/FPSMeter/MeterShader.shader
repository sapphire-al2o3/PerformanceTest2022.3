Shader "Unlit/MeterShader"
{
	Properties
	{
		_Color ("Color", Color) = (0.0, 1.0, 0.0, 1.0)
		_Size ("Size", Vector) = (0.1, 1.0, 0.0, 0.0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Cull Off
		ZTest Always

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
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			float4 _Size;
			
			v2f vert (appdata v)
			{
				v2f o;
				float2 p = v.uv;
				p = p * _Size.xy + _Size.zw;
				o.vertex = float4((p * 2.0 - 1.0), 1.0, 1.0);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
