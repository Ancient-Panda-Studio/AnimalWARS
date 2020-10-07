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
            $"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
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
        var partyMembers = new List<int> {_fromClient, Dictionaries.PlayersByName[sendTO]};
        var PartyLeader = Dictionaries.PlayerDataHolders[Dictionaries.PlayersByName[sendTO]];
        if (PartyLeader.inParty)
        {
            //Player is already in party 
            Parties.AddToExistingParty(PartyLeader.partyID, partyMembers[0]);
        }
        else
        {
          int partyId = Parties.AddParty(partyMembers);
            PartyLeader.inParty = true;
            PartyLeader.partyID = partyId;
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
        Debug.Log(Dictionaries.PlayerDataHolders[_fromClient].username);
        if (isClientInParty) //Player In Party
        {
            var partyID = _packet.ReadInt();
            var partyMembers = Parties.GetParty(partyID);
            foreach (var member in partyMembers)
            {
                HandleMatchMaking.AddToQueue(Dictionaries.PlayerDataHolders[member]);
            }
        }
        else //Player Is NOT IN PARTY
        {
            HandleMatchMaking.AddToQueue(Dictionaries.PlayerDataHolders[_fromClient]);
        }
    }

    public static void RemoveFromMatchMaking(int _fromClient, Packet _packet)
    {
        throw new NotImplementedException();
    }
}