Shader "N3K/SimpleDeformation"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", COLOR) = (1,1,1,1)
		_WaveHeight("WaveHeight", Float) = 1
		_WaveSpeed("_WaveSpeed", Float) = 1
	}
	SubShader
	{
		Tags { "Queue"="0" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
		LOD 100

		ZWrite On

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _WaveHeight;
			float _WaveSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.vertex.y += (sin(worldPos.x + (_Time.z * _WaveSpeed))) * _WaveHeight;


				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				 fixed4 col = _Color;
				//fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
