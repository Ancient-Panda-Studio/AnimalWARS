using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCall : MonoBehaviour
{
    private static UpdateCall _instance = null;
    private void Awake()
    {
        if( _instance == null )
        {
            _instance = this;
            DontDestroyOnLoad( this );
        }
        else if( this != _instance )
            Destroy( this );
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void CheckMM()
    {
        var x = HandleMatchMaking.CheckIfMatchMakingIsPossible();
        if (x.Count != 0)
        {
            HandleMatchMaking.GenerateMatch(x);
        }
        
    }
}
