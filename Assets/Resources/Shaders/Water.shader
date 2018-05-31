// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33538,y:32809,varname:node_3138,prsc:2|emission-7695-RGB,alpha-2231-OUT,voffset-2806-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:6204,x:32468,y:32666,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_6204,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:047cf533f12732d408f84adc6b5ec367,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7695,x:32789,y:32847,varname:node_7695,prsc:2,tex:047cf533f12732d408f84adc6b5ec367,ntxv:0,isnm:False|UVIN-5797-UVOUT,TEX-6204-TEX;n:type:ShaderForge.SFN_Panner,id:5797,x:32471,y:32857,varname:node_5797,prsc:2,spu:0,spv:1|UVIN-2902-UVOUT,DIST-6571-OUT;n:type:ShaderForge.SFN_TexCoord,id:2902,x:32224,y:32857,varname:node_2902,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:9316,x:32185,y:33082,varname:node_9316,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6571,x:32433,y:33155,varname:node_6571,prsc:2|A-9316-TSL,B-7694-OUT;n:type:ShaderForge.SFN_Slider,id:7694,x:32079,y:33270,ptovrint:False,ptlb:Water Speed,ptin:_WaterSpeed,varname:node_7694,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.25,max:1;n:type:ShaderForge.SFN_ChannelBlend,id:2231,x:32961,y:33430,varname:node_2231,prsc:2,chbt:1|M-1354-OUT,R-5035-OUT,BTM-9782-OUT;n:type:ShaderForge.SFN_DepthBlend,id:7051,x:32394,y:33505,varname:node_7051,prsc:2|DIST-2289-OUT;n:type:ShaderForge.SFN_OneMinus,id:1354,x:32595,y:33462,varname:node_1354,prsc:2|IN-7051-OUT;n:type:ShaderForge.SFN_Slider,id:2289,x:32029,y:33684,ptovrint:False,ptlb:Foam Depth,ptin:_FoamDepth,varname:node_2289,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;n:type:ShaderForge.SFN_VertexColor,id:544,x:32742,y:33104,varname:node_544,prsc:2;n:type:ShaderForge.SFN_Subtract,id:5035,x:32934,y:33102,varname:node_5035,prsc:2|A-7695-A,B-544-A;n:type:ShaderForge.SFN_Vector1,id:9782,x:32725,y:33547,varname:node_9782,prsc:2,v1:1;n:type:ShaderForge.SFN_FragmentPosition,id:5509,x:31933,y:34224,varname:node_5509,prsc:2;n:type:ShaderForge.SFN_Noise,id:2528,x:32506,y:34498,varname:node_2528,prsc:2|XY-7054-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:6373,x:32130,y:34224,varname:node_6373,prsc:2,cc1:1,cc2:2,cc3:-1,cc4:-1|IN-5509-XYZ;n:type:ShaderForge.SFN_Time,id:9642,x:32130,y:34389,varname:node_9642,prsc:2;n:type:ShaderForge.SFN_Multiply,id:316,x:32862,y:34278,cmnt:Old waves,varname:node_316,prsc:2|A-1823-OUT,B-38-OUT;n:type:ShaderForge.SFN_Vector1,id:38,x:32862,y:34422,varname:node_38,prsc:2,v1:0.5;n:type:ShaderForge.SFN_TexCoord,id:7054,x:32315,y:34498,varname:node_7054,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:4734,x:32350,y:34265,varname:node_4734,prsc:2,spu:0,spv:10|UVIN-6373-OUT,DIST-9642-TTR;n:type:ShaderForge.SFN_Blend,id:1823,x:32607,y:34261,varname:node_1823,prsc:2,blmd:5,clmp:True|SRC-4734-UVOUT,DST-2528-OUT;n:type:ShaderForge.SFN_Time,id:7943,x:32264,y:33816,varname:node_7943,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4748,x:32676,y:33702,varname:node_4748,prsc:2|A-5047-OUT,B-4843-OUT;n:type:ShaderForge.SFN_Tau,id:5047,x:32486,y:33711,varname:node_5047,prsc:2;n:type:ShaderForge.SFN_Sin,id:6614,x:32877,y:33702,varname:node_6614,prsc:2|IN-4748-OUT;n:type:ShaderForge.SFN_RemapRange,id:2705,x:33095,y:33723,varname:node_2705,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-6614-OUT;n:type:ShaderForge.SFN_Multiply,id:4843,x:32508,y:33873,varname:node_4843,prsc:2|A-7943-TSL,B-5926-OUT;n:type:ShaderForge.SFN_Slider,id:5926,x:32136,y:34001,ptovrint:False,ptlb:Wave Speed,ptin:_WaveSpeed,varname:node_5926,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:10;n:type:ShaderForge.SFN_Vector3,id:4167,x:33164,y:33556,varname:node_4167,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_Multiply,id:2806,x:33520,y:33561,varname:node_2806,prsc:2|A-4167-OUT,B-8406-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:8406,x:33120,y:33918,varname:node_8406,prsc:2|IN-6614-OUT,IMIN-3982-OUT,IMAX-3116-OUT,OMIN-7676-OUT,OMAX-8493-OUT;n:type:ShaderForge.SFN_Vector1,id:3982,x:32877,y:33898,varname:node_3982,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:3116,x:32877,y:33952,varname:node_3116,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:7676,x:32767,y:34066,ptovrint:False,ptlb:Min Wave Height,ptin:_MinWaveHeight,varname:node_7676,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-1;n:type:ShaderForge.SFN_ValueProperty,id:8493,x:32767,y:34169,ptovrint:False,ptlb:Max Wave Height,ptin:_MaxWaveHeight,varname:node_8493,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:6204-7694-2289-5926-7676-8493;pass:END;sub:END;*/

