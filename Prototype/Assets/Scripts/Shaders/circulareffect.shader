Shader "Custom/CircularEffect"
{
    Properties
    {
	   _MainTex("Texture", 2D) = "white" {}
		_Radius("Radius", Range(0.0, 1.0)) = 1.0
		_Soft("Softness", Range(0.0, 1.0)) = 0.5
		_Color("Color",Color) = (0,0.46,1,0)
		_Intensity("Intensity", Range(0.0,1.0)) = 0.6
    }
    SubShader
	{
	 Pass
	 {

	  CGPROGRAM
	  #pragma vertex vert_img
	  #pragma fragment frag
	  #include "UnityCG.cginc" // required for v2f_img
		// Properties
		sampler2D _MainTex;
		fixed4 _Color;
		float _Radius;
		float _Soft;
		float _Intensity;
		fixed4 frag(v2f_img input) : COLOR
		{
		// sample texture for color
		float4 base = tex2D(_MainTex, input.uv);
		float distFromCenter = distance(input.uv.xy, float2(0.5, 0.5));
		//float circularEffect = 1 - distFromCenter;
		//float circularEffect = smoothstep(_Radius, _Radius - _Soft, distFromCenter) *_Intensity ;
		
		float circularEffect = smoothstep( _Radius - _Soft, _Radius, distFromCenter) *_Intensity; //note inner and outer swapped to switch darkness to opacity
		//base = saturate(base * circularEffect);
		base.rgb = lerp(base.rgb, _Color, circularEffect);
		//return float4(distFromCenter, distFromCenter, distFromCenter, 1.0);
		return base;
		}
		ENDCG
	 }

    }
    FallBack "Diffuse"
}
