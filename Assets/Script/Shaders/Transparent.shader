Shader "Unlit/Transparent"
{
 

    SubShader
    {
        Tags { 
		"Queue"="Transparent"
		"RenderType"="Opaque" }
        

		GrabPass
		{
			"_GrabTexture"
		}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture;
			sampler2D _MainTex;
          
		  struct vertInput {   

				float4 pos : POSITION; 
				
				float2 uv :	TEXCOORD0;
		  };
		  struct vertOutput {   

				float4 pos : SV_POSITION;
				float2 uv: TEXCOORD0;
				float4 uvgrab : TEXCOORD1;
		  };
		  //Vertex Shader
		  vertOutput vert(vertInput input){
		  	  vertOutput o;
			  o.pos = UnityObjectToClipPos(input.pos); 
			  o.uvgrab = ComputeGrabScreenPos(o.pos);
			  o.uv = input.uv;
			  return o;
		  }
		  //Fragment Shader
		  half4 frag(vertOutput output):COLOR{
			  half4 background_col = tex2Dproj(_GrabTexture,output.uvgrab);

		  	  half4 texture_col = tex2D(_MainTex,output.uv);
				return background_col*texture_col;
			}

           
            ENDCG
        }
    }
}