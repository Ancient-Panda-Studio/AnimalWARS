namespace EmaceArt
{
    using UnityEngine;
    using UnityEditor;
    using System.Diagnostics;

    [CustomEditor(typeof(WindController))]
    public class WindControllerEditor : Editor
    {
        // Start is called before the first frame update
        
        SerializedProperty windStrenght;
        SerializedProperty windSpeed;
        SerializedProperty windWaveSize;
        SerializedProperty windAngleSpread;

        SerializedProperty wigglesStrength;
        SerializedProperty wigglesWaveSize;
        SerializedProperty wigglesSpeed;

        SerializedProperty phasesOffsetOpacity;
        SerializedProperty windNoiseTiling;
        SerializedProperty debugNoiseTexture;

        Texture2D EmaceArt_MainWind;
        Texture2D EmaceArt_WiggleWind;
        Texture2D EmaceArt_GlobalNoise;

        Texture2D  EmaceArt_InfoIcon;
        Texture2D  EmaceArt_HelpIcon;

        const float imageLabelWidthMin = 256.0f;
        const float imageLabelHeightMin = 64.0f;

        private void OnEnable()
        {
            windStrenght = serializedObject.FindProperty("windStrenght");
            windSpeed = serializedObject.FindProperty("windSpeed");
            windWaveSize = serializedObject.FindProperty("windWaveSize");
            windAngleSpread = serializedObject.FindProperty("windAngleSpread");

            wigglesStrength = serializedObject.FindProperty("wigglesStrength");
            wigglesWaveSize = serializedObject.FindProperty("wigglesWaveSize");
            wigglesSpeed = serializedObject.FindProperty("wigglesSpeed");

            phasesOffsetOpacity = serializedObject.FindProperty("phasesOffsetOpacity");
            windNoiseTiling = serializedObject.FindProperty("windNoiseTiling");
            debugNoiseTexture = serializedObject.FindProperty("debugNoiseTexture");

            EmaceArt_MainWind = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/EmaceArt_Stylized Foliage Pack/Scripts/Editor/MainWindLabel.png");
            EmaceArt_WiggleWind = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/EmaceArt_Stylized Foliage Pack/Scripts/Editor/WiggleWindLabel.png");
            EmaceArt_GlobalNoise = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/EmaceArt_Stylized Foliage Pack/Scripts/Editor/GlobalNoise.png");
            EmaceArt_InfoIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/EmaceArt_Stylized Foliage Pack/Scripts/Editor/InfoIcon.png");
           // EmaceArt_HelpIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/EmaceArt_Stylized Foliage Pack/Scripts/Editor/HelpIcon.png");

        }

        private Texture2D MakeTex( int width, int height, Color col )
        {
            Color[] pix = new Color[width * height];
            for( int i = 0; i < pix.Length; ++i )
            {
                pix[ i ] = col;
            }
            Texture2D result = new Texture2D( width, height );
            result.SetPixels( pix );
            result.Apply();
            return result;
        }

        public override void OnInspectorGUI()
        {
            GUIContent infoContentMainWind = new GUIContent( "  for models with alpha channel",                 
            EmaceArt_InfoIcon );
             GUIContent infoContentWiggleWind = new GUIContent( "   for models with red channel",                 
            EmaceArt_InfoIcon );
             GUIContent helpContent = new GUIContent( "READ DOCUMENTATION");
          

            GUIStyle style = new GUIStyle();
            style.normal.background = Texture2D.grayTexture; 
            style.padding = new RectOffset(110,60,0,0);
            style.fontSize = 12;
            
            
           
 
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(EmaceArt_MainWind);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

             EditorGUILayout.Space();
            GUILayout.Label(infoContentMainWind,style);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(windStrenght, new GUIContent("Main Wind Strength"));
            EditorGUILayout.PropertyField(windSpeed, new GUIContent("Main Wind Speed"));
            EditorGUILayout.PropertyField(windWaveSize, new GUIContent("Main Wind Wave Size"));
            EditorGUILayout.PropertyField(windAngleSpread, new GUIContent("Wind Angle Spread Size"));
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(EmaceArt_WiggleWind);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

             EditorGUILayout.Space();
            GUILayout.Label(infoContentWiggleWind,style);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(wigglesStrength, new GUIContent("Wiggle Strength"));
            EditorGUILayout.PropertyField(wigglesWaveSize, new GUIContent("Wiggle Wave Size"));
            EditorGUILayout.PropertyField(wigglesSpeed, new GUIContent("Wiggle Speed"));
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(EmaceArt_GlobalNoise);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(phasesOffsetOpacity, new GUIContent("Phases Offset Multipy"));
            EditorGUILayout.PropertyField(windNoiseTiling, new GUIContent("Global Noise Texture Size"));
            EditorGUILayout.PropertyField(debugNoiseTexture, new GUIContent("Debug Noise Colors"));
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            string pathToAsset = Application.dataPath + "/EmaceArt_Stylized Foliage Pack/Documentation/wind documetation.pdf";
             EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            style.fontSize = 12;
            style.normal.textColor = new Color(0.38f,0.38f,0.38f,1.0f);
            style.padding = new RectOffset(0,0,10,10);
            style.hover.background = Texture2D.whiteTexture;

            style.alignment = TextAnchor.MiddleCenter;
            if (GUILayout.Button(helpContent,style))
            {
                Process.Start("file://" + pathToAsset);
            }
        }
    }
}
