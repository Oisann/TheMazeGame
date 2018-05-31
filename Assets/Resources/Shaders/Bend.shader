// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33200,y:32789,varname:node_3138,prsc:2|emission-2107-RGB,voffset-9140-OUT;n:type:ShaderForge.SFN_TexCoord,id:1530,x:31925,y:32826,varname:node_1530,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Lerp,id:9140,x:32642,y:33056,varname:node_9140,prsc:2|A-1530-UVOUT,B-7089-R,T-2213-OUT;n:type:ShaderForge.SFN_Color,id:2107,x:32757,y:32662,ptovrint:False,ptlb:node_2107,ptin:_node_2107,varname:node_2107,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5172414,c3:0,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:8287,x:31593,y:33030,varname:node_8287,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:135,x:31783,y:33030,varname:node_135,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8287-XYZ;n:type:ShaderForge.SFN_Tex2d,id:7089,x:32138,y:33036,ptovrint:False,ptlb:node_7089,ptin:_node_7089,varname:node_7089,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4a11d65ce13d5f542a0ff136cc2f3fba,ntxv:3,isnm:True|UVIN-135-OUT;n:type:ShaderForge.SFN_Time,id:5346,x:31619,y:33387,varname:node_5346,prsc:2;n:type:ShaderForge.SFN_Cos,id:7414,x:31875,y:33417,varname:node_7414,prsc:2|IN-5346-T;n:type:ShaderForge.SFN_Noise,id:3211,x:32276,y:33251,varname:node_3211,prsc:2|XY-8072-OUT;n:type:ShaderForge.SFN_RemapRange,id:2213,x:32305,y:33434,varname:node_2213,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:0.3|IN-4358-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8072,x:32066,y:33241,varname:node_8072,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7089-RGB;n:type:ShaderForge.SFN_Sin,id:4358,x:32066,y:33452,varname:node_4358,prsc:2|IN-5346-T;proporder:2107-7089;pass:END;sub:END;*/

Shader "Shader Forge/Bend" {
    Properties {
        _node_2107 ("node_2107", Color) = (1,0.5172414,0,1)
        _node_7089 ("node_7089", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _node_2107;
            uniform sampler2D _node_7089; uniform float4 _node_7089_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float2 node_135 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float3 _node_7089_var = UnpackNormal(tex2Dlod(_node_7089,float4(TRANSFORM_TEX(node_135, _node_7089),0.0,0)));
                float4 node_5346 = _Time + _TimeEditor;
                float node_2213 = (sin(node_5346.g)*0.15+0.15);
                v.vertex.xyz += float3(lerp(o.uv0,float2(_node_7089_var.r,_node_7089_var.r),node_2213),0.0);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float3 emissive = _node_2107.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
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
            uniform sampler2D _node_7089; uniform float4 _node_7089_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float2 node_135 = mul(unity_ObjectToWorld, v.vertex).rgb.rg;
                float3 _node_7089_var = UnpackNormal(tex2Dlod(_node_7089,float4(TRANSFORM_TEX(node_135, _node_7089),0.0,0)));
                float4 node_5346 = _Time + _TimeEditor;
                float node_2213 = (sin(node_5346.g)*0.15+0.15);
                v.vertex.xyz += float3(lerp(o.uv0,float2(_node_7089_var.r,_node_7089_var.r),node_2213),0.0);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
