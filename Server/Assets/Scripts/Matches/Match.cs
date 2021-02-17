using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
#pragma warning disable 414
public enum MatchType {
    AztecMap,
    MysteriousTides,
    Tutorial
}
public class Match
{
    private List<PlayerDataHolder> _team1 = new List<PlayerDataHolder>(); //RED
    private List<PlayerDataHolder> _team2 = new List<PlayerDataHolder>(); //BLUE
    private Dictionary<int, GameObject> _repPlayer;
    private List<Map> _mapOrder = new List<Map>();
    private int _matchID;
    private int _currentBindMapId;
    public PlayerDataHolder currentPlayer;
    public GamePLay myGamePLay;
    public float _creationTime;
    public bool hasEnded;
    public Lobby MatchLobby;
    private static List<PlayerDataHolder> _acceptedPlayer = new List<PlayerDataHolder>();
    public List<int> HasAccepted = new List<int>();
    public bool SpawnRdy;
    public MatchType matchType;
    private int _spawningCompleted = 0;
    public string winner = null;

    public Match(IEnumerable<PlayerDataHolder> team1, IEnumerable<PlayerDataHolder> team2, MatchType type)
    {

        matchType = type;
        if (matchType == MatchType.AztecMap || matchType == MatchType.MysteriousTides)
        {
            _team1 = team1.ToList();
            _team2 = team2.ToList();
            _mapOrder.Add(Dictionaries.dictionaries.Maps[1]); //Maps.ParseIntToMap(mapOrder);
            _matchID = Random.Range(1, 10000000);
            Dictionaries.dictionaries.Matches.Add(_matchID, this);
            MatchLobby = new Lobby(team1.Concat(team2).ToList(), this);
            _currentBindMapId = 4;
        } else if (matchType == MatchType.Tutorial)
        {
            
        }
    }
    
    public Match(PlayerDataHolder player, MatchType type)
    {
        matchType = type;
        if (matchType != MatchType.Tutorial) return;
        _mapOrder.Add(Dictionaries.dictionaries.Maps[1]); //Maps.ParseIntToMap(mapOrder);
        _matchID = Random.Range(1, 10000000);
        currentPlayer = player;
        player.SetMatchId(_matchID);
        myGamePLay = new GamePLay(_currentBindMapId, this);
        Dictionaries.dictionaries.Matches.Add(_matchID, this);
        _currentBindMapId = 1;
    }
    public int GetMatchId() { return _matchID; }
    public int GetMatchIdNoStatic() { return _matchID; }
    public  void UpdateCurrentMap(int mapID) { _currentBindMapId = mapID; }

    public  List<Map> GetMapList() { return _mapOrder; }

    public  int GetCurrentMap() { return _currentBindMapId; }

    public void Begin(){
        //This method will Initiate the looby;
        //ServerSend.StartLobby();
        foreach (var playerHolder in GetAllPlayers())
        {
            var playerHolderTeam = FindPlayerTeam(playerHolder);
            switch (playerHolderTeam)
            {
                case 1:
                    ServerSend.BeginLobby(playerHolder.GetPlayerId(), _matchID, _team1, _team2);
                    break;
                case 2:
                    ServerSend.BeginLobby(playerHolder.GetPlayerId(), _matchID, _team2, _team1);
                    break;
            }
        }
        _creationTime = Time.time + 30;

    }

    public  IEnumerable<PlayerDataHolder> GetTeam1() { return _team1; }

    public  IEnumerable<PlayerDataHolder> GetTeam2() { return _team2; }

    public  IEnumerable<PlayerDataHolder> GetAllPlayers() { var allPLayers = _team1.Concat(_team2); return allPLayers.ToList(); }
    public  IEnumerable<PlayerDataHolder> GetAllPlayers(PlayerDataHolder except) { var allPLayers = _team1.Concat(_team2).Where(w => w.GetPlayerId() != except.GetPlayerId()); return allPLayers.ToList(); }

    public int FindPlayerTeam(PlayerDataHolder player) { return _team1.Contains(player)?1:2; }

    public void SetMatchId()
    {
        foreach (var px in GetAllPlayers())
        {
           // Debug.Log($"Setting to {px.Username} with matchId {_matchID}");
            px.SetMatchId(_matchID);
        }

    }

