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
    
    private static Random random = new Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#|+-";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    
    private static readonly int[] MapsArray = {1, 2, 3, 4, 5};
    public static bool Generating;
    private static List<PlayerDataHolder> MatchQueue = new List<PlayerDataHolder>(); //This list stores the PlayerDataHolder's of players looking for match
    //public List<Maps> availableMaps = new List<Maps>();
    
    public static void AddToQueue(PlayerDataHolder playerDataHolder)
    {
        MatchQueue.Add(playerDataHolder);
    }

    public static void RemoveFromQueue(PlayerDataHolder playerDataHolder)
    {
        MatchQueue.Remove(playerDataHolder);
    }


    public static void GenerateMatch([NotNull] List <PlayerDataHolder> holdersList)
    {
        if (holdersList == null) throw new ArgumentNullException(nameof(holdersList));
            Generating = true;
            Debug.Log("Match generation has started...");
            //Generate TEAMS
            var team1 = new List<PlayerDataHolder>();
            var team2 = new List<PlayerDataHolder>();
            Debug.Log("Defining teams...");
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
        Debug.Log("Randomizing MAP order...");
        var mapOrder = new List<int>();
        mapOrder.AddRange(MapsArray);
        mapOrder.ShuffleList();
        //AddMatchToDictionary
        var newMatch = new Match(team1, team2, mapOrder);
        var matchId = Match.GetMatchId();
        Dictionaries.CurrentMatches.Add(matchId,newMatch);
        Debug.Log("A new match has been created: " + matchId + " is the match id");
        var x = 500 * matchId;
        var mapToInstantiate = Object.Instantiate(Resources.Load("Prefabs/MapsPrefabs/MapSpawnTest") as GameObject);
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        var newMapScript = mapToInstantiate.GetComponentInChildren<SpawnedMap>();
        newMapScript.SetGameID(matchId);
        Debug.Log(newMapScript.GetGameId());
        Dictionaries.SpawnedMaps.Add(matchId, newMapScript);
        var temp = new Vector3(x,0,0);
        mapToInstantiate.transform.position += temp;
        mapToInstantiate.SetActive(false);
        //Move Map
        mapToInstantiate.SetActive(true);
        foreach (var player in holdersList)
        { 
            Debug.Log(player.username + " has been added to the new match");
            player.SetMatchId(matchId);
            //TODO LOBBY TO SELECT SKIN
            player.CallSpawn(newMapScript);
        }
        Debug.Log("Match generation has ended Spawning Players...");
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
                int partyCount = partyMembers.Count;
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
