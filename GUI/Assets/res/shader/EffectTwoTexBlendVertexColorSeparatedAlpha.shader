
Shader "DP/Effect/TwoTexBlendVertexColorSeparatedAlpha"
{
	Properties
	{
		[Enum(AlphaBlend,10,Additive,1)] _BlendMode ("BlendMode", Int) = 10
		[Enum(On,0,Off,2)] _TwoSide ("TwoSide", Int) = 2

		_MainTex		("MainTex (RGB)", 2D) = "white" {}
		_MainTexAlpha	("MainTexAlpha (RGB)", 2D) = "white" {}
		_MainColor		("MainColor (RGBA)", Color) = (1,1,1,1)
		_MainBright		("MainBright", Float) = 1

		_SecondTex		("SecondTex (RGB)", 2D) = "black" {}
		_SecondColor	("SecondColor (RGB)", Color) = (1,1,1,1)
		_SecondBright	("SecondBright", Float) = 1
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
			sampler2D _MainTexAlpha;
			sampler2D _SecondTex;
			half4 _MainTex_ST;
			half4 _SecondTex_ST;
            fixed4 _MainColor;
            fixed4 _SecondColor;
			fixed _MainBright;
			fixed _SecondBright;
			int _BlendMode;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

            struct V2F
            { 
                half4 pos :	POSITION;
                half4 texcoords	: TEXCOORD0;
				fixed4 color : COLOR;
            };

			V2F Vert(VertexInput v)
			{
				V2F Output;
				Output.pos = UnityObjectToClipPos(v.vertex);
				Output.texcoords.xy = TRANSFORM_TEX(v.texcoord, _MainTex).xy;
				Output.texcoords.zw = TRANSFORM_TEX(v.texcoord, _SecondTex).xy;
				Output.color = v.color;
				return Output;
			}

            fixed4 Frag(V2F Input) : COLOR
            {
                fixed4 MainTexColor;
				MainTexColor.rgb = tex2D(_MainTex, Input.texcoords.xy).rgb;
				MainTexColor.a = tex2D(_MainTexAlpha, Input.texcoords.xy).r;

				fixed3 SecondTexColor = tex2D(_SecondTex, Input.texcoords.zw).rgb;

              
				return MainTexColor;
				
            }

			ENDCG
		}
	}
}
