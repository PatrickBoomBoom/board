Shader "Hidden/DrawWord"
{
	Properties
	{
		_Tex("Texture" , 2D) = "white" {}
		_Size("Size", float) = 0
		_Color("Color" , color) = (1,1,1,1)
		_UV("UV" , vector) = (0,0,0,0)
		_LastUV("LastUV" , vector) = (0,0,0,0)
	}

	SubShader
	{

		ZTest Always Cull Off ZWrite Off Fog{ Mode Off }

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _Tex;
			float _Size;
			fixed4 _UV;
			fixed4 _LastUV;
			fixed4 _Color;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_Tex, i.uv);		

				float a  = _UV.x;
				float b  = _UV.y;
				float c  = _LastUV.x;
				float d  = _LastUV.y;
				float AA = d - b;
				float BB = a - c;
				float CC = b * c - a * d;

				float x = i.uv.x;
				float y = i.uv.y;

				float sqrDic1 = (x - a) * (x - a) + (y - b) * (y - b);
				float sqrDic2 = (x - c) * (x - c) + (y - d) * (y - d);
				float sqrDic11 = (AA * x + BB * y + CC) * (AA * x + BB * y + CC) / (AA * AA + BB * BB);
				float sqrDic22 = (x - (a + c) / 2) * (x - (a + c) / 2) + (y - (b + d) / 2) * (y - (b + d) / 2);

				float sqrDicStand1 = _Size/10000 * _Size/10000;
				float sqrDicStand2 = ((a - c) * (a - c) + (b - d) * (b - d)) / 4;

				if (sqrDic1 <= sqrDicStand1 || sqrDic2 <= sqrDicStand1 || (sqrDic11 <= sqrDicStand1 && sqrDic22 <= sqrDicStand2))
				{
					col = _Color;
				}

				return col;
			}
			ENDCG
		}
	}
}
