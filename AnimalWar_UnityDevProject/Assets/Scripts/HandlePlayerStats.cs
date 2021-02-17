using System;
using UnityEngine;
using UnityEngine.UI;

public class HandlePlayerStats : MonoBehaviour
{
    public bool IsLocal;
    public float CurrentHp;
    public float MAXHp;
    public Text HpText;
    public Slider HpSlider;
    public bool isTutorial = false;

    private void Start()
    {
        if (isTutorial)
        {
            SetSlider();
        }
    }

    public void SetSlider()
    {
        HpSlider.maxValue = MAXHp;
        HpSlider.value = MAXHp;
        if (IsLocal)
        {
            CurrentHp = MAXHp;
           // UpdateHealth(MAXHp);
        }
    }

    public void UpdateHealth(float value)
    {
        if (!isTutorial)
       {
            CurrentHp = value;
       }
        else
        {
            CurrentHp -= value;
        }

        if (IsLocal)
        {
            HpText.text = $"{CurrentHp}/{MAXHp}";
        }

        HpSlider.value = CurrentHp;
    }

}