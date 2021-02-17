using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DoNotDestroy : MonoBehaviour
{
     public List<GameObject> dontDestroyThis = new List<GameObject>();
     public bool PhaseScene = false;
     public int SceneToPhase;
     private void Awake()
     {
          foreach (var x in dontDestroyThis){
               DontDestroyOnLoad(x);
          }
     if(!PhaseScene) return;
     SceneManager.LoadScene(SceneToPhase);
     }
}
