// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Rim-lit shader for the elemental stones. The stones are the game's currency
// and have to be spotted quickly against cluttered terrain, so the silhouette
// glow is doing gameplay work, not just decoration.
Shader "Custom/shining"
{
    Properties
    {
        _MainTex("main tex",2D) = "black"{}
        _RimColor("rim color",Color) = (1,1,1,1)
        _RimPower("rim power",range(1,10)) = 2
        _RimIntensity("rim intensity",range(0,4)) = 1
        _Pulse("pulse speed (0 = off)",range(0,8)) = 0
        _PulseDepth("pulse depth",range(0,1)) = 0.35
    }

        SubShader
        {
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include"UnityCG.cginc"

                struct v2f
                {
                    // Was POSITION, which is an input semantic. SV_POSITION is
                    // the correct output semantic and is what several platforms
                    // actually require.
                    float4 vertex:SV_POSITION;
                    float2 uv:TEXCOORD0;
                    // The rim term used to be computed here in the vertex shader
                    // and interpolated. On low-poly meshes like these stones that
                    // makes the glow visibly faceted, because it is only sampled
                    // once per vertex. Carry the normal and view direction across
                    // instead and do the dot product per pixel.
                    float3 normalObj:TEXCOORD1;
                    float3 viewObj:TEXCOORD2;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _RimColor;
                float _RimPower;
                float _RimIntensity;
                float _Pulse;
                float _PulseDepth;

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.normalObj = v.normal;
                    // WorldSpaceViewDir returns a world-space direction; bring it
                    // into object space so it matches the object-space normal.
                    o.viewObj = mul(unity_WorldToObject, float4(WorldSpaceViewDir(v.vertex), 0)).xyz;
                    return o;
                }

                half4 frag(v2f IN) : SV_Target
                {
                    half4 c = tex2D(_MainTex, IN.uv);

                    // Interpolation denormalises both vectors, so renormalise
                    // before the dot product or the rim width drifts across a face.
                    float3 N = normalize(IN.normalObj);
                    float3 V = normalize(IN.viewObj);

                    // N.V approaches zero at the silhouette, so 1 - N.V is an
                    // edge mask. _RimPower controls how tightly it hugs the edge.
                    float rim = pow(saturate(1 - saturate(dot(N, V))), _RimPower);

                    // Optional slow breathe, off by default so existing materials
                    // look exactly as they did.
                    float pulse = 1;
                    if (_Pulse > 0)
                    {
                        pulse = 1 - _PulseDepth * (0.5 - 0.5 * cos(_Time.y * _Pulse));
                    }

                    c.rgb += rim * _RimIntensity * pulse * _RimColor.rgb;
                    return c;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
