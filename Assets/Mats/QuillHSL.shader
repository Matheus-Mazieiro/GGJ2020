Shader "Unlit/Quill Shader HSL"
{
	Properties
    {
        [Toggle] _EnableTransparency("Enable Transparency", Int) = 1
        [Enum(Yes,0,No,2)] _Cull("Double Sided", Int) = 2
        [Toggle] _EnableFog("Enable Fog", Int) = 0
		_Color("Alpha Color Key", Color) = (0,0,0,1)
       _HSLRangeMin ("HSL Affect Range Min", Range(0, 1)) = 0
       _HSLRangeMax ("HSL Affect Range Max", Range(0, 1)) = 1
       _HSLAAdjust ("HSLA Adjust", Vector) = (0, 0, 0, 0)
       _StencilComp ("Stencil Comparison", Float) = 8
       _Stencil ("Stencil ID", Float) = 0
       _StencilOp ("Stencil Operation", Float) = 0
       _StencilWriteMask ("Stencil Write Mask", Float) = 255
       _StencilReadMask ("Stencil Read Mask", Float) = 255
       _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 100
        Blend Off
        Cull[_Cull]
        ZWrite On
        ZTest On

        Pass
        {
            AlphaToMask[_EnableTransparency]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #pragma shader_feature _ENABLEFOG_ON
            #pragma shader_feature _ENABLETRANSPARENCY_ON
            #include "UnityCG.cginc"

			fixed _AlphaRatio;
			fixed4 _Color;
            float _HSLRangeMin;
            float _HSLRangeMax;
            float4 _HSLAAdjust;

            struct inVertex
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                #ifdef _ENABLEFOG_ON
                UNITY_FOG_COORDS(1)

                #endif

                #ifdef _ENABLETRANSPARENCY_ON

                float4 screenPos : TEXCOORD2;

                #endif
                float4 color : COLOR;
            };

            v2f vert(inVertex v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                #ifdef _ENABLEFOG_ON
                UNITY_TRANSFER_FOG(o, o.vertex);

                #endif

                #ifdef _ENABLETRANSPARENCY_ON

                o.screenPos = ComputeScreenPos(o.vertex);

                #endif
                return o;
            }
			
            float Epsilon = 1e-10;

            float3 rgb2hcv(in float3 RGB)
            {
                // Based on work by Sam Hocevar and Emil Persson
                float4 P = lerp(float4(RGB.bg, -1.0, 2.0/3.0), float4(RGB.gb, 0.0, -1.0/3.0), step(RGB.b, RGB.g));
                float4 Q = lerp(float4(P.xyw, RGB.r), float4(RGB.r, P.yzx), step(P.x, RGB.r));
                float C = Q.x - min(Q.w, Q.y);
                float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
                return float3(H, C, Q.x);
            }

            float3 rgb2hsl(in float3 RGB)
            {
                float3 HCV = rgb2hcv(RGB);
                float L = HCV.z - HCV.y * 0.5;
                float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
                return float3(HCV.x, S, L);
            }

            float3 hsl2rgb(float3 c)
            {
                c = float3(frac(c.x), clamp(c.yz, 0.0, 1.0));
                float3 rgb = clamp(abs(fmod(c.x * 6.0 + float3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
                return c.z + c.y * (rgb - 0.5) * (1.0 - abs(2.0 * c.z - 1.0));
            }

            float4 main(float4 color) : COLOR
            {
                float3 hsl = rgb2hsl(color.rgb);
                float affectMult = step(_HSLRangeMin, hsl.r) * step(hsl.r, _HSLRangeMax);
                float3 rgb = hsl2rgb(hsl + _HSLAAdjust.xyz * affectMult);
                return float4(rgb, color.a + _HSLAAdjust.a);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = i.color;

                #ifdef _ENABLEFOG_ON
                UNITY_APPLY_FOG(i.fogCoord, col);
                #endif

                #ifdef _ENABLETRANSPARENCY_ON
                float2 pos = _ScreenParams.xy * i.screenPos.xy / i.screenPos.w;
                const int MSAASampleCount = 8;
                float ran = frac(52.9829189*frac(dot(pos, float2(0.06711056,0.00583715))));
				col = main(col);
                #endif

                return col;
            }
            ENDCG
        }
    }
}
