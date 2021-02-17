namespace EmaceArt
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class WindController : MonoBehaviour
    {
        [Range(0.0f, 2.0f)]
        public float windStrenght = 1f;
        [Range(0.3f, 2.0f)]
        public float windSpeed = 0.3f;
        [Range(0.01f, 5.0f)]
        public float windWaveSize = 1.0f;
        [Range(0.0f, 180.0f)]
        public float windAngleSpread = 1.0f;

        [Range(0.0f, 5.0f)]
        public float wigglesStrength = 0.5f;
        [Range(0.01f, 1.0f)]
        public float wigglesWaveSize = 0.1f;
        [Range(0.01f, 1.0f)]
        public float wigglesSpeed = 1.0f;
        [Range(0.0f, 1.0f)]
        public float phasesOffsetOpacity = 1.0f;

        public float windNoiseTiling = 1.0f;

        public bool debugNoiseTexture = false;

        float wiggleTimer = 0.0f;
        float windTimer = 0.0f;
        Vector3 windDirection = Vector3.forward;

        const int numberOfHelpAngles = 6;
        const int numberOfHelpSpheres = 5;
        const int numberOfHelpLines = 5;
        const int numberOfArrowSubdiv = 10;
        Color orange = new Color(1.0f, 0.5f, 0.0f);

        private bool selected = false;

        private bool componentEnabled;

        float fract(float x)
        {
            return x - Mathf.Floor(x);
        }

        float pseudoRandom(float x)
        {
            return (Mathf.Sin(x) * Mathf.Cos(fract(Mathf.Sin(x) * 0.952f) * Mathf.PI * 2.4f)) * 0.5f + 0.5f;
        }

        Vector2 wiggleMovement(float t)
        {
            float x = (Mathf.Sin(t) * 0.5f + 0.5f) * (Mathf.Sin(t) * 0.5f + 0.5f) * Mathf.Cos(t);
            float y = (Mathf.Cos(t) * 0.5f + 0.5f) * (Mathf.Cos(t) * 0.5f + 0.5f) * Mathf.Sin(t);
            return new Vector2(x, y);
        }

        private void Start()
        {
            SetWindValues();
        }

        void DrawArrowLines(Vector3 position, Vector3 lineVector)
        {
            Vector3 rightLineArrowEnd = position + Quaternion.AngleAxis(135.0f, transform.up) * (lineVector * 0.2f);
            Vector3 leftLineArrowEnd = position + Quaternion.AngleAxis(225.0f, transform.up) * (lineVector * 0.2f);
            Gizmos.DrawLine(position, rightLineArrowEnd);
            Gizmos.DrawLine(position, leftLineArrowEnd);
        }

        float logDamper(float x)
        {
            float t = x * x * x * x;
            Debug.Log(t);
            return t;
        }

        private void OnDrawGizmosSelected()
        {
            wiggleTimer += Time.fixedDeltaTime * wigglesSpeed * 5.0f;
            windTimer += Time.fixedDeltaTime * windSpeed * 3.0f;
            selected = true;
            SetWindValues();
        }

        private void SetWindValues()
        {
            Shader.SetGlobalVector("_windStrenght_Speed_WaveSize_AngleSpread", new Vector4(windStrenght, windSpeed * 0.5f, windWaveSize*5.0f, windAngleSpread * Mathf.Deg2Rad * 0.5f));
            Shader.SetGlobalVector("_wiggles_Speed_WaveSize_NoiseTiling", new Vector4(wigglesStrength*2.0f, wigglesSpeed*0.5f, wigglesWaveSize*0.05f, windNoiseTiling * 0.0001f));
            Shader.SetGlobalVector("_windDirection", new Vector4(windDirection.x, windDirection.y, windDirection.z, phasesOffsetOpacity));
            if (debugNoiseTexture && componentEnabled)
                Shader.EnableKeyword("_NOISEDEBUG");
            else
                Shader.DisableKeyword("_NOISEDEBUG");
        }

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            Vector3 mainWindVector = transform.forward * windStrenght;
            Vector3 mainGizmoEnd = transform.position + mainWindVector * 1.25f;
            Gizmos.DrawLine(transform.position, mainGizmoEnd);

            if (!selected)
            {
                DrawArrowLines(mainGizmoEnd, mainWindVector);
                Gizmos.DrawWireSphere(transform.position, 0.3f);
                return;
            }
            windDirection = transform.right * (-1.0f);

            float arrowsSpacing = (1.0f / numberOfArrowSubdiv) * 1.25f;
            for (int k = 1; k < numberOfArrowSubdiv; k++)
            {
                Gizmos.color = orange;
                Vector3 linePoint = transform.position + mainWindVector * (k + fract(windTimer)) * arrowsSpacing;
                DrawArrowLines(linePoint, mainWindVector);
            }

            float windAngleSpreadNormalized = windAngleSpread / 360.0f;
            float angleRotationDifference = (90.0f / numberOfHelpAngles) * windAngleSpreadNormalized;
            Gizmos.color = Color.red;
            for (int t = 1; t < numberOfHelpAngles; t++)
            {
                Vector3 roatedVecRight = Quaternion.AngleAxis(angleRotationDifference * t, transform.up) * mainWindVector;
                Vector3 roatedVecLeft = Quaternion.AngleAxis(angleRotationDifference * (-t), transform.up) * mainWindVector;
                Gizmos.DrawLine(transform.position, transform.position + roatedVecRight);
                Gizmos.DrawLine(transform.position, transform.position + roatedVecLeft);
            }

            Vector3 halfPoint = transform.position + mainWindVector * 0.5f;
            Gizmos.color = Color.green;
            int numberOfHelpSpheres = Mathf.CeilToInt(wigglesWaveSize);
            Random.InitState(778);

            for (int t = 0; t < 1; t++)
            {
                float normalizedT = (((float)t) / numberOfHelpSpheres);
                Vector2 wiggleGenerated = wiggleMovement(wiggleTimer + normalizedT * Mathf.PI);
                Vector3 sinuSoidalOffset = wiggleGenerated.x * transform.forward + wiggleGenerated.y * transform.right;
                Vector3 randomVec;
                if (t != 0)
                    randomVec = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), transform.up) * transform.forward + sinuSoidalOffset * 0.5f;
                else
                    randomVec = sinuSoidalOffset * 0.5f;
                Gizmos.DrawWireSphere(halfPoint + randomVec * wigglesStrength * Random.value, 0.1f);
            }
            Gizmos.color = Color.blue;
            Vector3 leftLineEnd = transform.position + transform.right * (-1.5f);
            Vector3 rightLineEnd = transform.position + transform.right * 1.5f;
            selected = false;
        }

        private void OnValidate()
        {
            SetWindValues();
        }
        private void OnEnable()
        {
            Texture2D texture = Resources.Load<Texture2D>("windNoiseTex");
            Shader.SetGlobalTexture("_windNoiseTex", texture);
            Debug.Log("Enabled");
            Shader.EnableKeyword("_GLOBALWIND");
            componentEnabled = true;
            SetWindValues();
        }
        void disableWind()
        {
            componentEnabled = false;
            Shader.DisableKeyword("_NOISEDEBUG");
            Shader.DisableKeyword("_GLOBALWIND");     
        }

        private void OnDisable()
        {
            disableWind();
        }
        private void OnDestroy()
        {
            disableWind();
        }
    }
}