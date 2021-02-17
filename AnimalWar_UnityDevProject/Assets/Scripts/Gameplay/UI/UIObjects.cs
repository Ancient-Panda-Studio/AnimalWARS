using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
/*THIS CLASS WILL HOLD REFERENCES TO ALL UI ELEMENTS WITHIN THE MENUS OF THE GAME
    FUNCTIONS RELATED TO THIS OBJECTS WILL BE CALLED FROM STATIC CLASS UiMANAGER
*/

public class UIObjects : MonoBehaviour
{
    public int nextToCloseIs = 0; //0 = none 1 = canvas 2 = gameobject
    public static UIObjects Instance;
    public List<AudioSource> audiosToEnable = new List<AudioSource>();
    [HideInInspector]
    public GameObject nextToClose;
    [HideInInspector]
    public Canvas nextToCloseCanvas;
    public GameObject loadingBarHolder_Login;
    public GameObject mainMenuBig;
    public GameObject mainMenuEmptyHolder;
    public GameObject loginMenu;
    public GameObject registerMenu;
    public AudioClip mayaClip;
    public GameObject offline;
    public GameObject online;
    public Button loginButton;
    public Button submitButton;
    public Text errorText;
    public InputField Login_usernameInputField;
    public InputField Login_passwordInputField;
    public GameObject friendSuperHolder;
    public InputField Register_usernameInputField;
    public InputField Register_passwordInputField;
    public InputField Register_mailField;
    public GameObject friendListHolder;
    public GameObject settingsMenu;
    public Image loadingBarImage_Login;
    public GameObject inGameUi;
    public GameObject inviteUI;
    public GameObject videoSettingsUI;
    public GameObject audioSettingsUI;
    public GameObject storeUI;
    public GameObject interfaceSettingsUI;
    public GameObject bindsSettingsUI;
    public GameObject accountSettingsUI;
    public GameObject resetToDefaultUi;
    public GameObject resetButton;
    public GameObject partyUI;
    public GameObject matchStartedUi;
    public GameObject MatchFoundUI;
    public GameObject PlaySelector;
    public Transform friendList;
    public Button addFriend;
    public Text friendInput;
    public TMP_Text friendText;
    public GameObject FindGame;
    public GameObject StopGameFind;
    public int _beforeSettings = 0;
    public Dictionary<int, string> PartyMembers = new Dictionary<int, string>();
    public Text DeclineText;
    public GameObject matcfoundAcceptButton;

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
    private void Update()
    {        
    /*if(SceneManager.GetActiveScene().buildIndex != 1){
            foreach (var a in audiosToEnable) {
                if(!a.enabled) continue;
                a.enabled = false;
            }
        } else {
            foreach (var a in audiosToEnable) {
                if(a.enabled) continue;
                a.enabled = true;
            }
        }

        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        switch (nextToCloseIs)
        {
            case 0:

            break;
            case 1:
            nextToCloseCanvas.enabled = false;
            break;
            case 2:
            nextToClose.SetActive(false);
            break;
        }*/
    }

}
