Shader "Jettelly/BIRP/Electric Trap 2D/Jet_electric_particle_HLSL"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _cBottom("Color Bottom", Color) = (1, 1, 1, 1)
        _cMiddle("Color Middle", Color) = (1, 1, 1, 1)
        _cTop("Color Top", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _cBottom;
            float4 _cMiddle;
            float4 _cTop;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float circle(float2 p, float r)
            {
                float c = length(p - r);
                return c;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half c = circle(i.uv, 0.5);
                half r = smoothstep(c - 0.15, c + 0.15, 0.3);

                half4 ren = lerp(_cBottom, _cMiddle, r / 0.5) * step(r, 0.5);
                ren += lerp(_cMiddle, _cTop, (r - 0.5) / (1 - 0.5)) * step(0.5, r);
                ren *= r;

                float3 ranCol = sin((i.uv.y + _Time.y * 10) * float3(0.45, 0.3245, 0.345) * 10) * 0.5 + 0.5;
                ranCol *= r;
                ren.rgb += ranCol.rgb;
                ren = saturate(ren);

                return ren;
            }
            ENDCG
        }
    }
}
