using System.Collections.Generic;
public static class Dictionaries
{
    public static readonly Dictionary<string, int> PlayersByName = new Dictionary<string, int>();
    public static readonly Dictionary<int, string> PlayersById = new Dictionary<int, string>();
    public static Dictionary<int, int> PartiesDictionary = new Dictionary<int, int>();
    public static readonly Dictionary<int, PlayerDataHolder> PlayerDataHolders = new Dictionary<int, PlayerDataHolder>();
    public static readonly Dictionary<int,Map> Maps = new Dictionary<int,Map>();
    public static List<Match> matches = new List<Match>();
    public static readonly Dictionary<int,SpawnedMap> SpawnedMaps = new Dictionary<int,SpawnedMap>();
}