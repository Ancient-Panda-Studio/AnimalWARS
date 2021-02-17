using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

class ThreadManager : MonoBehaviour
{
    private static readonly List<Action> executeOnMainThread = new List<Action>();
    private static readonly List<Action> ExecuteCopiedOnMainThread = new List<Action>();
    private static bool _actionToExecuteOnMainThread = false;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Parser.PopulateMapDictionary();
    }
    
    private void FixedUpdate()
    {
        UpdateMain();
        /*WE CHECK IF THE MM IS POSSIBLE AND GENEREATE A MATCH*/
        var mm = HandleMatchMaking.CheckIfMatchMakingIsPossible();
        if (!HandleMatchMaking.Generating && mm != null)
        {
            HandleMatchMaking.GenerateMatch(mm);
        }
    }
    public static void ExecuteOnMainThread(Action action)
    {
        if (action == null)
        {
            Debug.Log("No action to execute on main thread!");
            return;
        }

        lock (executeOnMainThread)
        {
            executeOnMainThread.Add(action);
            _actionToExecuteOnMainThread = true;
        }
    }

    /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
    public static void UpdateMain()
    {
        if (_actionToExecuteOnMainThread)
        {
            ExecuteCopiedOnMainThread.Clear();
            lock (executeOnMainThread)
            {
                ExecuteCopiedOnMainThread.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                _actionToExecuteOnMainThread = false;
            }
            for (int i = 0; i < ExecuteCopiedOnMainThread.Count; i++)
            {
                ExecuteCopiedOnMainThread[i]();
            }
        }
    }
}
