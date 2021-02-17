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
    public PlayerMovement PlayerMov;
    public  PlayerDataHolder(int id, string username)
    {
        playerID = id;
        Username = username;
    }
    public void SetMatchId(int matchId) {  currentMatchId = matchId; } //WHEN PLAYER JOINS MATCH IT SETS THE MATCH ID TO THE PLAYER
    public int GetMatchId() { return currentMatchId; }
    public int GetPlayerId() { return playerID; }
    public void SetGameObject(GameObject obj) { playerGameObject = obj; }
    //public void SetInputsToPlayer(bool[] inputs,Quaternion rotation) { _myPlayer.SetInput(inputs,rotation); }
    public GameObject GetGameObject() { return playerGameObject; }
    public void DestroyGameObject()
    {
        Object.Destroy(playerGameObject);
    }

    public class PlayerMovement
    {
        public string Username { get; private set; }
        public int Id { get; private set; }

        public PlayerMovement(Vector3 spawnPosition, string username, int id)
        {
            Position = spawnPosition;
            Username = username;
            Id = id;
            Rotation = Quaternion.identity;
        }

        public Vector3 Position { get; set; }
        
        public Quaternion Rotation { get; set; }
    }
}
