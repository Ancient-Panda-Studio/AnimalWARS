using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

public class HandleMatchMaking
{
    
    private static Random random = new Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    
    private static readonly int[] MapsArray = {1, 2, 3, 4, 5};
    public static bool Generating;
    private static readonly List<PlayerDataHolder> MatchQueue = new List<PlayerDataHolder>(); //This list stores the ID's of players looking for match
    //public List<Maps> availableMaps = new List<Maps>();
    
    public static void AddToQueue(PlayerDataHolder playerDataHolder)
    {
        Debug.Log(playerDataHolder.username + " has joined the queue");
        MatchQueue.Add(playerDataHolder);
    }

    public static void RemoveFromQueue(PlayerDataHolder playerDataHolder)
    {
        Debug.Log(playerDataHolder.username + " has left the queue");
        MatchQueue.Remove(playerDataHolder);
    }


    public static void GenerateMatch([NotNull] List <PlayerDataHolder> playerDataHolders)
    {
        if (playerDataHolders == null) throw new ArgumentNullException(nameof(playerDataHolders));
            Generating = true;
            var currentMatchCount = Dictionaries.CurrentMatches.Count;
            int id;
            
            //Generate TEAMS
            var team1 = new List<PlayerDataHolder>();
            var team2 = new List<PlayerDataHolder>();
            var team1C = 0;
           while(team1C < 3) 
           {
               var nextToAdd = playerDataHolders.ToList()[team1C];
               if (!team1.Contains(nextToAdd))
               {
                   if (nextToAdd.inParty)
                   {
                       var partyMembers = Parties.GetParty(nextToAdd.partyID);
                       team1.Add(nextToAdd);
                       playerDataHolders.Remove(nextToAdd);
                       foreach (var partyMemberHolder in partyMembers)
                       {
                           var playerHolder = Dictionaries.PlayerDataHolders[partyMemberHolder];
                           if (!team1.Contains(playerHolder))
                           {
                               team1.Add(playerHolder);
                               playerDataHolders.Remove(playerHolder);
                           }
                       }
                   }
                   else
                   {
                       playerDataHolders.Remove(nextToAdd);
                       team1.Add(nextToAdd);
                   }

               }
               team1C++;
               // foreach (var dataHolder in playerDataHolders.ToList().Where(dataHolder => !team1.Contains(dataHolder)))
               //  {
               //      team1.Add(dataHolder);
               //      var partyMembers = Parties.GetParty(dataHolder.partyID);
               //      foreach (var partyMemberHolder in partyMembers.ToList()
               //          .Select(partyMember => Dictionaries.PlayerDataHolders[partyMember]))
               //      {
               //          playerDataHolders.Remove(partyMemberHolder);
               //          if (team1.Contains(partyMemberHolder)) continue ;
               //          team1.Add(partyMemberHolder);
               //      }
               //
               //      playerDataHolders.Remove(dataHolder);
               //  }
            }
            // while (team2.Count <= 3) {
            //    foreach (var dataHolder in playerDataHolders.ToList().Where(dataHolder => !team2.Contains(dataHolder)))
            //    {
            //        team2.Add(dataHolder);
            //        var partyMembers = Parties.GetParty(dataHolder.partyID);
            //        foreach (var partyMemberHolder in partyMembers.ToList()
            //            .Select(partyMember => Dictionaries.PlayerDataHolders[partyMember]))
            //        {
            //            playerDataHolders.Remove(partyMemberHolder);
            //            if (team2.Contains(partyMemberHolder)) continue ;
            //            team2.Add(partyMemberHolder);
            //        }
            //
            //        playerDataHolders.Remove(dataHolder);
            //    }
            // }
           
            // while (team2.Count < 2)
            // {
            //     foreach (var dataHolder in playerDataHolders.ToList().Where(dataHolder => !team2.Contains(dataHolder)))
            //     {
            //         team2.Add(dataHolder);
            //         var partyMembers = Parties.GetParty(dataHolder.partyID);
            //         foreach (var partyMemberHolder in partyMembers.ToList()
            //             .Select(partyMember => Dictionaries.PlayerDataHolders[partyMember]))
            //         {
            //             playerDataHolders.Remove(partyMemberHolder);
            //             team2.Add(partyMemberHolder);
            //         }
            //
            //         playerDataHolders.Remove(dataHolder);
            //     }
            // }

            Debug.Log(team1.Count + "    team 2: " + team2.Count);

            for (int i = 0; i < team1.Count; i++)
            {
                Debug.Log("Player " + team1[i].username + " is player " + i + " of team1");
            }
            // for (int i = 0; i < team2.Count; i++)
            // {
            //     Debug.Log("Player " + team2[i].username + " is player " + 2 + " of team2");
            // }
            // if (team1.Count != 3) //c    heck if team 1 is full
            // {
            //     foreach (var dataHolder in playerDataHolders.ToList())
            //     {
            //         Debug.Log(dataHolder.username);
            //         if (dataHolder.inParty) //Check for party
            //         {
            //             team1.Add(dataHolder);
            //             var partyMembers = Parties.GetParty(dataHolder.partyID);
            //             foreach (var partyMember in partyMembers.ToList())
            //             {
            //               var partyMemberHolder = Dictionaries.PlayerDataHolders[partyMember];
            //               playerDataHolders.Remove(partyMemberHolder);
            //               team1.Add(partyMemberHolder);
            //             }
            //             playerDataHolders.Remove(dataHolder);
            //         }
            //         else
            //         {
            //             team1.Add(dataHolder);
            //         }
            //     }
            // }
            // else
            // {
            //     foreach (var dataHolder in playerDataHolders.ToList())
            //     {
            //         if (dataHolder.inParty) //Check for party
            //         {
            //             team2.Add(dataHolder);
            //             var partyMembers = Parties.GetParty(dataHolder.partyID);
            //             foreach (var partyMember in partyMembers.ToList())
            //             {
            //                 var partyMemberHolder = Dictionaries.PlayerDataHolders[partyMember];
            //                 playerDataHolders.Remove(partyMemberHolder);
            //                 team2.Add(partyMemberHolder);
            //             }
            //             playerDataHolders.Remove(dataHolder);
            //         }
            //         else
            //         {
            //             team2.Add(dataHolder);
            //         }
            //     }
            // }
            // //Check for parties

            Debug.Log(team1.Count + " " + team2.Count);
            // for (var i = 0; i < 3; i++)
            // {
            //     // Debug.Log(team1[i] + " is player number 1 for team 1 and " +  team2[i] + " is player number 1 for team 2");
            // }
            //
            // if (currentMatchCount == 0)
            // {
            //     Dictionaries.CurrentMatches.Add(1,playerDataHolders);
            //     id = 1;
            // }
            // else
            // {
            //     Dictionaries.CurrentMatches.Add(currentMatchCount + 1,playerDataHolders);
            //     id = currentMatchCount + 1;
            // }
            var mapOrder = new List<int>();
            mapOrder.AddRange(MapsArray);
            mapOrder.ShuffleList();
            // foreach (var player in playerDataHolders)
            // {
            //     player.currentMatchId = id;
            //     MatchQueue.Remove(player);
            //     //Todo Spawn player
            // }
            Generating = false;
    }
    public static List<PlayerDataHolder> CheckIfMatchMakingIsPossible()
    {
        if (MatchQueue.Count < 6) return null;
        var playersToAdd = new List<PlayerDataHolder>();
        // ReSharper disable once InvertIf
        if (playersToAdd.Count != 6)
        {
            for (var i = 0; i < 1; i++)
            {
                if (MatchQueue[i].inParty)
                {
                    var partyMembers = Parties.GetParty(MatchQueue[i].partyID);
                    foreach (var playerID in partyMembers) //Once a party is found remove all party members from que list to avoid Adding them more than once!
                    {
                        MatchQueue.Remove(Dictionaries.PlayerDataHolders[playerID]);
                    }
                    playersToAdd.AddRange(partyMembers.Select(t => Dictionaries.PlayerDataHolders[t]));
                }
                else
                {
                    playersToAdd.Add(MatchQueue[i]);
                }
            }
        }
        return playersToAdd;
    }

}
