
Shader "DP/Effect/DP_Self_Distort" 
{
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_NoiseTex ("Distort Texture (RG)", 2D) = "white" {}
		_MainTex ("Alpha (A)", 2D) = "white" {}
		_HeatTime  ("Heat Time", range (-1,1)) = 0
		_ParaX  ("UV Move Scale X", range (-10,10)) = 0.1
		_ParaY  ("UV Move Scale Y", range (-10,10)) = 0.1
		_MainBright		("MainBright", Float) = 1
	}

	///*Category {
	//		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
	//		Blend SrcAlpha One
	//		Cull Off 
	//		Lighting Off 
	//		ZWrite Off 
			//Fog { Color (0,0,0,0) }*/
			//BindChannels {
			//	Bind "Color", color
			//	Bind "Vertex", vertex
			//	Bind "TexCoord", texcoord
			//}

		SubShader {
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

			Pass {
			Blend SrcAlpha One
			Cull Off
			Lighting Off
			ZWrite Off
			Fog{ Color(0,0,0,0) }


				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_particles
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord: TEXCOORD0;
				};

				struct v2f {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 uvmain : TEXCOORD1;
				};

				fixed4 _TintColor;
				fixed _ParaX;
				fixed _ParaY;
				float _HeatTime;
				float4 _MainTex_ST;
				float4 _NoiseTex_ST;
				sampler2D _NoiseTex;
				sampler2D _MainTex;
				float _MainBright;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
					return o;
				}



				float4 frag( v2f i ) : COLOR
				{
					//noise effect -- d 
					fixed4 offsetColor1 = tex2D(_NoiseTex, i.uvmain + _Time.xz*_HeatTime);
			
					fixed4 offsetColor2 = tex2D(_NoiseTex, i.uvmain + _Time.yx*_HeatTime);
					
					i.uvmain.x += ((offsetColor1.r + offsetColor2.r) - 1) * _ParaX;
					i.uvmain.y += ((offsetColor1.r + offsetColor2.r) - 1) * _ParaY;
					float4 ti = tex2D(_MainTex, i.uvmain);
					fixed4 vc =float4( (i.color).rgba );

					fixed3 rgbs = ti.rgb *2.0 *vc.rgb *_MainBright; 
					
					return    ( ti * 2.0 *vc * _TintColor *_MainBright);
					//return float4(rgbs,ti.a*vc.a*_TintColor.a );

				}
				ENDCG
			}
		}
	//}
}
