using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        var clientIdCheck = _packet.ReadInt();

        Debug.Log(
            $"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connexion successful from : {_fromClient}.");
    }

    public static void PlayerMovement(int fromClient, Packet _packet)
    {
        var inputs = new bool[_packet.ReadInt()];
        for (var i = 0; i < inputs.Length; i++)
        {
            inputs[i] = _packet.ReadBool();
        }

        var rotation = _packet.ReadQuaternion();

        Server.clients[fromClient].player.SetInput(inputs, rotation);
    }

    public static void LoginInformation(int _fromClient, Packet _packet)
    {
        var id = _packet.ReadInt();
        var username = _packet.ReadString();
        var password = _packet.ReadString();
        Debug.Log(username + "   " + password);
        Server.clients[_fromClient].RequestLogin(username, password, id);
    }

    public static void SendInvitationServer(int _fromClient, Packet _packet)
    {
        var id = _packet.ReadInt();
        var username = _packet.ReadString(); //Who SENT THE INVITATION
        var toUserName = _packet.ReadString(); // Who is THE INVITE FOR
        var sendToID = Dictionaries.PlayersByName[toUserName];
        Debug.Log("Sending invite from: " + username);
        ServerSend.SendInvite(_fromClient, username, toUserName, sendToID);
    }

    public static void SendInviteAnswer(int _fromClient, Packet _packet)
    {
        var answer = _packet.ReadBool();
        var sendTO = _packet.ReadString();
        var whoSent = _packet.ReadString();
        ServerSend.SendInviteAnswer(_fromClient, answer, whoSent, GetIdOf(sendTO));
        if (!answer) return;
        var partyMembers = new List<PlayerDataHolder> {Dictionaries.PlayerDataHolders[_fromClient], Dictionaries.PlayerDataHolders[Dictionaries.PlayersByName[sendTO]]};
        var PartyLeader = Dictionaries.PlayerDataHolders[Dictionaries.PlayersByName[sendTO]];
        var PartyMember = Dictionaries.PlayerDataHolders[Dictionaries.PlayersByName[Dictionaries.PlayersById[_fromClient]]];
        int id = Dictionaries.PlayersByName[Dictionaries.PlayersById[_fromClient]];
        if (PartyLeader.inParty)
        {
            Debug.Log(PartyLeader.username + " has a new member on his party " + PartyMember.username);
            //Player is already in party 
            Parties.AddToExistingParty(PartyLeader.partyID, Dictionaries.PlayerDataHolders[id]);
            ServerSend.RemoveLFGButton(id);
            PartyMember.partyID = PartyLeader.partyID;
            PartyMember.inParty = true;
        }
        else
        {
            var partyId = Parties.AddParty(partyMembers);
            Debug.Log("A new party has been created with " + PartyLeader.username + " as it's leader and " + PartyMember.username + " as his first member" + partyId + " is the id for the new party" );
            PartyMember.inParty = true;
            PartyLeader.inParty = true;
            PartyMember.partyID = partyId;
            PartyLeader.partyID = partyId;
            ServerSend.RemoveLFGButton(id);
        }
    }

    private static int GetIdOf(string _username)
    {
        return Dictionaries.PlayersByName[_username];
    }

    private static string GetUserNameOf(int _fromClient)
    {
        return Dictionaries.PlayersByName.FirstOrDefault(x => x.Value == _fromClient).Key;
    }

    public static void AddToMatchMaking(int _fromClient, Packet _packet)
    {
        bool isClientInParty = _packet.ReadBool();
        if (isClientInParty) //Player In Party
        {
            var partyID = Dictionaries.PlayerDataHolders[_fromClient].partyID;
            var partyMembers = Parties.GetParty(partyID);
            foreach (var member in partyMembers)
            {
                HandleMatchMaking.AddToQueue(member);
                ServerSend.MatchMakingState(Dictionaries.PlayersByName[member.username]);
            }
        }
        else //Player Is NOT IN PARTY
        {
            HandleMatchMaking.AddToQueue(Dictionaries.PlayerDataHolders[_fromClient]);
        }
    }

    public static void RemoveFromMatchMaking(int _fromClient, Packet _packet)
    {  
        var isClientInParty = _packet.ReadBool();
        Debug.Log(Dictionaries.PlayerDataHolders[_fromClient].username);
        if (isClientInParty) //Player In Party
        {
            var partyID = Dictionaries.PlayerDataHolders[_fromClient].partyID;            
            var partyMembers = Parties.GetParty(partyID);
            foreach (var member in partyMembers)
            {
                HandleMatchMaking.RemoveFromQueue(member);
                ServerSend.MatchMakingState(Dictionaries.PlayersByName[member.username]);
            }
        }
        else //Player Is NOT IN PARTY
        {
            HandleMatchMaking.RemoveFromQueue(Dictionaries.PlayerDataHolders[_fromClient]);
        }
    }
}