Shader "CustomShaders/Illumination"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		_rimColor("Rim Color", Color) = (0,0.5,0.5,0)
		_rimPower("Rim Power",Range(0.5,8.0)) = 3.0
		_myEmission("Example Emission", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	_BumpMap("Bumpmap", 2D) = "bump" {}
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
		#pragma surface surf Lambert vertex:vert nofog keepalpha
		#pragma multi_compile _ PIXELSNAP_ON
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		float4 _rimColor;
		fixed4 _myEmission;
		fixed4 _Color;
		sampler2D _BumpMap;
		sampler2D _MainTex;
		sampler2D _AlphaTex;
		float _rimPower;


		struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 viewDir;
		fixed4 color;
	};

	void vert(inout appdata_full v, out Input o)
	{
#if defined(PIXELSNAP_ON)
		v.vertex = UnityPixelSnap(v.vertex);
#endif

		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = v.color * _Color;
	}

	fixed4 SampleSpriteTexture(float2 uv)
	{
		fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
		color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

		return color;
	}
	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
		o.Albedo = c.rgb * c.a;
		o.Alpha = c.a;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
		o.Emission = _rimColor.rgb * pow(rim,_rimPower);
		//o.Albedo = (tex2D(_MainTex, IN.uv_MainTex)).rgb;
		//o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		//o.Albedo = _myColor.rgb;
		//o.Emission = _myEmission.rgb;
	}
	ENDCG

	}
		FallBack "Diffuse"
}
