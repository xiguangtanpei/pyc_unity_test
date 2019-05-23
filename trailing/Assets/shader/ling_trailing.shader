shader "Ling/ling_trailing"
{
    Properties 
    {
        [Header(________________readering mode________________ )]
		[Enum(AlphaBlend,10,Additive,1)] _BlendMode("BlendMode(融合模式)", Int) = 1
		[Enum(On,0,Off,2)] _TwoSide("TwoSide(双面)", Int) = 0
        [Space  (30)]
        
        [Header(________________parameter________________ )]
        _MainTex  ("轨迹图", 2D ) = "white" {}
         [KeywordEnum(AlphaTexA  , AlphaTexR )] _AlphaTex  ("alpha(透明)", Float) = 0
        _MainColor ("染色", color  ) = (1,1,1,1) 
        _MainBright ("亮度", float ) = 1.0 


    }
    SubShader
    {
        Tags {
		"RenderType" = "Transparent"
		"Queue" = "Transparent"
		"IgnoreProjector" = "true"

        }

        pass 
        {

			Blend SrcAlpha[_BlendMode]
			Cull[_TwoSide]
			Lighting Off
			ZWrite  Off 

            CGPROGRAM
			#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			#pragma only_renderers d3d11 gles3 metal
            #include "UnityCG.cginc"
            #pragma  multi_compile  _ALPHATEX_ALPHATEXA  _ALPHATEX_ALPHATEXR 

            struct appdata 
            {
                float4 vertex:POSITION ;
                float4 uv :TEXCOORD0 ;
                float4 color :COLOR ;
                
            };

            struct v2f  
            {
                float4  Hpos : SV_POSITION ; 
                float2    uv0  :TEXCOORD0 ; 
                float4 color : COLOR ; 
            };
            sampler2D _MainTex ; 
            float4 _MainColor , _MainTex_ST ; 
            float  _MainBright  ; 

            v2f  vert (appdata v ) 
            {
                v2f  o ; 
                o.Hpos = UnityObjectToClipPos(v.vertex ) ;
                o.uv0   = TRANSFORM_TEX(v.uv ,_MainTex ) ; 
                o.color = v.color ; 
                return  o  ;

            }

            float4  frag (v2f i ) : SV_TARGET 
            {
                   float2 newuv0 =float2(-i.uv0.y , -i.uv0.x ) ; 
                    // 拖尾的uv 可以用来用透明处理 
                    float4 texc = tex2D(_MainTex ,newuv0) ; 

                  #ifdef  _ALPHATEX_ALPHATEXA 
                    texc = texc *_MainColor*_MainBright*i.color ; 

                  #elif _ALPHATEX_ALPHATEXR 

                    texc = float4 (texc.rgb , texc.r)*_MainColor*_MainBright*i.color ; 
                  #endif 
                /// 需要制作uv 做透明
                    return texc ;  


            }

            ENDCG 


        }


    }

}