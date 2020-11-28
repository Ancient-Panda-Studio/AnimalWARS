using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDebug : MonoBehaviour
{
    public void StartMatch(){
        var x = new List<PlayerDataHolder>();
        for (int i = 0; i < 6; i++){
            var p = new PlayerDataHolder(i,$"user{(int)(i + Random.Range(1,25)) * Random.Range(1, 800) * .5f}");
            if(p.username == "user0"){
            Debug.Log($"{p.username}");
            }
        }
        HandleMatchMaking.GenerateMatch(x);
    }
}
