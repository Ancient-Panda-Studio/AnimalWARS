using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Network;
using UnityEngine.UI;

public class ExternalUIFunctions : MonoBehaviour
{

    public enum SceneToLoad
    {
        Credit = 0,
        MainMenu = 1,
        Game = 2
    }
    

    public void AnswerMatchFound(bool answer) //This is called from a button in the pop up either ACCEPT OR DECLINE
    {
        hasAnswered = true;
        if (answer)
        {
            UIObjects.Instance.matcfoundAcceptButton.transform.GetComponent<Image>().color = new Color(1, 152, 117, 1);
        }
        if (!answer)
        {
            UIObjects.Instance.MatchFoundUI.GetComponent<Canvas>().enabled = false;
            ClientHandle.MMState(new Packet());
        }
        ClientSend.MatchFoundAnswer(answer);
    }
    public void InvitationHandler(bool x)
    {
        ClientSend.SendInviteResponse(x, Constants.Inviter);
        UIObjects.Instance.inviteUI.SetActive(false);
        Constants.Inviter = null;
        Constants.InParty = x;
    }
    public void ToggleLogin()
    {
        UIObjects.Instance.loginMenu.SetActive(true);
        UIObjects.Instance.loginButton.interactable = true;
        UIObjects.Instance.registerMenu.SetActive(false);
    }
    public void ToggleRegister()
    {
        UIObjects.Instance.loginMenu.SetActive(false);
        UIObjects.Instance.registerMenu.SetActive(true);
    }
    public void ToggleVideoSettings(bool x)
    {
        UIObjects.Instance.videoSettingsUI.SetActive(x);
    }
    public void ToggleAudioSettings()
    {
        UIObjects.Instance.audioSettingsUI.SetActive(UIObjects.Instance.audioSettingsUI.activeSelf);
    }
    public void ToggleInterfaceSettings()
    {
        UIObjects.Instance.interfaceSettingsUI.SetActive(UIObjects.Instance.interfaceSettingsUI.activeSelf);
    }
    public void ToggleBindsSettings()
    {
        UIObjects.Instance.bindsSettingsUI.SetActive(UIObjects.Instance.bindsSettingsUI.activeSelf);
    }
    public void ToggleAccountSettings()
    {
        UIObjects.Instance.accountSettingsUI.SetActive(UIObjects.Instance.accountSettingsUI.activeSelf);
    }
    public void ToggleFriend(){
        if(!UIObjects.Instance.friendSuperHolder.activeSelf) {
            UIObjects.Instance.nextToCloseIs = 2;
            UIObjects.Instance.nextToClose = UIObjects.Instance.friendSuperHolder;
        }
        UIObjects.Instance.friendSuperHolder.SetActive(!UIObjects.Instance.friendSuperHolder.activeSelf);
    }
    public void ToggleSettings()
    {
        UIObjects.Instance._beforeSettings = 0;
        if (UIObjects.Instance.loginMenu.activeSelf)
        {
            UIObjects.Instance.loginMenu.SetActive(false);
            UIObjects.Instance._beforeSettings = 1;
        }

        if (UIObjects.Instance.registerMenu.activeSelf)
        {
            UIObjects.Instance.registerMenu.SetActive(false);
            UIObjects.Instance._beforeSettings = 2;
        }

        if (UIObjects.Instance.resetToDefaultUi.activeSelf)
        {
            UIObjects.Instance.resetToDefaultUi.SetActive(false);
        }
        var canvas = UIObjects.Instance.settingsMenu.GetComponent<Canvas>();
        UIObjects.Instance.nextToCloseIs = 1;
        UIObjects.Instance.nextToCloseCanvas = canvas;
        canvas.enabled = !canvas.enabled;
        //UIObjects.Instance.settingsMenu.SetActive(!UIObjects.Instance.settingsMenu.activeSelf);
        if (!UIObjects.Instance.settingsMenu.activeSelf) return;
        switch (UIObjects.Instance._beforeSettings)
        {
            case 1:
                UIObjects.Instance.loginMenu.SetActive(true);
                break;
            case 2:
                UIObjects.Instance.registerMenu.SetActive(true);
                break;
        }
    }

    public void TogglePlaySelector()
    {
        if(!UIObjects.Instance.PlaySelector.activeSelf) 
            UIObjects.Instance.PlaySelector.SetActive(true);
    }
    public void GoToScene(int x)
    {
        UIObjects.Instance.inGameUi.SetActive(false);
        SceneManager.LoadScene(x, LoadSceneMode.Single);
    }
    public static void GoToSceneStatic(int x)
    {
        UIObjects.Instance.inGameUi.SetActive(false);
        SceneManager.LoadScene(x, LoadSceneMode.Single);
    }
    public void GoToGame()
    {
        SceneManager.LoadScene(2);
    }
    public void ToggleStore()
    {
        if(!UIObjects.Instance.storeUI.activeSelf) {
            UIObjects.Instance.nextToClose = UIObjects.Instance.storeUI;
        }
        UIObjects.Instance.storeUI.SetActive(!UIObjects.Instance.storeUI.activeSelf);
    }
    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Loggin(){
        /*GameManager.Instance.isLogedIn = true;
        UIManager.AllowLogin();*/
        if(UIObjects.Instance.errorText.gameObject.activeSelf){
            UIObjects.Instance.errorText.gameObject.SetActive(false);
        }
        Constants.Username = UIObjects.Instance.Login_usernameInputField.text;
        UIObjects.Instance.loadingBarHolder_Login.SetActive(true);
        ClientSend.SendLoginInfo();
    }
    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
        if (activateDecline)
        {
            /*
             * HERE WE NEED A TIMER
             */
            if (_currentTimeForDecline > 0)
            {
                _currentTimeForDecline -= Time.deltaTime;
                //Update Text
                UIObjects.Instance.DeclineText.text = "Remaining... " + (int) _currentTimeForDecline;
                Debug.Log($"Timer -> {(int) _currentTimeForDecline} <-");
            }
            else
            {
                _currentTimeForDecline = 4f;
                LookForGame.SetFGButtons();
                if (!hasAnswered)
                {
                    ClientSend.MatchFoundAnswer(false);
                    Debug.Log("Match auto declined");
                }

                UIObjects.Instance.MatchFoundUI.GetComponent<Canvas>().enabled = false;
                activateDecline = false;
            }
               
        }

        UIObjects.Instance.loadingBarImage_Login.fillAmount += 0.005f;
        if (!(Math.Abs(UIObjects.Instance.loadingBarImage_Login.fillAmount - 1f) < 0.01f)) return;
        UIObjects.Instance.loadingBarImage_Login.fillClockwise = !UIObjects.Instance.loadingBarImage_Login.fillClockwise;
        UIObjects.Instance.loadingBarImage_Login.fillAmount = 0;
    }

    private void Start()
    {
        Debug.Log("Start");
        hasAnswered = false;
    }

    public static bool activateDecline = false;
    public static bool hasAnswered = false;
    private float _currentTimeForDecline = 4f;
}
