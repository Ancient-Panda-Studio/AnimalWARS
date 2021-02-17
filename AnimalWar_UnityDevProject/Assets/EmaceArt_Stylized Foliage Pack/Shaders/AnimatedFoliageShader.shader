Shader "EmaceArt/Animated Foliage"
{
	//Shader by Bartlomiej Gadzala
	//b.gadzala33@gmail.com
    Properties
    {
		_BaseColor("Base Color", Color) = (0.5, 0.5, 0.5, 1)
		[NoScaleOffset] _BaseMap("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}
		_Cutoff("Alpha Clipping", Range(0.0, 1.0)) = 0.5
		_TranslucentRange("Transluscent range", Range(-1.0, 0.0)) = 0.05
		_TranslucentScale("Transluscent power", Range(0.5, 8.0)) = 2.0
		_WindInfluence("Wind Influence", Range(0 , 1)) = 1
		[Toggle(_WINDENABLED_ON)] _WindEnabled ("Wind Enabled", Float) = 1
		[Toggle(_RECONSTRUCTNORMAL_ON)] _ReconstructNormal("Reconstruct Normal", Float) = 1
		[NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
		[NoScaleOffset] _TransMap("Transluscent Map", 2D) = "black" {}
		[NoScaleOffset] _ToonRamp("Toon Ramp", 2D) = "white" {}
		[NoScaleOffset] _SSSRamp("Transluscent ramp", 2D) = "white" {}

		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadows("Receive Shadows", Float) = 1.0
    }
    SubShader
    {
		Tags{"RenderType" = "Opaque" "Queue" = "Geometry+200" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" "DisableBatching" = "True"}
		LOD 300

		HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "FoliageWindVertexFunction.hlsl"
		ENDHLSL

		CGINCLUDE
		#include "UnityCG.cginc"
		

		ENDCG
			

        Pass
        {
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}

			ZWrite On
			Cull Off
	
			HLSLPROGRAM

			#pragma exclude_renderers d3d11_9x
			#pragma prefer_hlslcc gles
			#pragma target 3.0
			//
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			#pragma shader_feature _RECEIVE_SHADOWS_OFF
			#pragma shader_feature_local _RECONSTRUCTNORMAL_ON
			#pragma shader_feature_local _WINDENABLED_ON
			#pragma shader_feature _NOISEDEBUG
			#pragma multi_compile_fog
			//
			// GPU Instancing
			#pragma multi_compile_instancing
			#pragma multi_compile __ _GLOBALWIND

			#pragma vertex Vert
			#pragma fragment Frag
			



			CBUFFER_START(UnityPerMaterial)
				uniform float _WindInfluence;
				uniform sampler2D _BaseMap;
				uniform half4 _BaseColor;
				uniform half _Cutoff;
				uniform sampler2D _BumpMap;
				uniform sampler2D _TransMap;
				uniform sampler2D _ToonRamp;
				uniform half _TranslucentRange;
				uniform half _TranslucentScale;
				uniform sampler2D _SSSRamp;
				
			CBUFFER_END




			inline void getTransuluscentLight(Light light, float3 normalWS, inout half3 mergedLight) 
			{

				half3 lightDir = light.direction * (-1.0f);
				half3 lightColor = light.color.rgb;
				#ifndef _RECEIVE_SHADOWS_OFF
					half atten = (1.0f - saturate(light.shadowAttenuation)) + light.distanceAttenuation;
				#else
					half atten = light.distanceAttenuation;
				#endif

				half ndl = pow(abs(max(_TranslucentRange, dot(normalWS, lightDir))), _TranslucentScale);
				half3 toonRampTex = tex2D(_SSSRamp, float2(ndl, 0.5f)).rgb;

				mergedLight += lightColor * toonRampTex * atten;
			}
			inline void getDiffuseLight(Light light, float3 normalWS, inout half3 mergedLight)
			{

				half3 lightDir = light.direction;
				half3 lightColor = light.color.rgb;
				#ifndef _RECEIVE_SHADOWS_OFF
					half atten = light.shadowAttenuation * light.distanceAttenuation;
				#else
					half atten = light.distanceAttenuation;
				#endif
				half ndl = max(0, dot(normalWS, lightDir)*0.5f + 0.5f);
				half3 toonRampTex = tex2D(_ToonRamp, float2((ndl * atten), 0.5f)).rgb;

				mergedLight += lightColor * toonRampTex * atten;
			}

			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
				half4 vertexColors : COLOR;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};


			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
				float2 texcoords : TEXCOORD0;
			#ifdef _MAIN_LIGHT_SHADOWS
					float4 shadowCoord    : TEXCOORD1; 
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
					half3 vertexLights : TEXCOORD2;
			#endif
				float3 tangent : TEXCOORD3;
				float3 bitangent : TEXCOORD4;
				float4 worldPosAndFog : TEXCOORD5;
			#ifdef _NOISEDEBUG
				half4 debugColors: TEXCOORD6;
			#endif

				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings Vert(Attributes input)
			{
				Varyings output;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.texcoords.xy = input.texcoord0.xy;

				#if(defined _GLOBALWIND && defined _WINDENABLED_ON)
					#ifdef _RECONSTRUCTNORMAL_ON
						float3 newNormal = input.normal;
						float3 newTangent = input.tangent.xyz;
						float3 vertexWorldPos;
						#ifndef _NOISEDEBUG
							newWolrdNormalPosMain(input.vertex.xyz, input.vertexColors, _WindInfluence, newNormal, newTangent, vertexWorldPos);
						#else
							half4 debugColors;
							newWolrdNormalPosMain(input.vertex.xyz, input.vertexColors, _WindInfluence, newNormal, newTangent, vertexWorldPos, debugColors);
							output.debugColors = debugColors;
						#endif
						float4 vertexClipPos = newVertexClipPos(vertexWorldPos);
					#else
						float3 newNormal = TransformObjectToWorldNormal(input.normal);
						float3 newTangent = TransformObjectToWorldDir(input.tangent.xyz);
						#ifndef _NOISEDEBUG
							float3 vertexWorldPos = newVertexWorldPos(input.vertex.xyz, input.vertexColors, _WindInfluence);
						#else
							half4 debugColors;
							float3 vertexWorldPos = newVertexWorldPos(input.vertex.xyz, input.vertexColors, _WindInfluence, debugColors);
							output.debugColors = debugColors;
						#endif
						float4 vertexClipPos = newVertexClipPos(vertexWorldPos);
					#endif
				#else
					float3 newNormal = TransformObjectToWorldNormal(input.normal);
					float3 vertexWorldPos = TransformObjectToWorld(input.vertex.xyz);
					float4 vertexClipPos = TransformObjectToHClip(input.vertex.xyz);
					float3 newTangent = TransformObjectToWorldDir(input.tangent.xyz);
				#endif



				#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
					#if SHADOWS_SCREEN
						float4 shadowCoords = ComputeScreenPos(vertexClipPos);
					#else
						float4 shadowCoords = TransformWorldToShadowCoord(vertexWorldPos);
					#endif
						output.shadowCoord = shadowCoords;
				#endif

			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				// Vertex lighting
				output.vertexLights = VertexLighting(vertexWorldPos vettexNormalWs);
			#endif
				output.worldPosAndFog = float4(vertexWorldPos, ComputeFogFactor(vertexClipPos.z));

				// normal
				output.normal = newNormal;
				output.tangent = newTangent;
				output.bitangent = cross(newTangent, newNormal);

				// clip position
				output.positionCS = vertexClipPos;


				return output;
			}
				half4 Frag(Varyings input, half vface : VFACE) : SV_Target
				{
					//
					UNITY_SETUP_INSTANCE_ID(input);
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

					float3 positionWS = input.worldPosAndFog.xyz;
					float2 uvs = input.texcoords.xy;
					float4 normalMap = tex2D(_BumpMap, uvs);

					half3 normalTS = UnpackNormal(normalMap).rgb;
					half3 normalWS = TransformTangentToWorld(normalTS, half3x3(input.tangent,  input.bitangent, input.normal)) ;
					half3 viewDirWS = SafeNormalize(GetCameraPositionWS() - positionWS);

					#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
						Light mainLight = GetMainLight(input.shadowCoord);
					#else
						Light mainLight = GetMainLight();
					#endif

					half4 baseColorAndTex = tex2D(_BaseMap, uvs) * _BaseColor;

					half3 transLight = half3(0.0f, 0.0f, 0.0f);
					half3 spec = half3(0.0f, 0.0f, 0.0f);
					half3 diffuseLight = half3(0.0f, 0.0f, 0.0f);

					getDiffuseLight(mainLight, normalWS,diffuseLight);
					getTransuluscentLight(mainLight, normalWS, transLight);

#ifdef _ADDITIONAL_LIGHTS
				int additionalLightsCount = GetAdditionalLightsCount();
				for (int i = 0; i < additionalLightsCount; ++i)
				{
					Light light = GetAdditionalLight(i, positionWS);
					getDiffuseLight(light, normalWS,diffuseLight);
					getTransuluscentLight(light, normalWS, transLight);
				}
#endif
#ifdef _ADDITIONAL_LIGHTS_VERTEX
				diffuseLight += input.vertexLights;
#endif
				half3 giSampled = SampleSH(normalWS);
				float bakedGiDot = dot(giSampled, float3(0.299, 0.587, 0.114));
				half3 bakedGI = tex2D(_ToonRamp, float2(bakedGiDot, 0.5f)).rgb * giSampled;
				float transMap = tex2D(_TransMap, uvs).r;

				half4 result =  half4( max((transLight.rgb * transMap + bakedGI + diffuseLight) * baseColorAndTex.rgb, spec),1.0);
				clip(baseColorAndTex.a - _Cutoff);
				result.rgb = MixFogColor(result.rgb , unity_FogColor.rgb, input.worldPosAndFog.w);
				#ifdef _NOISEDEBUG
					result.rgb = input.debugColors.rgb;
				#endif
				return result;
				}
				ENDHLSL
        }



		Pass
		{
			Name "ShadowCaster"
			Tags{"LightMode" = "ShadowCaster"}

			ZWrite On
			ZTest LEqual
			Cull Off

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// -------------------------------------
			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			//#pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment
			#pragma multi_compile __ _GLOBALWIND
			#pragma shader_feature_local _WINDENABLED_ON
			#pragma shader_feature_local _RECONSTRUCTNORMAL_ON

			CBUFFER_START(UnityPerMaterial)
				uniform float _WindInfluence;
				uniform sampler2D _BaseMap;
				uniform half _Cutoff;
			CBUFFER_END

			#include "FoliageShadowPass.hlsl"
			ENDHLSL
		}

			Pass
		{
			Name "DepthOnly"
			Tags{"LightMode" = "DepthOnly"}

			ZWrite On
			ColorMask 0
			Cull Off

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			#pragma vertex DepthOnlyVertex
			#pragma fragment DepthOnlyFragment
			#pragma multi_compile __ _GLOBALWIND
			#pragma shader_feature_local _WINDENABLED_ON
			#pragma shader_feature_local _RECONSTRUCTNORMAL_ON
			

			CBUFFER_START(UnityPerMaterial)
				uniform float _WindInfluence;
				uniform sampler2D _BaseMap;
				uniform half _Cutoff;
			CBUFFER_END

			// -------------------------------------
			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			//#pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#include "FoliageDepthPass.hlsl"
			ENDHLSL
		}

    }
}
