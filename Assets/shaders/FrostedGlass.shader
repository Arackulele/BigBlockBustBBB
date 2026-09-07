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

        // IMPORTANT:
        // The glass itself is NOT alpha blended with the scene.
        // We want the blurred scene to completely replace the sharp
        // background underneath the panel.
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


            // ============================================================
            // HASH
            // ============================================================

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);

                return frac(p.x * p.y);
            }


            // ============================================================
            // VALUE NOISE
            // ============================================================

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


            // ============================================================
            // FBM
            // ============================================================

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
            // DISTORTION FIELD
            // ============================================================

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


            // ============================================================
            // GAUSSIAN
            // ============================================================

            float Gaussian(float x, float sigma)
            {
                return exp(
                    -(x * x) /
                    (2.0 * sigma * sigma)
                );
            }


            // ============================================================
            // LARGE FROSTED BLUR
            //
            // Multiple samples distributed over a large radius.
            // This is deliberately weighted toward the center so edges
            // dissolve smoothly instead of producing obvious streaks.
            // ============================================================

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

                float3 result =
                    float3(0, 0, 0);

                float totalWeight =
                    0.0;


                // --------------------------------------------------------
                // Center
                // --------------------------------------------------------

                float centerWeight =
                    Gaussian(
                        0,
                        sigma
                    );

                result +=
                    SampleSceneColor(uv) *
                    centerWeight;

                totalWeight +=
                    centerWeight;


                // --------------------------------------------------------
                // Large number of samples
                //
                // The loop is symmetric so that there isn't a directional
                // smear.
                // --------------------------------------------------------

                for (int y = -6; y <= 6; y++)
                {
                    for (int x = -6; x <= 6; x++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        float2 pos =
                            float2(x, y) / 6.0;

                        float distance =
                            length(pos);

                        if (distance > 1.0)
                            continue;


                        float sampleRadius =
                            distance *
                            radius;


                        float weight =
                            Gaussian(
                                sampleRadius,
                                sigma
                            );


                        float2 offset =
                            pos *
                            radius *
                            texel;


                        result +=
                            SampleSceneColor(
                                uv + offset
                            ) *
                            weight;

                        totalWeight +=
                            weight;
                    }
                }


                return
                    result /
                    max(totalWeight, 0.0001);
            }


            // ============================================================
            // FRAGMENT
            // ============================================================

            half4 frag(Varyings IN) : SV_Target
            {
                // --------------------------------------------------------
                // SCREEN UV
                // --------------------------------------------------------

                float2 screenUV =
                    IN.screenPos.xy /
                    IN.screenPos.w;


                // --------------------------------------------------------
                // ORGANIC DISTORTION
                // --------------------------------------------------------

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


                // --------------------------------------------------------
                // BLURRED BACKGROUND
                // --------------------------------------------------------

                float3 blurred =
                    SampleBlurredScene(
                        distortedUV
                    );


        // --------------------------------------------------------
        // BRIGHTNESS
        // --------------------------------------------------------

        blurred *= _Brightness;


        // --------------------------------------------------------
        // CONTRAST
        //
        // Apply contrast around the pixel's luminance rather than
        // around neutral grey. This preserves the colors of the
        // Balatro background.
        // --------------------------------------------------------

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


        // --------------------------------------------------------
        // GLASS COLOR
        //
        // Glass Strength controls whether the glass actually
        // influences the background.
        //
        // At 0:
        //     untouched blurred colors.
        //
        // At 1:
        //     Milkiness is fully applied.
        // --------------------------------------------------------

        float tintAmount =
            _Milkiness *
            _Opacity;

        float3 frostedColor =
            lerp(
                contrasted,
                _GlassColor.rgb,
                tintAmount
            );


        // --------------------------------------------------------
        // SUBTLE FROST CLOUDING
        //
        // Only changes local brightness. It does not pull the
        // image toward white.
        // --------------------------------------------------------

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

                // --------------------------------------------------------
                // EDGES
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


                // Slight edge darkening

                frostedColor *=
                    1.0 -
                    edgeFactor *
                    _EdgeDarkening;


                // Soft colored rim

                frostedColor +=
                    edgeFactor *
                    _GlassColor.rgb *
                    _EdgeHighlight;


                // --------------------------------------------------------
                // OUTPUT
                // --------------------------------------------------------

                return half4(
                    frostedColor,
                    1.0
                );
            }

            ENDHLSL
        }
    }
}