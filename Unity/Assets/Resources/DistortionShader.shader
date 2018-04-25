// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DistortionShader" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//mag_disort("Shift_Distortion", float) = 0.0f
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "PreviewType" = "Plane"}
		LOD 100
		ZWrite Off
		Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _MainTex_ST;

			float command1;
			float command2;

			struct v2f
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float3 normal : NORMAL;
			};

			v2f vert(v2f v)
			{
				v2f o;

				// Distortion
				// blink
				if (v.normal.z > 1.0f){
					v.vertex.x += command2*v.normal.x;
					v.vertex.y += command2*v.normal.y;
				}

				// breath
				if (v.normal.z < 0.5f){
					v.vertex.x += command1*v.normal.x;
					v.vertex.y += command1*v.normal.y;
				}
				else{
					v.vertex.y += command1;
				}

				//o.vertex.x += comm_mag*v.normal.x;
				//o.vertex.y += comm_mag*v.normal.y;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				return col;
			}
			ENDCG
		}
	}
}
