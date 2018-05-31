// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skatebrus/Black Lid" {
	Properties {
		_BgColor("Background", COLOR) = (0,0,0,1)
		_CoColor("Cutout", COLOR) = (1,1,1,0)
		_AlphaCutoff("Alpha Cutoff", Range(0, 1)) = 0.6
	}
	SubShader {
		Tags { 
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
		}
		//LOD 100

		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
			};

			fixed4 _BgColor;
			fixed4 _CoColor;
			half _AlphaCutoff;
			uniform sampler2D _GlobalLidTexture;
			
			v2f vert(appdata v) {
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				half alpha = tex2D(_GlobalLidTexture, i.uv).a;
				
				if (alpha >= _AlphaCutoff)
					return _CoColor;

				return _BgColor;
			}
			ENDCG
		}
	}
}
