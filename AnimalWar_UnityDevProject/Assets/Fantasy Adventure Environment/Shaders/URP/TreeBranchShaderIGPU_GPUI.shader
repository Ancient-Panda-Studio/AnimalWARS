Shader "GPUInstancer/PBR Master"
{
    Properties
    {
        _Cutoff("Alpha clipping", Range(0, 1)) = 0.5
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _HueVariation("Hue Variation", Color) = (1, 0.6213608, 0, 0.1686275)
        _TransmissionColor("Transmission Color", Color) = (1, 1, 1, 0.09411765)
        _AmbientOcclusion("Ambient Occlusion", Range(0, 1)) = 1
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _FlatLighting("Flat Lighting", Range(0, 1)) = 0
        _GradientBrightness("Gradient Brightness", Range(0, 2)) = 1
        _MaxWindStrength("Max Wind Strength", Range(0, 1)) = 1
        _WindAmplitudeMultiplier("Wind Amplitude Multiplier", Float) = 1
        [ToggleUI]_UseSpeedTreeWind("Use SpeedTree wind", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry+0"
        }
        
        Pass
        {
            Name "Universal Forward"
            Tags 
            { 
                "LightMode" = "UniversalForward"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Off
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_FORWARD
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float _Cutoff;
            float4 _HueVariation;
            float4 _TransmissionColor;
            float _AmbientOcclusion;
            float _Smoothness;
            float _FlatLighting;
            float _GradientBrightness;
            float _MaxWindStrength;
            float _WindAmplitudeMultiplier;
            float _UseSpeedTreeWind;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_E53DEB40_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            // baf344beb4069bc11e37cba678d5ea13
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/FAE.hlsl"
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Divide_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A / B;
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Sine_float3(float3 In, out float3 Out)
            {
                Out = sin(In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Branch_float(float Predicate, float True, float False, out float Out)
            {
                Out = Predicate ? True : False;
            }
            
            struct Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00
            {
                float3 WorldSpaceNormal;
                float3 WorldSpaceTangent;
                float3 WorldSpaceBiTangent;
                half4 uv1;
            };
            
            void SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(float Boolean_BA09D051, float Vector1_1EEB22DF, Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 IN, out float3 Offset_1)
            {
                float4 _CustomFunction_F763B077_windDir_0;
                float _CustomFunction_F763B077_trunkSpeed_1;
                float _CustomFunction_F763B077_trunkSwinging_3;
                float _CustomFunction_F763B077_trunkWeight_4;
                float _CustomFunction_F763B077_windSpeed_2;
                GetGlobalParams_float(_CustomFunction_F763B077_windDir_0, _CustomFunction_F763B077_trunkSpeed_1, _CustomFunction_F763B077_trunkSwinging_3, _CustomFunction_F763B077_trunkWeight_4, _CustomFunction_F763B077_windSpeed_2);
                float3 _Transform_408DEDF1_Out_1 = TransformWorldToObjectDir((_CustomFunction_F763B077_windDir_0.xyz).xyz);
                float _Split_7E1333CF_R_1 = _Transform_408DEDF1_Out_1[0];
                float _Split_7E1333CF_G_2 = _Transform_408DEDF1_Out_1[1];
                float _Split_7E1333CF_B_3 = _Transform_408DEDF1_Out_1[2];
                float _Split_7E1333CF_A_4 = 0;
                float4 _Combine_2DC33859_RGBA_4;
                float3 _Combine_2DC33859_RGB_5;
                float2 _Combine_2DC33859_RG_6;
                Unity_Combine_float(_Split_7E1333CF_R_1, 0, _Split_7E1333CF_B_3, 0, _Combine_2DC33859_RGBA_4, _Combine_2DC33859_RGB_5, _Combine_2DC33859_RG_6);
                float3 _Divide_CBE4D3A1_Out_2;
                Unity_Divide_float3((_CustomFunction_F763B077_trunkSpeed_1.xxx), float3(length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
                                         length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
                                         length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z))), _Divide_CBE4D3A1_Out_2);
                float3 _Multiply_A1BE4F17_Out_2;
                Unity_Multiply_float((_CustomFunction_F763B077_windSpeed_2.xxx), _Divide_CBE4D3A1_Out_2, _Multiply_A1BE4F17_Out_2);
                float3 _Multiply_91A2ECE2_Out_2;
                Unity_Multiply_float(_Combine_2DC33859_RGB_5, _Multiply_A1BE4F17_Out_2, _Multiply_91A2ECE2_Out_2);
                float3 _Sine_DEEB684D_Out_1;
                Unity_Sine_float3(_Multiply_91A2ECE2_Out_2, _Sine_DEEB684D_Out_1);
                float _Vector1_27C730_Out_0 = 0.5;
                float3 _Multiply_1BC85160_Out_2;
                Unity_Multiply_float(_Sine_DEEB684D_Out_1, (_Vector1_27C730_Out_0.xxx), _Multiply_1BC85160_Out_2);
                float3 _Add_36F313F8_Out_2;
                Unity_Add_float3(_Multiply_1BC85160_Out_2, (_Vector1_27C730_Out_0.xxx), _Add_36F313F8_Out_2);
                float3 _Lerp_1BCD68EF_Out_3;
                Unity_Lerp_float3(_Add_36F313F8_Out_2, _Sine_DEEB684D_Out_1, (_CustomFunction_F763B077_trunkSwinging_3.xxx), _Lerp_1BCD68EF_Out_3);
                float3 _Multiply_69E4A4AF_Out_2;
                Unity_Multiply_float(_Lerp_1BCD68EF_Out_3, (_CustomFunction_F763B077_trunkWeight_4.xxx), _Multiply_69E4A4AF_Out_2);
                float _Property_22D46B5A_Out_0 = Boolean_BA09D051;
                float4 _UV_44C52F2E_Out_0 = IN.uv1;
                float _Split_5A3BE0BE_R_1 = _UV_44C52F2E_Out_0[0];
                float _Split_5A3BE0BE_G_2 = _UV_44C52F2E_Out_0[1];
                float _Split_5A3BE0BE_B_3 = _UV_44C52F2E_Out_0[2];
                float _Split_5A3BE0BE_A_4 = _UV_44C52F2E_Out_0[3];
                float _Multiply_F32B98EF_Out_2;
                Unity_Multiply_float(_Split_5A3BE0BE_G_2, 0.01, _Multiply_F32B98EF_Out_2);
                float _Property_CD92AE06_Out_0 = Vector1_1EEB22DF;
                float _Branch_F33FDE09_Out_3;
                Unity_Branch_float(_Property_22D46B5A_Out_0, _Multiply_F32B98EF_Out_2, _Property_CD92AE06_Out_0, _Branch_F33FDE09_Out_3);
                float3 _Multiply_3473FC0B_Out_2;
                Unity_Multiply_float(_Multiply_69E4A4AF_Out_2, (_Branch_F33FDE09_Out_3.xxx), _Multiply_3473FC0B_Out_2);
                float _Split_6848EEB6_R_1 = _Multiply_3473FC0B_Out_2[0];
                float _Split_6848EEB6_G_2 = _Multiply_3473FC0B_Out_2[1];
                float _Split_6848EEB6_B_3 = _Multiply_3473FC0B_Out_2[2];
                float _Split_6848EEB6_A_4 = 0;
                float4 _Combine_FDFF0983_RGBA_4;
                float3 _Combine_FDFF0983_RGB_5;
                float2 _Combine_FDFF0983_RG_6;
                Unity_Combine_float(_Split_6848EEB6_R_1, 0, _Split_6848EEB6_B_3, 0, _Combine_FDFF0983_RGBA_4, _Combine_FDFF0983_RGB_5, _Combine_FDFF0983_RG_6);
                Offset_1 = _Combine_FDFF0983_RGB_5;
            }
            
            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
            
            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }
            
            struct Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a
            {
                float3 AbsoluteWorldSpacePosition;
            };
            
            void SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(float Vector1_76290B08, float Vector1_C9D81F7C, float Vector1_2A7979D, Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a IN, out float3 Offset_1)
            {
                float _Property_AB75A479_Out_0 = Vector1_2A7979D;
                float _Property_20872C63_Out_0 = Vector1_76290B08;
                float4 _CustomFunction_FB25DD51_windDir_0;
                float _CustomFunction_FB25DD51_trunkSpeed_1;
                float _CustomFunction_FB25DD51_trunkSwinging_3;
                float _CustomFunction_FB25DD51_trunkWeight_4;
                float _CustomFunction_FB25DD51_windSpeed_2;
                float _CustomFunction_FB25DD51_windFreq_7;
                float _CustomFunction_FB25DD51_windStrength_8;
                GetLocalParams_float(IN.AbsoluteWorldSpacePosition, _Property_20872C63_Out_0, _CustomFunction_FB25DD51_windDir_0, _CustomFunction_FB25DD51_trunkSpeed_1, _CustomFunction_FB25DD51_trunkSwinging_3, _CustomFunction_FB25DD51_trunkWeight_4, _CustomFunction_FB25DD51_windSpeed_2, _CustomFunction_FB25DD51_windFreq_7, _CustomFunction_FB25DD51_windStrength_8);
                float _Split_C6A87A8F_R_1 = _CustomFunction_FB25DD51_windDir_0[0];
                float _Split_C6A87A8F_G_2 = _CustomFunction_FB25DD51_windDir_0[1];
                float _Split_C6A87A8F_B_3 = _CustomFunction_FB25DD51_windDir_0[2];
                float _Split_C6A87A8F_A_4 = _CustomFunction_FB25DD51_windDir_0[3];
                float4 _Combine_51E3AC00_RGBA_4;
                float3 _Combine_51E3AC00_RGB_5;
                float2 _Combine_51E3AC00_RG_6;
                Unity_Combine_float(_Split_C6A87A8F_R_1, _Split_C6A87A8F_B_3, 0, 0, _Combine_51E3AC00_RGBA_4, _Combine_51E3AC00_RGB_5, _Combine_51E3AC00_RG_6);
                float2 _Multiply_34407C61_Out_2;
                Unity_Multiply_float(_Combine_51E3AC00_RG_6, (_CustomFunction_FB25DD51_windSpeed_2.xx), _Multiply_34407C61_Out_2);
                float2 _Add_F12CDF4E_Out_2;
                Unity_Add_float2(_Multiply_34407C61_Out_2, (_CustomFunction_FB25DD51_windFreq_7.xx), _Add_F12CDF4E_Out_2);
                float3 _CustomFunction_456640C0_vec_1;
                SampleWind_float((_Add_F12CDF4E_Out_2).x, _CustomFunction_456640C0_vec_1);
                float3 _Multiply_EC8FCAF4_Out_2;
                Unity_Multiply_float((_Property_AB75A479_Out_0.xxx), _CustomFunction_456640C0_vec_1, _Multiply_EC8FCAF4_Out_2);
                float3 _Multiply_611DED0C_Out_2;
                Unity_Multiply_float(_Multiply_EC8FCAF4_Out_2, (_CustomFunction_FB25DD51_windStrength_8.xxx), _Multiply_611DED0C_Out_2);
                float _Property_806E9BFF_Out_0 = Vector1_C9D81F7C;
                float3 _Multiply_45DFFD0A_Out_2;
                Unity_Multiply_float(_Multiply_611DED0C_Out_2, (_Property_806E9BFF_Out_0.xxx), _Multiply_45DFFD0A_Out_2);
                Offset_1 = _Multiply_45DFFD0A_Out_2;
            }
            
            // f18489b7b4b05a83941c32070dcf5796
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/VSPro_HDIndirect.cginc"
            
            void AddPragma_float(float3 A, out float3 Out)
            {
                Out = A;
            }
            
            struct Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32
            {
            };
            
            void SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(float3 Vector3_314C8600, Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 IN, out float3 ObjectSpacePosition_1)
            {
                float3 _Property_AF5E8C93_Out_0 = Vector3_314C8600;
                float3 _CustomFunction_E07FEE57_Out_1;
                InjectSetup_float(_Property_AF5E8C93_Out_0, _CustomFunction_E07FEE57_Out_1);
                float3 _CustomFunction_18EFD858_Out_1;
                AddPragma_float(_CustomFunction_E07FEE57_Out_1, _CustomFunction_18EFD858_Out_1);
                ObjectSpacePosition_1 = _CustomFunction_18EFD858_Out_1;
            }
            
            void Unity_Lerp_float(float A, float B, float T, out float Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Length_float3(float3 In, out float Out)
            {
                Out = length(In);
            }
            
            void Unity_Fraction_float(float In, out float Out)
            {
                Out = frac(In);
            }
            
            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
            
            void Unity_Normalize_half3(half3 In, out half3 Out)
            {
                Out = normalize(In);
            }
            
            void Unity_DotProduct_float3(float3 A, float3 B, out float Out)
            {
                Out = dot(A, B);
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 WorldSpaceTangent;
                float3 ObjectSpaceBiTangent;
                float3 WorldSpaceBiTangent;
                float3 ObjectSpacePosition;
                float3 AbsoluteWorldSpacePosition;
                float4 uv1;
                float4 VertexColor;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Property_DB0143B3_Out_0 = _UseSpeedTreeWind;
                float _Split_A80EDD15_R_1 = IN.VertexColor[0];
                float _Split_A80EDD15_G_2 = IN.VertexColor[1];
                float _Split_A80EDD15_B_3 = IN.VertexColor[2];
                float _Split_A80EDD15_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_2543C842;
                _GlobalTreeWindMotion_2543C842.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_2543C842.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_2543C842.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_2543C842.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_2543C842_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(_Property_DB0143B3_Out_0, _Split_A80EDD15_A_4, _GlobalTreeWindMotion_2543C842, _GlobalTreeWindMotion_2543C842_Offset_1);
                float _Property_94051D9D_Out_0 = _WindAmplitudeMultiplier;
                float _Property_FA421571_Out_0 = _MaxWindStrength;
                Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a _LocalWindMotion_496CCFEB;
                _LocalWindMotion_496CCFEB.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                float3 _LocalWindMotion_496CCFEB_Offset_1;
                SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(_Property_94051D9D_Out_0, _Property_FA421571_Out_0, _Split_A80EDD15_G_2, _LocalWindMotion_496CCFEB, _LocalWindMotion_496CCFEB_Offset_1);
                float3 _Add_59B53B15_Out_2;
                Unity_Add_float3(_GlobalTreeWindMotion_2543C842_Offset_1, _LocalWindMotion_496CCFEB_Offset_1, _Add_59B53B15_Out_2);
                float3 _Add_DF77ECB8_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Add_59B53B15_Out_2, _Add_DF77ECB8_Out_2);
                Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 _VSProHDInstancedIndirect_A8293AE2;
                float3 _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(_Add_DF77ECB8_Out_2, _VSProHDInstancedIndirect_A8293AE2, _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1);
                float3 _Vector3_B7A96EB5_Out_0 = float3(0, 1, 0);
                float _Property_A8C540BD_Out_0 = _FlatLighting;
                float3 _Lerp_FAE7CF7_Out_3;
                Unity_Lerp_float3(IN.ObjectSpaceNormal, _Vector3_B7A96EB5_Out_0, (_Property_A8C540BD_Out_0.xxx), _Lerp_FAE7CF7_Out_3);
                description.VertexPosition = _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                description.VertexNormal = _Lerp_FAE7CF7_Out_3;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float3 AbsoluteWorldSpacePosition;
                float4 uv0;
                float4 VertexColor;
            };
            
            struct SurfaceDescription
            {
                float3 Albedo;
                float3 Normal;
                float3 Emission;
                float Metallic;
                float Smoothness;
                float Occlusion;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Split_18BD7240_R_1 = IN.VertexColor[0];
                float _Split_18BD7240_G_2 = IN.VertexColor[1];
                float _Split_18BD7240_B_3 = IN.VertexColor[2];
                float _Split_18BD7240_A_4 = IN.VertexColor[3];
                float _Property_EDE257A8_Out_0 = _AmbientOcclusion;
                float _Lerp_418DAFB9_Out_3;
                Unity_Lerp_float(1, _Split_18BD7240_R_1, _Property_EDE257A8_Out_0, _Lerp_418DAFB9_Out_3);
                float _Property_9160F69B_Out_0 = _GradientBrightness;
                float4 _SampleTexture2D_E53DEB40_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_E53DEB40_R_4 = _SampleTexture2D_E53DEB40_RGBA_0.r;
                float _SampleTexture2D_E53DEB40_G_5 = _SampleTexture2D_E53DEB40_RGBA_0.g;
                float _SampleTexture2D_E53DEB40_B_6 = _SampleTexture2D_E53DEB40_RGBA_0.b;
                float _SampleTexture2D_E53DEB40_A_7 = _SampleTexture2D_E53DEB40_RGBA_0.a;
                float4 _Property_1C2D047C_Out_0 = _HueVariation;
                float _Split_4BC9A866_R_1 = _Property_1C2D047C_Out_0[0];
                float _Split_4BC9A866_G_2 = _Property_1C2D047C_Out_0[1];
                float _Split_4BC9A866_B_3 = _Property_1C2D047C_Out_0[2];
                float _Split_4BC9A866_A_4 = _Property_1C2D047C_Out_0[3];
                float _Length_5B09488C_Out_1;
                Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_5B09488C_Out_1);
                float _Fraction_73CC485B_Out_1;
                Unity_Fraction_float(_Length_5B09488C_Out_1, _Fraction_73CC485B_Out_1);
                float _Multiply_BFA62608_Out_2;
                Unity_Multiply_float(_Split_4BC9A866_A_4, _Fraction_73CC485B_Out_1, _Multiply_BFA62608_Out_2);
                float4 _Lerp_8219D562_Out_3;
                Unity_Lerp_float4(_SampleTexture2D_E53DEB40_RGBA_0, _Property_1C2D047C_Out_0, (_Multiply_BFA62608_Out_2.xxxx), _Lerp_8219D562_Out_3);
                float4 _Multiply_9984BC3D_Out_2;
                Unity_Multiply_float((_Property_9160F69B_Out_0.xxxx), _Lerp_8219D562_Out_3, _Multiply_9984BC3D_Out_2);
                float _Multiply_116AF097_Out_2;
                Unity_Multiply_float(_Split_18BD7240_A_4, 10, _Multiply_116AF097_Out_2);
                float _Saturate_338C849F_Out_1;
                Unity_Saturate_float(_Multiply_116AF097_Out_2, _Saturate_338C849F_Out_1);
                float4 _Lerp_82D3B781_Out_3;
                Unity_Lerp_float4(_Multiply_9984BC3D_Out_2, _Lerp_8219D562_Out_3, (_Saturate_338C849F_Out_1.xxxx), _Lerp_82D3B781_Out_3);
                float4 _Multiply_5840E19_Out_2;
                Unity_Multiply_float((_Lerp_418DAFB9_Out_3.xxxx), _Lerp_82D3B781_Out_3, _Multiply_5840E19_Out_2);
                half3 _CustomFunction_5F04B9C7_Direction_1;
                half3 _CustomFunction_5F04B9C7_Color_2;
                half _CustomFunction_5F04B9C7_DistanceAtten_3;
                half _CustomFunction_5F04B9C7_ShadowAtten_4;
                MainLight_half(IN.AbsoluteWorldSpacePosition, _CustomFunction_5F04B9C7_Direction_1, _CustomFunction_5F04B9C7_Color_2, _CustomFunction_5F04B9C7_DistanceAtten_3, _CustomFunction_5F04B9C7_ShadowAtten_4);
                float4 _Property_5AA1B37C_Out_0 = _TransmissionColor;
                float3 _Multiply_DBDA6F5E_Out_2;
                Unity_Multiply_float((_Property_5AA1B37C_Out_0.xyz), _CustomFunction_5F04B9C7_Color_2, _Multiply_DBDA6F5E_Out_2);
                float _Split_B84DA633_R_1 = _Property_5AA1B37C_Out_0[0];
                float _Split_B84DA633_G_2 = _Property_5AA1B37C_Out_0[1];
                float _Split_B84DA633_B_3 = _Property_5AA1B37C_Out_0[2];
                float _Split_B84DA633_A_4 = _Property_5AA1B37C_Out_0[3];
                float _Split_A80EDD15_R_1 = IN.VertexColor[0];
                float _Split_A80EDD15_G_2 = IN.VertexColor[1];
                float _Split_A80EDD15_B_3 = IN.VertexColor[2];
                float _Split_A80EDD15_A_4 = IN.VertexColor[3];
                half3 _Normalize_B7264509_Out_1;
                Unity_Normalize_half3(_CustomFunction_5F04B9C7_Direction_1, _Normalize_B7264509_Out_1);
                float _DotProduct_49DA6A99_Out_2;
                Unity_DotProduct_float3(_Normalize_B7264509_Out_1, -1 * mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2].xyz), _DotProduct_49DA6A99_Out_2);
                float _Saturate_85160733_Out_1;
                Unity_Saturate_float(_DotProduct_49DA6A99_Out_2, _Saturate_85160733_Out_1);
                float _Multiply_54F7AAE7_Out_2;
                Unity_Multiply_float(_Split_A80EDD15_B_3, _Saturate_85160733_Out_1, _Multiply_54F7AAE7_Out_2);
                float _Multiply_9F998B54_Out_2;
                Unity_Multiply_float(_Split_B84DA633_A_4, _Multiply_54F7AAE7_Out_2, _Multiply_9F998B54_Out_2);
                float3 _Multiply_B2E8C645_Out_2;
                Unity_Multiply_float(_Multiply_DBDA6F5E_Out_2, (_Multiply_9F998B54_Out_2.xxx), _Multiply_B2E8C645_Out_2);
                float3 _Multiply_3A2FF337_Out_2;
                Unity_Multiply_float((_CustomFunction_5F04B9C7_ShadowAtten_4.xxx), _Multiply_B2E8C645_Out_2, _Multiply_3A2FF337_Out_2);
                float _Property_47BF7E6E_Out_0 = _Smoothness;
                float _Property_3912DE45_Out_0 = _Cutoff;
                surface.Albedo = (_Multiply_5840E19_Out_2.xyz);
                surface.Normal = IN.TangentSpaceNormal;
                surface.Emission = _Multiply_3A2FF337_Out_2;
                surface.Metallic = 0;
                surface.Smoothness = _Property_47BF7E6E_Out_0;
                surface.Occlusion = 1;
                surface.Alpha = _SampleTexture2D_E53DEB40_A_7;
                surface.AlphaClipThreshold = _Property_3912DE45_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float3 normalWS;
                float4 tangentWS;
                float4 texCoord0;
                float4 color;
                float3 viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                float2 lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                float3 sh;
                #endif
                float4 fogFactorAndVertexLight;
                float4 shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if defined(LIGHTMAP_ON)
                #endif
                #if !defined(LIGHTMAP_ON)
                #endif
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float3 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                float4 interp03 : TEXCOORD3;
                float4 interp04 : TEXCOORD4;
                float3 interp05 : TEXCOORD5;
                float2 interp06 : TEXCOORD6;
                float3 interp07 : TEXCOORD7;
                float4 interp08 : TEXCOORD8;
                float4 interp09 : TEXCOORD9;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyzw = input.color;
                output.interp05.xyz = input.viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                output.interp06.xy = input.lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.interp07.xyz = input.sh;
                #endif
                output.interp08.xyzw = input.fogFactorAndVertexLight;
                output.interp09.xyzw = input.shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.color = input.interp04.xyzw;
                output.viewDirectionWS = input.interp05.xyz;
                #if defined(LIGHTMAP_ON)
                output.lightmapUV = input.interp06.xy;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.sh = input.interp07.xyz;
                #endif
                output.fogFactorAndVertexLight = input.interp08.xyzw;
                output.shadowCoord = input.interp09.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                output.ObjectSpacePosition =         input.positionOS;
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionWS);
                output.uv0 =                         input.texCoord0;
                output.VertexColor =                 input.color;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
            
