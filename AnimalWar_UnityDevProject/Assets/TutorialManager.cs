using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Collider startingCollider;
    public Collider attackCollider;
    public Collider ultimateCollider;

    public static TutorialManager Instance;
    private int lastStage = 1;
    public bool disableNext;
    private int currentText = 0;
    public GameObject WASDImage;
    public GameObject aok;
    public Text alertText;
    public Text alertTextBG;
    public System.Action playerIsInRangeOfDummies;
    public GameObject sok;
    public GameObject dok;
    public GameObject wok;
    public GameObject spaceok;
    private List<int> values = new List<int>();
    private List<KeyCode> pressedKeys = new List<KeyCode>();
    private string[] tutorialStrings = new[]
    {
        "Welcome! We have prepared a short circuit so you can refresh or learn the basics of Animal War!",
        "You are now playing as Bao (The cute panda) he is a Tank type of character which means he can endure much longer in the battlefield" +
        " but he deals less damage in return.",
        "Let's start by learning the basic movement Keys, Press \n to move Bao around.",
        "Great! Now move towards the dummies you'll see in front of you.",
        "Now try to attack the blue dummy by pressing LMB when you are close to it.",
        "Now try to defend from the red dummy by pressing RMB when you are close to it.",
        "Now press F to use your Ultimate Ability (JUMP OF THE MOON).",
        "Great! Now move around your cursor and press LMB on top of the Target you'll see on the ground.",
        "Amazing! You are now a force to be reckon with. Enter the portal and start your adventures."
    };

    public Text tutorialText;
    public Text tutorialTextBg;
    
    private void Awake()
    {
        Instance = this;
        tutorialText.text = tutorialStrings[0];
        tutorialTextBg.text = tutorialStrings[0];
        playerIsInRangeOfDummies += UpdateText;
        Invoke("UpdateText", 4);
        Invoke("UpdateText", 12);
        values.Add((int)KeyCode.W);
        values.Add((int)KeyCode.A);
        values.Add((int)KeyCode.S);
        values.Add((int)KeyCode.D);
        values.Add((int)KeyCode.Space);
    }

    private void UpdateText()
    {
        currentText++;
        if (currentText == 2)
        {
            WASDImage.SetActive(true);
        }
        else
        {
            if (WASDImage.activeSelf)
            {
                WASDImage.SetActive(false);
            }
        }
        tutorialText.text = tutorialStrings[currentText];
        tutorialTextBg.text = tutorialStrings[currentText];
    }

    private void GatherWASDCondition(KeyCode keyCode)
    {
        if (!pressedKeys.Contains(keyCode))
        {
            pressedKeys.Add(keyCode);
        }
    }

    private void Update()
    {
        
        if (currentText == 2)
        {
            if (pressedKeys.Count >= 5)
            {
                UpdateText();
                startingCollider.gameObject.SetActive(false);
            }

            if (!Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) &&
                !Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.Space)) return;
            KeyCode key;
            foreach (var t in values)
            {

                if (Input.GetKey((KeyCode) t))
                {
                    if ((KeyCode) t == KeyCode.A)
                    {
                        aok.SetActive(true);
                    }
                    if ((KeyCode) t == KeyCode.W)
                    {
                        wok.SetActive(true);
                    }
                    if ((KeyCode) t == KeyCode.S)
                    {
                        sok.SetActive(true);
                    }
                    if ((KeyCode) t == KeyCode.D)
                    {
                        dok.SetActive(true);
                    }
                    if ((KeyCode) t == KeyCode.Space)
                    {
                        spaceok.SetActive(true);
                    }
                    GatherWASDCondition((KeyCode) t);
                }
            }


        }

        if (currentText == 6)
        {
           if(attackCollider.gameObject.activeSelf)
            attackCollider.gameObject.SetActive(false);
        }
                
    }

    public void UpdateCurrentStage(int stage)
    {
        switch (stage)
        {
            case 1:
                startingCollider.enabled = false;
                break;
            case 2:
                attackCollider.enabled = false;
                break;
            case 3:
                ultimateCollider.enabled = false;
                break;
            default:
                Debug.LogError($"Invalid TUTORIAL STAGE {stage}");
                break;
        }

        disableNext = false;
    }

    public void UpdateCurrentStage()
    {
        if(currentText < 4)
            UpdateText();
    }
    public void NewStage()
    {
        if(currentText < 5)
            UpdateText();
    }

    public void ShowAlertText(string beCarefulYouAreTakingDamageFallBack)
    {
        alertText.gameObject.SetActive(true);
        alertTextBG.gameObject.SetActive(true);
        alertText.text = beCarefulYouAreTakingDamageFallBack;
        alertTextBG.text = beCarefulYouAreTakingDamageFallBack;
    }

    public void DummiesDone()
    {
        if(currentText < 6)
            UpdateText();    
    }

    public void UpdateUltiText()
    {
        if (currentText < 7)
        {
            UpdateText();
        }
    }

    public void LastStage()
    {
        if (currentText < 8)
        {
            UpdateText();
            ultimateCollider.gameObject.SetActive(false);
        }    
    }
}