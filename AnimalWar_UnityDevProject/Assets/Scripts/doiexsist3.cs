using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doiexsist3 : MonoBehaviour
{
   public static doiexsist3 Instance;
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
