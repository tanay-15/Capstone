Shader "Custom/Waves"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Freq("Frequency", Range(0,20)) = 10
		_Speed("Speed", Range(0,100)) = 10
		_Amp("Amplitude", Range(0,1)) = 0.5
		_Fade("Fade",Range(0.0,1.0)) = 1.0
    }
    SubShader
    {
		Blend SrcAlpha OneMinusSrcAlpha
        //Tags { "RenderType"="Opaque" }
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		
        //LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert vertex:vert alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        

        struct Input
        {
            float2 uv_MainTex;
        };


		float _Freq;
		float _Speed;
		float _Amp;
		float _Fade;

		const float pi = 3.141592653589793238462;
		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord: TEXCOORD0;
		};

		void vert(inout appdata v)
		{
			float t = _Time.x * _Speed;
			float waveHeight = sin(t + v.vertex.x * _Freq) * (_Amp/2) + sin(t *2 + v.vertex.x * _Freq*2) * (_Amp/2);
			v.vertex.y += waveHeight;
			
			v.normal = normalize(float3(v.normal.x + waveHeight, v.normal.y, v.normal.z));
			
		}
		sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c;
			c.a *= _Fade;
			o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
