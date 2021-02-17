using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MatchVariables;
using static UtilsOwn;
using UnityEngine.UI;
using UnityEngine.Events;

/*
 * 0 . PANDA
 * 1 . TIGER
 * 2 . WOLF
 */
//THIS IS THE LOCAL REPRESENTATION OF THE MATCH LOBBY A SIMILAR SCRIPT EXISTS IN THE SERVER
public class LobbyManager : MonoBehaviour
{
    public List<Text> userNameTexts = new List<Text>();
    private int _matchId;
    private float timer = 60;
    private bool hasPicked;
    public int Test;
    [SerializeField] public List<GameObject> ImageSlots = new List<GameObject>();
    public Dictionary<string,GameObject> PlayerToSlot = new Dictionary<string,GameObject>();
    [SerializeField]
    public List<Sprite> PickImages = new List<Sprite>();

    public List<Button> PickButtons = new List<Button>();
    private bool hasSent;
    public Text countDownText;
    private PickInfo[] picks = {
        new PickInfo("Tiger", "Strong melee assasin low hp", "Random Lore Tiger"),
        new PickInfo("Panda", "Absolute unit of a tank. GL moving him LMAO", "Random Panda Lore"),
        new PickInfo("Wolf", "Strong melee hard cc medium dmg", "Random Wolf Lore"),

    };

    #region InputClick
    bool isClicking = false; //IF THE CLIENT PRESSES THE MOUSE BUTTON 0 IT SETS TO TRUE
    float longClickTime = 2f; //IN SECONDS

    float totalDownTime = 0f; //SECONDS

    int CurrentPick;
    #endregion

    #region UIelements
    public Text pickName;
    public Text pickDescription;
    public Text pickLore;

    public GameObject ConfirmPickUi;
    public Slider ConfirmSlider;
    #endregion
    private void Awake()
    {
        Debug.Log("Scene is lobby");
    }

    private void Start()
    {
        Constants.LobbyManager = this;
        SetUI();
    }

    private void Update()
    {
        //Debug.Log($"MY USERNAME IS {Constants.Username} and my SERVER ID IS -> {Constants.ServerID} BUT MY DBID is {Constants.DbId}");
        //Debug.Log(CurrentPick);
        if (!(timer >= 0f)) return;
        timer -= Time.deltaTime;
        countDownText.text = ((int) timer).ToString();
        LongClick();
        
        
        
    }

    private void SendPlayerState(bool b)
    {
        
    }

    private void SetUI()
    {
        userNameTexts[0].text = Constants.Username;
        PlayerToSlot.Add(Constants.Username, ImageSlots[0]); 

        foreach (var teamMate in TeamMates.Values)
        {
            if (userNameTexts[1].text == "UserName")
            {
                userNameTexts[1].text = teamMate;
                PlayerToSlot.Add(teamMate, ImageSlots[1]); 
            }
            else
            {
                userNameTexts[2].text = teamMate;
                PlayerToSlot.Add(teamMate, ImageSlots[2]); 
            }
        }
    }

    public void SendPickUpdate(int whatPick)
    {

        if (Between(whatPick, -1, 3))
        {
            ClientSend.SendPickUpdate(whatPick);   
        }
        else
        {
            Debug.LogError(
                $"SendPickUpdate expected an int within the range of 0 to 2 the received int was {whatPick}");
        }
    }
    public void PickUpdate(int playerId, int whatPick)
    {
        if(TeamMates.ContainsKey(playerId) || playerId == Constants.ServerID)
            SetImage(playerId, whatPick);
    }

    private void SetImage(int who, int whatPick){
        //Debug.Log($"Setting image on {who} with pick {whatPick}");
        var user = who == Constants.ServerID ? Constants.Username : TeamMates[who];
        PlayerToSlot[user].GetComponent<Image>().sprite = PickImages[whatPick];
        if (user == Constants.Username)
        {
            SetButtonInteraction();

        }
        else
        {
            SetButtonInteraction(whatPick);
        }
    }
    private void SetButtonInteraction(int whatPick){ //ONLY 1 BUTTON
            PickButtons[whatPick].interactable = false;
    }

    private void SetButtonInteraction(){  //ALL BUTTONS TO NONE
        foreach(var btn in PickButtons){
            btn.interactable = false;
        }
    }

   /* private IEnumerator UpdateTimer()
    {
        yield return new WaitForSecondsRealtime(1);
        timer--;
    }*/


   public void EndLobby(int scene)
   {
        SceneManager.LoadScene(scene);
   }

public void ClickStart(int pick){
    totalDownTime = 0f;
    CurrentPick = pick;
    pickDescription.text = picks[pick].description;
    pickName.text = picks[pick].pickName.ToUpper();

    ShowConfirmPickUI();


}

void ShowConfirmPickUI(){
    ConfirmPickUi.SetActive(true);
    ConfirmSlider.value = 0f;
    isClicking = true;
}
   public void LongClick(){
        if(isClicking && Input.GetKey(KeyCode.E)){
            LongClickCount();
        }
        if (isClicking && Input.GetKeyUp(KeyCode.E))
        {
            ConfirmSlider.value = 0f;
            totalDownTime = 0f;
        }
   }

   private void LongClickCount(){
       totalDownTime += Time.deltaTime;
       if(totalDownTime >= longClickTime && isClicking == true){
           /*PLAYER HAS PERFRORMED A LONG CLICK*/
           SendPickUpdate(CurrentPick);
           isClicking = false;
       } else{
           ConfirmSlider.value = totalDownTime / 2;
       }
   }
}

public class PickInfo{
    public string pickName;
    public string description;
    public string lore;

    public PickInfo(string pickName, string description, string lore){
        this.pickName = pickName;
        this.description = description;
        this.lore = lore;
    }
}

