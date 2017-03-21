﻿Shader"Custom/ChromaColor" {
 
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_thresh ("Threshold", Range (0, 16)) = 0.65
		_slope ("Slope", Range (0, 1)) = 0.63
		_keyingColor ("KeyColour", Color) = (1,1,1,1)
		_fillColor ("FillColour", Color) = (1,1,1,1)
	}
	 
	SubShader {
		Tags {"Queue"="Transparent""IgnoreProjector"="True""RenderType"="Transparent"}
		LOD 100
		 
		Lighting Off
		ZWrite Off
		AlphaTest Off
		Blend SrcAlpha OneMinusSrcAlpha
		 
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			 
			sampler2D _MainTex;
			float3 _keyingColor;
			float3 _fillColor;
			float _thresh; //0.8
			float _slope; //0.2
			 
			#include "UnityCG.cginc"
			 
			float4 frag(v2f_img i) : COLOR{
				float3 input_color = tex2D(_MainTex,i.uv).rgb;
				float d = abs(length(abs(_keyingColor.rgb - input_color.rgb)));
				float edge0 = _thresh*(1 - _slope);
				float alpha = smoothstep(edge0,_thresh,d);
				return float4(_fillColor,alpha);
			 
			 
			}
			 
			ENDCG
		}
	}
		 
	FallBack "Unlit/Texture"
}