#include "./../../../GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Off
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_SHADOWCASTER
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float _Cutoff;
            float4 _HueVariation;
            float4 _TransmissionColor;
            float _AmbientOcclusion;
            float _Smoothness;
            float _FlatLighting;
            float _GradientBrightness;
            float _MaxWindStrength;
            float _WindAmplitudeMultiplier;
            float _UseSpeedTreeWind;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_E53DEB40_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            // baf344beb4069bc11e37cba678d5ea13
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/FAE.hlsl"
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Divide_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A / B;
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Sine_float3(float3 In, out float3 Out)
            {
                Out = sin(In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Branch_float(float Predicate, float True, float False, out float Out)
            {
                Out = Predicate ? True : False;
            }
            
            struct Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00
            {
                float3 WorldSpaceNormal;
                float3 WorldSpaceTangent;
                float3 WorldSpaceBiTangent;
                half4 uv1;
            };
            
            void SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(float Boolean_BA09D051, float Vector1_1EEB22DF, Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 IN, out float3 Offset_1)
            {
                float4 _CustomFunction_F763B077_windDir_0;
                float _CustomFunction_F763B077_trunkSpeed_1;
                float _CustomFunction_F763B077_trunkSwinging_3;
                float _CustomFunction_F763B077_trunkWeight_4;
                float _CustomFunction_F763B077_windSpeed_2;
                GetGlobalParams_float(_CustomFunction_F763B077_windDir_0, _CustomFunction_F763B077_trunkSpeed_1, _CustomFunction_F763B077_trunkSwinging_3, _CustomFunction_F763B077_trunkWeight_4, _CustomFunction_F763B077_windSpeed_2);
                float3 _Transform_408DEDF1_Out_1 = TransformWorldToObjectDir((_CustomFunction_F763B077_windDir_0.xyz).xyz);
                float _Split_7E1333CF_R_1 = _Transform_408DEDF1_Out_1[0];
                float _Split_7E1333CF_G_2 = _Transform_408DEDF1_Out_1[1];
                float _Split_7E1333CF_B_3 = _Transform_408DEDF1_Out_1[2];
                float _Split_7E1333CF_A_4 = 0;
                float4 _Combine_2DC33859_RGBA_4;
                float3 _Combine_2DC33859_RGB_5;
                float2 _Combine_2DC33859_RG_6;
                Unity_Combine_float(_Split_7E1333CF_R_1, 0, _Split_7E1333CF_B_3, 0, _Combine_2DC33859_RGBA_4, _Combine_2DC33859_RGB_5, _Combine_2DC33859_RG_6);
                float3 _Divide_CBE4D3A1_Out_2;
                Unity_Divide_float3((_CustomFunction_F763B077_trunkSpeed_1.xxx), float3(length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
                                         length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
                                         length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z))), _Divide_CBE4D3A1_Out_2);
                float3 _Multiply_A1BE4F17_Out_2;
                Unity_Multiply_float((_CustomFunction_F763B077_windSpeed_2.xxx), _Divide_CBE4D3A1_Out_2, _Multiply_A1BE4F17_Out_2);
                float3 _Multiply_91A2ECE2_Out_2;
                Unity_Multiply_float(_Combine_2DC33859_RGB_5, _Multiply_A1BE4F17_Out_2, _Multiply_91A2ECE2_Out_2);
                float3 _Sine_DEEB684D_Out_1;
                Unity_Sine_float3(_Multiply_91A2ECE2_Out_2, _Sine_DEEB684D_Out_1);
                float _Vector1_27C730_Out_0 = 0.5;
                float3 _Multiply_1BC85160_Out_2;
                Unity_Multiply_float(_Sine_DEEB684D_Out_1, (_Vector1_27C730_Out_0.xxx), _Multiply_1BC85160_Out_2);
                float3 _Add_36F313F8_Out_2;
                Unity_Add_float3(_Multiply_1BC85160_Out_2, (_Vector1_27C730_Out_0.xxx), _Add_36F313F8_Out_2);
                float3 _Lerp_1BCD68EF_Out_3;
                Unity_Lerp_float3(_Add_36F313F8_Out_2, _Sine_DEEB684D_Out_1, (_CustomFunction_F763B077_trunkSwinging_3.xxx), _Lerp_1BCD68EF_Out_3);
                float3 _Multiply_69E4A4AF_Out_2;
                Unity_Multiply_float(_Lerp_1BCD68EF_Out_3, (_CustomFunction_F763B077_trunkWeight_4.xxx), _Multiply_69E4A4AF_Out_2);
                float _Property_22D46B5A_Out_0 = Boolean_BA09D051;
                float4 _UV_44C52F2E_Out_0 = IN.uv1;
                float _Split_5A3BE0BE_R_1 = _UV_44C52F2E_Out_0[0];
                float _Split_5A3BE0BE_G_2 = _UV_44C52F2E_Out_0[1];
                float _Split_5A3BE0BE_B_3 = _UV_44C52F2E_Out_0[2];
                float _Split_5A3BE0BE_A_4 = _UV_44C52F2E_Out_0[3];
                float _Multiply_F32B98EF_Out_2;
                Unity_Multiply_float(_Split_5A3BE0BE_G_2, 0.01, _Multiply_F32B98EF_Out_2);
                float _Property_CD92AE06_Out_0 = Vector1_1EEB22DF;
                float _Branch_F33FDE09_Out_3;
                Unity_Branch_float(_Property_22D46B5A_Out_0, _Multiply_F32B98EF_Out_2, _Property_CD92AE06_Out_0, _Branch_F33FDE09_Out_3);
                float3 _Multiply_3473FC0B_Out_2;
                Unity_Multiply_float(_Multiply_69E4A4AF_Out_2, (_Branch_F33FDE09_Out_3.xxx), _Multiply_3473FC0B_Out_2);
                float _Split_6848EEB6_R_1 = _Multiply_3473FC0B_Out_2[0];
                float _Split_6848EEB6_G_2 = _Multiply_3473FC0B_Out_2[1];
                float _Split_6848EEB6_B_3 = _Multiply_3473FC0B_Out_2[2];
                float _Split_6848EEB6_A_4 = 0;
                float4 _Combine_FDFF0983_RGBA_4;
                float3 _Combine_FDFF0983_RGB_5;
                float2 _Combine_FDFF0983_RG_6;
                Unity_Combine_float(_Split_6848EEB6_R_1, 0, _Split_6848EEB6_B_3, 0, _Combine_FDFF0983_RGBA_4, _Combine_FDFF0983_RGB_5, _Combine_FDFF0983_RG_6);
                Offset_1 = _Combine_FDFF0983_RGB_5;
            }
            
            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
            
            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }
            
            struct Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a
            {
                float3 AbsoluteWorldSpacePosition;
            };
            
            void SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(float Vector1_76290B08, float Vector1_C9D81F7C, float Vector1_2A7979D, Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a IN, out float3 Offset_1)
            {
                float _Property_AB75A479_Out_0 = Vector1_2A7979D;
                float _Property_20872C63_Out_0 = Vector1_76290B08;
                float4 _CustomFunction_FB25DD51_windDir_0;
                float _CustomFunction_FB25DD51_trunkSpeed_1;
                float _CustomFunction_FB25DD51_trunkSwinging_3;
                float _CustomFunction_FB25DD51_trunkWeight_4;
                float _CustomFunction_FB25DD51_windSpeed_2;
                float _CustomFunction_FB25DD51_windFreq_7;
                float _CustomFunction_FB25DD51_windStrength_8;
                GetLocalParams_float(IN.AbsoluteWorldSpacePosition, _Property_20872C63_Out_0, _CustomFunction_FB25DD51_windDir_0, _CustomFunction_FB25DD51_trunkSpeed_1, _CustomFunction_FB25DD51_trunkSwinging_3, _CustomFunction_FB25DD51_trunkWeight_4, _CustomFunction_FB25DD51_windSpeed_2, _CustomFunction_FB25DD51_windFreq_7, _CustomFunction_FB25DD51_windStrength_8);
                float _Split_C6A87A8F_R_1 = _CustomFunction_FB25DD51_windDir_0[0];
                float _Split_C6A87A8F_G_2 = _CustomFunction_FB25DD51_windDir_0[1];
                float _Split_C6A87A8F_B_3 = _CustomFunction_FB25DD51_windDir_0[2];
                float _Split_C6A87A8F_A_4 = _CustomFunction_FB25DD51_windDir_0[3];
                float4 _Combine_51E3AC00_RGBA_4;
                float3 _Combine_51E3AC00_RGB_5;
                float2 _Combine_51E3AC00_RG_6;
                Unity_Combine_float(_Split_C6A87A8F_R_1, _Split_C6A87A8F_B_3, 0, 0, _Combine_51E3AC00_RGBA_4, _Combine_51E3AC00_RGB_5, _Combine_51E3AC00_RG_6);
                float2 _Multiply_34407C61_Out_2;
                Unity_Multiply_float(_Combine_51E3AC00_RG_6, (_CustomFunction_FB25DD51_windSpeed_2.xx), _Multiply_34407C61_Out_2);
                float2 _Add_F12CDF4E_Out_2;
                Unity_Add_float2(_Multiply_34407C61_Out_2, (_CustomFunction_FB25DD51_windFreq_7.xx), _Add_F12CDF4E_Out_2);
                float3 _CustomFunction_456640C0_vec_1;
                SampleWind_float((_Add_F12CDF4E_Out_2).x, _CustomFunction_456640C0_vec_1);
                float3 _Multiply_EC8FCAF4_Out_2;
                Unity_Multiply_float((_Property_AB75A479_Out_0.xxx), _CustomFunction_456640C0_vec_1, _Multiply_EC8FCAF4_Out_2);
                float3 _Multiply_611DED0C_Out_2;
                Unity_Multiply_float(_Multiply_EC8FCAF4_Out_2, (_CustomFunction_FB25DD51_windStrength_8.xxx), _Multiply_611DED0C_Out_2);
                float _Property_806E9BFF_Out_0 = Vector1_C9D81F7C;
                float3 _Multiply_45DFFD0A_Out_2;
                Unity_Multiply_float(_Multiply_611DED0C_Out_2, (_Property_806E9BFF_Out_0.xxx), _Multiply_45DFFD0A_Out_2);
                Offset_1 = _Multiply_45DFFD0A_Out_2;
            }
            
            // f18489b7b4b05a83941c32070dcf5796
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/VSPro_HDIndirect.cginc"
            
            void AddPragma_float(float3 A, out float3 Out)
            {
                Out = A;
            }
            
            struct Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32
            {
            };
            
            void SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(float3 Vector3_314C8600, Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 IN, out float3 ObjectSpacePosition_1)
            {
                float3 _Property_AF5E8C93_Out_0 = Vector3_314C8600;
                float3 _CustomFunction_E07FEE57_Out_1;
                InjectSetup_float(_Property_AF5E8C93_Out_0, _CustomFunction_E07FEE57_Out_1);
                float3 _CustomFunction_18EFD858_Out_1;
                AddPragma_float(_CustomFunction_E07FEE57_Out_1, _CustomFunction_18EFD858_Out_1);
                ObjectSpacePosition_1 = _CustomFunction_18EFD858_Out_1;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 WorldSpaceTangent;
                float3 ObjectSpaceBiTangent;
                float3 WorldSpaceBiTangent;
                float3 ObjectSpacePosition;
                float3 AbsoluteWorldSpacePosition;
                float4 uv1;
                float4 VertexColor;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Property_DB0143B3_Out_0 = _UseSpeedTreeWind;
                float _Split_A80EDD15_R_1 = IN.VertexColor[0];
                float _Split_A80EDD15_G_2 = IN.VertexColor[1];
                float _Split_A80EDD15_B_3 = IN.VertexColor[2];
                float _Split_A80EDD15_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_2543C842;
                _GlobalTreeWindMotion_2543C842.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_2543C842.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_2543C842.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_2543C842.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_2543C842_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(_Property_DB0143B3_Out_0, _Split_A80EDD15_A_4, _GlobalTreeWindMotion_2543C842, _GlobalTreeWindMotion_2543C842_Offset_1);
                float _Property_94051D9D_Out_0 = _WindAmplitudeMultiplier;
                float _Property_FA421571_Out_0 = _MaxWindStrength;
                Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a _LocalWindMotion_496CCFEB;
                _LocalWindMotion_496CCFEB.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                float3 _LocalWindMotion_496CCFEB_Offset_1;
                SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(_Property_94051D9D_Out_0, _Property_FA421571_Out_0, _Split_A80EDD15_G_2, _LocalWindMotion_496CCFEB, _LocalWindMotion_496CCFEB_Offset_1);
                float3 _Add_59B53B15_Out_2;
                Unity_Add_float3(_GlobalTreeWindMotion_2543C842_Offset_1, _LocalWindMotion_496CCFEB_Offset_1, _Add_59B53B15_Out_2);
                float3 _Add_DF77ECB8_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Add_59B53B15_Out_2, _Add_DF77ECB8_Out_2);
                Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 _VSProHDInstancedIndirect_A8293AE2;
                float3 _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(_Add_DF77ECB8_Out_2, _VSProHDInstancedIndirect_A8293AE2, _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1);
                float3 _Vector3_B7A96EB5_Out_0 = float3(0, 1, 0);
                float _Property_A8C540BD_Out_0 = _FlatLighting;
                float3 _Lerp_FAE7CF7_Out_3;
                Unity_Lerp_float3(IN.ObjectSpaceNormal, _Vector3_B7A96EB5_Out_0, (_Property_A8C540BD_Out_0.xxx), _Lerp_FAE7CF7_Out_3);
                description.VertexPosition = _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                description.VertexNormal = _Lerp_FAE7CF7_Out_3;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float4 uv0;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _SampleTexture2D_E53DEB40_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_E53DEB40_R_4 = _SampleTexture2D_E53DEB40_RGBA_0.r;
                float _SampleTexture2D_E53DEB40_G_5 = _SampleTexture2D_E53DEB40_RGBA_0.g;
                float _SampleTexture2D_E53DEB40_B_6 = _SampleTexture2D_E53DEB40_RGBA_0.b;
                float _SampleTexture2D_E53DEB40_A_7 = _SampleTexture2D_E53DEB40_RGBA_0.a;
                float _Property_3912DE45_Out_0 = _Cutoff;
                surface.Alpha = _SampleTexture2D_E53DEB40_A_7;
                surface.AlphaClipThreshold = _Property_3912DE45_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                output.ObjectSpacePosition =         input.positionOS;
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.uv0 =                         input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
            
