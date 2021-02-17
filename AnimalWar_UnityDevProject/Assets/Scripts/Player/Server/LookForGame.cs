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
    
    private void Start()
    {
        lfGameButton = UIObjects.Instance.FindGame;
        stopLfGameButton = UIObjects.Instance.StopGameFind;
    }
    public void StartLfGame(int mapId)
    {
        ClientSend.AddMatchMaking(mapId);
    }
    public static void SetFGButtons()
    {
        lfGameButton.SetActive(!lfGameButton.activeSelf);
        stopLfGameButton.SetActive(!stopLfGameButton.activeSelf);
        if(UIObjects.Instance.PlaySelector.activeSelf)
            UIObjects.Instance.PlaySelector.SetActive(false);
    }
    public void RemoveFromLfGame()
    {
        ClientSend.RemoveMatchMaking();
        if(UIObjects.Instance.PlaySelector.activeSelf)
            UIObjects.Instance.PlaySelector.SetActive(false);
    }

    public static void SetInteract()
    {
        lfGameButton.GetComponent<Button>().interactable = false;
    }
}
