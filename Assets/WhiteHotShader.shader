Shader "Custom/WhiteHotLit" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.0
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _Mode ("Mode", Int) = 0
        _Temp ("Temperature", Range(0, 1)) = 0.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200


        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        uniform int _Mode = 0;
        float _Temp;

        void surf(Input IN, inout SurfaceOutputStandard o) {
            switch (_Mode) {
            case 1:
                // Here I don't want shadows or lighting
                o.Albedo = fixed3(_Temp, _Temp, _Temp);
                break;
            case 2:
                // Here I don't want shadows or lighting
                o.Albedo = fixed3(1.0 - _Temp, 1.0 - _Temp, 1.0 - _Temp);
                break;
            default:
                // Here I want shadows and lighting
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
                break;
            }
        }
        ENDCG
    } 
    FallBack "Diffuse"
}