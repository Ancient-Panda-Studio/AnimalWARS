using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public InputField mailField;
    public Button submitButton;
    public Text errorText;


    public void CallRegister()
    {
        StartCoroutine(RegisterStart());
    }

    private IEnumerator RegisterStart()
    {
        var form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("pass", passwordField.text);
        form.AddField("email", mailField.text);
        using (var www = UnityWebRequest.Post(Constants.WebServer + "register.php", form))
        {
            yield return www.SendWebRequest();
            if (www.downloadHandler.text == "0")
            {
                Debug.Log("User created successfully");
            }
            else
            {
                errorText.gameObject.SetActive(true);
                errorText.text = www.downloadHandler.text;
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public void VerifyInput()
    {
        errorText.gameObject.SetActive(false);
        if (usernameField.text.Contains(" ") || passwordField.text.Contains(" "))
        {
            //Not allow because of spaces
            errorText.gameObject.SetActive(true);
            errorText.text = "Please do not use SPACES";
            submitButton.interactable = false;
        }
        else if (string.Equals(usernameField.text, passwordField.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "User and Password cannot be equal";
            submitButton.interactable = false;
        }
        else
        {
            submitButton.interactable = usernameField.text.Length >= 8 && passwordField.text.Length >= 8 && IsValidEmail(mailField.text);
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