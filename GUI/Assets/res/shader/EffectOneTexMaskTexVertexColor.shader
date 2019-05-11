
Shader "DP/Effect/OneTexMaskTexVertexColor"
{
	Properties
	{
		[Enum(AlphaBlend,10,Additive,1)] _BlendMode ("BlendMode", Int) = 1
		[Enum(On,0,Off,2)] _TwoSide ("TwoSide", Int) = 0

		_MainTex		("MainTex (RGB)", 2D) = "white" {}
		_MainTexAlpha   ("MainTexAlpha (R)", 2D) = "black" {}
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
			half4 _MainTex_ST;
			half4 _MainTexAlpha_ST;
            fixed4 _MainColor;
			fixed _MainBright;
			int _BlendMode;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

            struct V2F
            { 
                half4 pos		:	POSITION;
                half4 texcoords	:	TEXCOORD0;
				fixed4 color : COLOR;

            };

            V2F Vert(VertexInput v)
			{
				V2F Output;
				Output.pos = UnityObjectToClipPos(v.vertex);
				Output.texcoords.xy = TRANSFORM_TEX(v.texcoord, _MainTex).xy;				
				Output.texcoords.zw = TRANSFORM_TEX(v.texcoord, _MainTexAlpha).xy;			
				Output.color = v.color;
				return Output;
			}

            fixed4 Frag(V2F Input) : COLOR 
            {
                fixed4 MainTexColor;
                MainTexColor.rgb = tex2D(_MainTex, Input.texcoords.xy).rgb * Input.color.rgb;
                MainTexColor.a   = tex2D(_MainTexAlpha, Input.texcoords.zw).r * Input.color.a;
				MainTexColor = MainTexColor * _MainColor * _MainBright;

				return MainTexColor;
				
            }

			ENDCG
		}
	}
}
