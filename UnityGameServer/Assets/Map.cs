using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Map
{
    private static int _mapId;
    private static string _mapName;


    protected Map (int mapId, string mapName)
    {
        _mapId = mapId;
        _mapName = mapName;
    }
}
