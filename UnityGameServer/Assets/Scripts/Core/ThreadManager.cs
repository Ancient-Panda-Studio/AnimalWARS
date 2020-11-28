using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

class ThreadManager : MonoBehaviour
{
    private static readonly List<Action> executeOnMainThread = new List<Action>();
    private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
    private static bool actionToExecuteOnMainThread = false;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Parser.PopulateMapDictionary();
    }

    /// <summary>Sets an action to be executed on the main thread.</summary>
    /// <param name="_action">The action to be executed on the main thread.</param>
    private void FixedUpdate()
    {
        UpdateMain();
        /*WE CHECK IF THE MM IS POSSIBLE AND GENEREATE A MATCH*/
        var mm = HandleMatchMaking.CheckIfMatchMakingIsPossible();
        if (!HandleMatchMaking.Generating && mm != null)
        {
            HandleMatchMaking.GenerateMatch(mm);
        }
        /*PROCESS THIS IF THERE IS ATLEAST 1 MATCH TO CHECK*/
        if(Dictionaries.matches.Count != 0){
            for (int i = 0; i < Dictionaries.matches.Count; i++){
                if(Dictionaries.matches[i].IsTime() && !Dictionaries.matches[i].HasStarted()){
                    if(!Dictionaries.matches[i].Verify()){
                        Debug.Log(false);
                        Dictionaries.matches.RemoveAt(i);
                    } else {
                        Dictionaries.matches[i].Begin();
                    }
                }
            }
        }

    }
    public static void ExecuteOnMainThread(Action _action)
    {
        if (_action == null)
        {
            Debug.Log("No action to execute on main thread!");
            return;
        }

        lock (executeOnMainThread)
        {
            executeOnMainThread.Add(_action);
            actionToExecuteOnMainThread = true;
        }
    }

    /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
    public static void UpdateMain()
    {
        if (actionToExecuteOnMainThread)
        {
            executeCopiedOnMainThread.Clear();
            lock (executeOnMainThread)
            {
                executeCopiedOnMainThread.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }
            for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
            {
                executeCopiedOnMainThread[i]();
            }
        }
    }
}
