Shader "Custom/NormalMap"
{
	Properties
	{
	_MainTex("Sprite Texture", 2D) = "white" {}
	_NormalMap("Normal Map",2D) = "normal"{}
	_BumpIntensity("NormalMap Intensity", Range(-1, 2)) = 1
	[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "TransparentCutOut"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}
		Cull Off
		Lighting On
		ZWrite On
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		CGPROGRAM
#pragma surface surf Lambert vertex:vert alpha  fullforwardshadows
#pragma multi_compile _ PIXELSNAP_ON
		//#include "UnitySprites.cginc"

			sampler2D _MainTex;
			sampler2D _NormalMap;
			fixed _BumpIntensity;
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

				v.tangent = float4(-1, 0, 0, 1);
				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.color = v.color;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
				_BumpIntensity = 1 / _BumpIntensity;
				o.Normal.z = o.Normal.z * _BumpIntensity;
				o.Normal = normalize(o.Normal);

			}
			ENDCG
	}
		Fallback "Diffuse"

}