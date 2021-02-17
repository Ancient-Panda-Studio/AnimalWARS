using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ServerDebug : MonoBehaviour
{
    public static ServerDebug Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void StartMatch(){
        var x = new List<PlayerDataHolder>();
        for (var i = 0; i < 6; i++){
            var p = new PlayerDataHolder(i,$"user{(int)(i + Random.Range(1,25)) * Random.Range(1, 800) * .5f}");
            if(p.Username == "user0"){
                Debug.Log($"{p.Username}");
            }
        }
        HandleMatchMaking.GenerateMatch(x);
    }

    public void GOOOO(int _fromClient)
    {
        StartCoroutine(goInisdeMe(_fromClient));
    }

    public IEnumerator goInisdeMe(int _fromClient)
    {
        yield return new WaitForSecondsRealtime(3f);
        //ServerSend.StartMatch(_fromClient, 2);
    }
}

