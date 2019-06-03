Shader "Ling/ling_effect_overlay"
{
	Properties
	{
		[Header(________________readering mode________________ )]
		[Enum(AlphaBlend,10,Additive,1)] _BlendMode("BlendMode(融合模式)", Int) = 1
		[Enum(On,0,Off,2)] _TwoSide("TwoSide(双面)", Int) = 0
		[Enum(On,1,Off,0)] _ZWrite ("ZWrite(排序)", Int) = 1
		[Header(________________ ghost  ________________________)]
		[Toggle] _Ghost("ghost (沟边)", float) = 0.0
		_GhostColor("ghost color(ghost 颜色)", Color) = (1,1,1,1)
		_GhostIntensty ("ghost bright (ghost 强度)", Range(0, 5)) =1

	   [Header(________________texturemap____________________)]

       [KeywordEnum(OnetexAlphaA , OnetexAlphaR , MulTwotexAlphaA  ,MulTwotexAlphaR ,MulTwotexAlphaAtoThreeR ,AddTwotexAlphaA  ,AddTwotexAlphaR ,AddTwotexAlphaAtoThreeR )] _ABCTex ("ABC-blend type(ABC三种图融合模式)", Float) = 0
        _MainTex ("A tex (第一张贴图) ", 2D) = "white" {}
        _MainColor("A color (RGBA)(控制第贴图颜色和透明)", Color) = (1,1,1,1)
        _MainBright("A  bright(贴图亮度)", Float) = 1
        [Space (30)]
        _SubTex("B tex (第二张贴图)  ", 2D) = "white" {}
		_SubColor("B color (RGBA)(贴图颜色和透明)", Color) = (1,1,1,1)
		_SubBright("B bright(贴图亮度)", Float) = 1
        [Space (30)]
        [NOscaleoffset]	_SubMask("C tex (第三张贴图(r)) ", 2D) = "white" {}
        _ThreetexAlphaintensty  ("alpha  (透明度控制)", Range(0, 1)) =1

		[Header(________________   rim outline(vs) _______________________)]
		[Toggle] _Rimoutline ("rimoutline (是否开启边缘光) ", float) = 0.0
		_EdgeIn("edge in range(开始范围设置)", Range(0, 2)) = 0.5
		_EdgeOut("edge out range(结束范围设置)", Range(0, 2)) = 1.5
		_EdgeColor("edge color(边缘光颜色)", Color) = (1,1,1,1)

		[Header(________________uv  and move uv (one tex )______________)]
		[Toggle] _UVMOVE_SEQUENCE ("use uvmove-sequence(是否播放序列图 A tex ) ", float) = 0.0
		_Tile("x:col(列) y:row(行)  z:rate(速率) w:move(偏移)", Vector) = (4,4,1,1)

		[Header(________________   distance mask         ________________)]
		[Toggle] _Distancemask ("use distance mask(距离当mask透明) ", float) = 0.0
		_MinMax  ("x:min x   y:max x   z:min y   w:max y  ", Vector) = (-10 ,10 ,-10 ,10 )

		[Header(________________       dissove (A or AB tex )   ______________)]
		[Toggle] _Dissove_fire    ("dissove (溶解燃烧 -Aor AB tex 底图  C tex mask-r)", float) = 0.0
		_DissColor("dissolve color(燃烧颜色)", Color) = (1,1,1,1)
		_Amount("dissolve amount(燃烧完整度)", Range(0, 1)) = 0.5
		_StartAmount("start amount(燃烧边缘宽度设置)", Range(0, 1)) = 0.1

		[Header(________________    distort(A B tex)   _____________)]
		[Toggle] _Distort ("_distort (热扰动-A tex(rg)-扰动 B tex 底图 )", float) = 0.0
		_HeatTime("heat time(扰动速度)", range(-1,1)) = 0
		_ParaX("uv move scale X(x方向偏移)", range(-10,10)) = 0.1
		_ParaY("uv move scale Y(y方向偏移)", range(-10,10)) = 0.1
        
    }
    	SubShader
	{ 
	Tags { 
		// "RenderType" = "Transparent"
		// "Queue" = "Transparent"
		// "IgnoreProjector" = "True"

		"RenderType" = "Transparent"
		"Queue" = "Overlay"
		"IgnoreProjector" = "True"

        //"RenderType" = "Opaque"
        //"Queue" = "Geometry"
        //"IgnoreProjector" = "False"

	  }

		Pass
		{
			Blend SrcAlpha[_BlendMode]

			Cull[_TwoSide]
			Lighting Off
			ZWrite [_ZWrite]

			CGPROGRAM
			#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			#pragma only_renderers d3d11 gles3 metal
            #include "UnityCG.cginc"
            #pragma shader_feature  _GHOST_ON  
            #pragma multi_compile _ABCTEX_ONETEXALPHA _ABCTEX_ONETEXALPHAR _ABCTEX_MULTWOTEXALPHAA  _ABCTEX_MULTWOTEXALPHAR _ABCTEX_MULTWOTEXALPHAATOTHREER  _ABCTEX_ADDTWOTEXALPHAA  _ABCTEX_ADDTWOTEXALPHAR _ABCTEX_ADDTWOTEXALPHAATOTHREER
            #pragma shader_feature  _UVMOVE_SEQUENCE_ON      
            #pragma shader_feature  _DISTANCEMASK_ON   
            #pragma shader_feature  _RIMOUTLINE_ON    
            #pragma shader_feature  _DISSOVE_FIRE_ON  
            #pragma shader_feature  _DISTORT_ON 




            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 color:COLOR;
                half3 normal : NORMAL;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color:COLOR;
                #ifdef _DISTANCEMASK_ON 
                    float3 vpos :TEXCOORD1;
                #endif 
                #ifdef _RIMOUTLINE_ON
                    float3  rim :TEXCOORD2;
                #endif 
            };  

            sampler2D _MainTex;
            sampler2D _SubTex;
            sampler2D _SubMask;

            float4 _GhostColor ,_MainColor, _SubColor,_MainTex_ST,_SubTex_ST,_Tile ,_MinMax,_EdgeColor ,_DissColor;
            float _GhostIntensty ,_MainBright,_SubBright,_ThreetexAlphaintensty; 
            float _EdgeIn , _EdgeOut,_Amount ,_StartAmount,_HeatTime,_ParaX,_ParaY; 

            ///动态uv 处理 
            /// unity_time 是unity 内部时间
            /// index 默认是序号
            ///  tileinfo 表示 纵 行方向 数量 ，变换速率， 偏移数值  ( 4,4,1,1  )
            /// uv  顶点阶段uv 处理完成
            float2 ling_tileuv(float unity_time, int   index, float4   tileinfo, float2 uv)
            {
                float _startTime = 0;

                float4 _Tile = tileinfo;
                float2 timeReduced = fmod((unity_time - _startTime) * 0.25, 1) * 1000;
                index += _Tile.z * timeReduced + _Tile.w;
                float2 uv1 = uv * float2(1.0 / _Tile.x, 1.0 / _Tile.y) + float2((index) % _Tile.x * (1.0 / _Tile.x), (_Tile.y - floor((index + _Tile.x) / _Tile.x)) * (1.0 / _Tile.y));
                return uv1;
            }
            /// 顶点阶段的沟边
            /// local_ver 传入的local 顶点
            /// local_normal 传入的local 的法线 
            ///  edgein 限制够表范围 
            /// edgeout 限制的范围 
            /// outline_color  沟边颜色 
            half3 ling_vs_outline(half4 local_ver, half3 local_normal, half edgein, half edgeout, half3  outline_color)
            {
                half3 viewDir = normalize(ObjSpaceViewDir(local_ver));
                half dotProduct = max(0, dot(local_normal, viewDir));
                half3 rgb = (smoothstep(edgein, edgeout, 1 - dotProduct) * outline_color.rgb);
                return  rgb;
            }
            ///mapchannel_r 贴图的单色贴图通道
            ///dissolve_value 动态调节的溶解数值
            /// start_vlaue  设置的比较数值
            ///color  传入如的 颜色 可以进行染色光照最后处理 
            /// firecolor 这个是火焰的颜色
            float3 ling_dissolve(float mapchannel_r, float dissolve_value, float start_vlaue, float3 color, float3 firecolor)
            {
                float clipvalue = mapchannel_r - dissolve_value;
                clip(clipvalue);

                ///  
                float dissoveFactor = clipvalue < start_vlaue ? clipvalue / start_vlaue : 1;
                float3 rgb = (mapchannel_r > start_vlaue && clipvalue < start_vlaue) ? color.rgb * dissoveFactor * dissoveFactor * 9 * firecolor : color.rgb;
                return  rgb;
            }


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                /// 顶点阶段构建uv 
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex).xy;
                o.uv.zw = TRANSFORM_TEX(v.uv, _SubTex).xy; 
                o.color = v.color;

                    // uv 处理
                #ifdef _UVMOVE_SEQUENCE_ON
                o.uv.xy = ling_tileuv(_Time.x, 0, _Tile, o.uv.xy);
                #endif 	

                #ifdef _GHOST_ON 
                    half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                    half dotProduct = 0.5 - 0.5 * dot(v.normal, viewDir);
                    o.color.a = dotProduct*_GhostColor.a;
                #endif 

                #ifdef _DISTANCEMASK_ON
			    o.vpos = v.vertex.xyz;
		        #endif 

                #ifdef _RIMOUTLINE_ON
                    o.rim = ling_vs_outline(v.vertex, v.normal, _EdgeIn, _EdgeOut, _EdgeColor.rgb);
                #endif 



                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                float4 ling_vertercolor = i.color ;
                float2 ling_uv0 = i.uv.xy  ;
                float2 ling_uv1 = i.uv.zw ;
                float4 result = float4(0,0,0,1) ;
                float3  ling_rim = float3(0,0,0) ;

                #ifdef _RIMOUTLINE_ON
                    ling_rim = i.rim;
                #endif 


            //_ABCTEX_ONETEXALPHA _ABCTEX_ONETEXALPHAR _ABCTEX_TWOTEXALPHAA  _ABCTEX_TWOTEXALPHAR _ABCTEX_TWOTEXALPHAATOTHREER 
                /// 只用加 
                #ifdef _ABCTEX_ONETEXALPHA 
                /// 输出 最后result
                 float4 col = tex2D(_MainTex, ling_uv0);
                 float3  rgb = col.rgb *ling_vertercolor.rgb *_MainColor.rgb *_MainBright ;
                 float    a  = col.a *ling_vertercolor.a *_MainColor.a *_MainBright ; 
                 result = float4(rgb , a );

                #elif  _ABCTEX_ONETEXALPHAR 
                 float4 col = tex2D(_MainTex, ling_uv0);
                 float3  rgb = col.rgb *ling_vertercolor.rgb *_MainColor.rgb *_MainBright ;
                 float    a  = col.r *ling_vertercolor.a *_MainColor.a *_MainBright ; 
                 result = float4(rgb , a );
                #elif _ABCTEX_MULTWOTEXALPHAA

                    #ifdef _DISTORT_ON 
                        float2 t1 = ling_uv0 + _Time.xz*_HeatTime;
                        float2 t2 = ling_uv0 + _Time.yx*_HeatTime;
                        fixed4 offsetcolor1 = tex2D(_MainTex, t1);
                        fixed4 offsetcolor2 = tex2D(_MainTex, t2);
                        ling_uv0.x += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaX;
                        ling_uv0.y += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaY;



                        float4 col1 = tex2D (_SubTex , ling_uv0 );
                        float    a1 = col1.a * _SubColor.a *_SubBright; 
                        result = float4((col1.rgb )*ling_vertercolor.rgb*_SubColor.rgb , (a1)*ling_vertercolor.a );
                    #else 

                        float4 col = tex2D(_MainTex, ling_uv0);
                        float4 col1 = tex2D (_SubTex , ling_uv1 );
                        float3  rgb = col.rgb  *_MainColor.rgb *_MainBright ;
                        float3  rgb1 = col1.rgb *_SubColor.rgb *_SubBright ; 
                        float    a  = col.a  *_MainColor.a *_MainBright ; 
                        float    a1 = col1.a * _SubColor.a *_SubBright ; 
                        result = float4((rgb * rgb1 )*ling_vertercolor.rgb , (a+a1)*ling_vertercolor.a );
                    #endif 


                #elif _ABCTEX_ADDTWOTEXALPHAA
                    #ifdef _DISTORT_ON 
                        float2 t1 = ling_uv0 + _Time.xz*_HeatTime;
                        float2 t2 = ling_uv0 + _Time.yx*_HeatTime;
                        fixed4 offsetcolor1 = tex2D(_MainTex, t1);
                        fixed4 offsetcolor2 = tex2D(_MainTex, t2);
                        ling_uv0.x += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaX;
                        ling_uv0.y += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaY;



                        float4 col1 = tex2D (_SubTex , ling_uv0 );
                        float    a1 = col1.a * _SubColor.a *_SubBright; 
                        result = float4((col1.rgb )*ling_vertercolor.rgb*_SubColor.rgb , (a1)*ling_vertercolor.a );
                    #else 

                        float4 col = tex2D(_MainTex, ling_uv0);
                        float4 col1 = tex2D (_SubTex , ling_uv1 );
                        float3  rgb = col.rgb  *_MainColor.rgb *_MainBright ;
                        float3  rgb1 = col1.rgb *_SubColor.rgb *_SubBright ; 
                        float    a  = col.a  *_MainColor.a *_MainBright ; 
                        float    a1 = col1.a * _SubColor.a *_SubBright ; 
                        result = float4((rgb + rgb1 )*ling_vertercolor.rgb , (a+a1)*ling_vertercolor.a );
                    #endif 

                #elif _ABCTEX_MULTWOTEXALPHAR 

                    #ifdef _DISTORT_ON 
                        float2 t1 = ling_uv0 + _Time.xz*_HeatTime;
                        float2 t2 = ling_uv0 + _Time.yx*_HeatTime;
                        fixed4 offsetcolor1 = tex2D(_MainTex, t1);
                        fixed4 offsetcolor2 = tex2D(_MainTex, t2);
                        ling_uv0.x += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaX;
                        ling_uv0.y += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaY;


                        float4 col1 = tex2D (_SubTex , ling_uv0 );
                        float    a1 = col1.r * _SubColor.a* _SubBright; 
                        result = float4((col1.rgb )*ling_vertercolor.rgb *_SubColor.rgb , (a1)*ling_vertercolor.a );
                    #else 

                        float4 col = tex2D(_MainTex, ling_uv0);
                        float4 col1 = tex2D (_SubTex , ling_uv1 );

                        float3  rgb = col.rgb  *_MainColor.rgb *_MainBright ;
                        float3  rgb1 = col1.rgb *_SubColor.rgb *_SubBright ; 

                        float    a  = col.r  *_MainColor.a *_MainBright; 
                        float    a1 = col1.r * _SubColor.a *_SubBright; 

                        result = float4((rgb * rgb1 )*ling_vertercolor.rgb , (a+a1)*ling_vertercolor.a );
                    #endif 
                #elif _ABCTEX_ADDTWOTEXALPHAR
                    #ifdef _DISTORT_ON 
                        float2 t1 = ling_uv0 + _Time.xz*_HeatTime;
                        float2 t2 = ling_uv0 + _Time.yx*_HeatTime;
                        fixed4 offsetcolor1 = tex2D(_MainTex, t1);
                        fixed4 offsetcolor2 = tex2D(_MainTex, t2);
                        ling_uv0.x += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaX;
                        ling_uv0.y += ((offsetcolor1.r + offsetcolor2.r) - 1)*_ParaY;


                        float4 col1 = tex2D (_SubTex , ling_uv0 );
                        float    a1 = col1.r * _SubColor.a* _SubBright; 
                        result = float4((col1.rgb )*ling_vertercolor.rgb *_SubColor.rgb , (a1)*ling_vertercolor.a );
                    #else 

                        float4 col = tex2D(_MainTex, ling_uv0);
                        float4 col1 = tex2D (_SubTex , ling_uv1 );

                        float3  rgb = col.rgb  *_MainColor.rgb *_MainBright ;
                        float3  rgb1 = col1.rgb *_SubColor.rgb *_SubBright ; 

                        float    a  = col.r  *_MainColor.a *_MainBright; 
                        float    a1 = col1.r * _SubColor.a *_SubBright; 

                        result = float4((rgb + rgb1 )*ling_vertercolor.rgb , (a+a1)*ling_vertercolor.a );
                    #endif 
                #elif _ABCTEX_MULTWOTEXALPHAATOTHREER
                    float4 col = tex2D(_MainTex, ling_uv0);
                    float4 col1 = tex2D (_SubTex , ling_uv1 );
                    float4  col2 = tex2D (_SubMask ,ling_uv0) ; 

                    float3  rgb = col.rgb  *_MainColor.rgb *_MainBright ;
                    float3  rgb1 = col1.rgb *_SubColor.rgb *_SubBright ; 

                    float    a  = col.a  *_MainColor.a *_MainBright; 
                    float    a1 = col1.a * _SubColor.a *_SubBright; 
                    float    a2 = col2.r * _ThreetexAlphaintensty ;

                    result = float4((rgb * rgb1 )*ling_vertercolor.rgb , (a+a1+a2)*ling_vertercolor.a );


                #elif _ABCTEX_ADDTWOTEXALPHAATOTHREER
                    float4 col = tex2D(_MainTex, ling_uv0);
                    float4 col1 = tex2D (_SubTex , ling_uv1 );
                    float4  col2 = tex2D (_SubMask ,ling_uv0) ; 

                    float3  rgb = col.rgb  *_MainColor.rgb *_MainBright ;
                    float3  rgb1 = col1.rgb *_SubColor.rgb *_SubBright ; 

                    float    a  = col.a  *_MainColor.a *_MainBright; 
                    float    a1 = col1.a * _SubColor.a *_SubBright; 
                    float    a2 = col2.r * _ThreetexAlphaintensty ;

                    result = float4((rgb + rgb1 )*ling_vertercolor.rgb , (a+a1+a2)*ling_vertercolor.a );
                #endif 
                
                #ifdef  _DISTANCEMASK_ON 

                    result.a *= (i.vpos.x >= _MinMax.x);
                    result.a  *= (i.vpos.x <= _MinMax.y);
                    result.a  *= (i.vpos.y >= _MinMax.z);
                    result.a  *= (i.vpos.y <= _MinMax.w);
                #endif 

                #ifdef _DISSOVE_FIRE_ON 
                    float4  dismask  = tex2D (_SubMask ,ling_uv0) ; 
                    result.rgb = ling_dissolve(dismask.r , _Amount ,_StartAmount, result.rgb , _DissColor.rgb);
                    
                #endif 

                
                #ifdef _GHOST_ON 
                    result = float4((_GhostColor.rgb *_GhostIntensty +ling_rim) , ling_vertercolor.a);
                #else
                    result = float4(result.rgb + ling_rim , result.a ) ;
                #endif  

                return result  ;
            }

            ENDCG

        }

    }


}

