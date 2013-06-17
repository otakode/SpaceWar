Shader "Custom/StarShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_coef("Coef", Float) = 0
		_alpha("Alpha", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _coef;
		float _alpha;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = saturate(half3(c.r + _coef, c.g + _coef, c.b + _coef));
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
