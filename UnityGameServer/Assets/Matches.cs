using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matches
{
    private static List<PlayerDataHolder> _team1 = new List<PlayerDataHolder>();
    private static List<PlayerDataHolder> _team2 = new List<PlayerDataHolder>();
    private static List<int> _mapOrder = new List<int>();
    public static int MatchID;
    public void SetMatch(List<PlayerDataHolder> team1, List<PlayerDataHolder> team2, List<int> mapOrder)
    {
        _team1 = team1;
        _team2 = team2;
        _mapOrder = mapOrder;
        MatchID = Dictionaries.CurrentMatches.Count + 1;
    }
}
