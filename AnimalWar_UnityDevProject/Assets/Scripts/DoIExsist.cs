using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoIExsist : MonoBehaviour
{
    public static DoIExsist Instance;
      private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this.gameObject);
            Destroy(this);
        }
    }
}
