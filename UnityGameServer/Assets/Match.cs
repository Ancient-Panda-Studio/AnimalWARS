using System.Collections.Generic;

public class Match
{
    private static List<PlayerDataHolder> _team1 = new List<PlayerDataHolder>();
    private static List<PlayerDataHolder> _team2 = new List<PlayerDataHolder>();
    private static List<Map> _mapOrder = new List<Map>();
    private static int _matchID;
    private static int _currentBindMapId;

    public Match (List<PlayerDataHolder> team1, List<PlayerDataHolder> team2, List<int> mapOrder)
    {
        _team1 = team1;
        _team2 = team2;
        _mapOrder =  new List<Map>();//Maps.Parse(mapOrder);
        if (Dictionaries.CurrentMatches.Count == 0)
            _matchID = 1;
        else
        {
            _matchID = Dictionaries.CurrentMatches.Count + 1;
        }
    }
    public int GetMatchId() { return _matchID; }
    public static void UpdateCurrentMap(int mapID) { _currentBindMapId = mapID; }
    public static int GetCurrentMap() { return _currentBindMapId; }
    public static List<PlayerDataHolder> GetTeam1() { return _team1; }
    public static List<PlayerDataHolder> GetTeam2() { return _team2; }
}