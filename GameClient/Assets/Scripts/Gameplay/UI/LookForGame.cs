using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using UnityEngine.UI;

public class LookForGame : MonoBehaviour
{
    private static GameObject lfGameButton;
    private static GameObject stopLfGameButton;
    public GameObject FindGame;
    public GameObject StopGameFind;

    private void Start()
    {
        lfGameButton = FindGame;
        stopLfGameButton = StopGameFind;
    }
    public void StartLfGame()
    {
        ClientSend.AddMatchMaking();
    }
    public static void SetFGButtons()
    {
        lfGameButton.SetActive(!lfGameButton.activeSelf);
        stopLfGameButton.SetActive(!stopLfGameButton.activeSelf);
    }
    public void RemoveFromLfGame()
    {
        ClientSend.RemoveMatchMaking();
    }

    public static void SetInteract()
    {
        lfGameButton.GetComponent<Button>().interactable = false;
    }
}
