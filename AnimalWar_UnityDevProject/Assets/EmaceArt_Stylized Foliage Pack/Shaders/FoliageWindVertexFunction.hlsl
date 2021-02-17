float4 _windDirection;
float4 _windStrenght_Speed_WaveSize_AngleSpread;
float4 _wiggles_Speed_WaveSize_NoiseTiling;

sampler2D _windNoiseTex;
float _windNoiseTiling;
float _windAngleSpread;
float _windSpeed;
float _windStrenght;
float _wigglesSpeed;
float _wiggleWaveSize;
float _wigglesStrength;
float _windWaveSize;


inline float windCurve(float windSpeed, float windAmplitude, float wiggleSpeed, float wigglesWaveSize, float wigglesStrength, float windValues, float phaseOffset, float wiggleValues, float windWaveSize, float directionDot, float worldPosY)
{


    float temp_output_54_0_g7 = phaseOffset;
    float temp_output_206_0_g7 = (temp_output_54_0_g7 + _Time.y);
    float wiggleSpeed23 = wiggleSpeed;
    float temp_output_92_0_g7 = (temp_output_206_0_g7 * wiggleSpeed23);
    float directionDot87_g7 = directionDot;
    float wigglesWaveSize22 = wigglesWaveSize;
    float temp_output_100_0_g7 = (wigglesWaveSize22 * 1.0);
    float temp_output_150_0_g7 = (directionDot87_g7 * temp_output_100_0_g7);
    float temp_output_149_0_g7 = (temp_output_92_0_g7 + temp_output_150_0_g7);
    float wiggleValues137_g7 = wiggleValues;
    float wigglesStrength24 = wigglesStrength;
    float wigglesAmplitude139_g7 = wigglesStrength24;
    float temp_output_121_0_g7 = cos((cos((frac((sin((temp_output_149_0_g7 + (wiggleValues137_g7 * PI * 0.388))) * 0.46)) * PI)) * PI * sin(((sin(((wigglesAmplitude139_g7 * PI * 0.228) + temp_output_149_0_g7)) * 0.46) * PI))));
    float temp_output_106_0_g7 = abs(((frac(((worldPosY * temp_output_100_0_g7) + temp_output_92_0_g7)) - 0.5) * 2.0));
    float windStrength28 = windAmplitude;
    float windAmlitude81_g7 = windStrength28;
    float windSpeed27 = windSpeed;
    float temp_output_11_0_g7 = windSpeed27;
    float windWaveSize26 = windWaveSize;
    float temp_output_2_0_g7 = windWaveSize26;
    float temp_output_7_0_g7 = abs(((frac((temp_output_2_0_g7 * directionDot87_g7)) - 0.5) * 2.0));
    float fractalWave209_g7 = temp_output_7_0_g7;
    float temp_output_214_0_g7 = (fractalWave209_g7 * PI);
    float temp_output_219_0_g7 = (temp_output_54_0_g7 * PI);
    float temp_output_21_0_g7 = cos(sin((frac(sin(((temp_output_11_0_g7 * -0.18 * (temp_output_11_0_g7 * _Time.y)) + temp_output_214_0_g7 + temp_output_219_0_g7))) * PI)));
    float temp_output_211_0_g7 = (temp_output_11_0_g7 * (_Time.y + temp_output_214_0_g7 + temp_output_219_0_g7));
    float windValues84_g7 = windAmplitude;
    float temp_output_51_0_g7 = (temp_output_21_0_g7 * cos((cos((frac((sin((temp_output_211_0_g7 + (windValues84_g7 * PI * 0.06))) * 0.23)) * PI)) * PI * sin(((sin(((windAmlitude81_g7 * PI * 0.09) + temp_output_211_0_g7)) * 0.26) * PI)))));
    return ((temp_output_121_0_g7 * (cos(sin((frac(sin((((temp_output_92_0_g7 * temp_output_150_0_g7) + wiggleValues137_g7) * 3.231))) * PI))) * 2.0 + -1.0) * wigglesAmplitude139_g7 * temp_output_106_0_g7) + (windAmlitude81_g7 * temp_output_51_0_g7));

    
}

