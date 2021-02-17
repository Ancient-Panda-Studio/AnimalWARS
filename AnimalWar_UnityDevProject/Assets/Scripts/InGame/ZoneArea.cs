using System;
using Network;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

public enum TypeOfZone
{
    Neutral = 1,
    Conflict,
    Beneficial 
}
public enum TeamToBuff
{
    Team1,
    Team2
}


public class ZoneArea : MonoBehaviour
{
    public TypeOfZone myZoneType;
    public SphereCollider[] zoneColliders;
    public SphereCollider zoneCollider;
    #region Beneficial
    public TeamToBuff beneficialTo;
    public bool isDetrimentalToOpposite;
    public int zoneID;
    #endregion

    private void Awake()
    {
        
        if(myZoneType == TypeOfZone.Conflict || myZoneType == TypeOfZone.Neutral)
        {
            foreach (var zoneCollider in zoneColliders)
            {
                if(zoneCollider.GetComponent<Zone>()) continue;
                var s = zoneCollider.gameObject.AddComponent<Zone>();
                s.master = this;
                s.zoneId = zoneID;
                s.PerformOnCollision = PerformOnCollision;
            }
        }
        if(myZoneType == TypeOfZone.Beneficial)
        {
                if(zoneCollider.GetComponent<Zone>()) return;
                var s = zoneCollider.gameObject.AddComponent<Zone>();
                s.master = this;
                s.zoneId = zoneID;
                s.PerformOnCollision = PerformOnCollision;
        }
    }

    private void Update()
    {
        //Debug.Log($"Current Zone is -> {PlayerConstants.CurrentZone} <-");
    }

    private void PerformOnCollision()
    {
        switch (myZoneType)
        {
            case TypeOfZone.Neutral:
                ClientSend.EnteredBeneficialNeutral(zoneID);
                break;
            case TypeOfZone.Conflict:
                ClientSend.EnteredConflictZone(zoneID);
                break;
            case TypeOfZone.Beneficial:
                ClientSend.EnteredBeneficialZone(zoneID);
                break;
            default:
                //WTF HAS GONE WRONG GOD HELP US
            break;
        }
    }
}

public class Zone : MonoBehaviour
{
    public System.Action PerformOnCollision;
    public ZoneArea master;
    public int zoneId;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("LPlayer")) return;
        if (PlayerConstants.CurrentZone == -1)
        {
            PlayerConstants.CurrentZone = zoneId;
            PerformOnCollision.Invoke();
        }
        if(PlayerConstants.CurrentZone == zoneId) return;
        PerformOnCollision.Invoke();
        PlayerConstants.CurrentZone = zoneId;
    }
    
}

public static class PlayerConstants
{
    public static int CurrentZone = -1;
}
#if UNITY_EDITOR
[CustomEditor(typeof(ZoneArea))]
public class ZoneEditor : Editor
{
    private SerializedProperty _typeNum;
    private SerializedProperty _beneficialFor;
    private SerializedProperty _colliders;
    private SerializedProperty _collider;
    private SerializedProperty _isDetrimental;
    private SerializedProperty _zoneID;

    private void OnEnable()
    {
        _typeNum = serializedObject.FindProperty("myZoneType");
        _beneficialFor = serializedObject.FindProperty("beneficialTo");
        _colliders = serializedObject.FindProperty("zoneColliders");
        _collider = serializedObject.FindProperty("zoneCollider");
        _isDetrimental = serializedObject.FindProperty("isDetrimentalToOpposite");
        _zoneID = serializedObject.FindProperty("zoneID");
    }

    public override void OnInspectorGUI()
    {
        var script = (ZoneArea) target;
        //base.OnInspectorGUI();
        var type = EditorGUILayout.PropertyField(_typeNum);
        serializedObject.ApplyModifiedProperties();
        switch (script.myZoneType)
        {
            case TypeOfZone.Beneficial:
                var beneficial = EditorGUILayout.PropertyField(_beneficialFor);
                serializedObject.ApplyModifiedProperties();
                var stringToToggle = script.beneficialTo == TeamToBuff.Team1 ? "Team2" : "Team1";
                var toggleDetrimental = EditorGUILayout.PropertyField(_isDetrimental);
                var zoneid = EditorGUILayout.PropertyField(_zoneID);
                serializedObject.ApplyModifiedProperties();
                var colliderB = EditorGUILayout.PropertyField(_collider);
                break;
            case TypeOfZone.Conflict:
                var colliders = EditorGUILayout.PropertyField(_colliders);
                var zone = EditorGUILayout.PropertyField(_zoneID);
                break;
            case TypeOfZone.Neutral:
                var colliderN = EditorGUILayout.PropertyField(_colliders);
                var zoneid2 = EditorGUILayout.PropertyField(_zoneID);
                break;
            default:
            break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
