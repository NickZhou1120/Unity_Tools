Shader "Bleach/Alpha Separated Colored"
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
				fixed4 color : COLOR;
			};
	
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
				fixed4 col;
				col.rgb = tex2D(_MainTex, IN.texcoord).rgb;
				col.a = tex2D(_AlphaTex, IN.texcoord).a;

				if (IN.color.r < 0.001 && IN.color.g < 0.001 && IN.color.b < 0.001)  
				{  
					float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));  
					col.rgb = float3(grey, grey, grey);  
				}  
				else  
					col = col * IN.color;
				
				return col;  
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
