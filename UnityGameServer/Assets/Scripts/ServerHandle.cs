using UnityEngine;
using System;
using System.Linq;


class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        var clientIdCheck = _packet.ReadInt();
        
        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
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
        var username = _packet.ReadString();
        var toUserName = _packet.ReadString();
        var myKey = Dictionaries.PlayersByName.FirstOrDefault(x => x.Value == toUserName).Key;
        
        Dictionaries.Parties.Add(_fromClient,myKey); 
        ServerSend.SendInvite(_fromClient,username,toUserName,myKey);
    }

    public static void SendInviteAnswer(int _fromclient, Packet _packet)
    {
        var answer = _packet.ReadBool();
        var name = _packet.ReadString();
        var sendTO = Dictionaries.Parties.FirstOrDefault(x => x.Value == _fromclient).Key;
        ServerSend.SendInviteAnswer(_fromclient, answer, name, sendTO);
    }
}
