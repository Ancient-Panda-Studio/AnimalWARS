using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHolder
{
    private readonly int playerID;
    public readonly string Username;
    public bool InParty;
    public int PartyID;
    private int currentMatchId;
    private GameObject playerGameObject;
    private static Player myPlayer;
    public  PlayerDataHolder(int id, string username)
    {
        playerID = id;
        Username = username;
    }
    public void SetMatchId(int matchId) { currentMatchId = matchId; } //WHEN PLAYER JOINS MATCH IT SETS THE MATCH ID TO THE PLAYER
    public int GetMatchId() { return currentMatchId; }
    public int GetPlayerId() { return playerID; }
    public void SetGameObject(GameObject obj) { playerGameObject = obj; }
    public static void SetInputsToPlayer(bool[] inputs,Quaternion rotation) { myPlayer.SetInput(inputs,rotation); }
    public void CallSpawn() { Server.Clients[playerID].SendIntoMatch(Dictionaries.CurrentMatches[currentMatchId],playerID); }
    public GameObject GetGameObject() { return playerGameObject; }
}
