Shader "BBB/Blur Pane"
{
    Properties
    {
        _BlurSize ("Blur Size (pixels)", Range(0, 20)) = 4
        [Enum(Box, 0, Gaussian, 1)] _BlurMode ("Blur Mode", Float) = 1
        _BlurSpread ("Sample Spread", Range(0.5, 2)) = 1
        _Desaturation ("Desaturation", Range(0, 1)) = 0
        _TintColor ("Pane Tint", Color) = (1, 1, 1, 1)
        _TintStrength ("Tint Strength", Range(0, 1)) = 0.15
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

        Pass
        {
            Name "BlurPane"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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
                float4 screenPos : TEXCOORD0;
                float4 color : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _TintColor;
                half _TintStrength;
                float _BlurSize;
                float _BlurMode;
                float _BlurSpread;
                half _Desaturation;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.screenPos = ComputeScreenPos(output.positionHCS);
                output.color = input.color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.screenPos.xy / input.screenPos.w;
                float2 stepUV = _CameraOpaqueTexture_TexelSize.xy * _BlurSize * _BlurSpread;

                half3 center = SampleSceneColor(uv);
                half3 horizontal = SampleSceneColor(uv + float2(stepUV.x, 0))
                                 + SampleSceneColor(uv - float2(stepUV.x, 0));
                half3 vertical = SampleSceneColor(uv + float2(0, stepUV.y))
                               + SampleSceneColor(uv - float2(0, stepUV.y));
                half3 diagonals = SampleSceneColor(uv + stepUV)
                                + SampleSceneColor(uv - stepUV)
                                + SampleSceneColor(uv + float2(stepUV.x, -stepUV.y))
                                + SampleSceneColor(uv + float2(-stepUV.x, stepUV.y));

                // Box uses equal weights. Gaussian uses a 3x3 approximation
                // with more weight at the center and along the axes.
                half3 color;
                if (_BlurMode < 0.5)
                {
                    color = (center + horizontal + vertical + diagonals) / 9.0h;
                }
                else
                {
                    color = (center * 4.0h + (horizontal + vertical) * 2.0h + diagonals) / 16.0h;
                }

                half luminance = dot(color, half3(0.2126h, 0.7152h, 0.0722h));
                color = lerp(color, luminance.xxx, _Desaturation);

                color = lerp(color, color * _TintColor.rgb, _TintStrength);
                half alpha = _TintColor.a * input.color.a;
                return half4(color * input.color.rgb, alpha);
            }
            ENDHLSL
        }
    }
}
