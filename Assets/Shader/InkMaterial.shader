// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/InkMountain" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_black("black", 2D ) = "white" {}
		_gray("white",2D)= "white" {}
		_depth("depth", 2D)="white"{}
		_outline("outline", float)=1
		_ink("ink", Range(0,5))=2
	}
	SubShader {
		Pass{
			Tags{"RenderType"="Opaque"}
			cull front
			lighting off
			ZWrite on
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _outline;
			struct appdata
			{
			float4 vertex:POSITION;
			float3 normal:NORMAL;
			};

			struct v2f
			{
				float4 pos:POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				float3 normal = mul(UNITY_MATRIX_IT_MV, v.normal);
				normal.z = -0.5f;
				float depth = -pos.z/pos.w*0.01f;
				
				_outline = clamp(depth, 0, _outline);
				pos = pos + float4(normalize(normal), 0)*_outline;
				o.pos = mul(UNITY_MATRIX_P, pos);
				return o;
			}

			float4 frag(v2f i):COLOR
			{
				return float4(0,0,0,1);
			}

			ENDCG
		}
		
		Pass {
			Tags { "RenderType"="Opaque" }
			cull back
			lighting on
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _black;
			sampler2D _gray;
			sampler2D _depth;
			float4 _Color;	
			float _ink;		
			
			struct appdata {
			    float4 vertex : POSITION;
			    fixed3 normal : NORMAL;
			    half2 texcoord : TEXCOORD0;
			};
			
			struct v2f {
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
				float vdotn : TEXCOORD1;
				float2 depth: TEXCOORD2;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				float3 viewDir = normalize( mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1)).xyz - v.vertex);
				o.vdotn = dot(normalize(viewDir),v.normal);
				o.depth = o.vertex.zw;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 diff = tex2D(_MainTex, i.uv)*_Color;
				diff.xyz = dot(diff.xyz, float3(0.33f, 0.33f, 0.33f));
				
				float f = pow(i.vdotn,_ink);
				fixed4 outline = fixed4(1,1,1,1);
				float depth = Linear01Depth(i.depth.x/i.depth.y);
				float depthCol = tex2D(_depth, float2(depth, 0.5f));
				if(f<0.25)
			   	{
			      float2 findColor = float2(f*2.5f, i.uv.x*0.5f+i.uv.y*0.5f);
			      //float2 findColor = float2(f,0.5f);
			      outline = tex2D(_black, findColor);
			   	}
			   	else
			   	{
			   		float2 findColor = float2(f,0.5f);
					//if(depthCol < 1)
			      	outline = tex2D(_gray, findColor);
			   	}
			  	outline *= depthCol;
				
				return diff*outline;
			}
			
			ENDCG
		}
	} 
	Fallback "diffuse"
	CustomEditor "CustomShaderGUI"
}
