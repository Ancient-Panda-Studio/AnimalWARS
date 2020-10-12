using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleMatchMaking
{
    public static bool Generating = false;
    private static readonly List<PlayerDataHolder> _matchQueue = new List<PlayerDataHolder>(); //This list stores the ID's of players looking for match
    //public List<Maps> availableMaps = new List<Maps>();
    
    public static void AddToQueue(PlayerDataHolder _playerDataHolder)
    {
        _matchQueue.Add(_playerDataHolder);
    }

    public static void RemoveFromQueue(PlayerDataHolder _playerDataHolder)
    {
        _matchQueue.Remove(_playerDataHolder);
    }


    public static void GenerateMatch(List <PlayerDataHolder> _playerDataHolders)
    {
        Generating = true;
        Debug.Log("USER1: " + _playerDataHolders[0].username);
                  // " USER2: " +_playerDataHolders[1].username + " USER3: " +_playerDataHolders[2].username +
                //           " USER4: " +_playerDataHolders[3].username + " USER5: " + _playerDataHolders[4].username + "USER6: " +_playerDataHolders[5].username);
        
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
            var mapOrder = SelectMaps();
            Debug.Log(id);
            var x = 0;
            mapOrder.ForEach(i => Debug.Log("Item: " + x++ + "\t"+ i));
            foreach (var player in _playerDataHolders)
            {
                player.currentMatchId = id;
                _matchQueue.Remove(player);
                //Todo Spawn player
            }
            Generating = false;
    }

    private static List<int> SelectMaps()
    {
        int[] mapsArray = {1, 2, 3, 4, 5};
        List<int> maps = new List<int>();
        maps.AddRange(mapsArray);
        List<int> shuffledList = maps.OrderBy( x => Random.Range(1,5) ).ToList( );
        return shuffledList;
    }
    public static List<PlayerDataHolder> CheckIfMatchMakingIsPossible()
    {
        if (_matchQueue.Count < 1) return null;
        // ReSharper disable once CollectionNeverQueried.local
        var playersToAdd = new List<PlayerDataHolder>();
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

        return playersToAdd;
    }

}