inline float3 RotateAroundAxis(float3 center, float3 original, float3 u, float angle)
{
    original -= center;
    float C = cos(angle);
    float S = sin(angle);
    float t = 1 - C;
    float m00 = t * u.x * u.x + C;
    float m01 = t * u.x * u.y - S * u.z;
    float m02 = t * u.x * u.z + S * u.y;
    float m10 = t * u.x * u.y + S * u.z;
    float m11 = t * u.y * u.y + C;
    float m12 = t * u.y * u.z - S * u.x;
    float m20 = t * u.x * u.z - S * u.y;
    float m21 = t * u.y * u.z + S * u.x;
    float m22 = t * u.z * u.z + C;
    float3x3 finalMatrix = float3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
    return mul(finalMatrix, original) + center;
}

inline float4 newVertexClipPos(float3 vertexWorldPos)
{
    return TransformWorldToHClip(vertexWorldPos);
}



inline float3 newVertexWorldPos(float3 vertexObjectPos,float4 vertexColors, float bending)
{  

    
     ///
    half windStrength = _windStrenght_Speed_WaveSize_AngleSpread.x;
    half windSpeed = _windStrenght_Speed_WaveSize_AngleSpread.y;
    half windWaveSize = _windStrenght_Speed_WaveSize_AngleSpread.z;
    half windAngleSpread = _windStrenght_Speed_WaveSize_AngleSpread.w;
   
   
    half wigglesSpeed = _wiggles_Speed_WaveSize_NoiseTiling.y;
    half wigglesStrength = _wiggles_Speed_WaveSize_NoiseTiling.x;
    half wiggleWaveSize = _wiggles_Speed_WaveSize_NoiseTiling.z;
    half windNoiseTiling = _wiggles_Speed_WaveSize_NoiseTiling.w;

    float4 vertexColorsValue = vertexColors;
    float3 objectWorldPos = mul(GetObjectToWorldMatrix(), float4(0.0f, 0.0f, 0.0f, 1.0f)).xyz;
    float2 noiseTexCoords = ((objectWorldPos).xz * windNoiseTiling);
    float4 noiseFromTexture = tex2Dlod(_windNoiseTex, float4(noiseTexCoords, 0.0f, 0.0f));
    float3 worldPos = TransformObjectToWorld(vertexObjectPos);

    half angleOffset = (((frac((vertexColorsValue).b + (noiseFromTexture).g) * PI) - PI) * windAngleSpread);
    half phaseOffset = ((vertexColorsValue).g + (noiseFromTexture).r) * _windDirection.w;

    float2 windDirXZ = (float2(_windDirection.x, _windDirection.z));

    float cosAngle = cos(angleOffset);
    float sinAngle = sin(angleOffset);
    float2 rotatedWindDir = mul(windDirXZ, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle));
    half3 newWindDir = normalize((float3(rotatedWindDir.x, _windDirection.y, rotatedWindDir.y)));

    float worldYaxisScale = length(mul(GetObjectToWorldMatrix(), float4(0.0f, 1.0f, 0.0f, 0.0f)));
    float mainWindStrenght = (vertexColorsValue.a + noiseFromTexture.b) * worldYaxisScale;
    float wiggleValues = ((vertexColorsValue.r + noiseFromTexture.b) * worldYaxisScale);
    float wigglesAmplitude = wigglesStrength * noiseFromTexture.b;

    float3 scaledWorldPos = float3(noiseTexCoords.x, (objectWorldPos).y, noiseTexCoords.y);
    float windDotDirection = dot(scaledWorldPos, _windDirection.xyz);
    ///

    float windFromCurve = windCurve(windSpeed, windStrength* vertexColorsValue.a, wigglesSpeed, wiggleWaveSize, wigglesAmplitude * vertexColorsValue.r, mainWindStrenght, phaseOffset, wiggleValues, windWaveSize, windDotDirection, worldPos.y);
    float actualWindStrength = (windFromCurve *  bending);
    float3 rotatedVertices = RotateAroundAxis(objectWorldPos, worldPos.xyz, newWindDir, actualWindStrength);
    
    return rotatedVertices;

}

