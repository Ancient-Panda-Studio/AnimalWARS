using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;
using UnityEngine;
using UnityEngine.UI;
using static Constants;
using static MatchVariables;

public partial class LoadingScene : MonoBehaviour
{
    public Canvas loadingCanvas;

    public Text[] teamTexts = new Text[3];
    public Text[] enemiesTexts = new Text[3];
    private Dictionary<int, Image> _connections = new Dictionary<int, Image>();
    private List<Image> conSprites = new List<Image>();
    public List<GameObject> conGameObj = new List<GameObject>();
    public GameObject[] localsObj;
    public GameObject[] teamMatesObj;
    public GameObject[] enemiesObj;
    public Slider localHpSlider;
    public Text localHpText;
    public AztecManager Manager;
    public Text[] texts;
    public Slider zoneSlider;
    private int _spawnedModels = 0;
    private Vector3 SpawnPoint;
    public Text AlertText;
    public Text TimerTxt;
    public int CurrentTime = 1500;
    [Header("TopBar Red")]     public GameObject TeamRed;
    public Text ZonesRed;
     public Text DeathsRed;
     public Text KillsRed;
     [Header("TopBar Blue")]     public GameObject TeamBlue;

    public Text ZonesBlue;
     public Text DeathBlue;
     public Text KillsBlue;

     [Header("PostProcessings")] public GameObject postProcessAlive;
     public GameObject deadPostProcess;

     private Dictionary<string, List<int>>_teamToZone = new Dictionary<string, List<int>>();
    
    private void Awake()
    {
        InvokeRepeating(nameof(Timer), 1, 1);
        LoadingSceneScript = this;
        Manager = new AztecManager(localsObj[0], localsObj[1], localsObj[2], teamMatesObj[0], teamMatesObj[1],
            teamMatesObj[2], enemiesObj[0], enemiesObj[1], enemiesObj[2], localHpText, localHpSlider);
        if (MyTeam == 1)
        {
            TeamRed.transform.position = new Vector3(732, 1077, 0);
            TeamBlue.transform.position = new Vector3(1189, 1077, 0);
        }
        else
        {
            TeamRed.transform.position = new Vector3(1189, 1077, 0);
            TeamBlue.transform.position = new Vector3(732, 1077, 0);  
        }
        SetUI();
        _teamToZone.Add("Red", new List<int>());
        _teamToZone.Add("Blue", new List<int>());
        foreach (var t in conGameObj)
        {
            conSprites.Add(t.GetComponent<Image>());
        }

        SetDictionary();
    }

    private void Timer()
    {
            CurrentTime--;
        // ReSharper disable once PossibleLossOfFraction
        float minutes = Mathf.FloorToInt(CurrentTime / 60); 
        float seconds = Mathf.FloorToInt(CurrentTime % 60);
        TimerTxt.text = $"{minutes:00}:{seconds:00}";
    }
    private void Start()
    {
        ClientSend.SendSceneLoaded();
    }

    private void SetDictionary()
    {
        _connections.Add(ServerID, conSprites[0]);
        _connections.Add(TeamMates.Keys.ToList()[0], conSprites[1]);
        _connections.Add(TeamMates.Keys.ToList()[1], conSprites[2]);
        _connections.Add(Enemies.Keys.ToList()[0], conSprites[3]);
        _connections.Add(Enemies.Keys.ToList()[1], conSprites[4]);
        _connections.Add(Enemies.Keys.ToList()[2], conSprites[5]);

    }

    private void SetUI()
    {
        /*
         * 0 -> CLIENT
         * 1 -> T1
         * 2 -> T2
         */
        teamTexts[0].text = Username;
        teamTexts[1].text = TeamMates.Values.ToList()[0];
        teamTexts[2].text = TeamMates.Values.ToList()[1];
        enemiesTexts[0].text = Enemies.Values.ToList()[0];
        enemiesTexts[1].text = Enemies.Values.ToList()[1];
        enemiesTexts[2].text = Enemies.Values.ToList()[2];
    }

/*
 * DEFINE SERIES OF CHECKS THE PLAYER HAS TO TAKE BEFORE SERVER GRANTS ACCESS TO GAME
 * 
 */
    public void SetText(int zone1, int zone2, int zone3, int zone4, int zone7)
    {
        /*texts[0].text = $"Zone 1 = {zone1}";
        texts[1].text = $"Zone 2 = {zone2}";
        texts[2].text = $"Zone 3 = {zone3}";
        texts[3].text = $"Zone 4 = {zone4}";*/
    
        Debug.Log($"PlayerZone is {PlayerConstants.CurrentZone} {zone1} {zone2} {zone3} {zone4} {zone7}");
        switch (PlayerConstants.CurrentZone)
        {
            case 1:
                if (!zoneSlider.gameObject.activeSelf)
                {
                    zoneSlider.gameObject.SetActive(true);
                }
                zoneSlider.value = zone1;
                break;
            case 2:
                if (!zoneSlider.gameObject.activeSelf)
                {
                    zoneSlider.gameObject.SetActive(true);
                }
                zoneSlider.value = zone2;
                break;
            case 3:
                if (!zoneSlider.gameObject.activeSelf)
                {
                    zoneSlider.gameObject.SetActive(true);
                }
                zoneSlider.value = zone3;
                break;
            case 4:
                if (!zoneSlider.gameObject.activeSelf)
                {
                    zoneSlider.gameObject.SetActive(true);
                }
                zoneSlider.value = zone4;
                break;
            case 7:
                if (!zoneSlider.gameObject.activeSelf)
                {
                    zoneSlider.gameObject.SetActive(true);
                }
                zoneSlider.value = zone7;
                break;
            default:
                if (zoneSlider.gameObject.activeSelf)
                {
                    zoneSlider.gameObject.SetActive(false);
                }
                break;
        }
    }

