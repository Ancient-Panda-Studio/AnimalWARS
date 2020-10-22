using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnedMap : MonoBehaviour
{
    private GameObject myGeneralGameObject;
    private List<SpawnPoint> playerSpawnPoints;

    private void Start()
    {
        PopulateSpawnPoints();
    }

    private void PopulateSpawnPoints()
    {
        
    }


    public List<SpawnPoint> GetFreeSpawns(int team)
    {
        return playerSpawnPoints.Where(point => point.Full == false).Where(point => point.MyTeam == team).ToList();
    }
}

public abstract class SpawnPoint
{
    public bool Full = false;
    public readonly int MyTeam;

    protected SpawnPoint(int myTeam)
    {
        MyTeam = myTeam;
    }
}