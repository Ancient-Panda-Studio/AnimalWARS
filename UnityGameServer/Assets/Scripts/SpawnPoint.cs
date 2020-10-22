using UnityEngine;

public class SpawnPoint:MonoBehaviour
{
    private static bool _full;
    public int myTeam;
    public GameObject myGameObject;

    public static void SetFull(bool _newValue) { _full = _newValue; }

    public static bool GetFull() { return _full; }
}