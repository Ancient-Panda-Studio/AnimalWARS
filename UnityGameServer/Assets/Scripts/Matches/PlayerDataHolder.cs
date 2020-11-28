using System.Collections;
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
    private bool acceptedMatch = false;
    public  PlayerDataHolder(int _id, string _username)
    {
        playerID = _id;
        username = _username;
    }
    public void SetMatchId(int _matchId) {  acceptedMatch = false;  currentMatchId = _matchId; } //WHEN PLAYER JOINS MATCH IT SETS THE MATCH ID TO THE PLAYER
    public int GetMatchId() { return currentMatchId; }
    public int GetPlayerId() { return playerID; }
    public void SetGameObject(GameObject _obj) { playerGameObject = _obj; }
    public void SetInputsToPlayer(bool[] _inputs,Quaternion _rotation) { _myPlayer.SetInput(_inputs,_rotation); }
    public GameObject GetGameObject() { return playerGameObject; }
    public bool HasAccepted() {
        return acceptedMatch;
    }
    public void SetAccept(bool x) {
        acceptedMatch = x;
    }
    public void DestroyGameObject()
    {
        Object.Destroy(playerGameObject);
    }
}
