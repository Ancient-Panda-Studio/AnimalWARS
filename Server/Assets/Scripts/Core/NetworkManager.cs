using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public GameObject playerPrefab;
    private void Awake()
    {
        Dictionaries.InitializeDictionaries();
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Server.Start(50, 27017);
        ServerConsoleWriter.WriteLine($"Animal Wars Server Started ON : {DateTime.UtcNow}   PORT : {Server.Port}");
    }
    
    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public GameObject InstantiatePlayer(Transform _newPosition)
    {
        return Instantiate(playerPrefab, _newPosition.position, Quaternion.identity);
    }
}
