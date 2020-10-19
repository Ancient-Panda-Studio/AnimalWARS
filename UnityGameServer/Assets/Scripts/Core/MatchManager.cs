using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class MatchManager : MonoBehaviour //This Script Is Attached to an empty object within THE MAP 1 of this is generated for each map spawn
{
    private static Match _myMatch;

    public MatchManager(Match match)
    {
        _myMatch = match;
    }
}
