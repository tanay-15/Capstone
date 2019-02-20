Shader "Custom/NormalMap"
{
	Properties
	{
		_SpriteTex("Sprite Texture", 2D) = "white" {}
	_NormalMap("Normal Map",2D) = "normal"{}
	_Color("Color", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "False"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"

	sampler2D _SpriteTex;
	sampler2D _NormalMap;
	//fixed4 _Color;
	struct Input
	{
		float2 uv_MainTex;
		float2 uv_NormalMap;
		fixed4 color;
	};


	void vert(inout appdata_full v, out Input o)
	{
		v.vertex = UnityFlipSprite(v.vertex, _Flip);

#if defined(PIXELSNAP_ON) 
		v.vertex = UnityPixelSnap(v.vertex);
#endif
		v.normal = float3(0, 0, -1);
		v.tangent = float4(1, 0, 0, 1);
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = _Color;// *v.color;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_SpriteTex, IN.uv_MainTex) * IN.color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
		o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));

	}
	ENDCG
	}

		Fallback "Transparent/VertexLit"
}