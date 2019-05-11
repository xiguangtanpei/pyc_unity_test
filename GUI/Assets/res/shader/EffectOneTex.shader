﻿
Shader "DP/Effect/OneTex"
{
	Properties
	{
		[Enum(AlphaBlend,10,Additive,1)] _BlendMode ("BlendMode", Int) = 10
		[Enum(On,0,Off,2)] _TwoSide ("TwoSide", Int) = 2

		_MainTex		("MainTex (RGB)", 2D) = "white" {}
		_MainColor		("MainColor (RGB)", Color) = (1,1,1,1)
		_MainBright		("MainBright", Float) = 1
	}

	SubShader
	{
		Lod 100

		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"IgnoreProjector"="True"
		}

		Pass 
		{
			Blend SrcAlpha [_BlendMode]
			//Blend SrcAlpha One 
			//Blend SrcAlpha OneMinusSrcAlpha
			Cull [_TwoSide]
			Lighting Off
			ZWrite Off

	        CGPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag
			#pragma only_renderers d3d11 gles3 metal
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2 INSTANCING_ON DIRECTIONAL POINT POINT_COOKIE SPOT DIRECTIONAL_COOKIE LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK SHADOWS_CUBE

			#include "UnityCG.cginc"

            sampler2D _MainTex;
			half4 _MainTex_ST;
            fixed4 _MainColor;
			fixed _MainBright;
			int _BlendMode;

            struct V2F
            { 
                half4 pos		:	POSITION;
                half2 texcoords	:	TEXCOORD0;
            };

            V2F Vert(appdata_full v)
			{
				V2F Output;
				Output.pos = UnityObjectToClipPos(v.vertex);
				Output.texcoords.xy = TRANSFORM_TEX(v.texcoord, _MainTex).xy;

				return Output;
			}

            fixed4 Frag(V2F Input) : COLOR 
            {
                fixed4 MainTexColor = tex2D(_MainTex, Input.texcoords.xy);
				MainTexColor = MainTexColor * _MainColor * _MainBright;

				
				return MainTexColor;
				
            }

			ENDCG
		}
	}
}