#include "./../../../GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Off
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_DEPTHONLY
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float _Cutoff;
            float4 _HueVariation;
            float4 _TransmissionColor;
            float _AmbientOcclusion;
            float _Smoothness;
            float _FlatLighting;
            float _GradientBrightness;
            float _MaxWindStrength;
            float _WindAmplitudeMultiplier;
            float _UseSpeedTreeWind;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_E53DEB40_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            // baf344beb4069bc11e37cba678d5ea13
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/FAE.hlsl"
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Divide_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A / B;
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Sine_float3(float3 In, out float3 Out)
            {
                Out = sin(In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Branch_float(float Predicate, float True, float False, out float Out)
            {
                Out = Predicate ? True : False;
            }
            
            struct Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00
            {
                float3 WorldSpaceNormal;
                float3 WorldSpaceTangent;
                float3 WorldSpaceBiTangent;
                half4 uv1;
            };
            
            void SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(float Boolean_BA09D051, float Vector1_1EEB22DF, Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 IN, out float3 Offset_1)
            {
                float4 _CustomFunction_F763B077_windDir_0;
                float _CustomFunction_F763B077_trunkSpeed_1;
                float _CustomFunction_F763B077_trunkSwinging_3;
                float _CustomFunction_F763B077_trunkWeight_4;
                float _CustomFunction_F763B077_windSpeed_2;
                GetGlobalParams_float(_CustomFunction_F763B077_windDir_0, _CustomFunction_F763B077_trunkSpeed_1, _CustomFunction_F763B077_trunkSwinging_3, _CustomFunction_F763B077_trunkWeight_4, _CustomFunction_F763B077_windSpeed_2);
                float3 _Transform_408DEDF1_Out_1 = TransformWorldToObjectDir((_CustomFunction_F763B077_windDir_0.xyz).xyz);
                float _Split_7E1333CF_R_1 = _Transform_408DEDF1_Out_1[0];
                float _Split_7E1333CF_G_2 = _Transform_408DEDF1_Out_1[1];
                float _Split_7E1333CF_B_3 = _Transform_408DEDF1_Out_1[2];
                float _Split_7E1333CF_A_4 = 0;
                float4 _Combine_2DC33859_RGBA_4;
                float3 _Combine_2DC33859_RGB_5;
                float2 _Combine_2DC33859_RG_6;
                Unity_Combine_float(_Split_7E1333CF_R_1, 0, _Split_7E1333CF_B_3, 0, _Combine_2DC33859_RGBA_4, _Combine_2DC33859_RGB_5, _Combine_2DC33859_RG_6);
                float3 _Divide_CBE4D3A1_Out_2;
                Unity_Divide_float3((_CustomFunction_F763B077_trunkSpeed_1.xxx), float3(length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
                                         length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
                                         length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z))), _Divide_CBE4D3A1_Out_2);
                float3 _Multiply_A1BE4F17_Out_2;
                Unity_Multiply_float((_CustomFunction_F763B077_windSpeed_2.xxx), _Divide_CBE4D3A1_Out_2, _Multiply_A1BE4F17_Out_2);
                float3 _Multiply_91A2ECE2_Out_2;
                Unity_Multiply_float(_Combine_2DC33859_RGB_5, _Multiply_A1BE4F17_Out_2, _Multiply_91A2ECE2_Out_2);
                float3 _Sine_DEEB684D_Out_1;
                Unity_Sine_float3(_Multiply_91A2ECE2_Out_2, _Sine_DEEB684D_Out_1);
                float _Vector1_27C730_Out_0 = 0.5;
                float3 _Multiply_1BC85160_Out_2;
                Unity_Multiply_float(_Sine_DEEB684D_Out_1, (_Vector1_27C730_Out_0.xxx), _Multiply_1BC85160_Out_2);
                float3 _Add_36F313F8_Out_2;
                Unity_Add_float3(_Multiply_1BC85160_Out_2, (_Vector1_27C730_Out_0.xxx), _Add_36F313F8_Out_2);
                float3 _Lerp_1BCD68EF_Out_3;
                Unity_Lerp_float3(_Add_36F313F8_Out_2, _Sine_DEEB684D_Out_1, (_CustomFunction_F763B077_trunkSwinging_3.xxx), _Lerp_1BCD68EF_Out_3);
                float3 _Multiply_69E4A4AF_Out_2;
                Unity_Multiply_float(_Lerp_1BCD68EF_Out_3, (_CustomFunction_F763B077_trunkWeight_4.xxx), _Multiply_69E4A4AF_Out_2);
                float _Property_22D46B5A_Out_0 = Boolean_BA09D051;
                float4 _UV_44C52F2E_Out_0 = IN.uv1;
                float _Split_5A3BE0BE_R_1 = _UV_44C52F2E_Out_0[0];
                float _Split_5A3BE0BE_G_2 = _UV_44C52F2E_Out_0[1];
                float _Split_5A3BE0BE_B_3 = _UV_44C52F2E_Out_0[2];
                float _Split_5A3BE0BE_A_4 = _UV_44C52F2E_Out_0[3];
                float _Multiply_F32B98EF_Out_2;
                Unity_Multiply_float(_Split_5A3BE0BE_G_2, 0.01, _Multiply_F32B98EF_Out_2);
                float _Property_CD92AE06_Out_0 = Vector1_1EEB22DF;
                float _Branch_F33FDE09_Out_3;
                Unity_Branch_float(_Property_22D46B5A_Out_0, _Multiply_F32B98EF_Out_2, _Property_CD92AE06_Out_0, _Branch_F33FDE09_Out_3);
                float3 _Multiply_3473FC0B_Out_2;
                Unity_Multiply_float(_Multiply_69E4A4AF_Out_2, (_Branch_F33FDE09_Out_3.xxx), _Multiply_3473FC0B_Out_2);
                float _Split_6848EEB6_R_1 = _Multiply_3473FC0B_Out_2[0];
                float _Split_6848EEB6_G_2 = _Multiply_3473FC0B_Out_2[1];
                float _Split_6848EEB6_B_3 = _Multiply_3473FC0B_Out_2[2];
                float _Split_6848EEB6_A_4 = 0;
                float4 _Combine_FDFF0983_RGBA_4;
                float3 _Combine_FDFF0983_RGB_5;
                float2 _Combine_FDFF0983_RG_6;
                Unity_Combine_float(_Split_6848EEB6_R_1, 0, _Split_6848EEB6_B_3, 0, _Combine_FDFF0983_RGBA_4, _Combine_FDFF0983_RGB_5, _Combine_FDFF0983_RG_6);
                Offset_1 = _Combine_FDFF0983_RGB_5;
            }
            
            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
            
            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }
            
            struct Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a
            {
                float3 AbsoluteWorldSpacePosition;
            };
            
            void SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(float Vector1_76290B08, float Vector1_C9D81F7C, float Vector1_2A7979D, Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a IN, out float3 Offset_1)
            {
                float _Property_AB75A479_Out_0 = Vector1_2A7979D;
                float _Property_20872C63_Out_0 = Vector1_76290B08;
                float4 _CustomFunction_FB25DD51_windDir_0;
                float _CustomFunction_FB25DD51_trunkSpeed_1;
                float _CustomFunction_FB25DD51_trunkSwinging_3;
                float _CustomFunction_FB25DD51_trunkWeight_4;
                float _CustomFunction_FB25DD51_windSpeed_2;
                float _CustomFunction_FB25DD51_windFreq_7;
                float _CustomFunction_FB25DD51_windStrength_8;
                GetLocalParams_float(IN.AbsoluteWorldSpacePosition, _Property_20872C63_Out_0, _CustomFunction_FB25DD51_windDir_0, _CustomFunction_FB25DD51_trunkSpeed_1, _CustomFunction_FB25DD51_trunkSwinging_3, _CustomFunction_FB25DD51_trunkWeight_4, _CustomFunction_FB25DD51_windSpeed_2, _CustomFunction_FB25DD51_windFreq_7, _CustomFunction_FB25DD51_windStrength_8);
                float _Split_C6A87A8F_R_1 = _CustomFunction_FB25DD51_windDir_0[0];
                float _Split_C6A87A8F_G_2 = _CustomFunction_FB25DD51_windDir_0[1];
                float _Split_C6A87A8F_B_3 = _CustomFunction_FB25DD51_windDir_0[2];
                float _Split_C6A87A8F_A_4 = _CustomFunction_FB25DD51_windDir_0[3];
                float4 _Combine_51E3AC00_RGBA_4;
                float3 _Combine_51E3AC00_RGB_5;
                float2 _Combine_51E3AC00_RG_6;
                Unity_Combine_float(_Split_C6A87A8F_R_1, _Split_C6A87A8F_B_3, 0, 0, _Combine_51E3AC00_RGBA_4, _Combine_51E3AC00_RGB_5, _Combine_51E3AC00_RG_6);
                float2 _Multiply_34407C61_Out_2;
                Unity_Multiply_float(_Combine_51E3AC00_RG_6, (_CustomFunction_FB25DD51_windSpeed_2.xx), _Multiply_34407C61_Out_2);
                float2 _Add_F12CDF4E_Out_2;
                Unity_Add_float2(_Multiply_34407C61_Out_2, (_CustomFunction_FB25DD51_windFreq_7.xx), _Add_F12CDF4E_Out_2);
                float3 _CustomFunction_456640C0_vec_1;
                SampleWind_float((_Add_F12CDF4E_Out_2).x, _CustomFunction_456640C0_vec_1);
                float3 _Multiply_EC8FCAF4_Out_2;
                Unity_Multiply_float((_Property_AB75A479_Out_0.xxx), _CustomFunction_456640C0_vec_1, _Multiply_EC8FCAF4_Out_2);
                float3 _Multiply_611DED0C_Out_2;
                Unity_Multiply_float(_Multiply_EC8FCAF4_Out_2, (_CustomFunction_FB25DD51_windStrength_8.xxx), _Multiply_611DED0C_Out_2);
                float _Property_806E9BFF_Out_0 = Vector1_C9D81F7C;
                float3 _Multiply_45DFFD0A_Out_2;
                Unity_Multiply_float(_Multiply_611DED0C_Out_2, (_Property_806E9BFF_Out_0.xxx), _Multiply_45DFFD0A_Out_2);
                Offset_1 = _Multiply_45DFFD0A_Out_2;
            }
            
            // f18489b7b4b05a83941c32070dcf5796
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/VSPro_HDIndirect.cginc"
            
            void AddPragma_float(float3 A, out float3 Out)
            {
                Out = A;
            }
            
            struct Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32
            {
            };
            
            void SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(float3 Vector3_314C8600, Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 IN, out float3 ObjectSpacePosition_1)
            {
                float3 _Property_AF5E8C93_Out_0 = Vector3_314C8600;
                float3 _CustomFunction_E07FEE57_Out_1;
                InjectSetup_float(_Property_AF5E8C93_Out_0, _CustomFunction_E07FEE57_Out_1);
                float3 _CustomFunction_18EFD858_Out_1;
                AddPragma_float(_CustomFunction_E07FEE57_Out_1, _CustomFunction_18EFD858_Out_1);
                ObjectSpacePosition_1 = _CustomFunction_18EFD858_Out_1;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 WorldSpaceTangent;
                float3 ObjectSpaceBiTangent;
                float3 WorldSpaceBiTangent;
                float3 ObjectSpacePosition;
                float3 AbsoluteWorldSpacePosition;
                float4 uv1;
                float4 VertexColor;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Property_DB0143B3_Out_0 = _UseSpeedTreeWind;
                float _Split_A80EDD15_R_1 = IN.VertexColor[0];
                float _Split_A80EDD15_G_2 = IN.VertexColor[1];
                float _Split_A80EDD15_B_3 = IN.VertexColor[2];
                float _Split_A80EDD15_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_2543C842;
                _GlobalTreeWindMotion_2543C842.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_2543C842.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_2543C842.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_2543C842.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_2543C842_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(_Property_DB0143B3_Out_0, _Split_A80EDD15_A_4, _GlobalTreeWindMotion_2543C842, _GlobalTreeWindMotion_2543C842_Offset_1);
                float _Property_94051D9D_Out_0 = _WindAmplitudeMultiplier;
                float _Property_FA421571_Out_0 = _MaxWindStrength;
                Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a _LocalWindMotion_496CCFEB;
                _LocalWindMotion_496CCFEB.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                float3 _LocalWindMotion_496CCFEB_Offset_1;
                SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(_Property_94051D9D_Out_0, _Property_FA421571_Out_0, _Split_A80EDD15_G_2, _LocalWindMotion_496CCFEB, _LocalWindMotion_496CCFEB_Offset_1);
                float3 _Add_59B53B15_Out_2;
                Unity_Add_float3(_GlobalTreeWindMotion_2543C842_Offset_1, _LocalWindMotion_496CCFEB_Offset_1, _Add_59B53B15_Out_2);
                float3 _Add_DF77ECB8_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Add_59B53B15_Out_2, _Add_DF77ECB8_Out_2);
                Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 _VSProHDInstancedIndirect_A8293AE2;
                float3 _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(_Add_DF77ECB8_Out_2, _VSProHDInstancedIndirect_A8293AE2, _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1);
                float3 _Vector3_B7A96EB5_Out_0 = float3(0, 1, 0);
                float _Property_A8C540BD_Out_0 = _FlatLighting;
                float3 _Lerp_FAE7CF7_Out_3;
                Unity_Lerp_float3(IN.ObjectSpaceNormal, _Vector3_B7A96EB5_Out_0, (_Property_A8C540BD_Out_0.xxx), _Lerp_FAE7CF7_Out_3);
                description.VertexPosition = _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                description.VertexNormal = _Lerp_FAE7CF7_Out_3;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float4 uv0;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _SampleTexture2D_E53DEB40_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_E53DEB40_R_4 = _SampleTexture2D_E53DEB40_RGBA_0.r;
                float _SampleTexture2D_E53DEB40_G_5 = _SampleTexture2D_E53DEB40_RGBA_0.g;
                float _SampleTexture2D_E53DEB40_B_6 = _SampleTexture2D_E53DEB40_RGBA_0.b;
                float _SampleTexture2D_E53DEB40_A_7 = _SampleTexture2D_E53DEB40_RGBA_0.a;
                float _Property_3912DE45_Out_0 = _Cutoff;
                surface.Alpha = _SampleTexture2D_E53DEB40_A_7;
                surface.AlphaClipThreshold = _Property_3912DE45_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                output.ObjectSpacePosition =         input.positionOS;
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.uv0 =                         input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
            
