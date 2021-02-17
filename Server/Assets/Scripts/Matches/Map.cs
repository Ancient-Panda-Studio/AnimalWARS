using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private static int _mapId;
    private static string _mapName;
    public Map (int mapId, string mapName)
    {
        _mapId = mapId;
        _mapName = mapName;
    }
}
