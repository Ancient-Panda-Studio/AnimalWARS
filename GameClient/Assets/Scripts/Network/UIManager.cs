using System.Collections.Generic;
using System.Net.NetworkInformation;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public GameObject loginMenu;
    public GameObject offline;
    public GameObject online;
    public Button loginButton;
    public GameObject registerMenu;
    public GameObject settingsMenu;
    public GameObject startUI;
    public GameObject inGameUi;
    public InputField usernameField;
    public Transform friendList;
    public InputField passwordField;
    public Text errorText;
    public TMP_Text friendText;
    public GameObject inviteUI;
    private int beforeSettings;
    private readonly Dictionary<int, string> partyMembers = new Dictionary<int, string>();
    public InputField friendInput;
    public Button addFriend;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
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
        beforeSettings = 0;
        if (loginMenu.activeSelf)
        {
            loginMenu.SetActive(false);
            beforeSettings = 1;
        }

        if (registerMenu.activeSelf)
        {
            registerMenu.SetActive(false);
            beforeSettings = 2;
        }

        settingsMenu.SetActive(!settingsMenu.activeSelf);
        if (!settingsMenu.activeSelf) return;
        switch (beforeSettings)
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
            var friend = Instantiate(Resources.Load("Prefabs/FriendTemplate") as GameObject, friendList, true);
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

    public void GetInvite(int id, string username, string _who)
    {
        friendText.text = _who;
        Constants.Inviter = _who;
        inviteUI.SetActive(true);
        Constants.InvitationID = id;
    }

    public void InvitationHandler(bool x)
    {
        ClientSend.SendInviteResponse(x, Constants.Inviter, Constants.InvitationID);
        inviteUI.SetActive(false);
    }

    public void VerifyFriendInput()
    {
        if (friendInput.text.Trim() == "")
        {
            addFriend.interactable = false;
        }
    }

    public void StartFriendAdd()
    {
        //TODO START FRIEND ADDING PROCESS 
    }
    public void CreateGroup(int _id, string _user)
    {
        if (partyMembers.Count > 5) Debug.Log("Party is full");
        if (partyMembers.Count == 0)
        {
            //NEW PARTY HAS BEEN CREATED

            //Show Party UI
        }
        else
        {
            partyMembers.Add(_id, _user);
        }
    }
}