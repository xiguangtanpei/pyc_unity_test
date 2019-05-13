Shader "Unlit/test_decal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DecalTex ("Decal", 2D ) = "back" {} 
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
///https://blog.csdn.net/puppet_master/article/details/84310361
// 第一种直接利用2uv 来完成这部分的操作 
// 2uv 正常是在标准光照基础上，标准pbrglsl  很复杂这里就先过 
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv1 :TEXCOORD1 ;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex , _DecalTex ;
			float4 _MainTex_ST , _DecalTex_ST ;


			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy  = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw  = TRANSFORM_TEX(v.uv1 ,_DecalTex );

			//	UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv.xy );
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				float4 decal = tex2D(_DecalTex , i.uv.zw ) ; 

				//// 这部分颜色融合 处理模式
				col.rgb  = lerp(col.rgb , decal.rgb ,decal.a ) ;



				return col;
			}
			ENDCG
		}
	}
}
