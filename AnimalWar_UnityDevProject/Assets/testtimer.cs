using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testtimer : MonoBehaviour
{
    // Start is called before the first frame update
   float CurrentTime = 1500f;
   public Text TimerTxt;
    void Start()
    {
        InvokeRepeating("Timer", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Timer()
    {
        CurrentTime--;
        // ReSharper disable once PossibleLossOfFraction
        float minutes = Mathf.FloorToInt(CurrentTime / 60); 
        float seconds = Mathf.FloorToInt(CurrentTime % 60);
        TimerTxt.text = $"{minutes:00}:{seconds:00}";
    }
}
