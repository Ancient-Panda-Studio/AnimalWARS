using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MonoDebug : MonoBehaviour
{
    public Text playerTexts;

    private int Count;
    private List<String> usersIn = new List<string>();

    private void Update()
    {
        if (Count >= Dictionaries.dictionaries.PlayerDataHolders.Count) return;
        foreach (var dataHolder in Dictionaries.dictionaries.PlayerDataHolders.Values.Where(dataHolder => !usersIn.Contains(dataHolder.Username)))
        {
            playerTexts.text += dataHolder.Username + "\n";
            Count++;
            usersIn.Add(dataHolder.Username);
        }
    }
}
