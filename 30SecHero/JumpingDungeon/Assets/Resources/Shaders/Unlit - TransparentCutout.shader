// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TransparentCutout"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_Cutoff("Cutoff", Range (0.0, 1.0)) = 1.0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Offset -1, -1
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Cutoff;

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
#if defined(UNITY_HALF_TEXEL_OFFSET)
				o.vertex.xy = o.vertex.xy + float2(-1.0, 1.0) / _ScreenParams.xy;
#endif						
				return o;
			}

			float4 frag (v2f IN) : COLOR
			{
				// Sample the texture
				float4 col = tex2D(_MainTex, IN.texcoord);
				clip(col.a - _Cutoff);
				
				col.rgb = col.rgb * IN.color.rgb;
				col.a = IN.color.a;

				return col;
			}
			ENDCG
		}
	}
}