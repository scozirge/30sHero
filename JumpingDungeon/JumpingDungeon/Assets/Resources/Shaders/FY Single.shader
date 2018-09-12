Shader "FY/Single"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "AlphaTest"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
		}
		
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		Cull off

		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 

			uniform sampler2D _MainTex;
			uniform fixed4 _Color;

			fixed4 frag (float2 uv : TEXCOORD0) : COLOR
			{
				fixed4 main = tex2D(_MainTex, uv);
				return fixed4(_Color.rgb, _Color.a * main.a);
			}
			ENDCG
		}
	}

	Fallback off
}