﻿Shader "Custom/Waves"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Tint("Colour Tint", Color) = (1,1,1,1)
		_Freq("Frequency", Range(0,5)) = 3
		_Speed("Speed", Range(0,100)) = 10
		_Amp("Amplitude", Range(0,1)) = 0.5
		_Fade("Fade",Range(0.0,1.0)) = 1.0
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        

        struct Input
        {
            float2 uv_MainTex;
			float3 vertColor; // Adding tint based on the vertices
        };

		float4 _Tint;
		float _Freq;
		float _Speed;
		float _Amp;
		float _Fade;
		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord: TEXCOORD0;
		};

		void vert(inout appdata v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float t = _Time.x * _Speed;
			float waveHeight = sin(t + v.vertex.x * _Freq) * _Amp;

			v.vertex.y += waveHeight;
			v.normal = normalize(float3(v.normal.x + waveHeight, v.normal.y, v.normal.z));
			o.vertColor = waveHeight + 1;

		}
		sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			c.a *= _Fade;

			o.Albedo = c * IN.vertColor.rgb;
			o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