#include "./../../../GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
ENDHLSL
        }
        
        Pass
        {
            Name "Meta"
            Tags 
            { 
                "LightMode" = "Meta"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Off
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
        
            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_META
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float _Cutoff;
            float4 _HueVariation;
            float4 _TransmissionColor;
            float _AmbientOcclusion;
            float _Smoothness;
            float _FlatLighting;
            float _GradientBrightness;
            float _MaxWindStrength;
            float _WindAmplitudeMultiplier;
            float _UseSpeedTreeWind;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_E53DEB40_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            // baf344beb4069bc11e37cba678d5ea13
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/FAE.hlsl"
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Divide_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A / B;
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Sine_float3(float3 In, out float3 Out)
            {
                Out = sin(In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Branch_float(float Predicate, float True, float False, out float Out)
            {
                Out = Predicate ? True : False;
            }
            
            struct Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00
            {
                float3 WorldSpaceNormal;
                float3 WorldSpaceTangent;
                float3 WorldSpaceBiTangent;
                half4 uv1;
            };
            
            void SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(float Boolean_BA09D051, float Vector1_1EEB22DF, Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 IN, out float3 Offset_1)
            {
                float4 _CustomFunction_F763B077_windDir_0;
                float _CustomFunction_F763B077_trunkSpeed_1;
                float _CustomFunction_F763B077_trunkSwinging_3;
                float _CustomFunction_F763B077_trunkWeight_4;
                float _CustomFunction_F763B077_windSpeed_2;
                GetGlobalParams_float(_CustomFunction_F763B077_windDir_0, _CustomFunction_F763B077_trunkSpeed_1, _CustomFunction_F763B077_trunkSwinging_3, _CustomFunction_F763B077_trunkWeight_4, _CustomFunction_F763B077_windSpeed_2);
                float3 _Transform_408DEDF1_Out_1 = TransformWorldToObjectDir((_CustomFunction_F763B077_windDir_0.xyz).xyz);
                float _Split_7E1333CF_R_1 = _Transform_408DEDF1_Out_1[0];
                float _Split_7E1333CF_G_2 = _Transform_408DEDF1_Out_1[1];
                float _Split_7E1333CF_B_3 = _Transform_408DEDF1_Out_1[2];
                float _Split_7E1333CF_A_4 = 0;
                float4 _Combine_2DC33859_RGBA_4;
                float3 _Combine_2DC33859_RGB_5;
                float2 _Combine_2DC33859_RG_6;
                Unity_Combine_float(_Split_7E1333CF_R_1, 0, _Split_7E1333CF_B_3, 0, _Combine_2DC33859_RGBA_4, _Combine_2DC33859_RGB_5, _Combine_2DC33859_RG_6);
                float3 _Divide_CBE4D3A1_Out_2;
                Unity_Divide_float3((_CustomFunction_F763B077_trunkSpeed_1.xxx), float3(length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
                                         length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
                                         length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z))), _Divide_CBE4D3A1_Out_2);
                float3 _Multiply_A1BE4F17_Out_2;
                Unity_Multiply_float((_CustomFunction_F763B077_windSpeed_2.xxx), _Divide_CBE4D3A1_Out_2, _Multiply_A1BE4F17_Out_2);
                float3 _Multiply_91A2ECE2_Out_2;
                Unity_Multiply_float(_Combine_2DC33859_RGB_5, _Multiply_A1BE4F17_Out_2, _Multiply_91A2ECE2_Out_2);
                float3 _Sine_DEEB684D_Out_1;
                Unity_Sine_float3(_Multiply_91A2ECE2_Out_2, _Sine_DEEB684D_Out_1);
                float _Vector1_27C730_Out_0 = 0.5;
                float3 _Multiply_1BC85160_Out_2;
                Unity_Multiply_float(_Sine_DEEB684D_Out_1, (_Vector1_27C730_Out_0.xxx), _Multiply_1BC85160_Out_2);
                float3 _Add_36F313F8_Out_2;
                Unity_Add_float3(_Multiply_1BC85160_Out_2, (_Vector1_27C730_Out_0.xxx), _Add_36F313F8_Out_2);
                float3 _Lerp_1BCD68EF_Out_3;
                Unity_Lerp_float3(_Add_36F313F8_Out_2, _Sine_DEEB684D_Out_1, (_CustomFunction_F763B077_trunkSwinging_3.xxx), _Lerp_1BCD68EF_Out_3);
                float3 _Multiply_69E4A4AF_Out_2;
                Unity_Multiply_float(_Lerp_1BCD68EF_Out_3, (_CustomFunction_F763B077_trunkWeight_4.xxx), _Multiply_69E4A4AF_Out_2);
                float _Property_22D46B5A_Out_0 = Boolean_BA09D051;
                float4 _UV_44C52F2E_Out_0 = IN.uv1;
                float _Split_5A3BE0BE_R_1 = _UV_44C52F2E_Out_0[0];
                float _Split_5A3BE0BE_G_2 = _UV_44C52F2E_Out_0[1];
                float _Split_5A3BE0BE_B_3 = _UV_44C52F2E_Out_0[2];
                float _Split_5A3BE0BE_A_4 = _UV_44C52F2E_Out_0[3];
                float _Multiply_F32B98EF_Out_2;
                Unity_Multiply_float(_Split_5A3BE0BE_G_2, 0.01, _Multiply_F32B98EF_Out_2);
                float _Property_CD92AE06_Out_0 = Vector1_1EEB22DF;
                float _Branch_F33FDE09_Out_3;
                Unity_Branch_float(_Property_22D46B5A_Out_0, _Multiply_F32B98EF_Out_2, _Property_CD92AE06_Out_0, _Branch_F33FDE09_Out_3);
                float3 _Multiply_3473FC0B_Out_2;
                Unity_Multiply_float(_Multiply_69E4A4AF_Out_2, (_Branch_F33FDE09_Out_3.xxx), _Multiply_3473FC0B_Out_2);
                float _Split_6848EEB6_R_1 = _Multiply_3473FC0B_Out_2[0];
                float _Split_6848EEB6_G_2 = _Multiply_3473FC0B_Out_2[1];
                float _Split_6848EEB6_B_3 = _Multiply_3473FC0B_Out_2[2];
                float _Split_6848EEB6_A_4 = 0;
                float4 _Combine_FDFF0983_RGBA_4;
                float3 _Combine_FDFF0983_RGB_5;
                float2 _Combine_FDFF0983_RG_6;
                Unity_Combine_float(_Split_6848EEB6_R_1, 0, _Split_6848EEB6_B_3, 0, _Combine_FDFF0983_RGBA_4, _Combine_FDFF0983_RGB_5, _Combine_FDFF0983_RG_6);
                Offset_1 = _Combine_FDFF0983_RGB_5;
            }
            
            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
            
            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }
            
            struct Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a
            {
                float3 AbsoluteWorldSpacePosition;
            };
            
            void SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(float Vector1_76290B08, float Vector1_C9D81F7C, float Vector1_2A7979D, Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a IN, out float3 Offset_1)
            {
                float _Property_AB75A479_Out_0 = Vector1_2A7979D;
                float _Property_20872C63_Out_0 = Vector1_76290B08;
                float4 _CustomFunction_FB25DD51_windDir_0;
                float _CustomFunction_FB25DD51_trunkSpeed_1;
                float _CustomFunction_FB25DD51_trunkSwinging_3;
                float _CustomFunction_FB25DD51_trunkWeight_4;
                float _CustomFunction_FB25DD51_windSpeed_2;
                float _CustomFunction_FB25DD51_windFreq_7;
                float _CustomFunction_FB25DD51_windStrength_8;
                GetLocalParams_float(IN.AbsoluteWorldSpacePosition, _Property_20872C63_Out_0, _CustomFunction_FB25DD51_windDir_0, _CustomFunction_FB25DD51_trunkSpeed_1, _CustomFunction_FB25DD51_trunkSwinging_3, _CustomFunction_FB25DD51_trunkWeight_4, _CustomFunction_FB25DD51_windSpeed_2, _CustomFunction_FB25DD51_windFreq_7, _CustomFunction_FB25DD51_windStrength_8);
                float _Split_C6A87A8F_R_1 = _CustomFunction_FB25DD51_windDir_0[0];
                float _Split_C6A87A8F_G_2 = _CustomFunction_FB25DD51_windDir_0[1];
                float _Split_C6A87A8F_B_3 = _CustomFunction_FB25DD51_windDir_0[2];
                float _Split_C6A87A8F_A_4 = _CustomFunction_FB25DD51_windDir_0[3];
                float4 _Combine_51E3AC00_RGBA_4;
                float3 _Combine_51E3AC00_RGB_5;
                float2 _Combine_51E3AC00_RG_6;
                Unity_Combine_float(_Split_C6A87A8F_R_1, _Split_C6A87A8F_B_3, 0, 0, _Combine_51E3AC00_RGBA_4, _Combine_51E3AC00_RGB_5, _Combine_51E3AC00_RG_6);
                float2 _Multiply_34407C61_Out_2;
                Unity_Multiply_float(_Combine_51E3AC00_RG_6, (_CustomFunction_FB25DD51_windSpeed_2.xx), _Multiply_34407C61_Out_2);
                float2 _Add_F12CDF4E_Out_2;
                Unity_Add_float2(_Multiply_34407C61_Out_2, (_CustomFunction_FB25DD51_windFreq_7.xx), _Add_F12CDF4E_Out_2);
                float3 _CustomFunction_456640C0_vec_1;
                SampleWind_float((_Add_F12CDF4E_Out_2).x, _CustomFunction_456640C0_vec_1);
                float3 _Multiply_EC8FCAF4_Out_2;
                Unity_Multiply_float((_Property_AB75A479_Out_0.xxx), _CustomFunction_456640C0_vec_1, _Multiply_EC8FCAF4_Out_2);
                float3 _Multiply_611DED0C_Out_2;
                Unity_Multiply_float(_Multiply_EC8FCAF4_Out_2, (_CustomFunction_FB25DD51_windStrength_8.xxx), _Multiply_611DED0C_Out_2);
                float _Property_806E9BFF_Out_0 = Vector1_C9D81F7C;
                float3 _Multiply_45DFFD0A_Out_2;
                Unity_Multiply_float(_Multiply_611DED0C_Out_2, (_Property_806E9BFF_Out_0.xxx), _Multiply_45DFFD0A_Out_2);
                Offset_1 = _Multiply_45DFFD0A_Out_2;
            }
            
            // f18489b7b4b05a83941c32070dcf5796
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/VSPro_HDIndirect.cginc"
            
            void AddPragma_float(float3 A, out float3 Out)
            {
                Out = A;
            }
            
            struct Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32
            {
            };
            
            void SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(float3 Vector3_314C8600, Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 IN, out float3 ObjectSpacePosition_1)
            {
                float3 _Property_AF5E8C93_Out_0 = Vector3_314C8600;
                float3 _CustomFunction_E07FEE57_Out_1;
                InjectSetup_float(_Property_AF5E8C93_Out_0, _CustomFunction_E07FEE57_Out_1);
                float3 _CustomFunction_18EFD858_Out_1;
                AddPragma_float(_CustomFunction_E07FEE57_Out_1, _CustomFunction_18EFD858_Out_1);
                ObjectSpacePosition_1 = _CustomFunction_18EFD858_Out_1;
            }
            
            void Unity_Lerp_float(float A, float B, float T, out float Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Length_float3(float3 In, out float Out)
            {
                Out = length(In);
            }
            
            void Unity_Fraction_float(float In, out float Out)
            {
                Out = frac(In);
            }
            
            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
            
            void Unity_Normalize_half3(half3 In, out half3 Out)
            {
                Out = normalize(In);
            }
            
            void Unity_DotProduct_float3(float3 A, float3 B, out float Out)
            {
                Out = dot(A, B);
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 WorldSpaceTangent;
                float3 ObjectSpaceBiTangent;
                float3 WorldSpaceBiTangent;
                float3 ObjectSpacePosition;
                float3 AbsoluteWorldSpacePosition;
                float4 uv1;
                float4 VertexColor;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Property_DB0143B3_Out_0 = _UseSpeedTreeWind;
                float _Split_A80EDD15_R_1 = IN.VertexColor[0];
                float _Split_A80EDD15_G_2 = IN.VertexColor[1];
                float _Split_A80EDD15_B_3 = IN.VertexColor[2];
                float _Split_A80EDD15_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_2543C842;
                _GlobalTreeWindMotion_2543C842.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_2543C842.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_2543C842.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_2543C842.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_2543C842_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(_Property_DB0143B3_Out_0, _Split_A80EDD15_A_4, _GlobalTreeWindMotion_2543C842, _GlobalTreeWindMotion_2543C842_Offset_1);
                float _Property_94051D9D_Out_0 = _WindAmplitudeMultiplier;
                float _Property_FA421571_Out_0 = _MaxWindStrength;
                Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a _LocalWindMotion_496CCFEB;
                _LocalWindMotion_496CCFEB.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                float3 _LocalWindMotion_496CCFEB_Offset_1;
                SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(_Property_94051D9D_Out_0, _Property_FA421571_Out_0, _Split_A80EDD15_G_2, _LocalWindMotion_496CCFEB, _LocalWindMotion_496CCFEB_Offset_1);
                float3 _Add_59B53B15_Out_2;
                Unity_Add_float3(_GlobalTreeWindMotion_2543C842_Offset_1, _LocalWindMotion_496CCFEB_Offset_1, _Add_59B53B15_Out_2);
                float3 _Add_DF77ECB8_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Add_59B53B15_Out_2, _Add_DF77ECB8_Out_2);
                Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 _VSProHDInstancedIndirect_A8293AE2;
                float3 _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(_Add_DF77ECB8_Out_2, _VSProHDInstancedIndirect_A8293AE2, _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1);
                float3 _Vector3_B7A96EB5_Out_0 = float3(0, 1, 0);
                float _Property_A8C540BD_Out_0 = _FlatLighting;
                float3 _Lerp_FAE7CF7_Out_3;
                Unity_Lerp_float3(IN.ObjectSpaceNormal, _Vector3_B7A96EB5_Out_0, (_Property_A8C540BD_Out_0.xxx), _Lerp_FAE7CF7_Out_3);
                description.VertexPosition = _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                description.VertexNormal = _Lerp_FAE7CF7_Out_3;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float3 AbsoluteWorldSpacePosition;
                float4 uv0;
                float4 VertexColor;
            };
            
            struct SurfaceDescription
            {
                float3 Albedo;
                float3 Emission;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Split_18BD7240_R_1 = IN.VertexColor[0];
                float _Split_18BD7240_G_2 = IN.VertexColor[1];
                float _Split_18BD7240_B_3 = IN.VertexColor[2];
                float _Split_18BD7240_A_4 = IN.VertexColor[3];
                float _Property_EDE257A8_Out_0 = _AmbientOcclusion;
                float _Lerp_418DAFB9_Out_3;
                Unity_Lerp_float(1, _Split_18BD7240_R_1, _Property_EDE257A8_Out_0, _Lerp_418DAFB9_Out_3);
                float _Property_9160F69B_Out_0 = _GradientBrightness;
                float4 _SampleTexture2D_E53DEB40_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_E53DEB40_R_4 = _SampleTexture2D_E53DEB40_RGBA_0.r;
                float _SampleTexture2D_E53DEB40_G_5 = _SampleTexture2D_E53DEB40_RGBA_0.g;
                float _SampleTexture2D_E53DEB40_B_6 = _SampleTexture2D_E53DEB40_RGBA_0.b;
                float _SampleTexture2D_E53DEB40_A_7 = _SampleTexture2D_E53DEB40_RGBA_0.a;
                float4 _Property_1C2D047C_Out_0 = _HueVariation;
                float _Split_4BC9A866_R_1 = _Property_1C2D047C_Out_0[0];
                float _Split_4BC9A866_G_2 = _Property_1C2D047C_Out_0[1];
                float _Split_4BC9A866_B_3 = _Property_1C2D047C_Out_0[2];
                float _Split_4BC9A866_A_4 = _Property_1C2D047C_Out_0[3];
                float _Length_5B09488C_Out_1;
                Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_5B09488C_Out_1);
                float _Fraction_73CC485B_Out_1;
                Unity_Fraction_float(_Length_5B09488C_Out_1, _Fraction_73CC485B_Out_1);
                float _Multiply_BFA62608_Out_2;
                Unity_Multiply_float(_Split_4BC9A866_A_4, _Fraction_73CC485B_Out_1, _Multiply_BFA62608_Out_2);
                float4 _Lerp_8219D562_Out_3;
                Unity_Lerp_float4(_SampleTexture2D_E53DEB40_RGBA_0, _Property_1C2D047C_Out_0, (_Multiply_BFA62608_Out_2.xxxx), _Lerp_8219D562_Out_3);
                float4 _Multiply_9984BC3D_Out_2;
                Unity_Multiply_float((_Property_9160F69B_Out_0.xxxx), _Lerp_8219D562_Out_3, _Multiply_9984BC3D_Out_2);
                float _Multiply_116AF097_Out_2;
                Unity_Multiply_float(_Split_18BD7240_A_4, 10, _Multiply_116AF097_Out_2);
                float _Saturate_338C849F_Out_1;
                Unity_Saturate_float(_Multiply_116AF097_Out_2, _Saturate_338C849F_Out_1);
                float4 _Lerp_82D3B781_Out_3;
                Unity_Lerp_float4(_Multiply_9984BC3D_Out_2, _Lerp_8219D562_Out_3, (_Saturate_338C849F_Out_1.xxxx), _Lerp_82D3B781_Out_3);
                float4 _Multiply_5840E19_Out_2;
                Unity_Multiply_float((_Lerp_418DAFB9_Out_3.xxxx), _Lerp_82D3B781_Out_3, _Multiply_5840E19_Out_2);
                half3 _CustomFunction_5F04B9C7_Direction_1;
                half3 _CustomFunction_5F04B9C7_Color_2;
                half _CustomFunction_5F04B9C7_DistanceAtten_3;
                half _CustomFunction_5F04B9C7_ShadowAtten_4;
                MainLight_half(IN.AbsoluteWorldSpacePosition, _CustomFunction_5F04B9C7_Direction_1, _CustomFunction_5F04B9C7_Color_2, _CustomFunction_5F04B9C7_DistanceAtten_3, _CustomFunction_5F04B9C7_ShadowAtten_4);
                float4 _Property_5AA1B37C_Out_0 = _TransmissionColor;
                float3 _Multiply_DBDA6F5E_Out_2;
                Unity_Multiply_float((_Property_5AA1B37C_Out_0.xyz), _CustomFunction_5F04B9C7_Color_2, _Multiply_DBDA6F5E_Out_2);
                float _Split_B84DA633_R_1 = _Property_5AA1B37C_Out_0[0];
                float _Split_B84DA633_G_2 = _Property_5AA1B37C_Out_0[1];
                float _Split_B84DA633_B_3 = _Property_5AA1B37C_Out_0[2];
                float _Split_B84DA633_A_4 = _Property_5AA1B37C_Out_0[3];
                float _Split_A80EDD15_R_1 = IN.VertexColor[0];
                float _Split_A80EDD15_G_2 = IN.VertexColor[1];
                float _Split_A80EDD15_B_3 = IN.VertexColor[2];
                float _Split_A80EDD15_A_4 = IN.VertexColor[3];
                half3 _Normalize_B7264509_Out_1;
                Unity_Normalize_half3(_CustomFunction_5F04B9C7_Direction_1, _Normalize_B7264509_Out_1);
                float _DotProduct_49DA6A99_Out_2;
                Unity_DotProduct_float3(_Normalize_B7264509_Out_1, -1 * mul((float3x3)UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2].xyz), _DotProduct_49DA6A99_Out_2);
                float _Saturate_85160733_Out_1;
                Unity_Saturate_float(_DotProduct_49DA6A99_Out_2, _Saturate_85160733_Out_1);
                float _Multiply_54F7AAE7_Out_2;
                Unity_Multiply_float(_Split_A80EDD15_B_3, _Saturate_85160733_Out_1, _Multiply_54F7AAE7_Out_2);
                float _Multiply_9F998B54_Out_2;
                Unity_Multiply_float(_Split_B84DA633_A_4, _Multiply_54F7AAE7_Out_2, _Multiply_9F998B54_Out_2);
                float3 _Multiply_B2E8C645_Out_2;
                Unity_Multiply_float(_Multiply_DBDA6F5E_Out_2, (_Multiply_9F998B54_Out_2.xxx), _Multiply_B2E8C645_Out_2);
                float3 _Multiply_3A2FF337_Out_2;
                Unity_Multiply_float((_CustomFunction_5F04B9C7_ShadowAtten_4.xxx), _Multiply_B2E8C645_Out_2, _Multiply_3A2FF337_Out_2);
                float _Property_3912DE45_Out_0 = _Cutoff;
                surface.Albedo = (_Multiply_5840E19_Out_2.xyz);
                surface.Emission = _Multiply_3A2FF337_Out_2;
                surface.Alpha = _SampleTexture2D_E53DEB40_A_7;
                surface.AlphaClipThreshold = _Property_3912DE45_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyzw = input.texCoord0;
                output.interp02.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.texCoord0 = input.interp01.xyzw;
                output.color = input.interp02.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                output.ObjectSpacePosition =         input.positionOS;
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionWS);
                output.uv0 =                         input.texCoord0;
                output.VertexColor =                 input.color;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
            
