Shader "DP/Effect/BladeFlashEditorLow"
{
    Properties
    {
        _MainTex ("Diffuse", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _EdgeSize ("勾边大小", Float ) = 0.1
        _EdgeLight ("勾边亮度", Float ) = 100
        _DiffusePara ("diffuse强度", Float ) = 10
        _TintColor ("color", Color) = (0.1,0.4813794,0.8,1)
    }
    
    SubShader
    {
        Tags
        {
            "IgnoreProjector"="True"
            "Queue"="Overlay"
            "RenderType"="Transparent"
        }
        Pass
        {
            Name "FORWARD"
            Tags
            {
                "LightMode"="ForwardBase"
            }
			Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;
            uniform float _EdgeSize;
            uniform float _EdgeLight;
            uniform float _DiffusePara;
            uniform float4 _TintColor;
            
            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            
            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            
            VertexOutput vert (VertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            
            float CalcEdgeNoise(float a, float b)
            {
                return (b <= a) ? 1.0f : 0.0f;
            }
            
            float4 frag(VertexOutput i) : COLOR
            {
                float4 mainTex = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));

			//	float4 mainTex = tex2D(_MainTex, i.uv0);

                float3 emissive = _DiffusePara * _TintColor.rgb * mainTex.rgb * mainTex.r;
				float noise = tex2D(_NoiseTex, TRANSFORM_TEX(i.uv0, _NoiseTex)).r;

                float stepPara = CalcEdgeNoise(_EdgeSize + i.vertexColor.r, noise);
                float stepPara2 = CalcEdgeNoise(i.vertexColor.r, noise);
                float alpha = stepPara + (stepPara - stepPara2) * _EdgeLight;
				float  test = stepPara +(stepPara - stepPara2);

				//return  float4(test.rrr,1.0 );
                return fixed4(emissive, _TintColor.a * i.vertexColor.a * mainTex.a * alpha);
            }
            ENDCG
        }
    }
    FallBack Off
}