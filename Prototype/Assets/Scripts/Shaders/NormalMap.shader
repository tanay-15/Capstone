Shader "Custom/NormalMap"
{
	Properties
	{
	_MainTex("Sprite Texture", 2D) = "white" {}
	_NormalMap("Normal Map",2D) = "normal"{}
	_Color("Tint", Color) = (1,1,1,1)
	[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "false"
		"RenderType" = "TransparentCutOut"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}
		Cull Off
		Lighting On
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		CGPROGRAM
#pragma surface surf Lambert vertex:vert nofog alpha:fade
#pragma multi_compile _ PIXELSNAP_ON
//#include "UnitySprites.cginc"

	sampler2D _MainTex;
	sampler2D _NormalMap;
	fixed4 _Color;
	struct Input
	{
		float2 uv_MainTex;
		float2 uv_NormalMap;
		fixed4 color;
	};


	void vert(inout appdata_full v, out Input o)
	{

#if defined(PIXELSNAP_ON) 
		v.vertex = UnityPixelSnap(v.vertex);
#endif
		/*v.normal = float3(0, 0, -1);
		v.tangent = float4(1, 0, 0, 1);*/
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = v.color *_Color;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
		o.Albedo = c.rgb;
		
		o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
		o.Alpha = c.a;
	}
	ENDCG
	}
		Fallback "Diffuse"

}