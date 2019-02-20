Shader "CustomShaders/Illumination"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		//_rimColor("Rim Color", Color) = (0,0.5,0.5,0)
		//_rimPower("Rim Power",Range(0.5,8.0)) = 3.0
		//_myEmission("Example Emission", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_BumpIntensity("NormalMap Intensity", Range(-1, 2)) = 1
		_BumpIntensity("NormalMap Intensity", Float) = 1
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		
	}
		SubShader
	{
		Tags
	{
		"Queue" = "AlphaTest"
		"IgnoreProjector" = "True"
		"RenderType" = "TransparentCutOut"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}
		LOD 300
		Cull Off
		Lighting On
		ZWrite On
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha
		CGPROGRAM
		//#pragma surface surf Lambert vertex:vert nofog keepalpha
		#pragma surface surf Lambert alpha vertex:vert  alphatest:_Cutoff fullforwardshadows
		#pragma multi_compile _ PIXELSNAP_ON
		//#pragma exclude_renderers flash
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		//float4 _rimColor;
		//fixed4 _myEmission;
		fixed4 _Color;
		sampler2D _BumpMap;
		sampler2D _MainTex;	
		fixed _BumpIntensity;
		//float _rimPower;


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
	
		v.normal = float3(0, 0, -1);
		v.tangent = float4(-1, 0, 0, 1);
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = v.color * _Color;
	}

	
	void surf(Input IN, inout SurfaceOutput o)
	{
		
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) *IN.color;
		o.Albedo = c.rgb;	
		o.Alpha = c.a;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		_BumpIntensity = 1 / _BumpIntensity;
		o.Normal.z = o.Normal.z * _BumpIntensity;
		o.Normal = normalize((half3)o.Normal);
		/*half rim = 1 - dot(normalize(IN.viewDir), o.Normal);
		o.Emission = _rimColor.rgb*rim;*/
		
		
	}
	ENDCG

	}
		//FallBack "Diffuse"
		FallBack"Transparent/VertexLit"
		
}
