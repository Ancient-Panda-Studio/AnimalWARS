using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static readonly Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();
    public GameObject localPlayerPrefab; //Model the client player will use
    public GameObject playerPrefab; //Model the client player will see other clients using
    public Dictionary<int,string> FriendsList = new Dictionary<int, string>();
    public AudioSource musicSource;
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

       /* if (Constants.LobbyManager != null && SceneManager.GetActiveScene().buildIndex != 2)
        {
            SceneManager.LoadScene(2);
        }*/
        
        Debug.Log("-> Logged in -> " + Constants.IsLogged);
        if(Constants.IsLogged){
           UIManager.AllowLogin();
        }
        Debug.Log($"User is : {Constants.SysetmUser}");
    }

    public void GoToMap()
    {
        ClientHandle.HandleLobbyPacket(null);

    }

    private void Start() {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (!Constants.IsLogged){
                Client.instance.ConnectToServer();
        }
        if (!Constants.IsLogged) return;
        /*WE NEED TO REPOPULATE PARTY*/
         if(Constants.InParty) {
             UIManager.GeneratePartyVisual();
         }
         UIManager.AllowLogin();
    }
    private void Update() {
        /*if(Constants.isLogged && !UIObjects.Instance.inGameUi.activeSelf && SceneManager.GetActiveScene().buildIndex == 1){
                UIObjects.Instance.inGameUi.SetActive(true);
        }*/
        if(Constants.IsLogged && UIObjects.Instance.mainMenuBig.activeSelf) {
          UIManager.AllowLogin();
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