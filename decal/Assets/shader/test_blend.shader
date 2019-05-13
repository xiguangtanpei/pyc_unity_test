Shader "Unlit/test_blend"
{
	Properties
	{
		[Header(________________readering mode________________ )]
		[Enum(AlphaBlend,10,Additive,1)] _BlendMode("BlendMode(融合模式)", Int) = 1
		[Enum(On,0,Off,2)] _TwoSide("TwoSide(双面)", Int) = 0
		[Enum(On,1,Off,0)] _ZWrite ("ZWrite(排序)", Int) = 1


		_MainTex ("Texture", 2D) = "white" {}
		_Intancty ("强度",float ) = 1 
	}
	SubShader
	{
		Tags { "RenderType"="Transparent"  "Queue" = "Transparent"
		"IgnoreProjector" = "True"

		 }
		// LOD 100
			Blend SrcAlpha[_BlendMode]

			Cull[_TwoSide]
			Lighting Off
			ZWrite [_ZWrite]


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
				// UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Intancty ; 
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				// UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				// UNITY_APPLY_FOG(i.fogCoord, col);
				/// a 就是自己透明
				return col*_Intancty;
			}
			ENDCG
		}
	}
}
