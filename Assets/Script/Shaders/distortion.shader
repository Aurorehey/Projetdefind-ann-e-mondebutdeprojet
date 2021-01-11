Shader "Interface3/Shaders 4/Distortion Glass"
{
	SubShader
	{
		Tags {
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		GrabPass {
			"_GrabTexture"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture;

			struct vertInput {
				float4 pos : POSITION;
			};

			struct vertOutput {
				float4 pos : SV_POSITION;
				float2 uvgrab : TEXCOORD1;
			};

			// Vertex Shader
			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = UnityObjectToClipPos(input.pos);
				return o;
			}

			// Fragment Shader
			half4 frag(vertOutput output) : COLOR {
				return half4(1, 1, 1, 0);
			}

            ENDCG
        }
    }
}