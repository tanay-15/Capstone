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
		_CelRamp("Cel shading ramp", 2D) = "white" {}
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
		Lighting On
		ZWrite Off
		Fog{ Mode Off }
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
		sampler2D _CelRamp;
		float _rimPower;


		struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 viewDir;
		fixed4 color;
	};

		half4 LightingCustomLambert(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);
			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * (tex2D(_CelRamp, half2 (NdotL * 0.5 + 0.5, 0)))) * (atten * 2);
			c.a = s.Alpha;
			return c;
		}
	void vert(inout appdata_full v, out Input o)
	{
#if defined(PIXELSNAP_ON)
		v.vertex = UnityPixelSnap(v.vertex);
#endif
		/*v.normal = float3(0, 0, -1);
		v.tangent = float4(-1, 0, 0, 1);*/
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = v.color * _Color;
	}

	
	void surf(Input IN, inout SurfaceOutput o)
	{
		/*fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
		o.Albedo = c.rgb * c.a;
		o.Alpha = c.a;*/
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) *IN.color;
		o.Albedo = c.rgb;
		
		//o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb *IN.color;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		o.Alpha = c.a;
		//half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
		//o.Emission = _rimColor.rgb * pow(rim,_rimPower);
		//o.Albedo = (tex2D(_MainTex, IN.uv_MainTex)).rgb;
		//o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		//o.Albedo = _myColor.rgb;
		//o.Emission = _myEmission.rgb;
	}
	ENDCG

	}
		FallBack "Diffuse"
}
