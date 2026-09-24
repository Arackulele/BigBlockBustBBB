
Shader "BBB/UI Border"
{
    Properties
    {
        [PerRendererData]
        _MainTex ("Sprite Texture", 2D) = "white" {}

        [Header(Base)]
        _Color ("Border Color", Color) = (0.25, 0.55, 0.75, 1)
        _Brightness ("Brightness", Range(0, 2)) = 1

        [Header(Bevel)]
        _BevelSize ("Bevel Size", Range(0, 10)) = 1
        _BevelStrength ("Bevel Strength", Range(0, 1)) = 0.2
        _HighlightColor ("Highlight Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Bevel Shadow Color", Color) = (0, 0, 0, 1)

        [Header(Texture)]
        _TextureScale ("Texture Scale", Range(1, 100)) = 18
        _TextureStrength ("Texture Strength", Range(0, 1)) = 0.03

        [Header(Drop Shadow)]
        _ShadowOffsetX ("Shadow X", Range(-100, 100)) = 8
        _ShadowOffsetY ("Shadow Y", Range(-100, 100)) = -8
        _ShadowSize ("Shadow Size", Range(0, 30)) = 4
        _ShadowStrength ("Shadow Strength", Range(0, 1)) = 0.3
        _ShadowColor2 ("Shadow Color", Color) = (0, 0, 0, 1)



        [HideInInspector]
        _StencilComp ("Stencil Comparison", Float) = 8

        [HideInInspector]
        _Stencil ("Stencil ID", Float) = 0

        [HideInInspector]
        _StencilOp ("Stencil Operation", Float) = 0

        [HideInInspector]
        _StencilWriteMask ("Stencil Write Mask", Float) = 255

        [HideInInspector]
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        [HideInInspector]
        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)]
        _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }


    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "CanUseSpriteAtlas" = "True"
            "IgnoreProjector" = "True"
        }
        

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }


        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        Blend SrcAlpha OneMinusSrcAlpha

        ColorMask [_ColorMask]


        Pass
        {
            Name "UI Border"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"



            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };


            struct Varyings
            {
                float4 positionHCS : SV_POSITION;

                float2 uv : TEXCOORD0;

                float4 color : COLOR;

                #if defined(UNITY_UI_CLIP_RECT)
                float4 worldPosition : TEXCOORD3;
                #endif
            };


            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            CBUFFER_START(UnityPerMaterial)

                float4 _Color;
                float _Brightness;

                float _BevelSize;
                float _BevelStrength;

                float4 _HighlightColor;
                float4 _ShadowColor;

                float _TextureScale;
                float _TextureStrength;

                float _ShadowOffsetX;
                float _ShadowOffsetY;
                float _ShadowSize;
                float _ShadowStrength;
                float4 _ShadowColor2;

                float4 _ClipRect;

            CBUFFER_END
            

            float hash21(float2 p)
            {
                p =
                    frac(
                        p *
                        float2(123.34, 456.21)
                    );

                p +=
                    dot(
                        p,
                        p + 45.32
                    );

                return frac(
                    p.x *
                    p.y
                );
            }


            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                f =
                    f *
                    f *
                    (3.0 - 2.0 * f);

                float a =
                    hash21(i);

                float b =
                    hash21(
                        i +
                        float2(1.0, 0.0)
                    );

                float c =
                    hash21(
                        i +
                        float2(0.0, 1.0)
                    );

                float d =
                    hash21(
                        i +
                        float2(1.0, 1.0)
                    );

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

                value +=
                    noise(p) *
                    amplitude;

                p *= 2.0;
                amplitude *= 0.5;

                value +=
                    noise(p) *
                    amplitude;

                p *= 2.0;
                amplitude *= 0.5;

                value +=
                    noise(p) *
                    amplitude;

                return value;
            }
            
            float SampleAlpha(float2 uv)
            {
                return SAMPLE_TEXTURE2D(
                    _MainTex,
                    sampler_MainTex,
                    uv
                ).a;
            }



            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS =
                    TransformObjectToHClip(
                        IN.positionOS
                    );

                OUT.uv =
                    TRANSFORM_TEX(
                        IN.uv,
                        _MainTex
                    );

                OUT.color =
                    IN.color;

                #if defined(UNITY_UI_CLIP_RECT)

                OUT.worldPosition =
                    IN.positionOS;

                #endif

                return OUT;
            }

            float ShadowShape(float2 uv)
            {
                float2 texel =
                    _MainTex_TexelSize.xy;

                float radius =
                    max(
                        _ShadowSize,
                        0.001
                    );


                float result =
                    SampleAlpha(uv) *
                    0.30;


                result +=
                    SampleAlpha(
                        uv +
                        float2(
                            radius,
                            0
                        ) *
                        texel
                    ) *
                    0.12;


                result +=
                    SampleAlpha(
                        uv -
                        float2(
                            radius,
                            0
                        ) *
                        texel
                    ) *
                    0.12;


                result +=
                    SampleAlpha(
                        uv +
                        float2(
                            0,
                            radius
                        ) *
                        texel
                    ) *
                    0.12;


                result +=
                    SampleAlpha(
                        uv -
                        float2(
                            0,
                            radius
                        ) *
                        texel
                    ) *
                    0.12;


                result +=
                    SampleAlpha(
                        uv +
                        float2(
                            radius,
                            radius
                        ) *
                        texel
                    ) *
                    0.055;


                result +=
                    SampleAlpha(
                        uv +
                        float2(
                            -radius,
                            radius
                        ) *
                        texel
                    ) *
                    0.055;


                result +=
                    SampleAlpha(
                        uv +
                        float2(
                            radius,
                            -radius
                        ) *
                        texel
                    ) *
                    0.055;


                result +=
                    SampleAlpha(
                        uv +
                        float2(
                            -radius,
                            -radius
                        ) *
                        texel
                    ) *
                    0.055;


                return saturate(result);
            }

            

            half4 frag(Varyings IN) : SV_Target
            {

                float4 sprite =
                    SAMPLE_TEXTURE2D(
                        _MainTex,
                        sampler_MainTex,
                        IN.uv
                    );


                float alpha =
                    sprite.a *
                    IN.color.a;

                

                float3 color =
                    _Color.rgb;


                color *=
                    IN.color.rgb;


                color *=
                    _Brightness;

                

                float textureNoise =
                    fbm(
                        IN.uv *
                        _TextureScale
                    );


                float textureFactor =
                    lerp(
                        1.0 - _TextureStrength,
                        1.0 + _TextureStrength,
                        textureNoise
                    );


                color *=
                    textureFactor;

                

                float2 texel =
                    _MainTex_TexelSize.xy *
                    _BevelSize;


                float current =
                    alpha;


                float left =
                    SampleAlpha(
                        IN.uv -
                        float2(
                            texel.x,
                            0
                        )
                    ) *
                    IN.color.a;


                float right =
                    SampleAlpha(
                        IN.uv +
                        float2(
                            texel.x,
                            0
                        )
                    ) *
                    IN.color.a;


                float down =
                    SampleAlpha(
                        IN.uv -
                        float2(
                            0,
                            texel.y
                        )
                    ) *
                    IN.color.a;


                float up =
                    SampleAlpha(
                        IN.uv +
                        float2(
                            0,
                            texel.y
                        )
                    ) *
                    IN.color.a;



                float leftEdge =
                    saturate(
                        current -
                        left
                    );


                float rightEdge =
                    saturate(
                        current -
                        right
                    );


                float downEdge =
                    saturate(
                        current -
                        down
                    );


                float upEdge =
                    saturate(
                        current -
                        up
                    );


                float highlight =
                    max(
                        leftEdge,
                        upEdge
                    );


                float bevelShadow =
                    max(
                        rightEdge,
                        downEdge
                    );


                highlight *=
                    _BevelStrength;


                bevelShadow *=
                    _BevelStrength;



                color =
                    lerp(
                        color,
                        color +
                        _HighlightColor.rgb *
                        0.35,
                        highlight
                    );


                color =
                    lerp(
                        color,
                        color *
                        _ShadowColor.rgb,
                        bevelShadow *
                        0.35
                    );

                

                float2 shadowUV =
                    IN.uv;

                shadowUV -=
                    float2(
                        _ShadowOffsetX,
                        _ShadowOffsetY
                    ) *
                    _MainTex_TexelSize.xy;


                float shadowAlpha =
                    ShadowShape(
                        shadowUV
                    );


                shadowAlpha *=
                    _ShadowStrength;


                shadowAlpha *=
                    IN.color.a;

                

                float2 uvInside =
                    step(
                        0.0,
                        IN.uv
                    ) *
                    step(
                        IN.uv,
                        1.0
                    );


                float insideMask =
                    uvInside.x *
                    uvInside.y;


                float borderAlpha =
                    alpha *
                    insideMask;

                

                float finalAlpha =
                    max(
                        borderAlpha,
                        shadowAlpha
                    );


                float3 finalColor =
                    lerp(
                        _ShadowColor2.rgb,
                        color,
                        borderAlpha
                    );

                

                #if defined(UNITY_UI_CLIP_RECT)

                float2 clipFactor =
                    float2(
                        step(
                            _ClipRect.x,
                            IN.worldPosition.x
                        ),
                        step(
                            _ClipRect.y,
                            IN.worldPosition.y
                        )
                    );


                clipFactor *=
                    float2(
                        step(
                            IN.worldPosition.x,
                            _ClipRect.z
                        ),
                        step(
                            IN.worldPosition.y,
                            _ClipRect.w
                        )
                    );


                finalAlpha *=
                    clipFactor.x *
                    clipFactor.y;

                #endif

                

                #if defined(UNITY_UI_ALPHACLIP)

                clip(
                    finalAlpha -
                    0.001
                );

                #endif



                return half4(
                    finalColor,
                    finalAlpha
                );
            }


            ENDHLSL
        }
    }
}
