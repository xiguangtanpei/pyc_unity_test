// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ling/ling_farview"
{
	Properties
	{	
	[Header(Texture________________________________________________________________________)]
	_Albedo("Tex(RGB),AmbientOcclusion(A)", 2D) = "white" {}
	_Bump("Bump", 2D) = "Bump" {}
	//_LitMap("LitMap(RGB),Roughness(A)", 2D) = "white" {}	
	[Header(Parameter________________________________________________________________________)]
	_TilingA("UVscale",Range(0.5,10)) = 1
	_ambientColor("AmbColor",Color) = (0.45,0.45,0.45,1)
	_diffuseColor("SkyColor",Color) = (1,1,1,1)
	_Roughness("Roughness",Range(0.01,5.0)) =1
	_LitDir("LightDir", Vector) = (0.5,0.5,0.5,1)
	_LitColor("Light Color",Color) = (1,1,1,1)
	_LitIntensity("Light Intensity",Range(0.5,5.0)) =1
	_fogColor("Fog Color",Color)=(0.455,0.584,0.647,1)
	_fogParameter("Fog Parameter",Vector)=(100,200,5,10)
	_Illum("ToonMaping",Range(0,2)) = 1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent"  "RenderType" = "Transparent" "PerformanceChecks" = "False" }
		Cull Off// ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			Name "ling_farview"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "ling_common.cginc"
			sampler2D _Albedo,_Bump,_LitMap;
			float4    _Albedo_ST,_fogParameter;
			half3    _LitDir, _LitColor, _diffuseColor, _ambientColor, _fogColor;
			half     _LitIntensity, _Roughness, _TilingA,_Illum;
			/*
			float basis[16] = 
			{
				2.18212	2.09382	2.03155
				0.617698	0.66103	0.726236
				- 0.0492189 - 0.0395606 - 0.0277913
				0.0418531	0.0421737	0.0672506
				0.0385599	0.0199808	0.00944547
				0.184923	0.160617	0.119763
				- 0.177764 - 0.163258 - 0.152423
				- 0.036139 - 0.0386875 - 0.0467528
				- 0.193176 - 0.164738 - 0.145377
				0.277815	0.282894	0.291235
				- 0.0349441 - 0.0333024 - 0.0303678
				0.158055	0.156502	0.168379
				- 0.0378126 - 0.035689 - 0.027918
				0.164342	0.15561	0.158921
				0.0420723	0.0412566	0.0495938
				0.129513	0.125985	0.123919

			};*/


			struct appdata
			{
				float4 vertex  : POSITION;
				half4  normal  : NORMAL;
				half4  tangent : TANGENT;
				float2 uv0     : TEXCOORD0;
				float2 uv1     : TEXCOORD1;
			};
			struct v2f
			{
				float4 pos                           : SV_POSITION;
				float2 uv                            : TEXCOORD0;
				float2 uv2                           : TEXCOORD1;
				half3  viewDir                       : TEXCOORD2;				
				half4  tanToWorldwPos[3]             : TEXCOORD3;
				half3  halfDir                       : TEXCOORD6;
				half2 distance                       : TEXCOORD7;
			};
			v2f vert(appdata v)
			{
				v2f o;
				float4 posObject = v.vertex;
				float4 posWorld= mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv0*_TilingA;
				o.uv2 = v.uv1;
				o.viewDir = normalize(_WorldSpaceCameraPos - posWorld.xyz);
				float3 normalWorld = UnityObjectToWorldNormal(v.normal);
				float4 tangentWorld = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
				float3 binormalWorld = cross(normalWorld, tangentWorld.xyz)*tangentWorld.w;
				float3x3 tangentToWorld = half3x3(tangentWorld.xyz, binormalWorld, normalWorld);
				o.tanToWorldwPos[0].xyz = tangentToWorld[0];
				o.tanToWorldwPos[1].xyz = tangentToWorld[1];
				o.tanToWorldwPos[2].xyz = tangentToWorld[2];
				o.tanToWorldwPos[0].w = posWorld.x;
				o.tanToWorldwPos[1].w = posWorld.y;
				o.tanToWorldwPos[2].w = posWorld.z; 
				o.halfDir =SafeNormalize(normalize(_LitDir) + o.viewDir);
				o.distance =half2(o.pos.w, posWorld.y);		
				return o;
			}

			//FOG
			half3 ApplyFog(half3 color,half3 _fogColor ,half distance, half height)
			{
				half distanceFog = clamp((distance - _fogParameter.x) / _fogParameter.y, 0.0, 1.0);
				half heightFog = clamp((_fogParameter.z - height) / _fogParameter.w, 0.0, 1.0);
				return lerp(color, _fogColor.rgb,  max(heightFog,distanceFog ));
			}

			half4 frag(v2f i) : SV_Target
			{
				half4 albedoWithOcclusion = tex2D(_Albedo, i.uv);
				half4 normalWorldandao = tex2D(_Bump, i.uv);
				half4 litmapcolor = tex2D(_LitMap, i.uv2) ;
				//return litmapcolor*4.59;
				half3 albedo = albedoWithOcclusion.rgb;
				half  alpha = albedoWithOcclusion.a;
				half occlusion = normalWorldandao.a;
				half3 normalWorld = normalWorldandao.rgb;
				normalWorld.zy = normalWorld.yz;
				normalWorld.xz = 1 - normalWorld.xz;
				normalWorld = normalWorld * 2 - 1;

				//return half4(normalWorld, 1);
				half roughness= _Roughness;
				half3 litDir = normalize(_LitDir.rgb);
				half3 viewDir = i.viewDir;
/*
				normalTangent.xyz = normalTangent.xyz * 2 - 1;
				normalTangent.z = sqrt(1 - saturate(dot(normalTangent.xy, normalTangent.xy)));
				half3 tangent = i.tanToWorldwPos[0].xyz;
				half3 binormal = i.tanToWorldwPos[1].xyz;
				half3 normal = i.tanToWorldwPos[2].xyz;
				half3 normalWorld = normalize(tangent * normalTangent.x + binormal * normalTangent.y + normal * normalTangent.z);*/


				half3 halfDir =/* i.halfDir;*/ SafeNormalize(litDir + viewDir);
				half nl = saturate(dot(normalWorld, litDir));
				half nh = saturate(dot(normalWorld, halfDir));
				half nv = saturate(dot(normalWorld, viewDir));

				half d = Distribution_GGX(roughness, nh);
				half2 gf = GeometryFresnel_Metal(roughness, nv);

				half3 diffuse = albedo.rgb;
				half3 specular = albedo.rgb;

				//half3 diffuse = albedo.rgb;
				//half3 specular = (diffuse * gf.x + gf.y) * d;
				//return half4(nl.xxx, 1);

				specular = (specular * gf.x + gf.y) * d;
				//half3 color = (_diffuseColor*nl*_LitColor*_LitIntensity + _ambientColor) * (diffuse + specular *0.25) * occlusion*litmapcolor;
				half3 color = (specular*d*0.25 + diffuse)*nl*_diffuseColor*_LitColor*_LitIntensity*occlusion +(diffuse + gf.y*20)*_ambientColor;//乘2为了提高漫反射的颜色纯度和整体亮度
				//return half4(nl.xxx, 1);
				//half3 color = _LitColor * _LitIntensity * nl * (diffuse + specular * 0.25);//  +diffuse * _ambientColor * half3(0.35, 0.366, 0.5) * occlusion);// * albedo_ambient.a;
				//half3 color = (_LitColor*_LitIntensity * min(nl, litmapcolor.a) * (diffuse + specular * 0.25) * litmapcolor.rgb + diffuse * litmapcolor.rgb * half3(0.35, 0.366, 0.5) * occlusion);// * albedo_ambient.a;

				//half heightFog = clamp((_fogParameter.z - i.distance.y) / _fogParameter.w, 0.0, 1.0);
				//color = lerp(color, _fogColor.rgb, heightFog);//
				color=ApplyFog(color, _fogColor, i.distance.x, i.distance.y);
				//return half4((diffuse + gf.y)*_ambientColor*litmapcolor.rgb, 1);
				//return half4(lerp(color, _fogColor.rgb,clamp((i.distance.x-100)/100,0,1)),1);

				// Uncharted2ToneMapping(color, _Illum)*litmapcolor.rgb;
				return half4(color,alpha);
			}
			ENDCG
		}
	}
}
