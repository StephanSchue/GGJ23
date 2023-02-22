Shader "Jettelly/BIRP/Electric Trap 2D/Jet_electric_bolt_HLSL"
{
    Properties
    {        
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _Frequency ("Frequency", Float) = 1

        [HideInInspector] _cBottom("Color Bottom", Color) = (1, 1, 1, 1)
        [HideInInspector] _cMiddle("Color Middle", Color) = (1, 1, 1, 1)
        [HideInInspector] _cTop("Color Top", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Assets/JettellyCommon/CGinc/JettellyCommon.cginc"
            #include "Assets/JettellyCommon/CGinc/ShaderGraphNodes.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Frequency;

            float4 _cBottom;
            float4 _cMiddle;
            float4 _cTop;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }            

            float arc(float2 uv, float h, float s, float f, float p, float z, float g)
            {    
                float n = Unity_GradientNoise(uv - _Time.y, p);
                float lm = uv.x;
                float rm = 1 - uv.x;
                float a = 0.5;
                
                n *= lm;
                n *= rm;
                uv.x = lm * (cos(uv.x * f + n) * 0.5 + 0.5) * rm;

                uv.x -= a; 
                float x = abs(uv.x + n);     
                float o = fmod(_Time.y * s, h);
                o *= z;

                float c = ((-(x * x) + (a * a)) * (a / (a * a * a)) * o) + a;
                float r = abs(uv.y - c);

                return smoothstep(r - 0.035, r + 0.035, g);
            }

            half4 frag(v2f i) : SV_Target
            {
                half f = _Frequency;
                half s = pow(0.95, _Frequency);                
                half col = 0;
                
                col += arc(i.uv, 0.50, 2.66 * s, 14 + f, 3,  1, 0.020);
                col += arc(i.uv, 0.35, 1.90 * s, 24 + f, 5,  1, 0.008);
                col += arc(i.uv, 0.31, 0.35 * s, 06 + f, 9, -1, 0.012);
                col += arc(i.uv, 0.45, 2.46 * s, 11 + f, 2, -1, 0.011);
                col += arc(i.uv, 0.50, 3.46 * s, 01 + f, 1, -1, 0.029);
                col = saturate(col);    
                
                half4 ren = Jettelly_ThreeColorGradient(_cTop, _cMiddle, _cBottom, col);
                ren *= col;
                
                half4 ranCol = Jettelly_RainbowColor(i.uv, 10, 10);

                ranCol *= col;
                ren.rgb += ranCol.rgb;
                ren = saturate(ren);

                return ren;
            }
            ENDCG
        }
    }
}
