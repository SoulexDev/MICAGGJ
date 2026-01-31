Shader "Custom/RetroStandard"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [SingleLineTexture] _MainTex ("Albedo", 2D) = "white" {}
        [SingleLineTexture] _NormalMap ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Simple noshadow
        #pragma target 4.0

        half4 _Color;
        sampler2D _MainTex;
        sampler2D _NormalMap;

        struct Input
        {
            float2 uv_MainTex;
        };

        half4 LightingSimple(SurfaceOutput s, UnityGI gi)
        {
            half3 lightDir = normalize(gi.light.dir);
            half nl = dot(s.Normal, lightDir) * 0.5 + 0.5;

            half3 color = s.Albedo * gi.light.color * nl + gi.indirect.diffuse * s.Albedo;

            return half4(color, 1);
        }
        inline void LightingSimple_GI (SurfaceOutput s, UnityGIInput data, inout UnityGI gi)
        {
            #if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
                gi = UnityGlobalIllumination(data, 1, s.Normal);
            #else
                Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(0, data.worldViewDir, s.Normal, 0);
                gi = UnityGlobalIllumination(data, 1, s.Normal, g);
            #endif
        }
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex)).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
