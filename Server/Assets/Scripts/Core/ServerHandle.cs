using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class ServerHandle
{
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        var clientIdCheck = packet.ReadInt();
/*        ServerConsoleWriter.WriteLine(
            $"Connection with {Server.Clients[fromClient].TcpInstance.Socket.Client.RemoteEndPoint} has been successfully established and is now indexed as : {fromClient}.");*/
    }

    public static void PlayerMovement(int fromClient, Packet packet)
    {
        var matchId = packet.ReadInt();
        var position = packet.ReadVector3();
        var rotation = packet.ReadQuaternion();
        Dictionaries.dictionaries.Matches[Dictionaries.dictionaries.PlayerDataHolders[fromClient].GetMatchId()].myGamePLay.UpdatePlayerPosition(fromClient, position, rotation);
    }
    

    public static void LoginInformation(int fromClient, Packet packet)
    {
        var id = packet.ReadInt();
        var username = packet.ReadString();
        var password = packet.ReadString();
        Server.Clients[fromClient].RequestLogin(username, password);
    }

    public static void SendInvitationServer(int fromClient, Packet packet)
    {
        var id = packet.ReadInt();
        var username = packet.ReadString(); //Who SENT THE INVITATION
        var toUserName = packet.ReadString(); // Who is THE INVITE FOR
        if (Dictionaries.dictionaries.PlayersByName.ContainsKey(toUserName))
        {
            var sendToID = Dictionaries.dictionaries.PlayersByName[toUserName];
            ServerSend.SendInvite(fromClient, username, toUserName, sendToID);
        }
        else
        {   
            //The given user was not online
            ServerConsoleWriter.WriteUserLog(Dictionaries.dictionaries.PlayersById[fromClient],
                $"Tried to invite {toUserName} but this user was not online");
        }
    }

    public static void SendInviteAnswer(int fromClient, Packet packet)
    {
        var answer = packet.ReadBool();
        var sendTo = packet.ReadString();
        var whoSent = packet.ReadString();
        ServerSend.SendInviteAnswer(fromClient, answer, whoSent, GetIdOf(sendTo));
        if (!answer) return;
        var partyMembers = new List<PlayerDataHolder>
        {
            Dictionaries.dictionaries.PlayerDataHolders[fromClient],
            Dictionaries.dictionaries.PlayerDataHolders[Dictionaries.dictionaries.PlayersByName[sendTo]]
        };
        var partyLeader = Dictionaries.dictionaries.PlayerDataHolders[Dictionaries.dictionaries.PlayersByName[sendTo]];
        var partyMember =
            Dictionaries.dictionaries.PlayerDataHolders[Dictionaries.dictionaries.PlayersByName[Dictionaries.dictionaries.PlayersById[fromClient]]];
        var id = Dictionaries.dictionaries.PlayersByName[Dictionaries.dictionaries.PlayersById[fromClient]];
        if (partyLeader.InParty)
        {
            //Player is already in party
            Parties.AddToExistingParty(partyLeader.PartyID, Dictionaries.dictionaries.PlayerDataHolders[id]);
            ServerSend.RemoveLfgButton(id);
            partyMember.PartyID = partyLeader.PartyID;
            partyMember.InParty = true;
        }
        else
        {
            var partyId = Parties.AddParty(partyMembers);
            partyMember.InParty = true;
            partyLeader.InParty = true;
            partyMember.PartyID = partyId;
            partyLeader.PartyID = partyId;
            ServerSend.RemoveLfgButton(id);
        }
    }

    private static int GetIdOf(string username)
    {
        return Dictionaries.dictionaries.PlayersByName[username];
    }

    private static string GetUserNameOf(int fromClient)
    {
        return Dictionaries.dictionaries.PlayersByName.FirstOrDefault(x => x.Value == fromClient).Key;
    }

    public static void AddToMatchMaking(int fromClient, Packet packet)
    {
        if(HandleMatchMaking.IsPlayerInQueue(fromClient)) return;
        Debug.Log("Added to mm");
        var mapId = packet.ReadInt();
        switch (mapId)
        {
            case 0: //Tutorial
                Debug.Log("Tutorial Request");
                var match = new Match(Dictionaries.dictionaries.PlayerDataHolders[fromClient],
                    MatchType.Tutorial);
                break;
            case 1:
            case 2:
                var isClientInParty = packet.ReadBool();
                if (isClientInParty) //Player In Party
                {
                    var partyID = Dictionaries.dictionaries.PlayerDataHolders[fromClient].PartyID;
                    var partyMembers = Parties.GetParty(partyID);
                    foreach (var member in partyMembers)
                    {
                        HandleMatchMaking.AddToQueue(member);
                        ServerSend.MatchMakingState(Dictionaries.dictionaries.PlayersByName[member.Username]);
                    }
                }
                else //Player Is NOT IN PARTY
                {
                    HandleMatchMaking.AddToQueue(Dictionaries.dictionaries.PlayerDataHolders[fromClient]);
                    ServerSend.MatchMakingState(fromClient);
                }
                break;
            default:
                Debug.LogError($"Add To Match Making Method was called by {fromClient} with an invalid MAP ID {mapId} \n His request has been ignored.");
                break;
        }
   
    }

    public static void MatchAnswer(int fromClient, Packet packet)
    {
       Debug.LogWarning($"Deprecated Method SET MATCH ANSWER sent from -> {fromClient} client");
    }

    public static void RemoveFromMatchMaking(int fromClient, Packet packet)
    {
        var isClientInParty = packet.ReadBool();
        if (isClientInParty) //Player In Party
        {
            var partyID = Dictionaries.dictionaries.PlayerDataHolders[fromClient].PartyID;
            var partyMembers = Parties.GetParty(partyID);
            foreach (var member in partyMembers)
            {
                HandleMatchMaking.RemoveFromQueue(member);
                ServerSend.MatchMakingState(Dictionaries.dictionaries.PlayersByName[member.Username]);
            }
        }
        else //Player Is NOT IN PARTY
        {
            HandleMatchMaking.RemoveFromQueue(Dictionaries.dictionaries.PlayerDataHolders[fromClient]);
            ServerSend.MatchMakingState(fromClient);
        }
    }

    public static void HandleLobbyPlayer(int fromclient, Packet packet)
    {
        
        var clientMatchId = packet.ReadInt();
        var whatPick = packet.ReadInt();
        Dictionaries.dictionaries.Matches[Dictionaries.dictionaries.PlayerDataHolders[fromclient].GetMatchId()].MatchLobby.UpdateCurrentSelectedPick(fromclient, whatPick);
        /*
         * TO KEEP IT SIMPLE WE WILL ONLY GIVE ONE CHANCE TO THE PLAYER WHO PICKED SO AS SOON AS YOU CLICK A BUTTON THAT DECISION IS FINAL
         */
    }

    public static void HandleSceneUpdate(int fromclient, Packet packet)
    {
        var matchId = packet.ReadInt();
        Dictionaries.dictionaries.Matches[Dictionaries.dictionaries.PlayerDataHolders[fromclient].GetMatchId()].SetSceneLoaded(fromclient);
    }

    public static void HandleSceneCompleted(int fromclient, Packet packet)
    {
        var matchId = packet.ReadInt();
        Dictionaries.dictionaries.Matches[Dictionaries.dictionaries.PlayerDataHolders[fromclient].GetMatchId()].SetPlayerComplete();
    }


    public static void HandleGamePlayConflictZone(int fromclient, Packet packet)
    {
        var matchId = packet.ReadInt();
        var zone = packet.ReadInt();
        Dictionaries.dictionaries.Matches[Dictionaries.dictionaries.PlayerDataHolders[fromclient].GetMatchId()].myGamePLay.SetCurrentPlayerZone(fromclient, zone);
    }
    
}
