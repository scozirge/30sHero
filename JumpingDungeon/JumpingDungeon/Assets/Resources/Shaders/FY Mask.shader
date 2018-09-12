// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FY/Mask"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {} 
		_MaskTex ("Alpha (A)", 2D) = "white" {}
		_Offset ("Offset", Vector) = (0.0, 0.0, 1.0, 1.0)
	}
	
	SubShader
	{
		LOD 100

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
			Fog { Mode Off }
			Offset -1, -1
			//ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			struct appdata_t {
				float4 vertex : POSITION; 
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
	
			struct v2f {
				float4 vertex : POSITION; 
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
	
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MaskTex;
			float4 _Offset;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
#if defined(UNITY_HALF_TEXEL_OFFSET)
				o.vertex.xy = o.vertex.xy + float2(-1.0, 1.0) / _ScreenParams.xy;
#endif
				return o;
			}
				
			float4 frag (v2f i) : COLOR
			{
				float4 c = tex2D(_MainTex, i.texcoord) * i.color;
				c.a = c.a * tex2D(_MaskTex, float2((i.texcoord.x - _Offset.x) / (_Offset.z - _Offset.x), (i.texcoord.y - _Offset.y) / (_Offset.w - _Offset.y))).r;
				return c;
			}
			ENDCG
		}
	}
}