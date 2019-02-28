Shader "Unlit/Godrays"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color1("Color1",Color) = (0.1,1,1,1)
		_Color2("Color2",Color) = (0,0.46,1,0)
		_Speed("Speed",Range(0,5.0)) = 0.5
		_Size("Size",Range(1.0,30.0)) = 15.0
		_Fade("Fade",Range(0.0,1.0)) = 1.0
		_Direction("Direction", Range(-1,1)) = 0
		_Contrast("Contrast",Range(0.0,30.0)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
     /*       #pragma multi_compile_fog*/

            #include "UnityCG.cginc"

		float4 permute(float4 x) 
		{
			return (34.0* x * x + x) % 289.0;
		}

		float2 fade(float2 t) 
		
		{
			return t * t * t *(t*(t*6.0 - 15.0) + 10.0); // 6tpow(5) - 15tpow(4) - 10t(3)
		}

		float4 taylorInvSqrt(float4 r) 
		{
			return 1.79284291400159 - 0.85373472095314*r;
		}
		float PerlinNoise2D(float2 P) {
			float4 Pi = floor(P.xyxy) +float4(0.0,0.0,1.0,1.0); 
			float4 Pf = frac(P.xyxy) -float4(0.0, 0.0, 1.0, 1.0);
			float4 ix = Pi.xzxz;
			float4 iy = Pi.yyww;
			float4 fx = Pf.xzxz;
			float4 fy = Pf.yyww;
			float4 i = permute(permute(ix) + iy);
			float4 gx = frac(i / 41.0)*2.0 - 1.0;
			float4 gy = abs(gx) - 0.5;
			float4 tx = floor(gx + 0.5);
			gx = gx - tx;
			float2 g00 = float2(gx.x,gy.x);
			float2 g10 = float2(gx.y,gy.y);
			float2 g01 = float2(gx.z,gy.z);
			float2 g11 = float2(gx.w,gy.w);
			float4 norm = taylorInvSqrt(float4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
			g00 *= norm.x;
			g01 *= norm.y;
			g10 *= norm.z;
			g11 *= norm.w;
			float n00 = dot(g00,float2(fx.x,fy.x));
			float n10 = dot(g10,float2(fx.y,fy.y));
			float n01 = dot(g01,float2(fx.z,fy.z));
			float n11 = dot(g11,float2(fx.w,fy.w));
			float2 fade_xy = fade(Pf.xy);
			float2 n_x = lerp(float2(n00, n01),float2(n10, n11),fade_xy.x);
			float n_xy = lerp(n_x.x,n_x.y,fade_xy.y);
			return 2.3*n_xy;
		}
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 color: COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color1;
			fixed4 _Color2;
			float _Speed;
			float _Size;
			float _Fade;
			float _Contrast;
			float _Direction;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				/*o.uv.xy = v.uv.xy + frac(_Time.y * float2(_ScrollX, _ScrollY));*/
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
			fixed4 color = lerp(_Color1,_Color2, 1-i.uv.y);
			fixed nPosition = i.uv.x;
			nPosition += 0.5;
			nPosition *= _Size;
			nPosition +=  i.uv.y*(_Size*_Direction);
			fixed value = PerlinNoise2D(float2(nPosition, _Time.y*_Speed))/2+ 0.5f;
			value = _Contrast * (value - 0.5) +0.5;
			color.a *= lerp(value, value*i.uv.y,_Fade + 1);  // lerp between uv.x or y depending on which axis you want fading in. 
			color.a = saturate(color.a);	//clamping between 0 & 1.
			return color;
            }
            ENDCG
        }
    }
}
