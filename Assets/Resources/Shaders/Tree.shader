// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33265,y:32680,varname:node_4013,prsc:2|diff-2165-OUT,voffset-7765-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32462,y:32594,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2dAsset,id:50,x:32462,y:32944,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_50,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4168,x:32774,y:32814,varname:node_4168,prsc:2,ntxv:0,isnm:False|UVIN-253-UVOUT,TEX-50-TEX;n:type:ShaderForge.SFN_TexCoord,id:253,x:32462,y:32759,varname:node_253,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:2165,x:32943,y:32673,varname:node_2165,prsc:2|A-1304-RGB,B-4168-RGB;n:type:ShaderForge.SFN_TexCoord,id:2077,x:32278,y:33210,varname:node_2077,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Slider,id:4122,x:32121,y:33399,ptovrint:False,ptlb:Height Gradient,ptin:_HeightGradient,varname:node_4122,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Subtract,id:686,x:32577,y:33311,varname:node_686,prsc:2|A-2077-V,B-4122-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:6295,x:32616,y:33090,varname:node_6295,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6781,x:32849,y:33142,varname:node_6781,prsc:2|A-6295-Y,B-8592-OUT;n:type:ShaderForge.SFN_Time,id:2543,x:32894,y:33565,varname:node_2543,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7326,x:33190,y:33392,varname:node_7326,prsc:2|A-6781-OUT,B-4111-OUT;n:type:ShaderForge.SFN_Sin,id:4111,x:33092,y:33565,varname:node_4111,prsc:2|IN-2543-T;n:type:ShaderForge.SFN_Slider,id:7660,x:32458,y:33633,ptovrint:False,ptlb:Gradient Weakness,ptin:_GradientWeakness,varname:node_7660,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:2,max:10;n:type:ShaderForge.SFN_Divide,id:8592,x:32776,y:33387,varname:node_8592,prsc:2|A-686-OUT,B-7660-OUT;n:type:ShaderForge.SFN_Multiply,id:2039,x:33542,y:33412,varname:node_2039,prsc:2|A-7326-OUT,B-7579-OUT;n:type:ShaderForge.SFN_Vector4Property,id:6767,x:33260,y:33792,ptovrint:False,ptlb:Movement Vector,ptin:_MovementVector,varname:node_6767,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_ComponentMask,id:7579,x:33488,y:33712,varname:node_7579,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-6767-XYZ;n:type:ShaderForge.SFN_Multiply,id:7765,x:33294,y:33197,varname:node_7765,prsc:2|A-3902-OUT,B-2039-OUT;n:type:ShaderForge.SFN_Multiply,id:3902,x:32954,y:32985,varname:node_3902,prsc:2|A-7561-OUT,B-6295-XYZ;n:type:ShaderForge.SFN_Vector1,id:7561,x:32681,y:32966,varname:node_7561,prsc:2,v1:0.1;proporder:1304-50-4122-7660-6767;pass:END;sub:END;*/

Shader "Skatebrus/Basic Lit/Tree" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _HeightGradient ("Height Gradient", Range(0, 1)) = 0.5
        _GradientWeakness ("Gradient Weakness", Range(1, 10)) = 2
        _MovementVector ("Movement Vector", Vector) = (0,0,0,0)
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
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _HeightGradient;
            uniform float _GradientWeakness;
            uniform float4 _MovementVector;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_2543 = _Time + _TimeEditor;
                v.vertex.xyz += ((0.1*mul(unity_ObjectToWorld, v.vertex).rgb)*(((mul(unity_ObjectToWorld, v.vertex).g*((o.uv0.g-_HeightGradient)/_GradientWeakness))*sin(node_2543.g))*_MovementVector.rgb.rgb));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_4168 = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuseColor = (_Color.rgb*node_4168.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _HeightGradient;
            uniform float _GradientWeakness;
            uniform float4 _MovementVector;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_2543 = _Time + _TimeEditor;
                v.vertex.xyz += ((0.1*mul(unity_ObjectToWorld, v.vertex).rgb)*(((mul(unity_ObjectToWorld, v.vertex).g*((o.uv0.g-_HeightGradient)/_GradientWeakness))*sin(node_2543.g))*_MovementVector.rgb.rgb));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_4168 = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuseColor = (_Color.rgb*node_4168.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
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
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _HeightGradient;
            uniform float _GradientWeakness;
            uniform float4 _MovementVector;
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
                float4 node_2543 = _Time + _TimeEditor;
                v.vertex.xyz += ((0.1*mul(unity_ObjectToWorld, v.vertex).rgb)*(((mul(unity_ObjectToWorld, v.vertex).g*((o.uv0.g-_HeightGradient)/_GradientWeakness))*sin(node_2543.g))*_MovementVector.rgb.rgb));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
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
