// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FY/GuiGray Mask"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_Blend ("Blend", Float) = 0.0
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

			sampler2D _MainTex;
			float _Blend;
			sampler2D _MaskTex;
			float4 _Offset;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
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

			fixed4 frag (v2f i) : COLOR
			{
				float4 c = tex2D(_MainTex, i.texcoord) * i.color;
				c.a = c.a * tex2D(_MaskTex, float2((i.texcoord.x - _Offset.x) / (_Offset.z - _Offset.x), (i.texcoord.y - _Offset.y) / (_Offset.w - _Offset.y))).r;
				
				float grayscale = Luminance(c.rgb);
				float4 output = float4(grayscale, grayscale, grayscale, c.a); 
				
				output.rgb = (1.0 - _Blend) * output.rgb + _Blend * c.rgb;
				return output;
			}
			ENDCG
		}
	}
}