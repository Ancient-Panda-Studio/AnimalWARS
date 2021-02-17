using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public void CallRegister()
    {
        StartCoroutine(RegisterStart());
    }

    private IEnumerator RegisterStart()
    {
        var form = new WWWForm();
        form.AddField("user", UIObjects.Instance.Register_usernameInputField.text);
        form.AddField("pass", UIObjects.Instance.Register_passwordInputField.text);
        form.AddField("email", UIObjects.Instance.Register_mailField.text);
        using (var www = UnityWebRequest.Post(Constants.WebServer + "register.php", form))
        {
            yield return www.SendWebRequest();
            if (www.downloadHandler.text == "0")
            {
                Debug.Log("User created successfully");
            }
            else
            {
                UIObjects.Instance.errorText.gameObject.SetActive(true);
                UIObjects.Instance.errorText.text = www.downloadHandler.text;
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public void VerifyInput()
    {
        UIObjects.Instance.errorText.gameObject.SetActive(false);
        if (UIObjects.Instance.Register_usernameInputField.text.Contains(" ") || UIObjects.Instance.Register_passwordInputField.text.Contains(" "))
        {
            //Not allow because of spaces
            UIObjects.Instance.errorText.gameObject.SetActive(true);
            UIObjects.Instance.errorText.text = "Please do not use SPACES";
            UIObjects.Instance.submitButton.interactable = false;
        }
        else if (string.Equals(UIObjects.Instance.Register_usernameInputField.text, UIObjects.Instance.Register_passwordInputField.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            UIObjects.Instance.errorText.gameObject.SetActive(true);
            UIObjects.Instance.errorText.text = "User and Password cannot be equal";
            UIObjects.Instance.submitButton.interactable = false;
        }
        else
        {
            UIObjects.Instance.submitButton.interactable = UIObjects.Instance.Register_usernameInputField.text.Length >= 8 && UIObjects.Instance.Register_passwordInputField.text.Length >= 8 && IsValidEmail(UIObjects.Instance.Register_mailField.text);
        }
    }

    private bool IsValidEmail(string mail)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(mail);
            return addr.Address == mail;
        }
        catch
        {
            Debug.Log("Invalid Email");
            return false;
        }
    }
}