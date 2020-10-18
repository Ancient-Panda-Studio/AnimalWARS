using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dictionaries
{
    public static readonly Dictionary<string, int> PlayersByName = new Dictionary<string, int>();
    public static readonly Dictionary<int, string> PlayersById = new Dictionary<int, string>();
    public static Dictionary<int, int> PartiesDictionary = new Dictionary<int, int>();
    public static readonly Dictionary<int, PlayerDataHolder> PlayerDataHolders = new Dictionary<int, PlayerDataHolder>();
    public static readonly Dictionary<int,Match> CurrentMatches = new Dictionary<int,Match>();
    public static readonly Dictionary<int,Map> Maps = new Dictionary<int,Map>();
}
