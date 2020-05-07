﻿Shader "Unlit/backGround"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

	// required for UI.Mask
	_StencilComp("Stencil Comparison", Float) = 8
	_Stencil("Stencil ID", Float) = 0
	_StencilOp("Stencil Operation", Float) = 0
	_StencilWriteMask("Stencil Write Mask", Float) = 255
	_StencilReadMask("Stencil Read Mask", Float) = 255
	_ColorMask("Color Mask", Float) = 15
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		// required for UI.Mask
		 Stencil
		 {
			 Ref[_Stencil]
			 Comp[_StencilComp]
			 Pass[_StencilOp]
			 ReadMask[_StencilReadMask]
			 WriteMask[_StencilWriteMask]
		 }
		 ColorMask[_ColorMask]

	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
		// sample the texture
		fixed4 baseCol = fixed4(1, 0.0, 0.5, 1);
		fixed4 col = tex2D(_MainTex, i.uv + float2(0, sin(i.vertex.x / 30 + _Time[1] / 1) / 10));
		fixed4 fragCol = 0.9 * col + 0.1 * baseCol;
		//fixed4 col = tex2D(_MainTex, i.uv);
		

		// apply fog
		UNITY_APPLY_FOG(i.fogCoord, fragCol);
		return fragCol;
	}
	ENDCG
}
	}
}
