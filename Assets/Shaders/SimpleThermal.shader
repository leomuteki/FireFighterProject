Shader "Custom/SimpleThermal" {
	Properties {
		_MaxDist ("Max Distance", Float) = 2.0
		_Conductivity ("Conductivity", Range(0, 100)) = 1
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		float _MaxDist;
		half _Conductivity;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
			
		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		uniform float _HeatSources_Length;
		uniform float _TempCap;
		uniform float _Brightness;
		uniform float4 _HeatSources [20];

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			// Get the average of all heat sources' influence on current pixel
			float temp = 0;
			for (int i = 0; i < _HeatSources_Length; ++i)
			{
				half dist = distance(IN.worldPos, _HeatSources[i].xyz);
				half fraction = dist / _MaxDist;
				if (fraction > 1)
				{
					continue;
				}
				// Invert brightness and multiply by source temperature
				fraction = (1 - fraction) * _HeatSources[i].a;
				temp += fraction;
			}
			if (temp > _TempCap)
			{
				temp = _TempCap;
			}
			// exponential drop-off
			temp *= temp;
			temp *= temp;
			// Map to high brightness
			temp *= _Brightness * _Conductivity;
			o.Albedo = float3(temp, temp, temp);//float3(temp, temp, temp);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}