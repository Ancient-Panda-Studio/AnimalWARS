Shader "GPUInstancer/PBR Master"
{
    Properties
    {
        [NoScaleOffset]_Atlas("Atlas", 2D) = "white" {}
        [NoScaleOffset]_Normals("Normals", 2D) = "white" {}
        _Variationcolor("Variation color", Color) = (1, 0.5587921, 0, 0.1333333)
        _Cutoff("Cutoff", Float) = 0.5
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
            Cull Back
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
            #define _NORMALMAP 1
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
            float4 _Variationcolor;
            float _Cutoff;
            CBUFFER_END
            TEXTURE2D(_Atlas); SAMPLER(sampler_Atlas); float4 _Atlas_TexelSize;
            TEXTURE2D(_Normals); SAMPLER(sampler_Normals); float4 _Normals_TexelSize;
            SAMPLER(_SampleTexture2D_12F2B546_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_DAEB1803_Sampler_3_Linear_Repeat);
        
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
                float _Split_8383708B_R_1 = IN.VertexColor[0];
                float _Split_8383708B_G_2 = IN.VertexColor[1];
                float _Split_8383708B_B_3 = IN.VertexColor[2];
                float _Split_8383708B_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_B242AF30;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_B242AF30.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_B242AF30_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(0, _Split_8383708B_A_4, _GlobalTreeWindMotion_B242AF30, _GlobalTreeWindMotion_B242AF30_Offset_1);
                float3 _Add_13BB6B94_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _GlobalTreeWindMotion_B242AF30_Offset_1, _Add_13BB6B94_Out_2);
                description.VertexPosition = _Add_13BB6B94_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
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
                float4 _SampleTexture2D_12F2B546_RGBA_0 = SAMPLE_TEXTURE2D(_Atlas, sampler_Atlas, IN.uv0.xy);
                float _SampleTexture2D_12F2B546_R_4 = _SampleTexture2D_12F2B546_RGBA_0.r;
                float _SampleTexture2D_12F2B546_G_5 = _SampleTexture2D_12F2B546_RGBA_0.g;
                float _SampleTexture2D_12F2B546_B_6 = _SampleTexture2D_12F2B546_RGBA_0.b;
                float _SampleTexture2D_12F2B546_A_7 = _SampleTexture2D_12F2B546_RGBA_0.a;
                float4 _Property_D99C2C06_Out_0 = _Variationcolor;
                float _Split_77D4BD6C_R_1 = _Property_D99C2C06_Out_0[0];
                float _Split_77D4BD6C_G_2 = _Property_D99C2C06_Out_0[1];
                float _Split_77D4BD6C_B_3 = _Property_D99C2C06_Out_0[2];
                float _Split_77D4BD6C_A_4 = _Property_D99C2C06_Out_0[3];
                float _Length_11D7A1DF_Out_1;
                Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_11D7A1DF_Out_1);
                float _Fraction_5B4509B7_Out_1;
                Unity_Fraction_float(_Length_11D7A1DF_Out_1, _Fraction_5B4509B7_Out_1);
                float _Multiply_A28C52CE_Out_2;
                Unity_Multiply_float(_Split_77D4BD6C_A_4, _Fraction_5B4509B7_Out_1, _Multiply_A28C52CE_Out_2);
                float4 _Lerp_20DB7A0C_Out_3;
                Unity_Lerp_float4(_SampleTexture2D_12F2B546_RGBA_0, _Property_D99C2C06_Out_0, (_Multiply_A28C52CE_Out_2.xxxx), _Lerp_20DB7A0C_Out_3);
                float4 _SampleTexture2D_DAEB1803_RGBA_0 = SAMPLE_TEXTURE2D(_Normals, sampler_Normals, IN.uv0.xy);
                _SampleTexture2D_DAEB1803_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_DAEB1803_RGBA_0);
                float _SampleTexture2D_DAEB1803_R_4 = _SampleTexture2D_DAEB1803_RGBA_0.r;
                float _SampleTexture2D_DAEB1803_G_5 = _SampleTexture2D_DAEB1803_RGBA_0.g;
                float _SampleTexture2D_DAEB1803_B_6 = _SampleTexture2D_DAEB1803_RGBA_0.b;
                float _SampleTexture2D_DAEB1803_A_7 = _SampleTexture2D_DAEB1803_RGBA_0.a;
                float _Split_F62611C3_R_1 = _SampleTexture2D_12F2B546_RGBA_0[0];
                float _Split_F62611C3_G_2 = _SampleTexture2D_12F2B546_RGBA_0[1];
                float _Split_F62611C3_B_3 = _SampleTexture2D_12F2B546_RGBA_0[2];
                float _Split_F62611C3_A_4 = _SampleTexture2D_12F2B546_RGBA_0[3];
                float _Property_CF916119_Out_0 = _Cutoff;
                surface.Albedo = (_Lerp_20DB7A0C_Out_3.xyz);
                surface.Normal = (_SampleTexture2D_DAEB1803_RGBA_0.xyz);
                surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                surface.Metallic = 0;
                surface.Smoothness = 0;
                surface.Occlusion = 1;
                surface.Alpha = _Split_F62611C3_A_4;
                surface.AlphaClipThreshold = _Property_CF916119_Out_0;
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
                float3 interp04 : TEXCOORD4;
                float2 interp05 : TEXCOORD5;
                float3 interp06 : TEXCOORD6;
                float4 interp07 : TEXCOORD7;
                float4 interp08 : TEXCOORD8;
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
                output.interp04.xyz = input.viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                output.interp05.xy = input.lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.interp06.xyz = input.sh;
                #endif
                output.interp07.xyzw = input.fogFactorAndVertexLight;
                output.interp08.xyzw = input.shadowCoord;
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
                output.viewDirectionWS = input.interp04.xyz;
                #if defined(LIGHTMAP_ON)
                output.lightmapUV = input.interp05.xy;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.sh = input.interp06.xyz;
                #endif
                output.fogFactorAndVertexLight = input.interp07.xyzw;
                output.shadowCoord = input.interp08.xyzw;
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
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
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
            Cull Back
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
            #define _NORMALMAP 1
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
            float4 _Variationcolor;
            float _Cutoff;
            CBUFFER_END
            TEXTURE2D(_Atlas); SAMPLER(sampler_Atlas); float4 _Atlas_TexelSize;
            TEXTURE2D(_Normals); SAMPLER(sampler_Normals); float4 _Normals_TexelSize;
            SAMPLER(_SampleTexture2D_12F2B546_Sampler_3_Linear_Repeat);
        
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
                float _Split_8383708B_R_1 = IN.VertexColor[0];
                float _Split_8383708B_G_2 = IN.VertexColor[1];
                float _Split_8383708B_B_3 = IN.VertexColor[2];
                float _Split_8383708B_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_B242AF30;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_B242AF30.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_B242AF30_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(0, _Split_8383708B_A_4, _GlobalTreeWindMotion_B242AF30, _GlobalTreeWindMotion_B242AF30_Offset_1);
                float3 _Add_13BB6B94_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _GlobalTreeWindMotion_B242AF30_Offset_1, _Add_13BB6B94_Out_2);
                description.VertexPosition = _Add_13BB6B94_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
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
                float4 _SampleTexture2D_12F2B546_RGBA_0 = SAMPLE_TEXTURE2D(_Atlas, sampler_Atlas, IN.uv0.xy);
                float _SampleTexture2D_12F2B546_R_4 = _SampleTexture2D_12F2B546_RGBA_0.r;
                float _SampleTexture2D_12F2B546_G_5 = _SampleTexture2D_12F2B546_RGBA_0.g;
                float _SampleTexture2D_12F2B546_B_6 = _SampleTexture2D_12F2B546_RGBA_0.b;
                float _SampleTexture2D_12F2B546_A_7 = _SampleTexture2D_12F2B546_RGBA_0.a;
                float _Split_F62611C3_R_1 = _SampleTexture2D_12F2B546_RGBA_0[0];
                float _Split_F62611C3_G_2 = _SampleTexture2D_12F2B546_RGBA_0[1];
                float _Split_F62611C3_B_3 = _SampleTexture2D_12F2B546_RGBA_0[2];
                float _Split_F62611C3_A_4 = _SampleTexture2D_12F2B546_RGBA_0[3];
                float _Property_CF916119_Out_0 = _Cutoff;
                surface.Alpha = _Split_F62611C3_A_4;
                surface.AlphaClipThreshold = _Property_CF916119_Out_0;
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
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
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
            Cull Back
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
            #define _NORMALMAP 1
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
            float4 _Variationcolor;
            float _Cutoff;
            CBUFFER_END
            TEXTURE2D(_Atlas); SAMPLER(sampler_Atlas); float4 _Atlas_TexelSize;
            TEXTURE2D(_Normals); SAMPLER(sampler_Normals); float4 _Normals_TexelSize;
            SAMPLER(_SampleTexture2D_12F2B546_Sampler_3_Linear_Repeat);
        
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
                float _Split_8383708B_R_1 = IN.VertexColor[0];
                float _Split_8383708B_G_2 = IN.VertexColor[1];
                float _Split_8383708B_B_3 = IN.VertexColor[2];
                float _Split_8383708B_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_B242AF30;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_B242AF30.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_B242AF30_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(0, _Split_8383708B_A_4, _GlobalTreeWindMotion_B242AF30, _GlobalTreeWindMotion_B242AF30_Offset_1);
                float3 _Add_13BB6B94_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _GlobalTreeWindMotion_B242AF30_Offset_1, _Add_13BB6B94_Out_2);
                description.VertexPosition = _Add_13BB6B94_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
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
                float4 _SampleTexture2D_12F2B546_RGBA_0 = SAMPLE_TEXTURE2D(_Atlas, sampler_Atlas, IN.uv0.xy);
                float _SampleTexture2D_12F2B546_R_4 = _SampleTexture2D_12F2B546_RGBA_0.r;
                float _SampleTexture2D_12F2B546_G_5 = _SampleTexture2D_12F2B546_RGBA_0.g;
                float _SampleTexture2D_12F2B546_B_6 = _SampleTexture2D_12F2B546_RGBA_0.b;
                float _SampleTexture2D_12F2B546_A_7 = _SampleTexture2D_12F2B546_RGBA_0.a;
                float _Split_F62611C3_R_1 = _SampleTexture2D_12F2B546_RGBA_0[0];
                float _Split_F62611C3_G_2 = _SampleTexture2D_12F2B546_RGBA_0[1];
                float _Split_F62611C3_B_3 = _SampleTexture2D_12F2B546_RGBA_0[2];
                float _Split_F62611C3_A_4 = _SampleTexture2D_12F2B546_RGBA_0[3];
                float _Property_CF916119_Out_0 = _Cutoff;
                surface.Alpha = _Split_F62611C3_A_4;
                surface.AlphaClipThreshold = _Property_CF916119_Out_0;
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
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
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
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
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
            float4 _Variationcolor;
            float _Cutoff;
            CBUFFER_END
            TEXTURE2D(_Atlas); SAMPLER(sampler_Atlas); float4 _Atlas_TexelSize;
            TEXTURE2D(_Normals); SAMPLER(sampler_Normals); float4 _Normals_TexelSize;
            SAMPLER(_SampleTexture2D_12F2B546_Sampler_3_Linear_Repeat);
        
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
                float _Split_8383708B_R_1 = IN.VertexColor[0];
                float _Split_8383708B_G_2 = IN.VertexColor[1];
                float _Split_8383708B_B_3 = IN.VertexColor[2];
                float _Split_8383708B_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_B242AF30;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_B242AF30.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_B242AF30_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(0, _Split_8383708B_A_4, _GlobalTreeWindMotion_B242AF30, _GlobalTreeWindMotion_B242AF30_Offset_1);
                float3 _Add_13BB6B94_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _GlobalTreeWindMotion_B242AF30_Offset_1, _Add_13BB6B94_Out_2);
                description.VertexPosition = _Add_13BB6B94_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
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
                float4 _SampleTexture2D_12F2B546_RGBA_0 = SAMPLE_TEXTURE2D(_Atlas, sampler_Atlas, IN.uv0.xy);
                float _SampleTexture2D_12F2B546_R_4 = _SampleTexture2D_12F2B546_RGBA_0.r;
                float _SampleTexture2D_12F2B546_G_5 = _SampleTexture2D_12F2B546_RGBA_0.g;
                float _SampleTexture2D_12F2B546_B_6 = _SampleTexture2D_12F2B546_RGBA_0.b;
                float _SampleTexture2D_12F2B546_A_7 = _SampleTexture2D_12F2B546_RGBA_0.a;
                float4 _Property_D99C2C06_Out_0 = _Variationcolor;
                float _Split_77D4BD6C_R_1 = _Property_D99C2C06_Out_0[0];
                float _Split_77D4BD6C_G_2 = _Property_D99C2C06_Out_0[1];
                float _Split_77D4BD6C_B_3 = _Property_D99C2C06_Out_0[2];
                float _Split_77D4BD6C_A_4 = _Property_D99C2C06_Out_0[3];
                float _Length_11D7A1DF_Out_1;
                Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_11D7A1DF_Out_1);
                float _Fraction_5B4509B7_Out_1;
                Unity_Fraction_float(_Length_11D7A1DF_Out_1, _Fraction_5B4509B7_Out_1);
                float _Multiply_A28C52CE_Out_2;
                Unity_Multiply_float(_Split_77D4BD6C_A_4, _Fraction_5B4509B7_Out_1, _Multiply_A28C52CE_Out_2);
                float4 _Lerp_20DB7A0C_Out_3;
                Unity_Lerp_float4(_SampleTexture2D_12F2B546_RGBA_0, _Property_D99C2C06_Out_0, (_Multiply_A28C52CE_Out_2.xxxx), _Lerp_20DB7A0C_Out_3);
                float _Split_F62611C3_R_1 = _SampleTexture2D_12F2B546_RGBA_0[0];
                float _Split_F62611C3_G_2 = _SampleTexture2D_12F2B546_RGBA_0[1];
                float _Split_F62611C3_B_3 = _SampleTexture2D_12F2B546_RGBA_0[2];
                float _Split_F62611C3_A_4 = _SampleTexture2D_12F2B546_RGBA_0[3];
                float _Property_CF916119_Out_0 = _Cutoff;
                surface.Albedo = (_Lerp_20DB7A0C_Out_3.xyz);
                surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                surface.Alpha = _Split_F62611C3_A_4;
                surface.AlphaClipThreshold = _Property_CF916119_Out_0;
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
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
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
            Cull Back
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
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
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
            float4 _Variationcolor;
            float _Cutoff;
            CBUFFER_END
            TEXTURE2D(_Atlas); SAMPLER(sampler_Atlas); float4 _Atlas_TexelSize;
            TEXTURE2D(_Normals); SAMPLER(sampler_Normals); float4 _Normals_TexelSize;
            SAMPLER(_SampleTexture2D_12F2B546_Sampler_3_Linear_Repeat);
        
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
                float _Split_8383708B_R_1 = IN.VertexColor[0];
                float _Split_8383708B_G_2 = IN.VertexColor[1];
                float _Split_8383708B_B_3 = IN.VertexColor[2];
                float _Split_8383708B_A_4 = IN.VertexColor[3];
                Bindings_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00 _GlobalTreeWindMotion_B242AF30;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceNormal = IN.WorldSpaceNormal;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceTangent = IN.WorldSpaceTangent;
                _GlobalTreeWindMotion_B242AF30.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                _GlobalTreeWindMotion_B242AF30.uv1 = IN.uv1;
                float3 _GlobalTreeWindMotion_B242AF30_Offset_1;
                SG_GlobalTreeWindMotion_31693f3820a19d94cacd9a30b4cafb00(0, _Split_8383708B_A_4, _GlobalTreeWindMotion_B242AF30, _GlobalTreeWindMotion_B242AF30_Offset_1);
                float3 _Add_13BB6B94_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _GlobalTreeWindMotion_B242AF30_Offset_1, _Add_13BB6B94_Out_2);
                description.VertexPosition = _Add_13BB6B94_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
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
                float4 _SampleTexture2D_12F2B546_RGBA_0 = SAMPLE_TEXTURE2D(_Atlas, sampler_Atlas, IN.uv0.xy);
                float _SampleTexture2D_12F2B546_R_4 = _SampleTexture2D_12F2B546_RGBA_0.r;
                float _SampleTexture2D_12F2B546_G_5 = _SampleTexture2D_12F2B546_RGBA_0.g;
                float _SampleTexture2D_12F2B546_B_6 = _SampleTexture2D_12F2B546_RGBA_0.b;
                float _SampleTexture2D_12F2B546_A_7 = _SampleTexture2D_12F2B546_RGBA_0.a;
                float4 _Property_D99C2C06_Out_0 = _Variationcolor;
                float _Split_77D4BD6C_R_1 = _Property_D99C2C06_Out_0[0];
                float _Split_77D4BD6C_G_2 = _Property_D99C2C06_Out_0[1];
                float _Split_77D4BD6C_B_3 = _Property_D99C2C06_Out_0[2];
                float _Split_77D4BD6C_A_4 = _Property_D99C2C06_Out_0[3];
                float _Length_11D7A1DF_Out_1;
                Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_11D7A1DF_Out_1);
                float _Fraction_5B4509B7_Out_1;
                Unity_Fraction_float(_Length_11D7A1DF_Out_1, _Fraction_5B4509B7_Out_1);
                float _Multiply_A28C52CE_Out_2;
                Unity_Multiply_float(_Split_77D4BD6C_A_4, _Fraction_5B4509B7_Out_1, _Multiply_A28C52CE_Out_2);
                float4 _Lerp_20DB7A0C_Out_3;
                Unity_Lerp_float4(_SampleTexture2D_12F2B546_RGBA_0, _Property_D99C2C06_Out_0, (_Multiply_A28C52CE_Out_2.xxxx), _Lerp_20DB7A0C_Out_3);
                float _Split_F62611C3_R_1 = _SampleTexture2D_12F2B546_RGBA_0[0];
                float _Split_F62611C3_G_2 = _SampleTexture2D_12F2B546_RGBA_0[1];
                float _Split_F62611C3_B_3 = _SampleTexture2D_12F2B546_RGBA_0[2];
                float _Split_F62611C3_A_4 = _SampleTexture2D_12F2B546_RGBA_0[3];
                float _Property_CF916119_Out_0 = _Cutoff;
                surface.Albedo = (_Lerp_20DB7A0C_Out_3.xyz);
                surface.Alpha = _Split_F62611C3_A_4;
                surface.AlphaClipThreshold = _Property_CF916119_Out_0;
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
                output.uv1 =                         input.uv1;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
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
