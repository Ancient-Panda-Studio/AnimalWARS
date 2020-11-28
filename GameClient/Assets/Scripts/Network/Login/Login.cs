using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Network;

public class Login : MonoBehaviour
{

    public void CallLogin()
    {
        Constants.Username = UIObjects.Instance.Login_usernameInputField.text;
        ClientSend.SendLoginInfo();
    }

    public void VerifyInput()
    {
        UIObjects.Instance.errorText.gameObject.SetActive(false);
        if (UIObjects.Instance.Login_usernameInputField.text.Contains(" ") || UIObjects.Instance.Login_passwordInputField.text.Contains(" ") || UIObjects.Instance.Login_passwordInputField.text.Contains("    ") || UIObjects.Instance.Login_usernameInputField.text.Contains("    "))
        {
            //Not allow because of spaces
            UIObjects.Instance.errorText.gameObject.SetActive(true);
            UIObjects.Instance.errorText.text = "Please do not use SPACES";
            UIObjects.Instance.loginButton.interactable = false;
        }
        else
        {
            UIObjects.Instance.loginButton.interactable = UIObjects.Instance.Login_usernameInputField.text.Length >= 8 && UIObjects.Instance.Login_passwordInputField.text.Length >= 8;
        }
    }
}