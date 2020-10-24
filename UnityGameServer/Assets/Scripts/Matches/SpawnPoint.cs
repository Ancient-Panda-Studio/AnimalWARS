using UnityEngine;

public class SpawnPoint:MonoBehaviour
{
    [SerializeField]
    public bool _full = false;
    public int myTeam;
    public GameObject myGameObject;

    public void SetFull(bool _newValue) { _full = _newValue; }

    public bool GetFull() { return _full; }
}