    public void SetSceneLoaded(int playerId)
    {
        HasAccepted.Add(playerId);
    }
    public class Lobby
    {
        private Dictionary<int,PlayerDataHolder> players = new Dictionary<int, PlayerDataHolder>();
        private Dictionary<int, bool> playerStates = new Dictionary<int, bool>();
        public Dictionary<int, int> currentPick = new Dictionary<int, int>();
        private bool _spawnPlayer;
        private UnityEngine.Vector3[] positions = new[]
        {
           new UnityEngine.Vector3(-70, -6, 6.3f),
           new UnityEngine.Vector3(-70, -6, 9.3f),
           new UnityEngine.Vector3 (-70, -6, 12.3f),
           new UnityEngine.Vector3 (70, -6, 6.3f),
           new UnityEngine.Vector3 (70, -6, 9.3f),
           new UnityEngine.Vector3 (70, -6, 12.3f)
        };
        public Dictionary<int, UnityEngine.Vector3> CurrentPlayerPositions = new Dictionary<int, UnityEngine.Vector3>();
        private Match match;
        public Lobby (IEnumerable<PlayerDataHolder> players, Match match)
        {
            int posT1 = 0;
            int posT2 = 0;
            foreach (var x in players)
            {
                this.match = match;
                this.players.Add(x.GetPlayerId(),x);
                playerStates.Add(x.GetPlayerId(),false);
                currentPick.Clear();
                playerStates.Clear();
                if (currentPick.Keys.Count != 0)
                {
                    Debug.Log(
                        $"I AM A LOBBY FOR MATCH {this.match.GetMatchIdNoStatic()} I HAVE THIS PEOPLE IN CURRENT PICK {x.Username} has  picked -> {currentPick[x.GetPlayerId()]}");
                }

                //currentPick.Add(x.GetPlayerId(), -1);
                if (match.FindPlayerTeam(x) == 1)
                {
                    CurrentPlayerPositions.Add(x.GetPlayerId(), positions[posT1]);
                    posT1++;
                }
                else
                {
                    CurrentPlayerPositions.Add(x.GetPlayerId(), positions[posT2 + 3]);
                    posT2++;
                }
            }
        }
        public Lobby (PlayerDataHolder player, Match match)
        {
            int posT1 = 0;
            int posT2 = 0;
            this.match = match;
            
        }
        public void UpdateState(int id, bool value)
        {
            playerStates[id] = value;
        }
        public void UpdateCurrentSelectedPick(int whoUpdated, int whichPick)
        {
            playerStates[whoUpdated] = true;
            if (currentPick.ContainsKey(whoUpdated))
            {
                Debug.Log($"The KEY {whoUpdated} was present in {match.GetMatchIdNoStatic()} lobby dictionary");
                currentPick[whoUpdated] = whichPick;
            }
            currentPick.Add(whoUpdated, whichPick);
            foreach (var s in players)
            {
                ServerSend.UpdatePlayerPickLobby(sendTo: s.Key ,whoToUpdate: whoUpdated, whichPick: whichPick);
            }
        }
        public void SpawnPlayers()
        {
            
            foreach (var id in players.Values.Select(player => player.GetPlayerId()))
            {
               // Debug.Log($"Current BIG ID IS {id}");
                foreach (var oid in players.Values)
                {
                    if(oid.GetPlayerId() == id) continue;
                    ServerSend.SpawnPlayer(id, oid.GetPlayerId(),currentPick[oid.GetPlayerId()], CurrentPlayerPositions[oid.GetPlayerId()]);
                }
                ServerSend.SpawnPlayer(id, id,currentPick[id], CurrentPlayerPositions[id]);
                
            }
            match.HasAccepted = new int[]
            {
                1,
                2
            }.ToList();
        }
        public void EndLobby()
        {
            var sceneToLoad = playerStates.Where(cPlayer => cPlayer.Value).ToList().Count == players.ToList().Count ? 3 : 1;
            foreach (var player in players)
            {
                ServerSend.EndLobby(player.Key, sceneToLoad);
            }
            if (sceneToLoad != 1)
            {
                match.myGamePLay = new GamePLay(sceneToLoad - 2, match);
            }
            match.hasEnded = true;
        }

        public int CurrentPlayerPick(int getPlayerId)
        {
            return currentPick[getPlayerId];
        }

        public void SpawnPlayer(int playerId, int team)
        {           
            
            foreach (var id in players.Values.Select(player => player.GetPlayerId()))
            {
                // Debug.Log($"Current BIG ID IS {id}");
                foreach (var oid in players.Values)
                {
                    if(oid.GetPlayerId() == id) continue;
                    ServerSend.SpawnPlayer(id, oid.GetPlayerId(),currentPick[oid.GetPlayerId()], CurrentPlayerPositions[oid.GetPlayerId()]);
                }
                ServerSend.SpawnPlayer(id, id,currentPick[id], CurrentPlayerPositions[id]);
                
            }
        }
    }
    private void RemoveCanvas()
    {
        for (var i = 0; i < 6; i++)
        {
            var who = GetAllPlayers().ToList()[i];
//            Debug.Log($"Sending to {who}");
            ServerSend.RemoveCanva(who.GetPlayerId());
        }
    }
    public void SetPlayerComplete()
    {
        _spawningCompleted++;
        if (_spawningCompleted == 6)
        {
            RemoveCanvas();    
        }
    }


    public int GetPlayerTeam(int toClient)
    {
        return GetTeam1().ToList().Contains(Dictionaries.dictionaries.PlayerDataHolders[toClient]) ? 1 : 2;
    }
}