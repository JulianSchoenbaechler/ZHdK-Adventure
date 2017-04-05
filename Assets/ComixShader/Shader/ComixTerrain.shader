Shader "Comix/Terrain" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "" {}
		_ShadowMap("Shadow Map", 2D) = "black" {}
		_ShadowIntensity("Shadow Intensity", Range(0,1)) = 1
		_Shading("Shadow Threshold", Range(0,1)) = 0.5
		_ShadowSoftness("Shadow Softness", Range(0,1)) = 0.2
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 400

		CGPROGRAM

		// Custom Comic lighting model, and enable shadows on all light types
		#pragma surface surf Comic fullforwardshadows

		uniform half4 _Color;
		uniform sampler2D _MainTex;
		uniform sampler2D _NormalMap;
		uniform sampler2D _ShadowMap;
		uniform half _ShadowIntensity;
		uniform half _Shading;
		uniform half _ShadowSoftness;


		// Custom comic surface output
		struct ComicSurfaceOutput {
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Specular;
			half Alpha;
			half3 GlossColor;
			half3 ShadowTex;
		};

		half diffuseCalc(half3 a, half3 b) {
			// Ignore backface
			return dot(normalize(a), normalize(b)) * 0.5f + 0.5f;
		}

		half softStep(half a, half b, half minimum, half maximum, half smooth) {
			half soft = (clamp(b - a, 0, 1) / (pow(smooth + 1, 10) / 1000));
			return clamp(soft, minimum, maximum);
		}

		half4 LightingComic(ComicSurfaceOutput o, half3 lightDir, half3 viewDir, half atten) {
			
			half d = diffuseCalc(o.Normal, lightDir);									// Diffuse multiplier (create shadows from normal information)

			// Toon shading (clamp shading and lighting)
			half toonD = softStep(_Shading, min(d, atten), 1 - _ShadowIntensity, 1, _ShadowSoftness);

			// Calculate shadow texture
			//if(toonD < 1)
			//	toonD = saturate((1 - toonD) * o.ShadowTex + toonD);

			half3 diffuseColor = _LightColor0 * o.Albedo * toonD;

			return half4(diffuseColor, o.Alpha);

		}

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float3 viewDir;
			float4 screenPos;
		};

		void surf(Input IN, inout ComicSurfaceOutput o) {
			
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half3 n = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
			half nAngle = dot(IN.viewDir, n);

			o.Albedo = c.rgb;

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			o.ShadowTex = tex2D(_ShadowMap, screenUV).rgb;

			o.Normal = n;
			o.Alpha = c.a;

		}

		ENDCG
	}

	FallBack "Diffuse"
}
