using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class MapManager : MonoBehaviour
{
    /*public static MapManager Instance;
    public Zone Zone1;
    public List<Transform> zone1Obj = new List<Transform>();
    public List<Transform> zone2Obj = new List<Transform>();
    public List<Transform> zone3Obj = new List<Transform>();
    public List<Transform> zone4Obj = new List<Transform>();
    public Color colorZone1;
    public GameObject currentPos;
    public bool setInstance;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (var i = 0; i < zone1Obj.Count; i++)
        {
            Gizmos.DrawLine(zone1Obj[i].position,
                i == zone1Obj.Count - 1 ? zone1Obj[0].position : zone1Obj[i+1].position);
            Gizmos.DrawLine(new Vector3(zone1Obj[i].position.x, zone1Obj[i].position.y + 10, zone1Obj[i].position.z),
                i == zone1Obj.Count - 1 ? new Vector3(zone1Obj[i].position.x, zone1Obj[i].position.y + 10, zone1Obj[i].position.z) : new Vector3(zone1Obj[i+1].position.x, zone1Obj[i+1].position.y + 10, zone1Obj[i+1].position.z));
            Gizmos.DrawLine(zone1Obj[i].position,
                new Vector3(zone1Obj[i].position.x, zone1Obj[i].position.y + 10, zone1Obj[i].position.z));
            switch (i)
            {
                case 8:
                    Gizmos.DrawLine(zone1Obj[i].position,
                        zone1Obj[2].position);
                    break;
                case 7:
                    Gizmos.DrawLine(zone1Obj[i].position,
                        zone1Obj[3].position);
                    break;
                case 6:
                    Gizmos.DrawLine(zone1Obj[i].position,
                        zone1Obj[4].position);
                    break;
            }
        }
        Gizmos.color = Color.magenta;
        for (var i = 0; i < zone2Obj.Count; i++)
        {
            Gizmos.DrawLine(zone2Obj[i].position,
                i == zone2Obj.Count - 1 ? zone2Obj[0].position : zone2Obj[i+1].position);
        }
        Gizmos.color = Color.green;
        for (var i = 0; i < zone3Obj.Count; i++)
        {
            Gizmos.DrawLine(zone3Obj[i].position,
                i == zone3Obj.Count - 1 ? zone3Obj[0].position : zone3Obj[i+1].position);
        }
        Gizmos.color = Color.yellow;
        for (var i = 0; i < zone4Obj.Count; i++)
        {
            Gizmos.DrawLine(zone4Obj[i].position,
                i == zone4Obj.Count - 1 ? zone4Obj[0].position : zone4Obj[i+1].position);
        }
    }
}

public class Zone
{
    public Vector3 Zon;
    public Boundary ZoneLimits = new Boundary(4);
}

public class Boundary
{
    private int _amountOfPoints;
    public List<Vector3> ZonePoints = new List<Vector3>();

    public Boundary(int points)
    {
        _amountOfPoints = points;
    }*/
}
