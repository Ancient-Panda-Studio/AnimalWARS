using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostMatchHandler : MonoBehaviour
{
    public Text resultTxt;
    private void Start()
    {
        if (MatchVariables.WhoWon == MatchVariables.MatchId)
        {
            resultTxt.color = new Color(192, 62, 46, 1);
            resultTxt.text = $"YOU\nWON :)";
        }
        else
        {
            resultTxt.color = new Color(62, 192, 46, 1);
            resultTxt.text = $"YOU\nLOSE :(";
        }
        Invoke("GoToScene1", 5);
    }

    private void GoToScene1()
    {
        SceneManager.LoadScene(1);
    }
}
