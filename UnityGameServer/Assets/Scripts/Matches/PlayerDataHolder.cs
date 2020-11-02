﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHolder
{
    private readonly int playerID;
    public readonly string username;
    public bool inParty;
    public int partyID;
    private int currentMatchId;
    private GameObject playerGameObject;
    private static Player _myPlayer;
    public  PlayerDataHolder(int _id, string _username)
    {
        playerID = _id;
        username = _username;
    }
    public void SetMatchId(int _matchId) { currentMatchId = _matchId; } //WHEN PLAYER JOINS MATCH IT SETS THE MATCH ID TO THE PLAYER
    public int GetMatchId() { return currentMatchId; }
    public int GetPlayerId() { return playerID; }
    public void SetGameObject(GameObject _obj) { playerGameObject = _obj; }
    public static void SetInputsToPlayer(bool[] _inputs,Quaternion _rotation) { _myPlayer.SetInput(_inputs,_rotation); }
    public void CallSpawn(SpawnedMap _newMapScripts) { Server.Clients[playerID].SendIntoMatch(Dictionaries.CurrentMatches[currentMatchId],this,_newMapScripts); }
    public GameObject GetGameObject() { return playerGameObject; }

    public void DestroyGameObject()
    {
        Object.Destroy(playerGameObject);
    }
}