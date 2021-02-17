using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePLay
{
    /*
     * THIS CLASS HANDLES ALL THE LOGIC FOR THE GAMEPLAY INCLUDING
     *  -> MOVEMENT
     *  -> CHAT
     *  -> DISCONNECTIONS
     *  -> RECONNECTIONS
     *  -> LOGIC DEPENDENT ON MAP
     */
    
    #region STANDARD VARS
    private readonly Map gameMap;
    private readonly Match myMatch;
    private Dictionary<int, int> team1Zones = new Dictionary<int, int>();
    private Dictionary<int, int> team2Zones = new Dictionary<int, int>();
    private Dictionary<string, int> playerTeams = new Dictionary<string, int>();
    private List<InternalPlayerMethods> gamePlayers = new List<InternalPlayerMethods>();
    #endregion
    #region AztecMap
    private int zone1Value = 0;
    private int zone2Value = 0;
    private int zone3Value = 0;
    private int zone4Value = 0;
    private int zone7Value = 0;
    private int zone1LValue = 0;
    private int zone2LValue = 0;
    private int zone3LValue = 0;
    private int zone4LValue = 0;
    private int zone7LValue = 0;
    private float MatchTimer = 1500;
    public System.Action UpdateGamePlay;
    public System.Action FastUpdateGamePlay;
    #endregion
    #region Tutorial

    private PlayerDataHolder currentTutorialPlayer;
    private int currentPlayerPick = 2;
    private enum TutorialStage
    {
        Introduction = 0,
        AttackDefense,
        UltimateUsage,
        End
    }

    private TutorialStage currentPlayerStage = TutorialStage.Introduction;
    public System.Action UpdateTutorial;
    #endregion


    private void FastUpdateMethod()
    {
        SetBenefits();
        SetDetriments();
    }
    #region AztecMethods
  private void SetBenefits()
    {
        foreach (var internalPlayerMethodsEnumerable in from t1Poorly in team1Zones where t1Poorly.Value == 5 select gamePlayers.First(w => w.PlayerId == t1Poorly.Key))
        {
            internalPlayerMethodsEnumerable.RestoreHealth(internalPlayerMethodsEnumerable.maxHp * .10f);
        }

        foreach (var internalPlayerMethodsEnumerable in from t2Poorly in team2Zones where t2Poorly.Value == 6 select gamePlayers.First(w => w.PlayerId == t2Poorly.Key))
        {
            internalPlayerMethodsEnumerable.RestoreHealth(internalPlayerMethodsEnumerable.maxHp * .10f);
        }
    }
  public void SetCurrentPlayerZone(int playerId, int zone)
  {
      // THIS IS CALLED BY THE CLIENT WHEN THEY ENTER A ZONE
      if (team1Zones.ContainsKey(playerId))
      {
          team1Zones[playerId] = zone;
      }
      else
      {
          team2Zones[playerId] = zone;
      }
  }
  public int GetCurrentPlayerZone(int playerId)
  {
      return team1Zones.ContainsKey(playerId) ? team1Zones[playerId] : team2Zones[playerId];
  }
    private void SetDetriments()
    {
        /*var team1CountOnT2Base = team1Zones.Values.Count(zone => zone == 6);
        var team2CountOnT1Base = team2Zones.Values.Count(zone => zone == 5);*/
        //if(team1CountOnT2Base + team2CountOnT1Base == 0) return;
        foreach (var internalPlayerMethodsEnumerable in from t1Poorly in team1Zones where t1Poorly.Value == 6 select gamePlayers.First(w => w.PlayerId == t1Poorly.Key))
        {
            internalPlayerMethodsEnumerable.TakeDamage(internalPlayerMethodsEnumerable.maxHp * .25f);
        }

        foreach (var internalPlayerMethodsEnumerable in from t2Poorly in team2Zones where t2Poorly.Value == 5 select gamePlayers.First(w => w.PlayerId == t2Poorly.Key))
        {
            internalPlayerMethodsEnumerable.TakeDamage(internalPlayerMethodsEnumerable.maxHp * .25f);
        }
    }

    private void UpdateMethod()
    {
        /*This Is Called Externally every .5 seconds*/
        UpdateZone1();
        UpdateZone2();
        UpdateZone3();
        UpdateZone4();
        UpdateZone7();
        SendZonesValues();
    }

    private void SendZonesValues()
    {
        foreach (var player in myMatch.GetAllPlayers())
        {
            ServerSend.SendZoneValues(zone1Value, zone2Value, zone3Value, zone4Value,zone7Value, player.GetPlayerId());
        }
    }
    private void UpdateZone7()
    {
        var team1CountInZone7 = team1Zones.Values.Count(zone => zone == 7);
        var team2CountInZone7 = team2Zones.Values.Count(zone => zone == 7);
        if(team1CountInZone7 == 0 && team2CountInZone7 == 0) return;
        if (team1CountInZone7 == team2CountInZone7) return;
        zone7Value += team1CountInZone7 > team2CountInZone7 ? -1 : 1;
        if (zone7Value == 10 && zone7LValue != 10)
        {
            ServerSend.UpdateZoneState(7,2, myMatch.GetMatchIdNoStatic());
        }
        else if (zone7Value == -10 && zone7LValue != -10)
        {
            ServerSend.UpdateZoneState(7,1, myMatch.GetMatchIdNoStatic());
        }
        zone7Value = Mathf.Clamp(zone7Value, -10, 10);
        zone7LValue = zone7Value;
   
    }
    private void UpdateZone4()
    {
        var team1CountInZone4 = team1Zones.Values.Count(zone => zone == 4);
        var team2CountInZone4 = team2Zones.Values.Count(zone => zone == 4);
        if(team1CountInZone4 == 0 && team2CountInZone4 == 0) return;
        if (team1CountInZone4 == team2CountInZone4) return;
        zone4Value += team1CountInZone4 > team2CountInZone4 ? -1 : 1;
        if (zone4Value == 10 && zone4LValue != 10)
        {
            ServerSend.UpdateZoneState(4,2, myMatch.GetMatchIdNoStatic());
        }
        else if (zone4Value == -10 && zone4LValue != -10)
        {
            ServerSend.UpdateZoneState(4,1, myMatch.GetMatchIdNoStatic());
        }
        zone4Value = Mathf.Clamp(zone4Value, -10, 10);
        zone4LValue = zone4Value;

    }
    private void UpdateZone3()
    {
        var team1CountInZone3 = team1Zones.Values.Count(zone => zone == 3);
        var team2CountInZone3 = team2Zones.Values.Count(zone => zone == 3);
        if(team1CountInZone3 == 0 && team2CountInZone3 == 0) return;
        if (team1CountInZone3 == team2CountInZone3) return;
        zone3Value += team1CountInZone3 > team2CountInZone3 ? -1 : 1;
        if (zone3Value == 10 && zone3LValue != 10)
        {
            ServerSend.UpdateZoneState(3,2, myMatch.GetMatchIdNoStatic());
        }
        else if (zone3Value == -10 && zone3LValue != -10)
        {
            ServerSend.UpdateZoneState(3,1, myMatch.GetMatchIdNoStatic());
        }
        zone3Value = Mathf.Clamp(zone3Value, -10, 10);
        zone3LValue = zone3Value;


    }
    private void UpdateZone2()
    {
        var team1CountInZone2 = team1Zones.Values.Count(zone => zone == 2);
        var team2CountInZone2 = team2Zones.Values.Count(zone => zone == 2);
        if(team1CountInZone2 == 0 && team2CountInZone2 == 0) return;
        if (team1CountInZone2 == team2CountInZone2) return;
        //9
        zone2Value += team1CountInZone2 > team2CountInZone2 ? -1 : 1;
        //10
        //9
        if (zone2Value == 10 && zone2LValue != 10)
        {
            ServerSend.UpdateZoneState(2,2, myMatch.GetMatchIdNoStatic());
        }
        else if (zone2Value == -10 && zone2LValue != -10)
        {
            ServerSend.UpdateZoneState(2,1, myMatch.GetMatchIdNoStatic());
        }
        zone2Value = Mathf.Clamp(zone2Value, -10, 10);
        zone2LValue = zone2Value;


    }
    private void UpdateZone1()
    {
        var team1CountInZone1 = team1Zones.Values.Count(zone => zone == 1);
        var team2CountInZone1 = team2Zones.Values.Count(zone => zone == 1);
        if(team1CountInZone1 == 0 && team2CountInZone1 == 0) return;
        if (team1CountInZone1 == team2CountInZone1) return;
        zone1Value += team1CountInZone1 > team2CountInZone1 ? -1 : 1;
        if (zone1Value == 10 && zone1LValue != 10)
        {
            ServerSend.UpdateZoneState(1,2, myMatch.GetMatchIdNoStatic());
        }
        else if (zone1Value == -10 && zone1LValue != -10)
        {
            ServerSend.UpdateZoneState(1,1, myMatch.GetMatchIdNoStatic());
        }
        zone1Value = Mathf.Clamp(zone1Value, -10, -10);
        zone1LValue = zone1Value;

    }
    
    #endregion

    #region TutorialMethods

    private void UpdatePlayerTutorialStage()
    {
        if (currentPlayerStage == TutorialStage.End)
        {
            ReturnToMain();
        }
        else
        {
            currentPlayerStage++;
            UpdateLocalPlayerStage();
        }
    }

    private void UpdateLocalPlayerStage()
    {
        ServerSend.UpdateLocalPlayerTutorialStag(currentTutorialPlayer.GetPlayerId(), (int) currentPlayerStage);
    }

    private void ReturnToMain()
    {
        ServerSend.ForceSceneLoad(1, currentTutorialPlayer.GetPlayerId());
    }

    #endregion
    
    private enum Map
    {
        aztecRuins = 1,
        deadlyWaters
    }
    public GamePLay(int gameMap, Match myMatch)
    {
        this.myMatch = myMatch;
        if (myMatch.matchType == MatchType.Tutorial)
        {
            currentTutorialPlayer = myMatch.currentPlayer;
            currentPlayerStage = TutorialStage.Introduction;
            ServerSend.BeginTutorial(currentTutorialPlayer.GetPlayerId());
        }
        else
        {
            this.gameMap = (Map) gameMap;
            foreach (var player in myMatch.GetTeam1())
            {
                team1Zones.Add(player.GetPlayerId(), -1);
                gamePlayers.Add(new InternalPlayerMethods(1, player.GetPlayerId(), this.myMatch.MatchLobby.currentPick[player.GetPlayerId()], this));
            }
            foreach (var player in myMatch.GetTeam2())
            {
                team2Zones.Add(player.GetPlayerId(), -1);
                gamePlayers.Add(new InternalPlayerMethods(2, player.GetPlayerId(), this.myMatch.MatchLobby.currentPick[player.GetPlayerId()], this));
            }

            UpdateGamePlay += UpdateMethod;
            FastUpdateGamePlay += FastUpdateMethod;
        }
    }
    public void UpdatePlayerPosition(int fromClient, Vector3 position, Quaternion rotation)
    {
        if (myMatch.matchType == MatchType.Tutorial) return;
        if (position.y <= -10)
        {
            gamePlayers.Where(w => w.PlayerId == fromClient).ToList()[0].TakeDamage(true);
        }
        else
        {
            foreach (var player in myMatch.GetAllPlayers())
            {
//            Debug.Log($"Sending Player Update Pos {fromClient} to {player.GetPlayerId()}");
                ServerSend.PlayerPosition(player.GetPlayerId(), fromClient, position, rotation);
            }
        }
    }

   
    public void RemoveGameObjectFromMatch(int id)
    {
           //THIS METHOD HANDLES THE DISCONNECTION OF A PLAYER  WHILE IN MATCH
           playerTeams.Add(Dictionaries.dictionaries.PlayersById[id], team1Zones.Keys.Contains(id) ? 1 : 2);
           ServerSend.RemoveGameObjectFromMatch(id);
           
    }

    public void ReconnectToMatch(int playerId)
    {
        if (playerTeams.Keys.Contains(Dictionaries.dictionaries.PlayersById[playerId]))
        {
            var team = playerTeams[Dictionaries.dictionaries.PlayersById[playerId]];
            myMatch.MatchLobby.SpawnPlayer(playerId, team);
        }
    }
    private class InternalPlayerMethods
    {
        public int MyTeam { get; private set; }
        public int PlayerId { get; private set; }
        public int MyChampion { get; private set; }
        public int deathCount = 0;
        public int killCount = 0;
        public float maxHp = 1000;
        public int DeathTimer = 0;
        public float currentHp = 1000;
        public GamePLay myGame;

        public InternalPlayerMethods(int team, int id, int champion, GamePLay myGame)
        {
            MyTeam = team;
            PlayerId = id;
            MyChampion = champion;
            this.myGame = myGame;
            switch (champion)
            {
                case 0:
                    maxHp = 1000;
                    break;
                case 2:
                    maxHp = 800;
                    break;
                case 1:
                    maxHp = 1500;
                    break;
                default:
                    //WTF HAPPENED
                    break;
            }
            currentHp = maxHp;
        }

        private void UpdateHealth()
        {
            foreach (var player in myGame.myMatch.GetAllPlayers())
            {
                ServerSend.UpdatePlayerHealth(PlayerId, currentHp,player.GetPlayerId());
            }
        }
        public void TakeDamage(float value)
        {
            if (currentHp - value <= 0)
            {
                Die();
            }
            currentHp -= value;
            UpdateHealth();
        }
        public void TakeDamage(bool trueDeath)
        {
            Die();
        }

        private void Die()
        {
            DeathTimer = deathCount > 1 ?  5 * deathCount / 2 : 5;
            deathCount++;
            SendDeath();
        }

        private void SendDeath()
        {
            foreach (var player in myGame.myMatch.GetAllPlayers())
            {
                ServerSend.SetDeathOnPlayer(PlayerId, DeathTimer, deathCount, player.GetPlayerId());
            }
        }

        public void RestoreHealth(float value)
        {
            currentHp += value;
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
            UpdateHealth();
        }
     
     
    }

    public void SetWinner()
    {
        var zonesValues = zone1Value + zone2Value + zone3Value + zone4Value + zone7Value;
        switch (zonesValues)
        {
            case 10:
            case 20:
            case 30:
            case 40:
                Debug.Log("Team Blue Wins");
                break;
            case -10:
            case -20:
            case -30:
            case -40:
                Debug.Log("Team Red Wins");
                break;
        }
    }
    
    public void UpdateTimer()
    {
        if (myMatch.winner == null)
        {
            if (MatchTimer <= 1)
            {
                //TIMER HAS ENDED SO HAS MATCH
                SetWinner();
            }
            else
            {
                MatchTimer--;
            }
        }
    }
}