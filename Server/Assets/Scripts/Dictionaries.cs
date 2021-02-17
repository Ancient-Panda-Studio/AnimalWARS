using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dictionaries
{
    public static Dictionaries dictionaries;

    public static void InitializeDictionaries()
    {
        dictionaries = new Dictionaries();
    }
    public Dictionary<string, int> PlayersByName = new Dictionary<string, int>();
    public Dictionary<int, string> PlayersById = new Dictionary<int, string>();
    public Dictionary<int, int> PartiesDictionary = new Dictionary<int, int>();
    public Dictionary<int, PlayerDataHolder> PlayerDataHolders = new Dictionary<int, PlayerDataHolder>();
    public Dictionary<int,Map> Maps = new Dictionary<int,Map>();
    public Dictionary<int,Match> Matches = new Dictionary<int,Match>();
}