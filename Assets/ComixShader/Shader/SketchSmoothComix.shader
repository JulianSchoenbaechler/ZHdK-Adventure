Shader "Comix/Sketch Smooth" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "" {}
		_SpecMap("Specular Map", 2D) = "white" {}
		_SpecIntensity("Specular Intensity", Range(0,1)) = 1
		_ShadowMap("Shadow Map", 2D) = "black" {}
		_ShadowIntensity("Shadow Intensity", Range(0,1)) = 1
		_Glossiness("Gloss", Range(1,20)) = 10.0
		_Shading("Shadow Threshold", Range(0,1)) = 0.5
		_Lighting("Specular Threshold", Range(0,1)) = 0.5
		_Outline("Outline", Range(0,1)) = 0.1
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_ShadowSoftness("Shadow Softness", Range(0,1)) = 0.2
		_SpecSoftness("Specular Softness", Range(0,1)) = 0.2
		_OutlineSoftness("Outline Softness", Range(0,1)) = 0.2
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		// Custom Comic lighting model, and enable shadows on all light types
		#pragma surface surf Comic fullforwardshadows

		uniform half4 _Color;
		uniform sampler2D _MainTex;
		uniform sampler2D _NormalMap;
		uniform sampler2D _SpecMap;
		uniform half _SpecIntensity;
		uniform sampler2D _ShadowMap;
		uniform half _ShadowIntensity;
		uniform half _Glossiness;
		uniform half _Shading;
		uniform half _Lighting;
		uniform half _Outline;
		uniform half4 _OutlineColor;
		uniform half _ShadowSoftness;
		uniform half _SpecSoftness;
		uniform half _OutlineSoftness;


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
			half s = pow(diffuseCalc(o.Normal, lightDir + viewDir), _Glossiness);		// Diffuse multiplier (create specular from light and view information)

			// Toon shading (clamp shading and lighting)
			half toonD = softStep(_Shading, min(d, atten), 1 - _ShadowIntensity, 1, _ShadowSoftness);
			half toonS = softStep(_Lighting, s, 0, 1, _SpecSoftness);

			// Calculate shadow texture
			if(toonD < 1)
				toonD = saturate((1 - toonD) * o.ShadowTex + toonD);

			half3 diffuseColor = _LightColor0 * o.Albedo * toonD;
			half3 specularColor = _LightColor0 * o.GlossColor * toonS;
			half3 finalColor = (diffuseColor + specularColor);

			return half4(finalColor, o.Alpha);

		}

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float4 screenPos;
		};

		void surf(Input IN, inout ComicSurfaceOutput o) {
			
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half3 n = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			half4 s = tex2D(_SpecMap, IN.uv_MainTex) * half4(_SpecIntensity, _SpecIntensity, _SpecIntensity, 1);
			half nAngle = dot(IN.viewDir, n);

			// Outline:
			// outline								-> outline mask (outline = black, color = white)
			//											this will create a black border specified by nAngle
			// lerp between color and outline color	-> invert mask and use it as interpolate factor
			if(_Outline > 0) {
				half outline = softStep(_Outline, nAngle, 0, 1, _OutlineSoftness);
				o.Albedo = lerp(c.rgb, _OutlineColor, 1 - outline);
			}
			else
			{
				o.Albedo = c.rgb;
			}

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			o.ShadowTex = tex2D(_ShadowMap, screenUV).rgb;

			o.Normal = n;
			o.GlossColor = s.rgb;
			o.Alpha = c.a;

		}

		ENDCG
	}

	FallBack "Diffuse"
}
