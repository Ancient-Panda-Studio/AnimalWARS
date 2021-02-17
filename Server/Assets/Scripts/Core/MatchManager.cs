using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class MatchManager : MonoBehaviour //This Script Is Attached to an empty object within THE MAP 1 of this is generated for each map spawn
{
    public static MatchManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(UpdateGamePlays());
        StartCoroutine(FastUpdateGamePlays());
        StartCoroutine(UpdateTimers());
        //StartCoroutine(UpdateTutorialFast());
        Invoke("CheckWinners", 30);
    }

    private IEnumerator UpdateTutorialFast()
    {
        yield return new WaitForSeconds(.1f);
        foreach (var match in Dictionaries.dictionaries.Matches.Values.ToList().Where(match => match.matchType == MatchType.Tutorial))
        {
            match.myGamePLay?.UpdateTutorial();
        }

        StartCoroutine(UpdateTutorialFast());    }

    private IEnumerator CheckWinners() //CHECKS EVERY 15 SECONDS FOR A WINNER
    {
        yield return new WaitForSeconds(15f);
        foreach (var match in Dictionaries.dictionaries.Matches.Values.ToList().Where(match => match.matchType != MatchType.Tutorial))
        {
            match.myGamePLay?.SetWinner();
        }

        StartCoroutine(CheckWinners());
    }
    private IEnumerator UpdateTimers() //HANDLES INTERNAL TIMERS ON THE MATCH AS WELL AS THE BIG 25min timer
    {
        yield return new WaitForSeconds(.9f);
        foreach (var match in Dictionaries.dictionaries.Matches.Values.ToList().Where(match => match.matchType != MatchType.Tutorial))
        {
            match.myGamePLay?.UpdateTimer();
        }

        StartCoroutine(UpdateTimers());
    }
    private IEnumerator FastUpdateGamePlays() //HEALTH DEATHS ARE HANDLED HERE
    {
        yield return new WaitForSeconds(.5f);
        foreach (var match in Dictionaries.dictionaries.Matches.Values.ToList().Where(match => match.matchType != MatchType.Tutorial))
        {
            match.myGamePLay?.FastUpdateGamePlay();
        }

        StartCoroutine(FastUpdateGamePlays());
    }
    private void Update()
    {
        foreach (var x in Dictionaries.dictionaries.Matches.Values.ToList().Where(match => match.matchType != MatchType.Tutorial))
        {
            if (x.hasEnded && x.HasAccepted.Count == 6)
            {
                x.MatchLobby.SpawnPlayers();
      
            }
            else if(x.hasEnded)
            { continue; }
            else
            {
                if (x._creationTime <= Time.time)
                {
                    /*
                        LOBBY END
                     */
                    x.MatchLobby.EndLobby();
                }   
            }
        
        }
    }

    private IEnumerator  UpdateGamePlays()
    { 
        yield return new WaitForSecondsRealtime(3f);
        foreach (var match in Dictionaries.dictionaries.Matches.Values.ToList().Where(match => match.matchType != MatchType.Tutorial))
        {
            match.myGamePLay?.UpdateGamePlay();
        }

        StartCoroutine(UpdateGamePlays());
    }
}
