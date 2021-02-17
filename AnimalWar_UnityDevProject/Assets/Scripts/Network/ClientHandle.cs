using System;
using System.Net;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;
using static MatchVariables;
using static PlayerVariables;
using static UIManager;

public class ClientHandle
{
    public static void Welcome(Packet _packet)
    {
        var msg = _packet.ReadString();
        var myId = _packet.ReadInt();
        Debug.Log($"Message from server: {msg}");
        Client.instance.myId = myId;
        UIObjects.Instance.offline.SetActive(false);
        UIObjects.Instance.online.SetActive(true);
        ClientSend.WelcomeReceived();
        Client.instance.udp.Connect(((IPEndPoint) Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
    public static void SpawnPlayer(Packet _packet)
    {
            var id = _packet.ReadInt();
            var pick = _packet.ReadInt();
            var position = _packet.ReadVector3();
            LoadingSceneScript.SpawnPlayer(id, pick, position);

        //GameManager.Instance.SpawnPlayer(id, position, rotation);
    }

    public static void MatchFound(Packet packet)
    {
        Debug.Log($"MatchFound was called");

        /*    
         * This will hold the logic for showing the YOUR MATCH IS READY UI #5897
         */
        Debug.Log($"My match is ready :D {packet.GetHashCode()}");
        UIObjects.Instance.MatchFoundUI.GetComponent<Canvas>().enabled = true; //GAME OBJECTS ARE ALWAYS ENABLED
        ExternalUIFunctions.activateDecline = true;

    }
    public static void InvitationReceived(Packet _packet)
    {
        Debug.Log($"InvitationReceived was called");

        var id = _packet.ReadInt();
        var username = _packet.ReadString();
        var who = _packet.ReadString();

        //TODO Pop Invite UI

        GetInvite(id, username);
    }
    public static void HandleLogin(Packet _packet)
    {
        Debug.Log($"HandleLogin was called");

        UIObjects.Instance.loadingBarHolder_Login.SetActive(false);
        var id = _packet.ReadInt();
        var allowed = _packet.ReadBool();
        var error = _packet.ReadString();
        var dbId = _packet.ReadInt();
        if (allowed)
        {
            UserID = dbId;
            DbId = dbId;
            ServerID = id;
            IsLogged = true;
            AllowLogin();
            HandleAsync.Instance.Routine(HandleAsync.Instance.GetUserBasicData());
            HandleAsync.Instance.Routine(HandleAsync.Instance.GetAllSkins());
            HandleAsync.Instance.Routine(HandleAsync.Instance.GetFriends());
        }
        else
        {
            ForbidLogin(error);
            IsLogged = false;
            Username = null;
            UserName = null;
        }
    }
    public static void HandleLobbyPacket(Packet _packet)
    {
        Debug.Log("HandleLobbyPacket");
            var myId = _packet.ReadInt();
            var matchId = _packet.ReadInt();
            var myTeamNumber = _packet.ReadInt();
            MyTeam = myTeamNumber;
            MatchId = matchId;
            //MY TEAM
            var teamMate1Username  = _packet.ReadString();
            var teamMate1Id = _packet.ReadInt();
            var teamMate2Username  = _packet.ReadString();
            var teamMate2Id = _packet.ReadInt();
            var teamMate3Username  = _packet.ReadString();
            var teamMate3Id = _packet.ReadInt();
            //ENEMIES
            var enemy1Username = _packet.ReadString();
            var enemy1Id = _packet.ReadInt();
            var enemy2Username = _packet.ReadString();  
            var enemy2Id = _packet.ReadInt();
            var enemy3Username = _packet.ReadString();
            var enemy3Id = _packet.ReadInt();
            WriteLobbyConstants(teamMate1Username, teamMate1Id, teamMate2Username, teamMate2Id, teamMate3Username, teamMate3Id, enemy1Username, enemy1Id, enemy2Username, enemy2Id, enemy3Username, enemy3Id);

    }
    private static void WriteLobbyConstants(string teamMate1Username, int teamMate1Id, string teamMate2Username, int teamMate2Id, string teamMate3Username, int teamMate3Id, string enemy1Username, int enemy1Id,string enemy2Username,int enemy2Id,string enemy3Username,int enemy3Id)
    {
        Debug.Log("WriteLobbyConstants");

        if (teamMate1Username == Username)
        {
            TeamMates.Add(teamMate2Id,teamMate2Username);
            TeamMates.Add(teamMate3Id,teamMate3Username);
        }
        else if(teamMate2Username == Username)
        {
            TeamMates.Add(teamMate1Id,teamMate1Username);
            TeamMates.Add(teamMate3Id,teamMate3Username);
        }
        else
        {
            TeamMates.Add(teamMate1Id,teamMate1Username);
            TeamMates.Add(teamMate2Id,teamMate2Username);
        }
        
        Enemies.Add(enemy1Id,enemy1Username);
        Enemies.Add(enemy2Id,enemy2Username);
        Enemies.Add(enemy3Id,enemy3Username);

        UIObjects.Instance.inGameUi.GetComponentInChildren<Canvas>().enabled = false;
        SceneManager.LoadScene(2);
    }

    private static void LoadScene(int index)
    {

        SceneManager.LoadScene(index);
    }
    private static void LoadScene(string index)
    {

        SceneManager.LoadScene(index);
    }
    public static void PlayerPosition(Packet _packet)
    {
        var whoToUpdate = _packet.ReadInt();
        if(whoToUpdate == ServerID) return;
        var position = _packet.ReadVector3();
        var rotation = _packet.ReadQuaternion();
        LoadingSceneScript.Manager.UpdatePosition(whoToUpdate, position, rotation);
    }
    public static void PlayerRotation(Packet _packet)
    {

        var id = _packet.ReadInt();
        var rotation = _packet.ReadQuaternion();

        GameManager.Players[id].transform.rotation = rotation;
    }
    public static void PlayerDisconnected(Packet _packet)
    {

        var id = _packet.ReadInt();
        //Destroy(GameManager.Players[id].gameObject);
        GameManager.Players.Remove(id);
    }
    public static void InvitationResponse(Packet _packet)
    {

        var from = _packet.ReadInt();
        var x = _packet.ReadBool();
        var name = _packet.ReadString();
        switch (x)
        {
            case true:
                //Invitation Accepted
                CreateGroup(from, name);
                Debug.Log("Invitation you sent to " + from + " has been accepted");
                break;
            case false:

                //Invitation Declined
                Debug.Log("Invitation you sent to " + from + " has been declined");
                break;
        }
    }
    public static void MMState(Packet _packet)
    {

        LookForGame.SetFGButtons();
    }
    public static void InteractableButtons(Packet _packet)
    {

        LookForGame.SetInteract();
    }
    public static void GoToScene(Packet _packet) {

        ExternalUIFunctions.GoToSceneStatic(_packet.ReadInt());
    }
    
    public static void HandlePickLobby(Packet _packet)
    {
        var fromID = _packet.ReadInt(); 
        var whatPick = _packet.ReadInt(); 

        Constants.LobbyManager.PickUpdate(fromID, whatPick);
    }


    public static void ReceivePlayerConnectionState(Packet _packet)
    {
        var whoToUpdate = _packet.ReadInt(); //IS THE ID OF THE PLAYER WE NEED TO UPDATE
        var lss = LoadingSceneScript;
        if (lss != null)
        {
            lss.UpdateConnectionStatus(whoToUpdate);
        }
    }
    

    public static void HandleLobbyEnd(Packet _packet)
    {
        var s = _packet.ReadInt();
        Constants.LobbyManager.EndLobby(s);
    }

    public static void RemoveCanvas(Packet _packet)
    {
        Constants.LoadingSceneScript.RemoveCanvas();
    }

    public static void HandleZoneUpdate(Packet _packet)
    {
        var z1 = _packet.ReadInt();
        var z2 = _packet.ReadInt();
        var z3 = _packet.ReadInt();
        var z4 = _packet.ReadInt();
        var z7 = _packet.ReadInt();
        if(LoadingSceneScript != null)
            LoadingSceneScript.SetText(z1,z2,z3,z4, z7);
    }

    public static void UpdateHealth(Packet _packet)
    {
        var whoToUpdate = _packet.ReadInt();
        var newHp = _packet.ReadFloat();
        if(LoadingSceneScript != null)
            LoadingSceneScript.Manager.GetPlayerStats(whoToUpdate).UpdateHealth(newHp);
    }

    public static void HandleDeath(Packet _packet)
    {
        var whoToDie = _packet.ReadInt();
        var deathTimer = _packet.ReadFloat();
        var deathCount = _packet.ReadInt();
        if (whoToDie == Constants.ServerID)
        {
            LoadingSceneScript.HandleLocalDeath(deathTimer, deathCount);
        }
        else
        {
            if (TeamMates.ContainsKey(whoToDie))
            {
                LoadingSceneScript.HandleTeamMateDeath(whoToDie,deathTimer, deathCount);
            }
            else
            {
                LoadingSceneScript.HandleEnemyDeath(whoToDie,deathTimer, deathCount);

            }
        }
    }

    public static void UpdateZoneState(Packet packet)
    {
        var zoneToUpdate = packet.ReadInt();
        var team = packet.ReadInt();
        LoadingSceneScript.UpdateZoneState(zoneToUpdate, team);
    }
    public static void SetMatchEndResult(Packet packet)
    {
        var whoWon = packet.ReadString(); //READS THE STRING REPRESENTING BLUE OR RED == TEAM
        var whoWonInt = whoWon == "Red" ? 1 : 2;
        var perfectScore = packet.ReadBool();
        MatchVariables.WhoWon = whoWonInt;
        if (whoWonInt == MyTeam) //LOCAL PLAYER HAS WON THE MATCH :)
        {
            SceneManager.LoadScene(sceneBuildIndex: 4);
        } else //LOCAL PLAYER HAS LOST THE MATCH :(
        {
            SceneManager.LoadScene(sceneBuildIndex: 4);
        }
    }

    public static void UpdateTutorialState(Packet _packet)
    {
        var newStage = _packet.ReadInt();
        TutorialManager.Instance.UpdateCurrentStage(newStage);
    }

    public static void LoadSceneForce(Packet _packet)
    {
        SceneManager.LoadScene(_packet.ReadInt());
    }

    public static void BeginTutorial(Packet _packet)
    {
        ExternalUIFunctions.GoToSceneStatic(5);
    }
}