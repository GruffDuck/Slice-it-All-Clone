// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skybox/Farland Skies/Cloudy Crown" {
    
    Properties{
        [Header(Sky)]

            _TopColor("Color Top", Color) = (.247, .318, .561, 1.0)
            _BottomColor("Color Bottom", Color) = (.773, .455, .682, 1.0)

        [Header(Stars)]

            _StarsTint("Stars Tint", Color) = (.5, .5, .5, 1.0)
            _StarsExtinction("Stars Extinction", Range(0, 10)) = 2.0
            _StarsTwinklingSpeed("Stars Twinkling Speed", Range(0, 25)) = 4.0
            [NoScaleOffset]
            _StarsTex("Stars Cubemap", Cube) = "grey" {}

        [Header(Sun)]

            _SunSize("Sun Size", Range(0.1, 3)) = 1.0
            _SunTint("Sun Tint", Color) = (.5, .5, .5, 1.0)

            [NoScaleOffset]
            _SunTex("Sun Texture", 2D) = "grey" {}

        [Header(Moon)]

            _MoonSize("Moon Size", Range(0.1, 3)) = 1.0
            _MoonTint("Moon Tint", Color) = (.5, .5, .5, 1.0)

            [NoScaleOffset]
            _MoonTex("Moon Texture", 2D) = "grey" {}

        [Header(Clouds)]

            _CloudsHeight("Clouds Height", Range(-0.75, 0.75)) = 0
            _CloudsOffset("Clouds Offset", Range(0, 1)) = 0.2
            _CloudsRotationSpeed("Clouds Rotation Speed", Range(-50, 50)) = 1
            [NoScaleOffset]
            _CloudsTex("Clouds Cubemap", Cube) = "grey" {}

        [Header(General)]

            [Gamma] _Exposure("Exposure", Range(0, 10)) = 1.0
    }

    CustomEditor "CloudyCrownShaderGUI"

    SubShader{
        Tags{ "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off ZWrite Off

        Pass{
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature STARS_OFF
            #pragma shader_feature SUN_OFF
            #pragma shader_feature MOON_OFF

            #include "UnityCG.cginc"

            // Exposed
            half3 _TopColor;
            half3 _BottomColor;

            fixed _CloudsHeight;
            fixed _CloudsOffset;
            half _CloudsRotationSpeed;
            samplerCUBE _CloudsTex;

            #if !STARS_OFF
                half4 _StarsTint;
                half _StarsTwinklingSpeed;
                half _StarsExtinction;
                samplerCUBE _StarsTex;
            #endif

            #if !SUN_OFF
                half _SunSize;
                half4 _SunTint;
                sampler2D _SunTex;
                float4x4 sunMatrix;
            #endif

            #if !MOON_OFF
                half _MoonSize;
                half4 _MoonTint;
                sampler2D _MoonTex;
                float4x4 moonMatrix;
            #endif

            half _Exposure;

            // -----------------------------------------
            // Structs
            // -----------------------------------------

            struct v2f {
                float4 position : SV_POSITION;
                float3 vertex : TEXCOORD0;
                float3 cloudsPosition1 : TEXCOORD1;
                float3 cloudsPosition2 : TEXCOORD2;
                float3 cloudsPosition3 : TEXCOORD3;
                float3 cloudsPosition4 : TEXCOORD4;
                float3 cloudsPosition5 : TEXCOORD5;
                #if !STARS_OFF
                    float3 twinklingPosition : TEXCOORD6;
                #endif
                #if !SUN_OFF
                    float3 sunPosition : TEXCOORD7;
                #endif
                #if !MOON_OFF
                    float3 moonPosition : TEXCOORD8;
                #endif
            };

            // -----------------------------------------
            // Functions
            // -----------------------------------------

            float4 RotateAroundYInDegrees(float4 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return float4(mul(m, vertex.xz), vertex.yw).xzyw;
            }
            #if !STARS_OFF
                float4 RotateAroundYXInDegrees(float4 vertex, float degrees)
                {
                    float alpha = degrees * UNITY_PI / 180.0;
                    float sina, cosa;
                    sincos(alpha, sina, cosa);
                
                    float2x2 m = float2x2(cosa, -sina, sina, cosa);
                    float4 rot = float4(mul(m, vertex.xz), vertex.yw).xzyw;

                    m = float2x2(cosa, sina, -sina, cosa);
                    rot = float4(mul(m, rot.yz), rot.xw).yzxw;

                    return rot;
                }
            #endif

            #if !SUN_OFF || !MOON_OFF
                static const half4 kHaloBase = half4(1.0, 1.0, 1.0, 0);

                half4 CelestialColor(float3 position, sampler2D tex, fixed size, half4 tint) {
                    fixed depthCheck = step(position.z, 0); // equivalent of (position.z < 0)			
                    half4 sTex = tex2D(tex, position.xy / (0.5 * size) + float2(0.5, 0.5));

                    half4 halo = 1.0 - smoothstep(0, 0.35 * size, length(position.xy));
                    sTex = sTex.r + (kHaloBase + 1.75 * halo * halo) * sTex.b;

                    tint.rgb = unity_ColorSpaceDouble.rgb * tint.rgb;

                    return depthCheck * sTex * tint;
                }
            #endif

            v2f vert(appdata_base v)
            {
                v2f OUT;

                // General
                OUT.position = UnityObjectToClipPos(v.vertex);
                OUT.vertex = v.vertex;
                
                // Stars
                #if !STARS_OFF
                    OUT.twinklingPosition = RotateAroundYXInDegrees(v.vertex, _Time.y * _StarsTwinklingSpeed);
                #endif

                // Sun
                #if !SUN_OFF
                    OUT.sunPosition = mul(sunMatrix, v.vertex);
                #endif

                // Moon
                #if !MOON_OFF
                    OUT.moonPosition = mul(moonMatrix, v.vertex);
                #endif

                // Clouds
                OUT.cloudsPosition1 = RotateAroundYInDegrees(v.vertex, 2.00 * _CloudsRotationSpeed * _Time.y);
                OUT.cloudsPosition1.y -= _CloudsHeight - 1.275 * _CloudsOffset;

                OUT.cloudsPosition2 = RotateAroundYInDegrees(v.vertex, 1.25 * _CloudsRotationSpeed * _Time.y + 72);
                OUT.cloudsPosition2.y -= _CloudsHeight - 0.600 * _CloudsOffset;

                OUT.cloudsPosition3 = RotateAroundYInDegrees(v.vertex, 0.75 * _CloudsRotationSpeed * _Time.y + 144);
                OUT.cloudsPosition3.y -= _CloudsHeight;

                OUT.cloudsPosition4 = RotateAroundYInDegrees(v.vertex, 0.40 * _CloudsRotationSpeed * _Time.y + 216);
                OUT.cloudsPosition4.y -= _CloudsHeight + 0.500 * _CloudsOffset;

                OUT.cloudsPosition5 = RotateAroundYInDegrees(v.vertex, 0.25 * _CloudsRotationSpeed * _Time.y + 288);
                OUT.cloudsPosition5.y -= _CloudsHeight + 0.950 *_CloudsOffset;

                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {				
                half3 color = _TopColor;

                // Stars
                #if !STARS_OFF
                    half starsVal = texCUBE(_StarsTex, IN.vertex).r;
                    half twinklingVal = texCUBE(_StarsTex, IN.twinklingPosition).b;
                    half extinction = saturate((IN.vertex.y - _CloudsHeight - _CloudsOffset) * _StarsExtinction);
                    half starsCoef = starsVal * _StarsTint.a * extinction * twinklingVal;
                    color = color * (1 - starsCoef) + (_StarsTint.rgb * unity_ColorSpaceDouble.rgb) * starsCoef;
                #endif

                // Sun
                #if !SUN_OFF
                    half4 sunColor = CelestialColor(IN.sunPosition, _SunTex, _SunSize, _SunTint);
                    color = lerp(color, sunColor.rgb, sunColor.a);
                #endif

                // Moon
                #if !MOON_OFF
                    half4 moonColor = CelestialColor(IN.moonPosition, _MoonTex, _MoonSize, _MoonTint);
                    color = lerp(color, moonColor.rgb, moonColor.a);
                #endif

                // Clouds
                half3 cloudsTex = texCUBE(_CloudsTex, IN.cloudsPosition5);
                color = cloudsTex.r * lerp(_BottomColor, _TopColor, 0.8) + cloudsTex.b * color;

                cloudsTex = texCUBE(_CloudsTex, IN.cloudsPosition4);
                color = cloudsTex.r * lerp(_BottomColor, _TopColor, 0.6) + cloudsTex.b * color;

                cloudsTex = texCUBE(_CloudsTex, IN.cloudsPosition3);
                color = cloudsTex.r * lerp(_BottomColor, _TopColor, 0.4) + cloudsTex.b * color;

                cloudsTex = texCUBE(_CloudsTex, IN.cloudsPosition2);
                color = cloudsTex.r * lerp(_BottomColor, _TopColor, 0.2) + cloudsTex.b * color;

                cloudsTex = texCUBE(_CloudsTex, IN.cloudsPosition1);
                color = cloudsTex.r * _BottomColor + cloudsTex.b * color;
                
                // General
                color *= _Exposure;

                return half4(color, 1);
            }
            ENDCG
        }
    }
}