Shader "Skatebrus/Unlit/Water" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _WaterSpeed ("Water Speed", Range(0, 1)) = 0.25
        _FoamDepth ("Foam Depth", Range(0, 10)) = 1
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 0.1
        _MinWaveHeight ("Min Wave Height", Float ) = -1
        _MaxWaveHeight ("Max Wave Height", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _WaterSpeed;
            uniform float _FoamDepth;
            uniform float _WaveSpeed;
            uniform float _MinWaveHeight;
            uniform float _MaxWaveHeight;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                float4 node_7943 = _Time + _TimeEditor;
                float node_6614 = sin((6.28318530718*(node_7943.r*_WaveSpeed)));
                float node_3982 = (-1.0);
                v.vertex.xyz += (float3(0,1,0)*(_MinWaveHeight + ( (node_6614 - node_3982) * (_MaxWaveHeight - _MinWaveHeight) ) / (1.0 - node_3982)));
                o.pos = UnityObjectToClipPos(v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float4 node_9316 = _Time + _TimeEditor;
                float2 node_5797 = (i.uv0+(node_9316.r*_WaterSpeed)*float2(0,1));
                float4 node_7695 = tex2D(_MainTex,TRANSFORM_TEX(node_5797, _MainTex));
                float3 emissive = node_7695.rgb;
                float3 finalColor = emissive;
                float node_1354 = (1.0 - saturate((sceneZ-partZ)/_FoamDepth));
                return fixed4(finalColor,(lerp( 1.0, (node_7695.a-i.vertexColor.a), node_1354.r )));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _WaveSpeed;
            uniform float _MinWaveHeight;
            uniform float _MaxWaveHeight;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 node_7943 = _Time + _TimeEditor;
                float node_6614 = sin((6.28318530718*(node_7943.r*_WaveSpeed)));
                float node_3982 = (-1.0);
                v.vertex.xyz += (float3(0,1,0)*(_MinWaveHeight + ( (node_6614 - node_3982) * (_MaxWaveHeight - _MinWaveHeight) ) / (1.0 - node_3982)));
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
