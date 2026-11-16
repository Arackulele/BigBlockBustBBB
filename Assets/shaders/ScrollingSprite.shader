Shader "BBB/Scrolling Sprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ScrollTex ("Scrolling Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _ScrollTiling ("Scrolling Texture Tiling (XY)", Vector) = (10, 10, 0, 0)
        _ScrollSpeed ("Scroll Speed", Float) = 0.1
        _ScrollAngle ("Scroll Angle (Degrees)", Range(0, 360)) = 0
        _ScrollColor ("Scrolling Texture Tint", Color) = (1, 1, 1, 1)
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

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "ScrollingSprite"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float2 scrollBase : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            TEXTURE2D(_ScrollTex);
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_ScrollTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _ScrollTiling;
                float4 _ScrollColor;
                float _ScrollSpeed;
                float _ScrollAngle;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                OUT.scrollBase = IN.positionOS.xy;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half spriteAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a * IN.color.a;

                float angle = radians(_ScrollAngle);
                float2 direction = float2(cos(angle), sin(angle));
                float2 scrollOffset = direction * (_ScrollSpeed * _Time.y);
                float2 scrollUV = frac(IN.scrollBase * _ScrollTiling.xy - scrollOffset);
                half4 overlay = SAMPLE_TEXTURE2D(_ScrollTex, sampler_ScrollTex, scrollUV) * _ScrollColor;

                half alpha = spriteAlpha * overlay.a * _ScrollColor.a;
                return half4(overlay.rgb * IN.color.rgb, alpha);
            }
            ENDHLSL
        }
    }
}
