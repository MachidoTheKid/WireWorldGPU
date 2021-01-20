Shader "Unlit/BufferRender"
{
    Properties
    {
		[HideInInspector]_MainTex("Texture", 2D) = "white" {}
        _LineColor ("Line Color", Color) = (1,1,1,1)
        _LineSize("Line Size", Range(0,1)) = 0.15
        [IntRange] _GridSize("Grid Size", Range(1,100)) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define S_empty float3(0,0,0)
            #define S_wire float3(1.0, 0.6,0.2)
            #define S_head float3(.3,.5,.1)
            #define S_tail float3(1.,.2,.1)

            sampler2D _MainTex;

            //Grid Setup
            float4 _LineColor;
            float _GridSize;
		    float _LineSize;

			uniform StructuredBuffer<int> _Cells;
			uniform int _Size;

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

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            uint cellcoordinatetoindex(float2 coord){
                return uint (coord.x*_Size +coord.y);
            }

            float4 StateToColour(int state){

                float4 color;
                if (round(state) == 0)
			    {
				    color = float4(S_empty, 1.0);
			    }
                if (round(state) == 1)
			    {
				    color = float4(S_wire, 1.0);
			    }
                if (round(state) == 2)
			    {
				    color = float4(S_head, 1.0);
			    }
                if (round(state) == 3)
			    {
				    color = float4(S_tail, 1.0);
			    }
                return color;
            }

            float3 grid(float2 uv){

			    float gsize = floor(_GridSize);
			    gsize += _LineSize;

				// UV to Coordinate
			    float2 id;
			    id.x = floor(uv.x/(1.0/gsize));
			    id.y = floor(uv.y/(1.0/gsize));

				//Coordinate to Index, Value from Buffer by Index, Value to Colour 
			    float4 color = StateToColour(_Cells[cellcoordinatetoindex(id)]);

				//Check if UV is a line
			    if (frac(uv.x*gsize) <= _LineSize || frac(uv.y*gsize) <= _LineSize)
			    {
				    color = _LineColor;
			    }

                return color;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(grid(i.uv), 1);
            }
            ENDCG
        }
    }
}
