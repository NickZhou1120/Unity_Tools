Shader "Bleach/Walk Light Colored"
{
	Properties
	{
		_MainTex ("RGB", 2D) = "black" {}
		_AlphaTex ("Alpha", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			uniform fixed2 _LightURange;				// 光图片在 u 上的范围
			uniform fixed _LightWidthToMain;			// 光图片宽度占主图片宽度的比例
			uniform fixed _ReachBoundary;				// 小于0表示未到边界，大于0表示到达边界
			uniform fixed _TimeRate;					// 时间/周期
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				half2 uv2 : TEXCOORD1;		// 流光图的 uv 坐标
			};
	
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				fixed fixedU=(v.color.x - _LightURange.x) / (_LightURange.y - _LightURange.x);		// 把 u 坐标转到[0,1]范围
				o.uv2 = fixed2(fixedU,v.color.y);
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
				fixed4 main;
				main.rgb = tex2D(_MainTex, IN.texcoord).rgb;
				main.a = tex2D(_AlphaTex, IN.texcoord).a;
				
				// 流光
				fixed x = _ReachBoundary > 0 && IN.uv2.x < _LightWidthToMain ? IN.uv2.x+1 : IN.uv2.x;
				fixed lightU = (x - _TimeRate) / _LightWidthToMain;
				lightU = (_LightURange.y - _LightURange.x) * lightU + _LightURange.x;				// 把 u 坐标从[0,1]范围转回来
				lightU = clamp(lightU, _LightURange.x, _LightURange.y);
				fixed2 lightUV = fixed2(lightU, IN.uv2.y);
				fixed lightA = tex2D(_AlphaTex, lightUV).a;

				// 融合
				fixed3 col = main.rgb * (1 + lightA * 1.5);
				return fixed4(col, main.a);
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}

			SetTexture [_AlphaTex] 
			{
				Combine previous, texture * primary 
			}
		}
	}
}
