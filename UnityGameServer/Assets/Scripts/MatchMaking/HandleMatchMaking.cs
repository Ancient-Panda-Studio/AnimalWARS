using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandleMatchMaking
{
    private static readonly int[] mapsArray = {1, 2, 3, 4, 5};
    public static bool Generating;
    private static readonly List<PlayerDataHolder> _matchQueue = new List<PlayerDataHolder>(); //This list stores the ID's of players looking for match
    //public List<Maps> availableMaps = new List<Maps>();
    
    public static void AddToQueue(PlayerDataHolder _playerDataHolder)
    {
        Debug.Log(_playerDataHolder.username + " has joined the queue");
        _matchQueue.Add(_playerDataHolder);
    }

    public static void RemoveFromQueue(PlayerDataHolder _playerDataHolder)
    {
        Debug.Log(_playerDataHolder.username + " has left the queue");
        _matchQueue.Remove(_playerDataHolder);
    }


    public static void GenerateMatch(List <PlayerDataHolder> _playerDataHolders)
    {
        Generating = true;
            var currentMatchCount = Dictionaries.CurrentMatches.Count;
            int id;
            if (currentMatchCount == 0)
            {
                Dictionaries.CurrentMatches.Add(1,_playerDataHolders);
                id = 1;
            }
            else
            {
                Dictionaries.CurrentMatches.Add(currentMatchCount + 1,_playerDataHolders);
                id = currentMatchCount + 1;
            }
            List<int> mapOrder = new List<int>();
            mapOrder.AddRange(mapsArray);
            mapOrder.ShuffleList();
            for (int i = 0; i < mapOrder.Count; i++)
            {
                Debug.Log("Map " + mapOrder[i] + " was selected as map " + i);
            }
            foreach (var player in _playerDataHolders)
            {
                player.currentMatchId = id;
                _matchQueue.Remove(player);
                //Todo Spawn player
            }
            Generating = false;
    }
    public static List<PlayerDataHolder> CheckIfMatchMakingIsPossible()
    {
        if (_matchQueue.Count < 6) return null;
        var playersToAdd = new List<PlayerDataHolder>();
        if (playersToAdd.Count != 6)
         {
            for (var i = 0; i < 1; i++)
            {
                if (_matchQueue[i].inParty)
                {
                    var partyMembers = Parties.GetParty(_matchQueue[i].partyID);
                    playersToAdd.AddRange(partyMembers.Select(_t => Dictionaries.PlayerDataHolders[_t]));
                }
                else
                {
                    playersToAdd.Add(_matchQueue[i]);
                }
            }
        }
        return playersToAdd;
    }

}
