// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skatebrus/Draw Simple" {
	SubShader {
		ZWrite Off
		ZTest Always
		Lighting Off
		
		Pass {
			CGPROGRAM
			#pragma vertex VShader
			#pragma fragment FShader

			struct VertexToFragment {
				float4 pos:POSITION;
			};

			VertexToFragment VShader(VertexToFragment i) {
				VertexToFragment o;
				o.pos = UnityObjectToClipPos(i.pos);
				return o;
			}

			half4 FShader() : COLOR0 {
				return half4(1,1,1,1);
			}

			ENDCG
		}
	}
}