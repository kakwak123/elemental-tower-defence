// Fog curtain for the monster spawn doors. A scrolling fog texture faded out
// toward the plane's border by a mask, so the geometry's edges do not give the
// effect away. Cheaper and easier to author than an equivalent particle system.
Shader "Custom/fog"
{
    Properties
    {
        _MainTex ("Fog texture", 2D) = "white" {}
        [NoScaleOffset] _Mask ("Mask", 2D) = "white" {}
        _Color ("Color", color) = (1., 1., 1., 1.)
        _ScrollSpeed ("Scroll speed", Range(0, 4)) = 1
        _ScrollDir ("Scroll direction (xy)", Vector) = (-1, 1, 0, 0)
        // Second, slower counter-scrolling sample. Layering two offsets breaks up
        // the obvious repeat of a single scrolling tile. 0 reproduces the original
        // single-layer look exactly.
        _DetailStrength ("Second layer strength", Range(0, 1)) = 0.35
        _DetailScale ("Second layer scale", Range(0.25, 4)) = 1.7
    }

    SubShader
    {
        // The original had no tags. With ZWrite off and alpha blending, that left
        // the material in the default Geometry queue, where it renders before the
        // transparent objects it is supposed to sort against.
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 vertCol : COLOR0;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv2 = v.texcoord;
                o.vertCol = v.color;
                return o;
            }

            sampler2D _Mask;
            fixed4 _Color;
            float _ScrollSpeed;
            float4 _ScrollDir;
            float _DetailStrength;
            float _DetailScale;

            fixed4 frag(v2f i) : SV_Target
            {
                // _Time.x climbs without bound for as long as the level runs. The
                // original accumulated the scroll offset in `fixed`, which on
                // platforms where fixed is genuinely low precision (roughly -2..2
                // with ~1/256 steps) makes the scroll visibly stutter and then
                // stop resolving at all. Float throughout.
                float2 dir = normalize(_ScrollDir.xy + 1e-6);
                float t = _Time.x * _ScrollSpeed;

                float2 uvA = i.uv + dir * t;
                fixed4 col = tex2D(_MainTex, uvA);

                if (_DetailStrength > 0)
                {
                    // Counter-scroll at a different rate and scale so the two
                    // layers never line up into a visible tile.
                    float2 uvB = i.uv * _DetailScale - dir * (t * 0.6);
                    fixed4 detail = tex2D(_MainTex, uvB);
                    col = lerp(col, col * detail * 2, _DetailStrength);
                }

                col *= _Color * i.vertCol;
                col.a *= tex2D(_Mask, i.uv2).r;
                return col;
            }
            ENDCG
        }
    }

    FallBack Off
}
