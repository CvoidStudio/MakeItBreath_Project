Shader "Unlit/LineShader"
{
	Properties{
		_MainTex("Particle Texture", 2D) = "white" {}
	}

		Category{
		Tags{ "Queue" = "Overlay" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
		Lighting Off 
		ZWrite Off 
		//Fog{ Color(0,0,0,0) }

		BindChannels{
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}

		SubShader{
		Pass{
		SetTexture[_MainTex]{
		combine texture * primary
	}
	}
	}
	}
}
