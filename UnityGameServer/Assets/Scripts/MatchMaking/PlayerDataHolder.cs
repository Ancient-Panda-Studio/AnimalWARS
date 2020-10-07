using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHolder
{
    private static int playerID;
    private readonly string username;
    public bool inParty;
    public int partyID;
    
    public  PlayerDataHolder(int _playerID)
    {
        playerID = _playerID;
    }
    
    public void CallSpawn()
    {
        Server.clients[playerID].SendIntoGame(Dictionaries.PlayersById[playerID]);
    }

    
}
