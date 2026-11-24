
Shader "Custom/ARGlitch"
{
    Properties
    {
        _GlitchIntensity ("Glitch Intensity", Range(0, 1)) = 0
        _GlitchTime ("Glitch Time", Float) = 0
        _RGBSplit ("RGB Split", Range(0, 0.05)) = 0.015
        _NoiseAmount ("Noise Amount", Range(0, 0.3)) = 0.08
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _GlitchIntensity;
            float _GlitchTime;
            float _RGBSplit;
            float _NoiseAmount;

            float RandomNoise(float2 p)
            {
                return frac(
                    sin(dot(p, float2(12.9898, 78.233)))
                    * 43758.5453
                );
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;

                float intensity = saturate(_GlitchIntensity);

                // 수평 줄 단위의 화면 밀림
                float row = floor(uv.y * 35.0);
                float randomValue = RandomNoise(
                    float2(row, floor(_GlitchTime * 18.0))
                );

                float shift = (randomValue - 0.5)
                    * 0.08 * intensity;

                if (randomValue > 0.78)
                {
                    uv.x += shift;
                }

                // RGB 색상 분리
                float split = _RGBSplit * intensity;

                half red = SAMPLE_TEXTURE2D_X(
                    _BlitTexture, sampler_LinearClamp,
                    uv + float2(split, 0)
                ).r;

                half green = SAMPLE_TEXTURE2D_X(
                    _BlitTexture, sampler_LinearClamp, uv
                ).g;

                half blue = SAMPLE_TEXTURE2D_X(
                    _BlitTexture, sampler_LinearClamp,
                    uv - float2(split, 0)
                ).b;

                half3 color = half3(red, green, blue);

                // 디지털 노이즈
                float noise = RandomNoise(
                    floor(uv * 900.0) + _GlitchTime
                );

                color += (noise - 0.5)
                    * _NoiseAmount * intensity;

                half4 original = SAMPLE_TEXTURE2D_X(
                    _BlitTexture, sampler_LinearClamp,
                    input.texcoord
                );

                color = lerp(original.rgb, color, intensity);

                return half4(color, original.a);
            }

            ENDHLSL
        }
    }
}
