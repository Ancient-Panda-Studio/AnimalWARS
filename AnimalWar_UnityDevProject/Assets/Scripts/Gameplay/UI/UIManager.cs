using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class UIManager
{
    public static Dictionary<int, string> PartyMembers = new Dictionary<int, string>();
    public static string UwU = "LOLXD";
    public static void ToggleResetToDefaultUi()
    {
        UIObjects.Instance.resetToDefaultUi.SetActive(! UIObjects.Instance.resetToDefaultUi.activeSelf);
        UIObjects.Instance.resetButton.GetComponent<Button>().interactable = false;
    }
    public static void ResetSettings()
    {
        XMLDataManager.Instance.Entry.GenerateNewFile();
        UIObjects.Instance.resetToDefaultUi.SetActive(false);
        UIObjects.Instance.resetButton.GetComponent<Button>().interactable = true;
        XMLDataManager.Instance.Entry.DocumentHandler();
        XMLDataManager.Instance.Entry.ResetDropDown();

    }
    public static void SendLogin()
    {
        Constants.Username = UIObjects.Instance.Login_usernameInputField.text;
        ClientSend.SendLoginInfo();
    }

    public static void CreateFriends(List<string> x, List<int> f)
    {
        for (var i = 0; i < x.Count; i++)
        {
            var friend = GameObject.Instantiate(Resources.Load("Prefabs/FriendTemplate") as GameObject, UIObjects.Instance.friendList, false);
            var y = friend.GetComponent<FriendController>();
            y.name = x[i];
            y.playerID = f[i];
            y.connectionState = true;
        }
    }

    public static void AllowLogin()
    {
        UIObjects.Instance.mainMenuBig.SetActive(false);
        UIObjects.Instance.registerMenu.SetActive(false);
        UIObjects.Instance.inGameUi.SetActive(true);
    }

    public static void ForbidLogin(string _error)
    {
        UIObjects.Instance.errorText.gameObject.SetActive(true);
        UIObjects.Instance.errorText.text = _error;
        UIObjects.Instance.loginButton.interactable = true;
    }

    public static void GetInvite(int id, string _who)
    {
        UIObjects.Instance.friendText.text = _who;
        Constants.Inviter = _who;
        UIObjects.Instance.inviteUI.SetActive(true);
    }


    public static void VerifyFriendInput()
    {
        if (UIObjects.Instance.friendInput.text.Trim() == "")
        {
            UIObjects.Instance.addFriend.interactable = false;
        }
    }

    public static void OpenFriendsList()
    {
        UIObjects.Instance.friendListHolder.SetActive(!UIObjects.Instance.friendListHolder.activeSelf);
    }
    public static void StartFriendAdd()
    {
        //TODO START FRIEND ADDING PROCESS
    }
    public static void CreateGroup(int _id, string _user)
    {
     if (PartyMembers.Count >= 3) Debug.Log("Party is full");
        if (PartyMembers.Count == 0)
        {
            //NEW PARTY HAS BEEN CREATED
            Constants.InParty = true;
            //Show Party UI
            UIObjects.Instance.partyUI.SetActive(true);
            PartyMembers.Add(_id,_user);
            GeneratePartyVisual(Constants.Username, Constants.ServerID);
            PartyMembers.Add(Constants.ServerID,Constants.Username);
            GeneratePartyVisual(_user, _id);
        }
        else
        {
            PartyMembers.Add(_id, _user);
            GeneratePartyVisual(_user, _id);
        }
    }

    public static void DisbandParty()
    {
        UIObjects.Instance.partyUI.SetActive(false);
        Constants.InParty = false;
    }
    public static void GeneratePartyVisual()
    {
        foreach (var member in PartyMembers) {
            var partyMember = GameObject.Instantiate(Resources.Load("Prefabs/PartyMember") as GameObject, UIObjects.Instance.partyUI.transform, false);
            var y = partyMember.GetComponent<PartyMemberInfoHolder>();
            y.nameText.text = member.Value;
            y.id = member.Key;
        }
    }
    public static void GeneratePartyVisual(string _user, int _id)
    {
        var partyMember = GameObject.Instantiate(Resources.Load("Prefabs/PartyMember") as GameObject, UIObjects.Instance.partyUI.transform, false);
            var y = partyMember.GetComponent<PartyMemberInfoHolder>();
            y.nameText.text = _user;
            y.id = _id;
    }
    
}