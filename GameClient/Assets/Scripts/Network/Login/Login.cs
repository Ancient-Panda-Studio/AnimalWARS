using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Button submitButton;
    public Text errorText;

    public void CallLogin()
    {
        StartCoroutine(LoginStart());
    }

    private IEnumerator LoginStart()
    {
        var form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("pass", passwordField.text);
        var www = new WWW(Constants.WebServer + "login.php", form);
        yield return www;
        if (www.text[0] == '0')
        {
            //Login
            Constants.Username = usernameField.text;
            Constants.Score = int.Parse(www.text.Split('\t')[1]);
            //Client.instance.ConnectToServer();
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
        else
        {
            submitButton.interactable = usernameField.text.Length >= 8 && passwordField.text.Length >= 8;
        }
    }
}