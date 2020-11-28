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
    public float _creationTime;
    private static List<PlayerDataHolder> _acceptedPlayer = new List<PlayerDataHolder>();
    public Match(IEnumerable<PlayerDataHolder> team1, IEnumerable<PlayerDataHolder> team2)
    {
        try{
        _team1 = team1.ToList();
        _team2 = team2.ToList();
        _mapOrder.Add(Dictionaries.Maps[1]); //Maps.ParseIntToMap(mapOrder);
        _matchID = Dictionaries.matches.Count + 1;
        Dictionaries.matches.Add(this);
        _creationTime = Time.time + 20;
        Debug.Log($"Creation Time for {_matchID} -> {_creationTime} <-");
        } catch{
        Debug.Log("Error during match creation");
        }
    }
    public static int GetMatchId() { return _matchID; }
    public int GetMatchIdNoStatic() { return _matchID; }
    public  void UpdateCurrentMap(int mapID) { _currentBindMapId = mapID; }

    public  List<Map> GetMapList() { return _mapOrder; }

    public  int GetCurrentMap() { return _currentBindMapId; }

    public void Begin(){
        //This method will Initiate the looby;
        //ServerSend.StartLobby();
    }
    public void SendPopUp(){
        foreach (var sendTo in GetAllPlayers().ToList()){
           ServerSend.MatchFound(sendTo.GetPlayerId(), _matchID);
        }
    }
    public bool HasStarted(){
        return false;
    }
    public bool IsTime(){
        return Time.time >= _creationTime;
    }
    public bool Verify() {

        /*THIS METHOD WILL RETURN TRUE IF EVERYONE IN THIS MATCH HAS ACCEPTED THE POP UP*/
        foreach (var x in GetAllPlayers()) {
            Debug.Log(x.HasAccepted());
            if(x.HasAccepted()) {
                _acceptedPlayer.Add(x);
            }
        }
        if(_acceptedPlayer.Count == 6)
        return true;
        else
        return false;
    }
    public  IEnumerable<PlayerDataHolder> GetTeam1() { return _team1; }

    public  IEnumerable<PlayerDataHolder> GetTeam2() { return _team2; }

    public  IEnumerable<PlayerDataHolder> GetAllPlayers() { var allPLayers = _team1.Concat(_team2); return allPLayers.ToList(); }

    public int FindPlayerTeam(PlayerDataHolder _player) { return _team1.Contains(_player)?1:2; }
}