inline float3 newVertexWorldPos(float3 vertexObjectPos, float4 vertexColors, float bending, out half4 debugColors)
{

    
     ///
    half windStrength = _windStrenght_Speed_WaveSize_AngleSpread.x;
    half windSpeed = _windStrenght_Speed_WaveSize_AngleSpread.y;
    half windWaveSize = _windStrenght_Speed_WaveSize_AngleSpread.z;
    half windAngleSpread = _windStrenght_Speed_WaveSize_AngleSpread.w;
   
   
    half wigglesSpeed = _wiggles_Speed_WaveSize_NoiseTiling.y;
    half wigglesStrength = _wiggles_Speed_WaveSize_NoiseTiling.x;
    half wiggleWaveSize = _wiggles_Speed_WaveSize_NoiseTiling.z;
    half windNoiseTiling = _wiggles_Speed_WaveSize_NoiseTiling.w;

    float4 vertexColorsValue = vertexColors;
    float3 objectWorldPos = mul(GetObjectToWorldMatrix(), float4(0.0f, 0.0f, 0.0f, 1.0f)).xyz;
    float2 noiseTexCoords = ((objectWorldPos).xz * windNoiseTiling);
    float4 noiseFromTexture = tex2Dlod(_windNoiseTex, float4(noiseTexCoords, 0.0f, 0.0f));
    float3 worldPos = TransformObjectToWorld(vertexObjectPos);

    half angleOffset = (((frac((vertexColorsValue).b + (noiseFromTexture).g) * PI) - PI) * windAngleSpread);
    half phaseOffset = ((vertexColorsValue).g + (noiseFromTexture).r) * _windDirection.w;

    float2 windDirXZ = (float2(_windDirection.x, _windDirection.z));

    float cosAngle = cos(angleOffset);
    float sinAngle = sin(angleOffset);
    float2 rotatedWindDir = mul(windDirXZ, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle));
    half3 newWindDir = normalize((float3(rotatedWindDir.x, _windDirection.y, rotatedWindDir.y)));

    float worldYaxisScale = length(mul(GetObjectToWorldMatrix(), float4(0.0f, 1.0f, 0.0f, 0.0f)));
    float mainWindStrenght = (vertexColorsValue.a + noiseFromTexture.b) * worldYaxisScale;
    float wiggleValues = ((vertexColorsValue.r + noiseFromTexture.b) * worldYaxisScale);
    float wigglesAmplitude = wigglesStrength * noiseFromTexture.b;

    float3 scaledWorldPos = float3(noiseTexCoords.x, (objectWorldPos).y, noiseTexCoords.y);
    float windDotDirection = dot(scaledWorldPos, _windDirection.xyz);
    ///

    float windFromCurve = windCurve(windSpeed, windStrength* vertexColorsValue.a, wigglesSpeed, wiggleWaveSize, wigglesAmplitude * vertexColorsValue.r, mainWindStrenght, phaseOffset, wiggleValues, windWaveSize, windDotDirection, worldPos.y);
    float actualWindStrength = (windFromCurve *  bending);
    float3 rotatedVertices = RotateAroundAxis(objectWorldPos, worldPos.xyz, newWindDir, actualWindStrength);

    float directionDot = abs(((frac((windWaveSize * windDotDirection)) - 0.5) * 2.0));
    
    debugColors = noiseFromTexture * directionDot;
    
    return rotatedVertices;

}

