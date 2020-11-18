using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;
// ReSharper disable ForCanBeConvertedToForeach

public class HandleMatchMaking
{
    private static readonly int[] MapsArray = {1, 2, 3, 4, 5};
    public static bool Generating;
    private static List<PlayerDataHolder> MatchQueue = new List<PlayerDataHolder>(); //This list stores the PlayerDataHolder's of players looking for match
    //public List<Maps> availableMaps = new List<Maps>();
    
    public static void AddToQueue(PlayerDataHolder playerDataHolder)
    {
        ServerConsoleWriter.WriteLine($"{playerDataHolder.username} has joined the match Queue");
        MatchQueue.Add(playerDataHolder);
        
    }

    public static void RemoveFromQueue(PlayerDataHolder playerDataHolder)
    {
        ServerConsoleWriter.WriteLine($"{playerDataHolder.username} has left the match Queue");
        MatchQueue.Remove(playerDataHolder);
    }


    public static void GenerateMatch([NotNull] List <PlayerDataHolder> holdersList)
    {
        if (holdersList == null) throw new ArgumentNullException(nameof(holdersList));
            Generating = true;
            //Generate TEAMS
            var team1 = new List<PlayerDataHolder>();
            var team2 = new List<PlayerDataHolder>();
            for (var i = 0; i < holdersList.Count; i++)
            {
                if(team1.Contains(holdersList[i]) || team2.Contains(holdersList[i])) continue;
                if (holdersList[i].inParty) {
                    var partyMembers = Parties.GetParty(holdersList[i].partyID);
                    var partyCount = partyMembers.Count;
                    if (team1.Count + partyCount <= 3) { team1.AddRange(partyMembers); }
                    else { team2.AddRange(partyMembers); }
                } else { 
                    if (team1.Count + 1 <= 3) { team1.Add(holdersList[i]); }
                    else { team2.Add(holdersList[i]); }
                }
            }
        var mapOrder = new List<int>();
        mapOrder.AddRange(MapsArray);
        mapOrder.ShuffleList();
        //AddMatchToDictionary
        var newMatch = new Match(team1, team2, mapOrder);
        var matchId = Match.GetMatchId();
        Dictionaries.CurrentMatches.Add(matchId,newMatch);
        var x = 500 * matchId;
        var mapToInstantiate = Object.Instantiate(Resources.Load("Prefabs/MapsPrefabs/MapSpawnTest") as GameObject);
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        var newMapScript = mapToInstantiate.GetComponentInChildren<SpawnedMap>();
        newMapScript.SetGameID(matchId);
        Dictionaries.SpawnedMaps.Add(matchId, newMapScript);
        var temp = new Vector3(x,0,0);
        mapToInstantiate.transform.position += temp;
        mapToInstantiate.SetActive(false);
        //Move Map
        mapToInstantiate.SetActive(true);
        foreach (var caller in holdersList)
        {
            switch (team1.Contains(caller))
            {
                case true:
                    var spawnsTeam1 =  newMapScript.GetFreeSpawn(1);
                    Debug.Log(newMapScript.GetFreeSpawns(1).Count);
                    spawnsTeam1.SetFull(true);
                    Debug.Log(newMapScript.GetFreeSpawns(1).Count);
                    var newPlayerTeam1 = NetworkManager.Instance.InstantiatePlayer(spawnsTeam1.myGameObject.transform);
                    caller.SetGameObject(newPlayerTeam1);
                    caller.GetGameObject().name =
                        $"Team {1}  Name {caller.username} Match {newMatch.GetMatchIdNoStatic()}";
                    caller.SetMatchId(matchId);
                    break;
                case false:
                    var spawnsTeam2 =  newMapScript.GetFreeSpawn(2);
                    Debug.Log(newMapScript.GetFreeSpawns(2).Count);
                    spawnsTeam2.SetFull(true);
                    Debug.Log(newMapScript.GetFreeSpawns(2).Count);
                    var newPlayerTeam2 = NetworkManager.Instance.InstantiatePlayer(spawnsTeam2.myGameObject.transform);
                    caller.SetGameObject(newPlayerTeam2);
                    caller.GetGameObject().name =
                        $"Team {2}  Name {caller.username} Match {newMatch.GetMatchIdNoStatic()}";
                    caller.SetMatchId(matchId);
                    break;
            }
        }
        foreach (var holder in holdersList)
        {
            Debug.Log(holder.GetPlayerId() + " is now being Spawned");
            ServerSend.SpawnPlayer(holder.GetPlayerId(),holder.GetGameObject(), Parser.ParseHolderToInt(holdersList));
        }
        ServerConsoleWriter.WriteLine($"A new match has been created id: {newMatch.GetMatchIdNoStatic()} Map order will be the following: 1- {mapOrder[0]} \n 2- {mapOrder[1]} \n 3- {mapOrder[2]} \n 4- {mapOrder[3]} \n 5- {mapOrder[4]}");
        ServerConsoleWriter.WriteMatchLog(newMatch.GetMatchIdNoStatic(),
            $"New match log {newMatch.GetMatchIdNoStatic()}");
        ServerConsoleWriter.WriteMatchLog(newMatch.GetMatchIdNoStatic(),
            $"Team 1 : \n Player 1: {newMatch.GetTeam1().ToList()[0]} \n Player 2: {newMatch.GetTeam1().ToList()[1]} \n Player 3: {newMatch.GetTeam1().ToList()[2]}");
        ServerConsoleWriter.WriteMatchLog(newMatch.GetMatchIdNoStatic(),
            $"Team 2 : \n Player 1: {newMatch.GetTeam2().ToList()[0]} \n Player 2: {newMatch.GetTeam2().ToList()[1]} \n Player 3: {newMatch.GetTeam2().ToList()[2]}");
        
        Generating = false; //Finished Generating Match
    }
    public static List<PlayerDataHolder> CheckIfMatchMakingIsPossible()
    {
        if (MatchQueue.Count < 6) return null;
        var playersToAdd = new List<PlayerDataHolder>();
        // ReSharper disable once InvertIf
        for (var i = 0; i < MatchQueue.Count; i++)
        {
            if(playersToAdd.Contains(MatchQueue[i])) continue;
            if (MatchQueue[i].inParty)
            {
                var partyMembers = Parties.GetParty(MatchQueue[i].partyID);
                var partyCount = partyMembers.Count;
                if (playersToAdd.Count + partyCount > 6) continue;
                playersToAdd.AddRange(partyMembers.Select(t => Dictionaries.PlayerDataHolders[Dictionaries.PlayersByName[t.username]]));
            }
            else
            {
                playersToAdd.Add(MatchQueue[i]);
            }
        }
        var newMm = MatchQueue.Except(playersToAdd).ToList();
        MatchQueue = newMm;
        return playersToAdd;
    }
}
