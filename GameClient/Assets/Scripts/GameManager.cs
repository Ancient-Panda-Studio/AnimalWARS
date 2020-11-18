using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static readonly Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();
    public GameObject localPlayerPrefab; //Model the client player will use
    public GameObject playerPrefab; //Model the client player will see other clients using
    public Dictionary<int,string> FriendsList = new Dictionary<int, string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        Debug.Log($"User is : {Constants.SysetmUser}");
    }

    public void SpawnPlayer(int id, Vector3 position, Quaternion rotation) 
    {
        
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        if (UIManager.Instance.inGameUi.activeSelf)
        {
            UIManager.Instance.inGameUi.SetActive(false);
            UIManager.Instance.matchStartedUi.SetActive(true);
        }

        Debug.Log($"A player with ID {id} is being spawned...");
        if (id == Constants.ServerID)
        {
            var localPlayerObj =  Instantiate(localPlayerPrefab, position, rotation);
            localPlayerObj.GetComponent<PlayerManager>().id = id;
            Players.Add(id, localPlayerObj.GetComponent<PlayerManager>());
        }
        else
        {
            var playerObj = Instantiate(playerPrefab, position, rotation);
            playerObj.GetComponent<PlayerManager>().id = id;
            Players.Add(id, playerObj.GetComponent<PlayerManager>());
        }
    }
}