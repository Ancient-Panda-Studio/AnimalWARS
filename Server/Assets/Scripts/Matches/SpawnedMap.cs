using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnedMap : MonoBehaviour
{
    private int myGameID;
    private GameObject myGeneralGameObject;
    public  List<SpawnPoint> playerSpawnPoints;

    public void SetGameID(int _id) { myGameID = _id;}
    public SpawnPoint GetFreeSpawn(int _team) {
       var x = playerSpawnPoints.Where(_point => !_point.GetFull() && _point.myTeam == _team).ToList();
       return x[0];
    }
    public List<SpawnPoint> GetFreeSpawns(int _team) {
        return playerSpawnPoints.Where(_point => !_point.GetFull() && _point.myTeam == _team).ToList();
    }
    public int GetGameId() { return myGameID; }
}