inline float3 newWolrdNormalPos(float3 vertexObjectPos, float4 vertexColors, float bending, float3 normal)
{
    
    ///
    half windStrength = _windStrenght_Speed_WaveSize_AngleSpread.x;
    half windSpeed = _windStrenght_Speed_WaveSize_AngleSpread.y;
    half windWaveSize = _windStrenght_Speed_WaveSize_AngleSpread.z;
    half windAngleSpread = _windStrenght_Speed_WaveSize_AngleSpread.w;
   
   
    half wigglesSpeed = _wiggles_Speed_WaveSize_NoiseTiling.y;
    half wigglesStrength = _wiggles_Speed_WaveSize_NoiseTiling.x;
    half wiggleWaveSize = _wiggles_Speed_WaveSize_NoiseTiling.z;
    half windNoiseTiling = _wiggles_Speed_WaveSize_NoiseTiling.w;

    float4 vertexColorsValue = vertexColors;
    float3 objectWorldPos = mul(GetObjectToWorldMatrix(), float4(0.0f, 0.0f, 0.0f, 1.0f)).xyz;
    float2 noiseTexCoords = ((objectWorldPos).xz * windNoiseTiling);
    float4 noiseFromTexture = tex2Dlod(_windNoiseTex, float4(noiseTexCoords, 0.0f, 0.0f));
    float3 worldPos = TransformObjectToWorld(vertexObjectPos);

    half angleOffset = (((frac((vertexColorsValue).b + (noiseFromTexture).g) * PI) - PI) * windAngleSpread);
    half phaseOffset = ((vertexColorsValue).g + (noiseFromTexture).r) * _windDirection.w;

    float2 windDirXZ = (float2(_windDirection.x, _windDirection.z));

    float cosAngle = cos(angleOffset);
    float sinAngle = sin(angleOffset);
    float2 rotatedWindDir = mul(windDirXZ, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle));
    half3 newWindDir = normalize((float3(rotatedWindDir.x, _windDirection.y, rotatedWindDir.y)));

    float worldYaxisScale = length(mul(GetObjectToWorldMatrix(), float4(0.0f, 1.0f, 0.0f, 0.0f)));
    float mainWindStrenght = (vertexColorsValue.a + noiseFromTexture.b) * worldYaxisScale;
    float wiggleValues = ((vertexColorsValue.r + noiseFromTexture.b) * worldYaxisScale);
    float wigglesAmplitude = wigglesStrength * noiseFromTexture.b;

    float3 scaledWorldPos = float3(noiseTexCoords.x, (objectWorldPos).y, noiseTexCoords.y);
    float windDotDirection = dot(scaledWorldPos, _windDirection.xyz);
    ///
    float windFromCurve = windCurve(windSpeed, windStrength* vertexColorsValue.a, wigglesSpeed, wiggleWaveSize, wigglesAmplitude * vertexColorsValue.r, mainWindStrenght, phaseOffset, wiggleValues, windWaveSize, windDotDirection, worldPos.y);
    float actualWindStrength = (windFromCurve *  bending);
    float3 rotatedVertices = RotateAroundAxis(objectWorldPos, worldPos.xyz, newWindDir, actualWindStrength);
    float3 rotatedVerticesWithNormals = RotateAroundAxis(objectWorldPos, worldPos.xyz + TransformObjectToWorldNormal(normal), newWindDir, actualWindStrength);

    float3 newNormal = normalize(rotatedVerticesWithNormals - rotatedVertices);

    return newNormal;
    
    //return TransformObjectToWorldNormal(normal);

}

