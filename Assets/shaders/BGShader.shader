Shader "BBB/Balatro Background"
{
    Properties
    {
        [Header(Coloring)]
        _Color1 ("Color 1", Color) = (0.05, 0.11, 0.12, 1)
        _Color2 ("Color 2", Color) = (0.10, 0.24, 0.24, 1)
        _Color3 ("Color 3", Color) = (0.22, 0.43, 0.42, 1)
        _Color4 ("Color 4", Color) = (0.45, 0.68, 0.64, 1)

        [Header(NoiseWave)]
        _WaveScale ("Wave Scale", Range(1, 20)) = 6
        _WaveStrength ("Wave Strength", Range(0, 3)) = 1
        _BandCount ("Band Count", Range(2, 20)) = 8
        _BandWidth ("Band Width", Range(0.02, 0.5)) = 0.18

        [Header(Dist)]
        _WarpScale ("Warp Scale", Range(0.5, 8)) = 2.5
        _WarpStrength ("Warp Strength", Range(0, 3)) = 1.3
        _DetailWarp ("Detail Warp", Range(0, 2)) = 0.5

        [Header(Anims)]
        _Speed ("Speed", Range(0, 2)) = 0.18
        _Direction ("Direction", Range(0, 6.28318)) = 0.7

        [Header(Vignette Filter)]
        _VignetteSize ("Vignette Size", Range(0, 2)) = 0.85
        _VignetteStrength ("Vignette Strength", Range(0, 1)) = 0.2

        [Header(Extra)]
        _Brightness ("Brightness", Range(0, 2)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off
        ZWrite On

        Pass
        {
            Name "SpriteUnlit"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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
                float4 color : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)

                float4 _Color1;
                float4 _Color2;
                float4 _Color3;
                float4 _Color4;

                float _WaveScale;
                float _WaveStrength;
                float _BandCount;
                float _BandWidth;

                float _WarpScale;
                float _WarpStrength;
                float _DetailWarp;

                float _Speed;
                float _Direction;

                float _VignetteSize;
                float _VignetteStrength;

                float _Brightness;

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
                    TransformObjectToHClip(IN.positionOS);

                OUT.uv = IN.uv;
                OUT.color = IN.color;

                return OUT;
            }
            

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                float time = _Time.y * _Speed;

                float2 direction =
                    float2(
                        cos(_Direction),
                        sin(_Direction)
                    );

                float2 perpendicular =
                    float2(
                        -direction.y,
                        direction.x
                    );
                
                float2 p = uv - 0.5;


                p.x *= 1.15;
                
                float2 warpUV =
                    p * _WarpScale;

                warpUV += direction * time * 0.35;

                float warpA =
                    fbm(warpUV);

                float warpB =
                    fbm(
                        warpUV * 0.7
                        + perpendicular * 2.3
                    );

                float2 warp;

                warp.x =
                    (warpA - 0.5) * 2.0;

                warp.y =
                    (warpB - 0.5) * 2.0;

                p +=
                    warp *
                    _WarpStrength;

                

                float2 detailUV =
                    p * (_WarpScale * 2.5);

                detailUV +=
                    perpendicular *
                    time *
                    0.25;

                float detail =
                    fbm(detailUV);

                p +=
                    perpendicular *
                    (detail - 0.5) *
                    _DetailWarp;
                

                float waveX =
                    p.x * _WaveScale;

                float waveY =
                    p.y * (_WaveScale * 0.65);
                

                float wave1 =
                    sin(
                        waveX
                        + sin(waveY * 1.7 + time * 1.4)
                        * _WaveStrength
                        - time * 1.3
                    );

                float wave2 =
                    sin(
                        waveY * 1.35
                        + cos(waveX * 1.4 - time)
                        * 1.4
                        + time * 0.8
                    );

                float wave3 =
                    sin(
                        (waveX + waveY * 0.55)
                        * 0.85
                        + sin(waveY * 2.2)
                        * 1.8
                        - time * 0.9
                    );

                float field =
                    wave1 * 0.55 +
                    wave2 * 0.25 +
                    wave3 * 0.20;

                

                float largeNoise =
                    fbm(
                        p * 1.7
                        + direction * time * 0.15
                    );

                field +=
                    (largeNoise - 0.5)
                    * 1.5;
                
                float bandPosition =
                    field * _BandCount;

                float band =
                    frac(
                        bandPosition * 0.5
                        + 0.5
                    );


                // Sharpen the bands.
                float edge =
                    smoothstep(
                        0.5 - _BandWidth,
                        0.5,
                        band
                    )
                    -
                    smoothstep(
                        0.5,
                        0.5 + _BandWidth,
                        band
                    );


                float palette =
                    frac(field * 0.5 + 0.5);


                float3 color;

                if (palette < 0.3333)
                {
                    float t =
                        palette / 0.3333;

                    color =
                        lerp(
                            _Color1.rgb,
                            _Color2.rgb,
                            t
                        );
                }
                else if (palette < 0.6666)
                {
                    float t =
                        (palette - 0.3333) / 0.3333;

                    color =
                        lerp(
                            _Color2.rgb,
                            _Color3.rgb,
                            t
                        );
                }
                else
                {
                    float t =
                        (palette - 0.6666) / 0.3334;

                    color =
                        lerp(
                            _Color3.rgb,
                            _Color4.rgb,
                            t
                        );
                }

                color +=
                    edge *
                    0.12;

                

                float distanceFromCenter =
                    length(p);

                float vignette =
                    1.0 -
                    smoothstep(
                        _VignetteSize * 0.45,
                        _VignetteSize,
                        distanceFromCenter
                    );

                color *=
                    lerp(
                        1.0 - _VignetteStrength,
                        1.0,
                        vignette
                    );
                

                color *= _Brightness;


                return half4(
                    color,
                    IN.color.a
                );
            }

            ENDHLSL
        }
    }
}