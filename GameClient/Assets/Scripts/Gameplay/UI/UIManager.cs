using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("GAME OBJECTS")]
    public GameObject startMenu;
    public GameObject loginMenu;
    public GameObject offline;
    public GameObject online;
    public GameObject friendListHolder;
    public GameObject registerMenu;
    public GameObject settingsMenu;
    public GameObject startUI;
    public GameObject inGameUi;
    public GameObject inviteUI;
    public GameObject videoSettingsUI;
    public GameObject audioSettingsUI;
    public GameObject interfaceSettingsUI;
    public GameObject bindsSettingsUI;
    public GameObject accountSettingsUI;
    public GameObject resetToDefaultUi;
    public GameObject resetButton;
    public GameObject partyUI;
    
    [Header("TRANSFORMS")]
    public Transform friendList;
    [Header("BUTTONS")]
    public Button loginButton;
    public Button addFriend;
    [Header("INPUT FIELDS")]

    public InputField usernameField;
    public InputField friendInput;
    public InputField passwordField;
    [Header("TEXTS")]
    public Text errorText;
    public TMP_Text friendText;
    private int _beforeSettings;
    public  Dictionary<int, string> PartyMembers = new Dictionary<int, string>();

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public void ToggleResetToDefaultUi()
    {
        resetToDefaultUi.SetActive(!resetToDefaultUi.activeSelf);
        resetButton.GetComponent<Button>().interactable = false;
    }

    public void ResetSettings()
    {
        XMLDataManager.Instance.Entry.GenerateNewFile();
        resetToDefaultUi.SetActive(false);
        resetButton.GetComponent<Button>().interactable = true;
        XMLDataManager.Instance.Entry.DocumentHandler();
        XMLDataManager.Instance.Entry.ResetDropDown();

    }
    public void ToggleVideoSettings(bool x)
    {
        videoSettingsUI.SetActive(x);
    }
    public void ToggleAudioSettings()
    {
        audioSettingsUI.SetActive(audioSettingsUI.activeSelf);
    }
    public void ToggleInterfaceSettings()
    {
        interfaceSettingsUI.SetActive(interfaceSettingsUI.activeSelf);
    }
    public void ToggleBindsSettings()
    {
        bindsSettingsUI.SetActive(bindsSettingsUI.activeSelf);
    }
    public void ToggleAccountSettings()
    {
        accountSettingsUI.SetActive(accountSettingsUI.activeSelf);
    }
    private void Start()
    {
        Client.instance.ConnectToServer();
    }
    
    public void ToggleRegister()
    {
        loginMenu.SetActive(false);
        registerMenu.SetActive(true);
    }
    public void ToggleSettings()
    {
        _beforeSettings = 0;
        if (loginMenu.activeSelf)
        {
            loginMenu.SetActive(false);
            _beforeSettings = 1;
        }

        if (registerMenu.activeSelf)
        {
            registerMenu.SetActive(false);
            _beforeSettings = 2;
        }

        if (resetToDefaultUi.activeSelf)
        {
            resetToDefaultUi.SetActive(false);
        }
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        if (!settingsMenu.activeSelf) return;
        switch (_beforeSettings)
        {
            case 1:
                loginMenu.SetActive(true);
                break;
            case 2:
                registerMenu.SetActive(true);
                break;
        }
    }

    public void ToggleLogin()
    {
        loginMenu.SetActive(true);
        loginButton.interactable = true;
        registerMenu.SetActive(false);
    }

    public void SendLogin()
    {
        Constants.Username = usernameField.text;
        ClientSend.SendLoginInfo();
        Debug.Log(usernameField.text + "    ___" + passwordField.text);
    }

    public void CreateFriends(List<string> x, List<int> f)
    {
        for (var i = 0; i < x.Count; i++)
        {
            Debug.Log(x.Count);
            var friend = Instantiate(Resources.Load("Prefabs/FriendTemplate") as GameObject, friendList, false);
            var y = friend.GetComponent<FriendController>();
            y.name = x[i];
            y.playerID = f[i];
            y.connectionState = true;
        }
    }

    public void AllowLogin()
    {
        startUI.SetActive(false);
        registerMenu.SetActive(false);
        inGameUi.SetActive(true);
    }

    public void ForbidLogin(string _error)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = _error;
        loginButton.interactable = true;
    }

    public void GetInvite(int id, string _who)
    {
        friendText.text = _who;
        Constants.Inviter = _who;
        inviteUI.SetActive(true);
    }

    public void InvitationHandler(bool x)
    {
        ClientSend.SendInviteResponse(x, Constants.Inviter);
        inviteUI.SetActive(false);
        Constants.Inviter = null;
        Constants.InParty = x;
    }

    public void VerifyFriendInput()
    {
        if (friendInput.text.Trim() == "")
        {
            addFriend.interactable = false;
        }
    }

    public void OpenFriendsList()
    {
        friendListHolder.SetActive(!friendListHolder.activeSelf);
    }
    public void StartFriendAdd()
    {
        //TODO START FRIEND ADDING PROCESS 
    }
    public void CreateGroup(int _id, string _user)
    {
        if (PartyMembers.Count >= 3) Debug.Log("Party is full");
        if (PartyMembers.Count == 0)
        {
            //NEW PARTY HAS BEEN CREATED
            Constants.InParty = true;
            //Show Party UI
            partyUI.SetActive(true);
            
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

    public void DisbandParty()
    {
        
        partyUI.SetActive(false);
        Constants.InParty = false;
    }

    public void GeneratePartyVisual(string _user, int _id)
    {
        var partyMember = Instantiate(Resources.Load("Prefabs/PartyMember") as GameObject, partyUI.transform, false);
            var y = partyMember.GetComponent<PartyMemberInfoHolder>();
            y.nameText.text = _user;
            y.id = _id;
    }
}