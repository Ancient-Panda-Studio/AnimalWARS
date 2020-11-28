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
    public bool isLogedIn = false;
    private float Counter = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        Debug.Log("-> Logged in -> " + isLogedIn);
        if(isLogedIn){
           UIManager.AllowLogin();
        }
        Debug.Log($"User is : {Constants.SysetmUser}");
    }


    private void Start() {
        if(!isLogedIn){
                Client.instance.ConnectToServer();
        }
         if(isLogedIn){
           UIManager.AllowLogin();
           Cursor.lockState = CursorLockMode.None;
           Cursor.visible = true;
        }
    }

    private void Update() {
      if(isLogedIn && UIObjects.Instance.mainMenuBig.activeSelf) {
          UIManager.AllowLogin();
          Debug.Log("ALLOW LOGIN");
      }
    }
    public void SpawnPlayer(int id, Vector3 position, Quaternion rotation) 
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        if (UIObjects.Instance.inGameUi.activeSelf)
        {
            UIObjects.Instance.inGameUi.SetActive(false);
            UIObjects.Instance.matchStartedUi.SetActive(true);
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