﻿using System.Net;
using Network;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public UIManager uiManager;

    public static void Welcome(Packet _packet)
    {
        var _msg = _packet.ReadString();
        var _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        UIManager.Instance.offline.SetActive(false);
        UIManager.Instance.online.SetActive(true);
        Constants.ServerID = _myId;
        ClientSend.WelcomeReceived();
        Client.instance.udp.Connect(((IPEndPoint) Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        var _id = _packet.ReadInt();
        var _username = _packet.ReadString();
        var _position = _packet.ReadVector3();
        var _rotation = _packet.ReadQuaternion();

        GameManager.Instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void InvitationReceived(Packet _packet)
    {
        var _id = _packet.ReadInt();
        var _username = _packet.ReadString();
        var _who = _packet.ReadString();

        //TODO Pop Invite UI

        UIManager.Instance.GetInvite(_id, _username);
    }

    public static void HandleLogin(Packet _packet)
    {
        var _id = _packet.ReadInt();
        var _allowed = _packet.ReadBool();
        var _error = _packet.ReadString();
        var _dbId = _packet.ReadInt();
        if (_allowed)
        {
            PlayerVariables.UserID = _dbId;
            Constants.ID = _dbId;
            UIManager.Instance.AllowLogin();
            HandleAsync.Instance.Routine(HandleAsync.Instance.GetUserBasicData());
            HandleAsync.Instance.Routine(HandleAsync.Instance.GetAllSkins());
            HandleAsync.Instance.Routine(HandleAsync.Instance.GetFriends());
        }
        else
        {
            UIManager.Instance.ForbidLogin(_error);
            Constants.Username = null;
            PlayerVariables.UserName = null;
        }
    }


    public static void PlayerPosition(Packet _packet)
    {
        var _id = _packet.ReadInt();
        var _position = _packet.ReadVector3();
        GameManager.Players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        var _id = _packet.ReadInt();
        var _rotation = _packet.ReadQuaternion();

        GameManager.Players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        var _id = _packet.ReadInt();
//        Destroy(GameManager.Players[_id].gameObject);
        GameManager.Players.Remove(_id);
    }

    public static void InvitationResponse(Packet _packet)
    {
        var from = _packet.ReadInt();
        var x = _packet.ReadBool();
        var _name = _packet.ReadString();
        switch (x)
        {
            case true:
                //Invitation Accepted
                UIManager.Instance.CreateGroup(from, _name);
                Debug.Log("Invitation you sent to " + from + " has been accepted");
                break;
            case false:

                //Invitation Declined
                Debug.Log("Invitation you sent to " + from + " has been declined");
                break;
        }
    }
    public static void MatchFound(Packet _packet)
    {
        throw new System.NotImplementedException();
    }

    public static void MMState(Packet _packet)
    {
        LookForGame.SetFGButtons();
    }

    public static void InteractableButtons(Packet _packet)
    {
        LookForGame.SetInteract();
    }
}