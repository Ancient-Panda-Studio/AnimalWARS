using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Network;
public class ExternalUIFunctions : MonoBehaviour
{

    public enum SceneToLoad
    {
        Credit = 0,
        MainMenu = 1,
        Game = 2
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
        UIObjects.Instance.settingsMenu.SetActive(!UIObjects.Instance.settingsMenu.activeSelf);
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
    public void GoToScene(int x)
    {
      SceneManager.LoadScene(x,LoadSceneMode.Single);
    }

    public void GoToGame(){
        SceneManager.LoadScene(2);
    }

    public void ToggleStore(){
                UIObjects.Instance.storeUI.SetActive(!UIObjects.Instance.storeUI.activeSelf);
    }   
    public void OpenUrl(string url){
        Application.OpenURL(url);
    }
    public void ExitGame(){
        Application.Quit();
    }
    public void Loggin(){
        GameManager.Instance.isLogedIn = true;
        UIManager.AllowLogin();
    }
}
