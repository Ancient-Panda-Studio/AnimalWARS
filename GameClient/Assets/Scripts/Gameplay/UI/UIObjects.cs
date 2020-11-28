using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
/*THIS CLASS WILL HOLD REFERENCES TO ALL UI ELEMENTS WITHIN THE MENUS OF THE GAME
    FUNCTIONS RELATED TO THIS OBJECTS WILL BE CALLED FROM STATIC CLASS UiMANAGER
*/
public class UIObjects : MonoBehaviour
{
    public static UIObjects Instance;

    /*RN I'M GONNA REORDER EVERYTHING HERE*/
    [FoldoutGroup("GENERAL")]
    public GameObject mainMenuBig;
    [FoldoutGroup("GENERAL")]
    public GameObject mainMenuEmptyHolder   ;
    [FoldoutGroup("LOGIN")]
    public GameObject loginMenu;
    [FoldoutGroup("REGISTER")]
    public GameObject registerMenu;
    [FoldoutGroup("GENERAL")]
    public GameObject offline;
    [FoldoutGroup("GENERAL")]
    public GameObject online;
    [FoldoutGroup("LOGIN")]
    public Button loginButton;
    [FoldoutGroup("REGISTER")]
    public Button submitButton;
    [FoldoutGroup("GENERAL")]
    public Text errorText;
    [FoldoutGroup("LOGIN")]
    public InputField Login_usernameInputField;
    [FoldoutGroup("LOGIN")]
    public InputField Login_passwordInputField;
    [FoldoutGroup("REGISTER")]
    public InputField Register_usernameInputField;
    [FoldoutGroup("REGISTER")]
    public InputField Register_passwordInputField;
    [FoldoutGroup("REGISTER")]
    public InputField Register_mailField;
    [FoldoutGroup("FRIENDS")]
    public GameObject friendListHolder;
    [FoldoutGroup("SETTINGS")]
    public GameObject settingsMenu;
    [FoldoutGroup("GENERAL")]
    public GameObject inGameUi;
    [FoldoutGroup("PARTIES")]
    public GameObject inviteUI;
    [FoldoutGroup("SETTINGS")]
    public GameObject videoSettingsUI;
    [FoldoutGroup("SETTINGS")]
    public GameObject audioSettingsUI;
    [FoldoutGroup("SETTINGS")]
    public GameObject storeUI;
    [FoldoutGroup("SETTINGS")]
    public GameObject interfaceSettingsUI;
    [FoldoutGroup("SETTINGS")]
    public GameObject bindsSettingsUI;
    [FoldoutGroup("SETTINGS")]
    public GameObject accountSettingsUI;
    [FoldoutGroup("SETTINGS")]
    public GameObject resetToDefaultUi;
    [FoldoutGroup("SETTINGS")]
    public GameObject resetButton;
    [FoldoutGroup("PARTIES")]
    public GameObject partyUI;
    [FoldoutGroup("MATCH")]
    public GameObject matchStartedUi;
    [FoldoutGroup("MATCH")]
    public GameObject MatchFoundUI;
    [FoldoutGroup("FRIENDS")]
    public Transform friendList;
    [FoldoutGroup("FRIENDS")]
    public Button addFriend;
    [FoldoutGroup("FRIENDS")]
    public Text friendInput;
    [FoldoutGroup("FRIENDS")]
    public TMP_Text friendText;
    [FoldoutGroup("MATCH")]
    public GameObject FindGame;
    [FoldoutGroup("MATCH")]
    public GameObject StopGameFind;
    [FoldoutGroup("SETTINGS")]
    public int _beforeSettings = 0;
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

}
