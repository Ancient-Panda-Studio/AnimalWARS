using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DoNotDestroy : MonoBehaviour
{
     public List<GameObject> dontDestroyThis = new List<GameObject>();
     private void Awake()
     {
          foreach (var x in dontDestroyThis)
          {
               DontDestroyOnLoad(x);
          }
     }
}
