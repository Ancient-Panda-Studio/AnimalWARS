﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
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
        var www = new WWW(Constants.WebServer + "register.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("User created successfully");
        }
        else
        {
            errorText.gameObject.SetActive(true);
            errorText.text = www.text;
            Debug.Log(www.text);
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
            submitButton.interactable = usernameField.text.Length >= 8 && passwordField.text.Length >= 8;
        }
    }
}