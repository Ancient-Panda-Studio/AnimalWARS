using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoIExist5 : MonoBehaviour
{
    public static DoIExist5 Instance;
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
