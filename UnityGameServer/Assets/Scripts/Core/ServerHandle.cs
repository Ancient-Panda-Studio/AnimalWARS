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
        var username = _packet.ReadString(); //Who SENT THE INVITATION
        var toUserName = _packet.ReadString(); // Who is THE INVITE FOR
        var sendToID = Dictionaries.playersByName[toUserName]; 
        Debug.Log("Sending invite from: " + username);
        ServerSend.SendInvite(_fromClient,username,toUserName,sendToID);
    }

    public static void SendInviteAnswer(int _fromclient, Packet _packet)
    {
        var answer = _packet.ReadBool();
        var sendTO = _packet.ReadString();
        var whoSent = _packet.ReadString();
        ServerSend.SendInviteAnswer(_fromclient, answer, whoSent, GetIdOf(sendTO));
    }

    private static int GetIdOf(string _username)
    {
        return Dictionaries.playersByName[_username];
    }
    
    private static string GetUserNameOf(int _fromclient)
    {
        return Dictionaries.playersByName.FirstOrDefault(x => x.Value == _fromclient).Key;
    }
}
