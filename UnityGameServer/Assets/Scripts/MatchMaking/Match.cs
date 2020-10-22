using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match
{
    private static List<PlayerDataHolder> _team1 = new List<PlayerDataHolder>();
    private static List<PlayerDataHolder> _team2 = new List<PlayerDataHolder>();
    private static List<Map> _mapOrder = new List<Map>();
    private static int _matchID;
    private static int _currentBindMapId;
    public Match(List<PlayerDataHolder> team1, List<PlayerDataHolder> team2, IEnumerable<int> mapOrder)
    {
        _team1 = team1;
        _team2 = team2;
        _mapOrder.Add(Dictionaries.Maps[1]); //Maps.ParseIntToMap(mapOrder);
        if (Dictionaries.CurrentMatches.Count == 0)
            _matchID = 1;
        else
            _matchID = Dictionaries.CurrentMatches.Count + 1;
    }
    public static int GetMatchId() { return _matchID; }
    public int GetMatchIdNoStatic() { return _matchID; }
    public  void UpdateCurrentMap(int mapID) { _currentBindMapId = mapID; }

    public  List<Map> GetMapList() { return _mapOrder; }

    public  int GetCurrentMap() { return _currentBindMapId; }

    public  IEnumerable<PlayerDataHolder> GetTeam1() { return _team1; }

    public  IEnumerable<PlayerDataHolder> GetTeam2() { return _team2; }

    public  IEnumerable<PlayerDataHolder> GetAllPlayers() { 
        var allPLayers = _team1.Concat(_team2);
        return allPLayers.ToList();
    }
}

