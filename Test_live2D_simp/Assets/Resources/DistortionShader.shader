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

			float mouse_x;
			float mouse_y;

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
				float center=-0.7;//center of the head,in case 1_HM,center is -0.5; 3_HM : 0.5; 4_HM : -0.7; 
				float dx = mouse_x - (v.vertex.x-center); 
				float dy = mouse_y - v.vertex.y;
				float dist = 0.9f*smoothstep(-4.0,4.0,sqrt(dx*dx + dy*dy));		// linear
				v.vertex.x += sqrt(v.vertex.z) * (sqrt(abs(v.vertex.x-center))+0.1)/ 25.0f*-(dx); //make face more "cylinder"
				v.vertex.x += dx / 15.0f; 
				v.vertex.y += ((v.vertex.z+0.1) * dy*abs(dy)/dist) / 90.0f; //add a coe 0.1 to make thing more nature
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
