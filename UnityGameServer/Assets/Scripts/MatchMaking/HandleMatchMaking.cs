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
        /*
            Here we know who is gonna Join the match so now we save this data In a MATCH object
            When all the players accept the POP UP that appears on their GAME CLIENT
            the match is Started calling the method Match[THIS MATCH ID].Begin()
            if not everyone accepts -> the match is Destroyed and all players who did accept the Match pop up are Added to the queue again.
        */
        var SavedMatch = new Match(team1, team2);
        /*SEND THE POP UP TO EVERYONE IN THE MATCH :)
        -> THIS WILL SHOW THE UI IN THE CLIENTS
        -> AFTER 20 SECONDS IT WILL CHECK IF EVERYONE IN THE MATCH HAS ACCEPTED
        -> IF EVERYONE HAS THE MATCH WILL START Match[_matchId].Begin();
        */
        SavedMatch.SendPopUp();
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