#include "./../../../GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
ENDHLSL
        }
        
        Pass
        {
            // Name: <None>
            Tags 
            { 
                "LightMode" = "Universal2D"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Off
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_2D
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float _Cutoff;
            float4 _HueVariation;
            float4 _TransmissionColor;
            float _AmbientOcclusion;
            float _Smoothness;
            float _FlatLighting;
            float _GradientBrightness;
            float _MaxWindStrength;
            float _WindAmplitudeMultiplier;
            float _UseSpeedTreeWind;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_E53DEB40_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            // baf344beb4069bc11e37cba678d5ea13
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/FAE.hlsl"
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Divide_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A / B;
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Sine_float3(float3 In, out float3 Out)
            {
                Out = sin(In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Branch_float(float Predicate, float True, float False, out float Out)
            {
                Out = Predicate ? True : False;
            }
            
            struct Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00
            {
                float3 WorldSpaceNormal;
                float3 WorldSpaceTangent;
                float3 WorldSpaceBiTangent;
                half4 uv1;
            };
            
            void SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(float Boolean_BA09D051, float Vector1_1EEB22DF, Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 IN, out float3 Offset_1)
            {
                float4 _CustomFunction_F763B077_windDir_0;
                float _CustomFunction_F763B077_trunkSpeed_1;
                float _CustomFunction_F763B077_trunkSwinging_3;
                float _CustomFunction_F763B077_trunkWeight_4;
                float _CustomFunction_F763B077_windSpeed_2;
                GetGlobalParams_float(_CustomFunction_F763B077_windDir_0, _CustomFunction_F763B077_trunkSpeed_1, _CustomFunction_F763B077_trunkSwinging_3, _CustomFunction_F763B077_trunkWeight_4, _CustomFunction_F763B077_windSpeed_2);
                float3 _Transform_408DEDF1_Out_1 = TransformWorldToObjectDir((_CustomFunction_F763B077_windDir_0.xyz).xyz);
                float _Split_7E1333CF_R_1 = _Transform_408DEDF1_Out_1[0];
                float _Split_7E1333CF_G_2 = _Transform_408DEDF1_Out_1[1];
                float _Split_7E1333CF_B_3 = _Transform_408DEDF1_Out_1[2];
                float _Split_7E1333CF_A_4 = 0;
                float4 _Combine_2DC33859_RGBA_4;
                float3 _Combine_2DC33859_RGB_5;
                float2 _Combine_2DC33859_RG_6;
                Unity_Combine_float(_Split_7E1333CF_R_1, 0, _Split_7E1333CF_B_3, 0, _Combine_2DC33859_RGBA_4, _Combine_2DC33859_RGB_5, _Combine_2DC33859_RG_6);
                float3 _Divide_CBE4D3A1_Out_2;
                Unity_Divide_float3((_CustomFunction_F763B077_trunkSpeed_1.xxx), float3(length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
                                         length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
                                         length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z))), _Divide_CBE4D3A1_Out_2);
                float3 _Multiply_A1BE4F17_Out_2;
                Unity_Multiply_float((_CustomFunction_F763B077_windSpeed_2.xxx), _Divide_CBE4D3A1_Out_2, _Multiply_A1BE4F17_Out_2);
                float3 _Multiply_91A2ECE2_Out_2;
                Unity_Multiply_float(_Combine_2DC33859_RGB_5, _Multiply_A1BE4F17_Out_2, _Multiply_91A2ECE2_Out_2);
                float3 _Sine_DEEB684D_Out_1;
                Unity_Sine_float3(_Multiply_91A2ECE2_Out_2, _Sine_DEEB684D_Out_1);
                float _Vector1_27C730_Out_0 = 0.5;
                float3 _Multiply_1BC85160_Out_2;
                Unity_Multiply_float(_Sine_DEEB684D_Out_1, (_Vector1_27C730_Out_0.xxx), _Multiply_1BC85160_Out_2);
                float3 _Add_36F313F8_Out_2;
                Unity_Add_float3(_Multiply_1BC85160_Out_2, (_Vector1_27C730_Out_0.xxx), _Add_36F313F8_Out_2);
                float3 _Lerp_1BCD68EF_Out_3;
                Unity_Lerp_float3(_Add_36F313F8_Out_2, _Sine_DEEB684D_Out_1, (_CustomFunction_F763B077_trunkSwinging_3.xxx), _Lerp_1BCD68EF_Out_3);
                float3 _Multiply_69E4A4AF_Out_2;
                Unity_Multiply_float(_Lerp_1BCD68EF_Out_3, (_CustomFunction_F763B077_trunkWeight_4.xxx), _Multiply_69E4A4AF_Out_2);
                float _Property_22D46B5A_Out_0 = Boolean_BA09D051;
                float4 _UV_44C52F2E_Out_0 = IN.uv1;
                float _Split_5A3BE0BE_R_1 = _UV_44C52F2E_Out_0[0];
                float _Split_5A3BE0BE_G_2 = _UV_44C52F2E_Out_0[1];
                float _Split_5A3BE0BE_B_3 = _UV_44C52F2E_Out_0[2];
                float _Split_5A3BE0BE_A_4 = _UV_44C52F2E_Out_0[3];
                float _Multiply_F32B98EF_Out_2;
                Unity_Multiply_float(_Split_5A3BE0BE_G_2, 0.01, _Multiply_F32B98EF_Out_2);
                float _Property_CD92AE06_Out_0 = Vector1_1EEB22DF;
                float _Branch_F33FDE09_Out_3;
                Unity_Branch_float(_Property_22D46B5A_Out_0, _Multiply_F32B98EF_Out_2, _Property_CD92AE06_Out_0, _Branch_F33FDE09_Out_3);
                float3 _Multiply_3473FC0B_Out_2;
                Unity_Multiply_float(_Multiply_69E4A4AF_Out_2, (_Branch_F33FDE09_Out_3.xxx), _Multiply_3473FC0B_Out_2);
                float _Split_6848EEB6_R_1 = _Multiply_3473FC0B_Out_2[0];
                float _Split_6848EEB6_G_2 = _Multiply_3473FC0B_Out_2[1];
                float _Split_6848EEB6_B_3 = _Multiply_3473FC0B_Out_2[2];
                float _Split_6848EEB6_A_4 = 0;
                float4 _Combine_FDFF0983_RGBA_4;
                float3 _Combine_FDFF0983_RGB_5;
                float2 _Combine_FDFF0983_RG_6;
                Unity_Combine_float(_Split_6848EEB6_R_1, 0, _Split_6848EEB6_B_3, 0, _Combine_FDFF0983_RGBA_4, _Combine_FDFF0983_RGB_5, _Combine_FDFF0983_RG_6);
                Offset_1 = _Combine_FDFF0983_RGB_5;
            }
            
            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
            
            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }
            
            struct Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a
            {
                float3 AbsoluteWorldSpacePosition;
            };
            
            void SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(float Vector1_76290B08, float Vector1_C9D81F7C, float Vector1_2A7979D, Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a IN, out float3 Offset_1)
            {
                float _Property_AB75A479_Out_0 = Vector1_2A7979D;
                float _Property_20872C63_Out_0 = Vector1_76290B08;
                float4 _CustomFunction_FB25DD51_windDir_0;
                float _CustomFunction_FB25DD51_trunkSpeed_1;
                float _CustomFunction_FB25DD51_trunkSwinging_3;
                float _CustomFunction_FB25DD51_trunkWeight_4;
                float _CustomFunction_FB25DD51_windSpeed_2;
                float _CustomFunction_FB25DD51_windFreq_7;
                float _CustomFunction_FB25DD51_windStrength_8;
                GetLocalParams_float(IN.AbsoluteWorldSpacePosition, _Property_20872C63_Out_0, _CustomFunction_FB25DD51_windDir_0, _CustomFunction_FB25DD51_trunkSpeed_1, _CustomFunction_FB25DD51_trunkSwinging_3, _CustomFunction_FB25DD51_trunkWeight_4, _CustomFunction_FB25DD51_windSpeed_2, _CustomFunction_FB25DD51_windFreq_7, _CustomFunction_FB25DD51_windStrength_8);
                float _Split_C6A87A8F_R_1 = _CustomFunction_FB25DD51_windDir_0[0];
                float _Split_C6A87A8F_G_2 = _CustomFunction_FB25DD51_windDir_0[1];
                float _Split_C6A87A8F_B_3 = _CustomFunction_FB25DD51_windDir_0[2];
                float _Split_C6A87A8F_A_4 = _CustomFunction_FB25DD51_windDir_0[3];
                float4 _Combine_51E3AC00_RGBA_4;
                float3 _Combine_51E3AC00_RGB_5;
                float2 _Combine_51E3AC00_RG_6;
                Unity_Combine_float(_Split_C6A87A8F_R_1, _Split_C6A87A8F_B_3, 0, 0, _Combine_51E3AC00_RGBA_4, _Combine_51E3AC00_RGB_5, _Combine_51E3AC00_RG_6);
                float2 _Multiply_34407C61_Out_2;
                Unity_Multiply_float(_Combine_51E3AC00_RG_6, (_CustomFunction_FB25DD51_windSpeed_2.xx), _Multiply_34407C61_Out_2);
                float2 _Add_F12CDF4E_Out_2;
                Unity_Add_float2(_Multiply_34407C61_Out_2, (_CustomFunction_FB25DD51_windFreq_7.xx), _Add_F12CDF4E_Out_2);
                float3 _CustomFunction_456640C0_vec_1;
                SampleWind_float((_Add_F12CDF4E_Out_2).x, _CustomFunction_456640C0_vec_1);
                float3 _Multiply_EC8FCAF4_Out_2;
                Unity_Multiply_float((_Property_AB75A479_Out_0.xxx), _CustomFunction_456640C0_vec_1, _Multiply_EC8FCAF4_Out_2);
                float3 _Multiply_611DED0C_Out_2;
                Unity_Multiply_float(_Multiply_EC8FCAF4_Out_2, (_CustomFunction_FB25DD51_windStrength_8.xxx), _Multiply_611DED0C_Out_2);
                float _Property_806E9BFF_Out_0 = Vector1_C9D81F7C;
                float3 _Multiply_45DFFD0A_Out_2;
                Unity_Multiply_float(_Multiply_611DED0C_Out_2, (_Property_806E9BFF_Out_0.xxx), _Multiply_45DFFD0A_Out_2);
                Offset_1 = _Multiply_45DFFD0A_Out_2;
            }
            
            // f18489b7b4b05a83941c32070dcf5796
            #include "Assets/Fantasy Adventure Environment/Shaders/URP/VSPro_HDIndirect.cginc"
            
            void AddPragma_float(float3 A, out float3 Out)
            {
                Out = A;
            }
            
            struct Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32
            {
            };
            
            void SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(float3 Vector3_314C8600, Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 IN, out float3 ObjectSpacePosition_1)
            {
                float3 _Property_AF5E8C93_Out_0 = Vector3_314C8600;
                float3 _CustomFunction_E07FEE57_Out_1;
                InjectSetup_float(_Property_AF5E8C93_Out_0, _CustomFunction_E07FEE57_Out_1);
                float3 _CustomFunction_18EFD858_Out_1;
                AddPragma_float(_CustomFunction_E07FEE57_Out_1, _CustomFunction_18EFD858_Out_1);
                ObjectSpacePosition_1 = _CustomFunction_18EFD858_Out_1;
            }
            
            void Unity_Lerp_float(float A, float B, float T, out float Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Length_float3(float3 In, out float Out)
            {
                Out = length(In);
            }
            
            void Unity_Fraction_float(float In, out float Out)
            {
                Out = frac(In);
            }
            
            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 WorldSpaceTangent;
                float3 ObjectSpaceBiTangent;
                float3 WorldSpaceBiTangent;
                float3 ObjectSpacePosition;
                float3 AbsoluteWorldSpacePosition;
                float4 uv1;
                float4 VertexColor;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Property_DB0143B3_Out_0 = _UseSpeedTreeWind;
                float _Split_A80EDD15_R_1 = IN.VertexColor[0];
                float _Split_A80EDD15_G_2 = IN.VertexColor[1];
                float _Split_A80EDD15_B_3 = IN.VertexColor[2];
                float _Split_A80EDD15_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_2543C842;
                _GlobalTreeWindMotion_2543C842.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_2543C842.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_2543C842.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_2543C842.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_2543C842_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(_Property_DB0143B3_Out_0, _Split_A80EDD15_A_4, _GlobalTreeWindMotion_2543C842, _GlobalTreeWindMotion_2543C842_Offset_1);
                float _Property_94051D9D_Out_0 = _WindAmplitudeMultiplier;
                float _Property_FA421571_Out_0 = _MaxWindStrength;
                Bindings_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a _LocalWindMotion_496CCFEB;
                _LocalWindMotion_496CCFEB.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                float3 _LocalWindMotion_496CCFEB_Offset_1;
                SG_LocalWindMotion_1962a3451584fb44fb07a0f06553eb4a(_Property_94051D9D_Out_0, _Property_FA421571_Out_0, _Split_A80EDD15_G_2, _LocalWindMotion_496CCFEB, _LocalWindMotion_496CCFEB_Offset_1);
                float3 _Add_59B53B15_Out_2;
                Unity_Add_float3(_GlobalTreeWindMotion_2543C842_Offset_1, _LocalWindMotion_496CCFEB_Offset_1, _Add_59B53B15_Out_2);
                float3 _Add_DF77ECB8_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Add_59B53B15_Out_2, _Add_DF77ECB8_Out_2);
                Bindings_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32 _VSProHDInstancedIndirect_A8293AE2;
                float3 _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                SG_VSProHDInstancedIndirect_5daaeae117458b94ca071c13e7a67c32(_Add_DF77ECB8_Out_2, _VSProHDInstancedIndirect_A8293AE2, _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1);
                float3 _Vector3_B7A96EB5_Out_0 = float3(0, 1, 0);
                float _Property_A8C540BD_Out_0 = _FlatLighting;
                float3 _Lerp_FAE7CF7_Out_3;
                Unity_Lerp_float3(IN.ObjectSpaceNormal, _Vector3_B7A96EB5_Out_0, (_Property_A8C540BD_Out_0.xxx), _Lerp_FAE7CF7_Out_3);
                description.VertexPosition = _VSProHDInstancedIndirect_A8293AE2_ObjectSpacePosition_1;
                description.VertexNormal = _Lerp_FAE7CF7_Out_3;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float4 uv0;
                float4 VertexColor;
            };
            
            struct SurfaceDescription
            {
                float3 Albedo;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Split_18BD7240_R_1 = IN.VertexColor[0];
                float _Split_18BD7240_G_2 = IN.VertexColor[1];
                float _Split_18BD7240_B_3 = IN.VertexColor[2];
                float _Split_18BD7240_A_4 = IN.VertexColor[3];
                float _Property_EDE257A8_Out_0 = _AmbientOcclusion;
                float _Lerp_418DAFB9_Out_3;
                Unity_Lerp_float(1, _Split_18BD7240_R_1, _Property_EDE257A8_Out_0, _Lerp_418DAFB9_Out_3);
                float _Property_9160F69B_Out_0 = _GradientBrightness;
                float4 _SampleTexture2D_E53DEB40_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_E53DEB40_R_4 = _SampleTexture2D_E53DEB40_RGBA_0.r;
                float _SampleTexture2D_E53DEB40_G_5 = _SampleTexture2D_E53DEB40_RGBA_0.g;
                float _SampleTexture2D_E53DEB40_B_6 = _SampleTexture2D_E53DEB40_RGBA_0.b;
                float _SampleTexture2D_E53DEB40_A_7 = _SampleTexture2D_E53DEB40_RGBA_0.a;
                float4 _Property_1C2D047C_Out_0 = _HueVariation;
                float _Split_4BC9A866_R_1 = _Property_1C2D047C_Out_0[0];
                float _Split_4BC9A866_G_2 = _Property_1C2D047C_Out_0[1];
                float _Split_4BC9A866_B_3 = _Property_1C2D047C_Out_0[2];
                float _Split_4BC9A866_A_4 = _Property_1C2D047C_Out_0[3];
                float _Length_5B09488C_Out_1;
                Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_5B09488C_Out_1);
                float _Fraction_73CC485B_Out_1;
                Unity_Fraction_float(_Length_5B09488C_Out_1, _Fraction_73CC485B_Out_1);
                float _Multiply_BFA62608_Out_2;
                Unity_Multiply_float(_Split_4BC9A866_A_4, _Fraction_73CC485B_Out_1, _Multiply_BFA62608_Out_2);
                float4 _Lerp_8219D562_Out_3;
                Unity_Lerp_float4(_SampleTexture2D_E53DEB40_RGBA_0, _Property_1C2D047C_Out_0, (_Multiply_BFA62608_Out_2.xxxx), _Lerp_8219D562_Out_3);
                float4 _Multiply_9984BC3D_Out_2;
                Unity_Multiply_float((_Property_9160F69B_Out_0.xxxx), _Lerp_8219D562_Out_3, _Multiply_9984BC3D_Out_2);
                float _Multiply_116AF097_Out_2;
                Unity_Multiply_float(_Split_18BD7240_A_4, 10, _Multiply_116AF097_Out_2);
                float _Saturate_338C849F_Out_1;
                Unity_Saturate_float(_Multiply_116AF097_Out_2, _Saturate_338C849F_Out_1);
                float4 _Lerp_82D3B781_Out_3;
                Unity_Lerp_float4(_Multiply_9984BC3D_Out_2, _Lerp_8219D562_Out_3, (_Saturate_338C849F_Out_1.xxxx), _Lerp_82D3B781_Out_3);
                float4 _Multiply_5840E19_Out_2;
                Unity_Multiply_float((_Lerp_418DAFB9_Out_3.xxxx), _Lerp_82D3B781_Out_3, _Multiply_5840E19_Out_2);
                float _Property_3912DE45_Out_0 = _Cutoff;
                surface.Albedo = (_Multiply_5840E19_Out_2.xyz);
                surface.Alpha = _SampleTexture2D_E53DEB40_A_7;
                surface.AlphaClipThreshold = _Property_3912DE45_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                output.interp01.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                output.color = input.interp01.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                output.ObjectSpacePosition =         input.positionOS;
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.uv0 =                         input.texCoord0;
                output.VertexColor =                 input.color;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
            
#include "./../../../GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
ENDHLSL
        }
        
    }
    CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}
