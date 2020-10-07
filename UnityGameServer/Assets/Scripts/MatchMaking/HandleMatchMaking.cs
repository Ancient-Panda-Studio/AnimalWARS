using System.Collections.Generic;
using System.Linq;

public class HandleMatchMaking
{
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
        foreach (var player in _playerDataHolders)
        {
            player.CallSpawn();
        }
    }
    
    public static List<PlayerDataHolder> CheckIfMatchMakingIsPossible()
    {
        if (_matchQueue.Count < 4) return null;
        // ReSharper disable once CollectionNeverQueried.local
        var playersToAdd = new List<PlayerDataHolder>();
        for (var i = 0; i < 4; i++)
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
