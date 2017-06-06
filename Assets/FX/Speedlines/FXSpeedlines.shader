Shader "Hidden/FXSpeedlines" 
{
	Properties
	{
		_AlphaCutout("AlphaCutout", Range(0, 1)) = 1.0
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Pass
	{
		CGPROGRAM

#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

#define rand(x) frac(sin(x)*1e4)
#define iResolution float2(1., 1.)
#define iGlobalTime _Time.y

		float _AlphaCutout;
		uniform sampler2D _MainTex;

		float4 frag(v2f_img i) : COLOR
		{
			float2 uv = i.uv.xy / iResolution.xy;
			float2 v = uv * (1. - uv);

			uv -= .5;
			uv.x *= iResolution.x / iResolution.y;

			float3 ro = float3(0., 0., 0.);
			float3 rd = normalize(float3(uv, 1.));
			float3 dh = float3(0., 0., 1.);
			float c = 0.;

			for (float f = 0.; f < 120.; ++f)
			{
				float fr = rand(f) * 100. + iGlobalTime;
				float rr = frac(fr);
				float rb = floor(fr);

				float4 rnd = rand(rb + float4(0., 3., 5., 8.));
				float a = rnd.w * 6.2831;

				float r = 1.; //rnd.x+rnd.y;r=min(r,2.-r);
				float3 p = float3(cos(a) * r, sin(a) * r, 0.);
				float3 rop = ro - p;

				float z = lerp(50., -10., rr);
				float3 n = normalize(cross(dh,rd));
				float d = abs(dot(n,rop));
				float3 n2 = cross(rd,n);
				float t = dot(rop,n2) / dot(dh,n2);
				c += .002 / d / d * step(0.,t) * smoothstep(5.,0., abs(t - z)) * max(0., 1. - .03*t);
			}

			float3 clr = lerp(float3(1.,.1,.2), float3(1.,.4,.2), c) * c;
			clr = sqrt(clr) * pow(v.x*v.y*25., .25);

			//discard black fragments
			//clip(length(clr) - 1.);

			if (length(clr) < _AlphaCutout)
				return tex2D(_MainTex, i.uv);

			return float4(clr, 1.0f);
		}

		ENDCG
	}
	}
}