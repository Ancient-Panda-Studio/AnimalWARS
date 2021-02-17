#ifndef UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED
#define UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 position : POSITION;
    float2 texcoord : TEXCOORD0;
    half4 vertexColors : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv : TEXCOORD0;
    float4 positionCS : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

    Varyings DepthOnlyVertex(Attributes input)
    {
        Varyings output = (Varyings) 0;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

        output.uv = input.texcoord;
        
        #if(defined _GLOBALWIND && defined _WINDENABLED_ON)
            float3 worldPos = newVertexWorldPos(input.position.xyz, input.vertexColors, _WindInfluence);
            output.positionCS = newVertexClipPos(worldPos);
        #else
            output.positionCS = TransformObjectToHClip(input.position.xyz);
        #endif
        return output;
    }

    half4 DepthOnlyFragment(Varyings input) : SV_TARGET
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        half4 textureValues = tex2D(_BaseMap, input.uv).rgba;
        clip(textureValues.a - _Cutoff);
        return 0;
    }
#endif
