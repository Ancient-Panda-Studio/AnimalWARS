using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class LookForGame : MonoBehaviour
{
    public GameObject lfGameButton;
    public GameObject stopLfGameButton;
    
    
    public void StartLfGame()
    {
        ClientSend.AddMatchMaking();
        lfGameButton.SetActive(false);
        stopLfGameButton.SetActive(true);
    }
    
    public void RemoveFromLfGame()
    {
        ClientSend.RemoveMatchMaking();
        lfGameButton.SetActive(true);
        stopLfGameButton.SetActive(false);
    }
}
