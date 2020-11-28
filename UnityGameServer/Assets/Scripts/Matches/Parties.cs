using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parties
{
    private static readonly SortedDictionary<int, List<PlayerDataHolder>> CurrentParties = new SortedDictionary<int, List<PlayerDataHolder>>();
    public static int AddParty(List<PlayerDataHolder> _members)
    {
        if (CurrentParties.Count == 0)
        {
            CurrentParties.Add(1,_members);
            return 1;
        }
        else
        {
            var x = CurrentParties.Count + 1;
            CurrentParties.Add(x,_members);
            return x;
        }
    }
    public static void RemoveParty(int _partyID)
    {
        CurrentParties.Remove(_partyID);
    }
    public static List<PlayerDataHolder> GetParty(int _partyID)
    {
        return CurrentParties[_partyID];
    }
    public static void AddToExistingParty(int _partyLeaderPartyID, PlayerDataHolder _partyMember)
    {
        List<PlayerDataHolder> x = GetParty(_partyLeaderPartyID);
        x.Add(_partyMember);
        CurrentParties[_partyLeaderPartyID] = x;

    }
    public static void RemoveFromExistingParty(int _partyLeaderPartyID, PlayerDataHolder _partyMember)
    {
        List<PlayerDataHolder> x = GetParty(_partyLeaderPartyID);
        x.Remove(_partyMember);
        CurrentParties[_partyLeaderPartyID] = x;

    }
}