inline void newWolrdNormalPosMain(float3 vertexObjectPos, float4 vertexColors, float bending, inout float3 normal, inout float3 tangent, inout float3 newWorldPos)
{
     ///
    half windStrength = _windStrenght_Speed_WaveSize_AngleSpread.x;
    half windSpeed = _windStrenght_Speed_WaveSize_AngleSpread.y;
    half windWaveSize = _windStrenght_Speed_WaveSize_AngleSpread.z;
    half windAngleSpread = _windStrenght_Speed_WaveSize_AngleSpread.w;
   
   
    half wigglesSpeed = _wiggles_Speed_WaveSize_NoiseTiling.y;
    half wigglesStrength = _wiggles_Speed_WaveSize_NoiseTiling.x;
    half wiggleWaveSize = _wiggles_Speed_WaveSize_NoiseTiling.z;
    half windNoiseTiling = _wiggles_Speed_WaveSize_NoiseTiling.w;

    float4 vertexColorsValue = vertexColors;
    float3 objectWorldPos = mul(GetObjectToWorldMatrix(), float4(0.0f, 0.0f, 0.0f, 1.0f)).xyz;
    float2 noiseTexCoords = ((objectWorldPos).xz * windNoiseTiling);
    float4 noiseFromTexture = tex2Dlod(_windNoiseTex, float4(noiseTexCoords, 0.0f, 0.0f));
    float3 worldPos = TransformObjectToWorld(vertexObjectPos);

    half angleOffset = (((frac((vertexColorsValue).b + (noiseFromTexture).g) * PI) - PI) * windAngleSpread);
    half phaseOffset = ((vertexColorsValue).g + (noiseFromTexture).r) * _windDirection.w;

    float2 windDirXZ = (float2(_windDirection.x, _windDirection.z));

    float cosAngle = cos(angleOffset);
    float sinAngle = sin(angleOffset);
    float2 rotatedWindDir = mul(windDirXZ, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle));
    half3 newWindDir = normalize((float3(rotatedWindDir.x, _windDirection.y, rotatedWindDir.y)));

    float worldYaxisScale = length(mul(GetObjectToWorldMatrix(), float4(0.0f, 1.0f, 0.0f, 0.0f)));
    float mainWindStrenght = (vertexColorsValue.a + noiseFromTexture.b) * worldYaxisScale;
    float wiggleValues = ((vertexColorsValue.r + noiseFromTexture.b) * worldYaxisScale);
    float wigglesAmplitude = wigglesStrength * noiseFromTexture.b;

    float3 scaledWorldPos = float3(noiseTexCoords.x, (objectWorldPos).y, noiseTexCoords.y);
    float windDotDirection = dot(scaledWorldPos, _windDirection.xyz);
    ///
    float windFromCurve = windCurve(windSpeed, windStrength* vertexColorsValue.a, wigglesSpeed, wiggleWaveSize, wigglesAmplitude * vertexColorsValue.r, mainWindStrenght, phaseOffset, wiggleValues, windWaveSize, windDotDirection, worldPos.y);
    float actualWindStrength = (windFromCurve *  bending);
    float3 rotatedVertices = RotateAroundAxis(objectWorldPos, worldPos.xyz, newWindDir, actualWindStrength);
    float3 rotatedVerticesWithNormals = RotateAroundAxis(objectWorldPos, worldPos.xyz + TransformObjectToWorldNormal(normal).xyz, newWindDir, actualWindStrength);
    float3 rotatedVerticesWithTangents = RotateAroundAxis(objectWorldPos, worldPos.xyz + TransformObjectToWorldDir(tangent).xyz, newWindDir, actualWindStrength);

    float3 newNormal = normalize(rotatedVerticesWithNormals - rotatedVertices);
    float3 newTangent = normalize(rotatedVerticesWithTangents - rotatedVertices);


    normal = newNormal;
    tangent = newTangent;
    newWorldPos = rotatedVertices;

}