    public void SpawnPlayer(int playerId, int modelToSpawn, Vector3 pos)
    {
        Debug.Log($"Instantiating player {playerId} at pos {pos} that picked {modelToSpawn}");
        _spawnedModels++;
        UpdateConnectionStatus(playerId);
        if (playerId == ServerID)
        {
            //THIS IS LOCAL PLAYER  
            Debug.Log($"Instantiating Local");
            SpawnPoint = pos;
            Manager.InstantiateLocalPlayer(modelToSpawn, pos);
        }
        else if (TeamMates.ContainsKey(playerId))
        {
            //TEAMMATE
            Debug.Log($"Instantiating TeamMate");

            Manager.InstantiateTeamMate(playerId, modelToSpawn, pos);
        }
        else if (Enemies.ContainsKey(playerId))
        {
            //ENEMY
            Debug.Log($"Instantiating Enemy");

            Manager.InstantiateEnemy(playerId, modelToSpawn, pos);
        }
        else
        {
            Application.Quit();
        }
    }

    private void Update()
    {
        if (_spawnedModels == 6)
        {
            /*
             * EVERYONE HAS BEEN SPAWNED HERE
             */
            ClientSend.SceneIsFullyReady();
            _spawnedModels = 0;
        }
    }

    private void SetColor()
    {
        for (int i = 0; i < _connections.Keys.Count; i++)
        {
            UpdateConnectionStatus(i);
        }
    }

    public void UpdateConnectionStatus(int whoToUpdate)
    {
        _connections[whoToUpdate].color = Color.green;
    }

    public void RemoveCanvas()
    {
        //Debug.Log("REMOVING CANVAS");
        loadingCanvas.enabled = false;
    }

    private IEnumerator Canvas()
    {
        yield return new WaitForSeconds(2f);
    }

    public void UpdateZoneState(int zoneToUpdate, int team)
    {
        var zoneToString = "";
        var teamToString = team == 1 ? "Red" : "Blue";
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (zoneToUpdate == 1)
            zoneToString = "Zone 1";
        else if (zoneToUpdate == 2)
            zoneToString = "Zone 2";
        else if (zoneToUpdate == 3)
            zoneToString = "Zone 3";
        else if (zoneToUpdate == 4)
            zoneToString = "Zone 4";
        else if (zoneToUpdate == 7) zoneToString = "Zone 5";
        AlertText.gameObject.SetActive(true);
        if (teamToString == "Red")
        {
            ZonesRed.text = (int.Parse(ZonesRed.text) + 1).ToString();
            if (_teamToZone["Blue"].Contains(zoneToUpdate))
            {
                ZonesBlue.text = (int.Parse(ZonesBlue.text) - 1).ToString();
                _teamToZone["Blue"].Remove(zoneToUpdate);
            }
            _teamToZone["Red"].Add(zoneToUpdate);

        }
        else
        {
            ZonesBlue.text = (int.Parse(ZonesBlue.text) + 1).ToString();
            if (_teamToZone["Red"].Contains(zoneToUpdate))
            {
                ZonesRed.text = (int.Parse(ZonesRed.text) - 1).ToString();
                _teamToZone["Red"].Remove(zoneToUpdate);    
            }
            _teamToZone["Blue"].Add(zoneToUpdate);

        }
        AlertText.text = $"Team {teamToString} has taken control over {zoneToString}";
    }
    public void HandleLocalDeath(float deathTimer, int deathCount)
    { 
        Manager.UpdateLocalPosition(SpawnPoint);
        SetPostproduction();
        Manager.SetDeathLocal();
        Invoke("Respawn", deathTimer);
        if (MyTeam == 1)
        {
            DeathsRed.text = deathCount.ToString();
        }
        else
        {
            DeathBlue.text = deathCount.ToString();
        }
    }

    private void SetPostproduction()
    {
        if (postProcessAlive.activeSelf)
        {
            deadPostProcess.SetActive(true);
            postProcessAlive.SetActive(false);
        }
        else
        {
            deadPostProcess.SetActive(false);
            postProcessAlive.SetActive(true);
        }
    }

    public void Respawn()
    {
        Manager.SetDeathLocal();
        SetPostproduction();
    }
    public void HandleTeamMateDeath(int whoToDie, float deathTimer, int deathCount)
    {
        if (MyTeam == 1)
        {
            DeathsRed.text = deathCount.ToString();
        }
        else
        {
            DeathBlue.text = deathCount.ToString();
        }    
    }

    public void HandleEnemyDeath(int whoToDie, float deathTimer, int deathCount)
    {
        if (MyTeam == 1)
        {
            DeathsRed.text = deathCount.ToString();
        }
        else
        {
            DeathBlue.text = deathCount.ToString();
        }    }
}