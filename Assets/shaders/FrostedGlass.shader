Shader "BBB/Frosted Glass"
{
    Properties
    {
        [Header(Glass)]
        _GlassColor ("Glass Color", Color) = (0.8, 0.9, 0.9, 1)
        _Opacity ("Opacity", Range(0, 1)) = 0.55

        [Header(Frost)]
        _BlurSize ("Blur Size", Range(0, 0.05)) = 0.012
        _Distortion ("Distortion", Range(0, 0.03)) = 0.004
        _NoiseScale ("Noise Scale", Range(1, 100)) = 25
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.15

        [Header(Lighting)]
        _Brightness ("Brightness", Range(0, 2)) = 1.0
        _Contrast ("Contrast", Range(0, 2)) = 1.0

        [Header(Edge)]
        _EdgeDarkening ("Edge Darkening", Range(0, 1)) = 0.15
        _EdgeHighlight ("Edge Highlight", Range(0, 1)) = 0.15
        _EdgeSize ("Edge Size", Range(0, 0.5)) = 0.08
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

        Blend SrcAlpha OneMinusSrcAlpha
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

                float _BlurSize;
                float _Distortion;

                float _NoiseScale;
                float _NoiseStrength;

                float _Brightness;
                float _Contrast;

                float _EdgeDarkening;
                float _EdgeHighlight;
                float _EdgeSize;

            CBUFFER_END


            // ============================================================
            // HASH / NOISE
            // ============================================================

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));

                p += dot(
                    p,
                    p + 45.32
                );

                return frac(
                    p.x * p.y
                );
            }


            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                f = f * f * (3.0 - 2.0 * f);

                float a = hash21(i);
                float b = hash21(i + float2(1, 0));
                float c = hash21(i + float2(0, 1));
                float d = hash21(i + float2(1, 1));

                return lerp(
                    lerp(a, b, f.x),
                    lerp(c, d, f.x),
                    f.y
                );
            }


            // ============================================================
            // VERTEX
            // ============================================================

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


            // ============================================================
            // SAMPLE BLURRED SCENE
            // ============================================================

            float3 SampleBlurredScene(float2 uv)
            {
                float2 pixel =
                    _BlurSize / _ScreenParams.xy;


                float3 result = 0;


                // Center
                result +=
                    SampleSceneColor(
                        uv
                    ) * 0.16;


                // Cardinal directions
                result +=
                    SampleSceneColor(
                        uv + float2(pixel.x, 0)
                    ) * 0.10;

                result +=
                    SampleSceneColor(
                        uv - float2(pixel.x, 0)
                    ) * 0.10;

                result +=
                    SampleSceneColor(
                        uv + float2(0, pixel.y)
                    ) * 0.10;

                result +=
                    SampleSceneColor(
                        uv - float2(0, pixel.y)
                    ) * 0.10;


                // Diagonals
                result +=
                    SampleSceneColor(
                        uv + pixel
                    ) * 0.09;

                result +=
                    SampleSceneColor(
                        uv - pixel
                    ) * 0.09;

                result +=
                    SampleSceneColor(
                        uv + float2(
                            pixel.x,
                            -pixel.y
                        )
                    ) * 0.09;

                result +=
                    SampleSceneColor(
                        uv + float2(
                            -pixel.x,
                            pixel.y
                        )
                    ) * 0.09;


                // Wider samples
                float2 wide =
                    pixel * 2.0;


                result +=
                    SampleSceneColor(
                        uv + float2(wide.x, 0)
                    ) * 0.02;

                result +=
                    SampleSceneColor(
                        uv - float2(wide.x, 0)
                    ) * 0.02;

                result +=
                    SampleSceneColor(
                        uv + float2(0, wide.y)
                    ) * 0.02;

                result +=
                    SampleSceneColor(
                        uv - float2(0, wide.y)
                    ) * 0.02;


                return result;
            }


            // ============================================================
            // FRAGMENT
            // ============================================================

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenUV =
                    IN.screenPos.xy /
                    IN.screenPos.w;


                // --------------------------------------------------------
                // FROST NOISE
                // --------------------------------------------------------

                float2 noiseUV =
                    screenUV *
                    _NoiseScale;

                noiseUV +=
                    _Time.y * 0.015;


                float noiseX =
                    noise(
                        noiseUV
                        + float2(17.3, 4.7)
                    );

                float noiseY =
                    noise(
                        noiseUV
                        + float2(3.1, 29.4)
                    );


                float2 distortion =
                    float2(
                        noiseX - 0.5,
                        noiseY - 0.5
                    );

                distortion *=
                    _Distortion *
                    _NoiseStrength;


                // --------------------------------------------------------
                // BLURRED BACKGROUND
                // --------------------------------------------------------

                float2 distortedUV =
                    screenUV +
                    distortion;


                float3 background =
                    SampleBlurredScene(
                        distortedUV
                    );


                // --------------------------------------------------------
                // CONTRAST
                // --------------------------------------------------------

                background =
                    lerp(
                        0.5,
                        background,
                        _Contrast
                    );


                // --------------------------------------------------------
                // BRIGHTNESS
                // --------------------------------------------------------

                background *=
                    _Brightness;


                // --------------------------------------------------------
                // GLASS TINT
                // --------------------------------------------------------

                background =
                    lerp(
                        background,
                        background *
                        _GlassColor.rgb,
                        _GlassColor.a
                    );


                // --------------------------------------------------------
                // SPRITE EDGE
                // --------------------------------------------------------

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


                // Slight darkening toward edge
                background *=
                    1.0 -
                    edgeFactor *
                    _EdgeDarkening;


                // Small bright rim
                background +=
                    edgeFactor *
                    _EdgeHighlight;


                // --------------------------------------------------------
                // OUTPUT
                // --------------------------------------------------------

                float alpha =
                    _Opacity *
                    IN.color.a;

                return half4(
                    background,
                    alpha
                );
            }

            ENDHLSL
        }
    }
}