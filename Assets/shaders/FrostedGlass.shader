Shader "BBB/Frosted Glass"
{
    Properties
    {
        [Header(Glass)]
        _GlassColor ("Glass Color", Color) = (0.82, 0.88, 1.0, 1)
        _Opacity ("Glass Strength", Range(0, 1)) = 0.75
        _Milkiness ("Milkiness", Range(0, 1)) = 0.35

        [Header(Blur)]
        _BlurSize ("Blur Size", Range(0, 150)) = 50
        _BlurSoftness ("Blur Softness", Range(0.2, 3)) = 1.2

        [Header(Distortion)]
        _Distortion ("Distortion", Range(0, 50)) = 6
        _NoiseScale ("Frost Scale", Range(1, 100)) = 18
        _NoiseStrength ("Frost Strength", Range(0, 1)) = 0.35

        [Header(Lighting)]
        _Brightness ("Brightness", Range(0, 2)) = 1
        _Contrast ("Contrast", Range(0, 2)) = 0.7

        [Header(Edge)]
        _EdgeDarkening ("Edge Darkening", Range(0, 1)) = 0.1
        _EdgeHighlight ("Edge Highlight", Range(0, 1)) = 0.25
        _EdgeSize ("Edge Size", Range(0.001, 0.5)) = 0.06
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "CanUseSpriteAtlas" = "True"
        }
        
        Blend One Zero

        Cull Off
        ZWrite Off

        Pass
        {
            Name "FrostedGlass"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 color : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)

                float4 _GlassColor;

                float _Opacity;
                float _Milkiness;

                float _BlurSize;
                float _BlurSoftness;

                float _Distortion;
                float _NoiseScale;
                float _NoiseStrength;

                float _Brightness;
                float _Contrast;

                float _EdgeDarkening;
                float _EdgeHighlight;
                float _EdgeSize;

            CBUFFER_END

            

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);

                return frac(p.x * p.y);
            }

            

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                f = f * f * (3.0 - 2.0 * f);

                float a = hash21(i);
                float b = hash21(i + float2(1.0, 0.0));
                float c = hash21(i + float2(0.0, 1.0));
                float d = hash21(i + float2(1.0, 1.0));

                return lerp(
                    lerp(a, b, f.x),
                    lerp(c, d, f.x),
                    f.y
                );
            }

            float fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;

                value += noise(p) * amplitude;

                p *= 2.0;
                amplitude *= 0.5;

                value += noise(p) * amplitude;

                p *= 2.0;
                amplitude *= 0.5;

                value += noise(p) * amplitude;

                p *= 2.0;
                amplitude *= 0.5;

                value += noise(p) * amplitude;

                return value;
            }
            

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS =
                    TransformObjectToHClip(
                        IN.positionOS
                    );

                OUT.uv = IN.uv;

                OUT.screenPos =
                    ComputeScreenPos(
                        OUT.positionHCS
                    );

                OUT.color = IN.color;

                return OUT;
            }

            float2 GetDistortionField(float2 uv)
            {
                float2 p =
                    uv * _NoiseScale;

                p +=
                    float2(
                        _Time.y * 0.012,
                        _Time.y * 0.009
                    );

                float x =
                    fbm(
                        p +
                        float2(17.1, 4.3)
                    );

                float y =
                    fbm(
                        p * 0.83 +
                        float2(38.7, 21.4)
                    );

                return float2(
                    x - 0.5,
                    y - 0.5
                );
            }
            

            float Gaussian(float x, float sigma)
            {
                return exp(
                    -(x * x) /
                    (2.0 * sigma * sigma)
                );
            }
            

            float3 SampleBlurredScene(float2 uv)
            {
                float2 texel =
                    _CameraOpaqueTexture_TexelSize.xy;

                float radius =
                    _BlurSize;

                float sigma =
                    max(
                        radius / _BlurSoftness,
                        1.0
                    );

                float3 result = 0.0;
                float totalWeight = 0.0;

                // A compact 13-tap disk keeps the broad frosted look while
                // avoiding the previous 100+ texture samples per pixel.
                [unroll]
                for (int y = -2; y <= 2; y++)
                {
                    [unroll]
                    for (int x = -2; x <= 2; x++)
                    {
                        float2 sampleOffset = float2(x, y) * 0.5;
                        float distanceSquared = dot(sampleOffset, sampleOffset);
                        if (distanceSquared > 1.0)
                            continue;

                        float distance = sqrt(distanceSquared);
                        float weight = Gaussian(distance * radius, sigma);
                        float2 offset = sampleOffset * radius * texel;
                        result += SampleSceneColor(uv + offset) * weight;
                        totalWeight += weight;
                    }
                }

                return result / max(totalWeight, 0.0001);
            }
            

            half4 frag(Varyings IN) : SV_Target
            {

                float2 screenUV =
                    IN.screenPos.xy /
                    IN.screenPos.w;
                

                float2 distortionField =
                    GetDistortionField(
                        screenUV
                    );

                float2 texel =
                    _CameraOpaqueTexture_TexelSize.xy;

                float2 distortion =
                    distortionField *
                    texel *
                    _Distortion *
                    _NoiseStrength;


                float2 distortedUV =
                    screenUV +
                    distortion;
                

                float3 blurred =
                    SampleBlurredScene(
                        distortedUV
                    );

                

        blurred *= _Brightness;
                

        float luminance =
            dot(
                blurred,
                float3(
                    0.2126,
                    0.7152,
                    0.0722
                )
            );

        float3 contrasted =
            luminance +
            (blurred - luminance) *
            _Contrast;

                

        // Keep tint strength independent from overall glass strength so the
        // selected tint remains visible when _Opacity is set low.
        float tintAmount = _Milkiness;

        float3 frostedColor =
            lerp(
                contrasted,
                _GlassColor.rgb,
                tintAmount
            );



        float cloud =
            fbm(
                screenUV *
                (_NoiseScale * 0.65)
            );

        float cloudFactor =
            lerp(
                0.97,
                1.03,
                cloud
            );

        frostedColor *=
            lerp(
                1.0,
                cloudFactor,
                _NoiseStrength * 0.25
            );
                

                float2 edgeDistance =
                    min(
                        IN.uv,
                        1.0 - IN.uv
                    );

                float edge =
                    min(
                        edgeDistance.x,
                        edgeDistance.y
                    );

                float edgeFactor =
                    1.0 -
                    smoothstep(
                        0.0,
                        _EdgeSize,
                        edge
                    );

                

                frostedColor *=
                    1.0 -
                    edgeFactor *
                    _EdgeDarkening;

                
                frostedColor +=
                    edgeFactor *
                    _GlassColor.rgb *
                    _EdgeHighlight;

                
                return half4(
                    frostedColor,
                    1.0
                );
            }

            ENDHLSL
        }
    }
}
