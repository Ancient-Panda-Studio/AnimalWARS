using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHolder
{
    private int playerID;
    public string username;
    public bool inParty;
    public int partyID;
    public int currentMatchId;

    public  PlayerDataHolder(int _playerID, string _username)
    {
        playerID = _playerID;
        username = _username;
    }
    
    public void CallSpawn()
    {
        Server.clients[playerID].SendIntoGame(Dictionaries.PlayersById[playerID]);
    }

    
}