inline void newWolrdNormalPosMain(float3 vertexObjectPos, float4 vertexColors, float bending, inout float3 normal, inout float3 tangent, out float3 newWorldPos, out half4 debugColors)
{
      ///
    half windStrength = _windStrenght_Speed_WaveSize_AngleSpread.x;
    half windSpeed = _windStrenght_Speed_WaveSize_AngleSpread.y;
    half windWaveSize = _windStrenght_Speed_WaveSize_AngleSpread.z;
    half windAngleSpread = _windStrenght_Speed_WaveSize_AngleSpread.w;
   
   
    half wigglesSpeed = _wiggles_Speed_WaveSize_NoiseTiling.y;
    half wigglesStrength = _wiggles_Speed_WaveSize_NoiseTiling.x;
    half wiggleWaveSize = _wiggles_Speed_WaveSize_NoiseTiling.z;
    half windNoiseTiling = _wiggles_Speed_WaveSize_NoiseTiling.w;

    float4 vertexColorsValue = vertexColors;
    float3 objectWorldPos = mul(GetObjectToWorldMatrix(), float4(0.0f, 0.0f, 0.0f, 1.0f)).xyz;
    float2 noiseTexCoords = ((objectWorldPos).xz * windNoiseTiling);
    float4 noiseFromTexture = tex2Dlod(_windNoiseTex, float4(noiseTexCoords, 0.0f, 0.0f));
    float3 worldPos = TransformObjectToWorld(vertexObjectPos);

    half angleOffset = (((frac((vertexColorsValue).b + (noiseFromTexture).g) * PI) - PI) * windAngleSpread);
    half phaseOffset = ((vertexColorsValue).g + (noiseFromTexture).r) * _windDirection.w;

    float2 windDirXZ = (float2(_windDirection.x, _windDirection.z));

    float cosAngle = cos(angleOffset);
    float sinAngle = sin(angleOffset);
    float2 rotatedWindDir = mul(windDirXZ, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle));
    half3 newWindDir = normalize((float3(rotatedWindDir.x, _windDirection.y, rotatedWindDir.y)));

    float worldYaxisScale = length(mul(GetObjectToWorldMatrix(), float4(0.0f, 1.0f, 0.0f, 0.0f)));
    float mainWindStrenght = (vertexColorsValue.a + noiseFromTexture.b) * worldYaxisScale;
    float wiggleValues = ((vertexColorsValue.r + noiseFromTexture.b) * worldYaxisScale);
    float wigglesAmplitude = wigglesStrength * noiseFromTexture.b;

    float3 scaledWorldPos = float3(noiseTexCoords.x, (objectWorldPos).y, noiseTexCoords.y);
    float windDotDirection = dot(scaledWorldPos, _windDirection.xyz);
    ///
    float windFromCurve = windCurve(windSpeed, windStrength* vertexColorsValue.a, wigglesSpeed, wiggleWaveSize, wigglesAmplitude * vertexColorsValue.r, mainWindStrenght, phaseOffset, wiggleValues, windWaveSize, windDotDirection, worldPos.y);
    float actualWindStrength = (windFromCurve *  bending);
    float3 rotatedVertices = RotateAroundAxis(objectWorldPos, worldPos.xyz, newWindDir, actualWindStrength);
    float3 rotatedVerticesWithNormals = RotateAroundAxis(objectWorldPos, worldPos.xyz + TransformObjectToWorldNormal(normal).xyz, newWindDir, actualWindStrength);
    float3 rotatedVerticesWithTangents = RotateAroundAxis(objectWorldPos, worldPos.xyz + TransformObjectToWorldDir(tangent).xyz, newWindDir, actualWindStrength);

    float3 newNormal = normalize(rotatedVerticesWithNormals - rotatedVertices);
    float3 newTangent = normalize(rotatedVerticesWithTangents - rotatedVertices);

    float directionDot = abs(((frac((windWaveSize * windDotDirection)) - 0.5) * 2.0));
    
    debugColors = noiseFromTexture * directionDot;
    

    
    normal = newNormal;
    tangent = newTangent;
    newWorldPos = rotatedVertices;

}