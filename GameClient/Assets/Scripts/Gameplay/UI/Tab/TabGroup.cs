using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;
    public List<GameObject> objectsToSwap = new List<GameObject>();
    public bool defaultTab = false;
    public bool isOption = false;
    public TabButton selectedTab;
    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
        StartingTab();
    }
    public void ReEvaluateTabs()
    {
        StartingTab();
    }
   public void StartingTab()
    {
        foreach (var tab in tabButtons)
        {
            if (tab.isDefault)
            {
                selectedTab = tab;
                OnTabSelected(tab);
            }
        }
    }
    public void OnTabHover(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
       //     TabButton.DeSelect();
        }
        selectedTab = button;
        button.Select();
        ResetTabs();
        button.background.sprite = tabSelected;
        if (isOption) return;
        int index = button.transform.GetSiblingIndex();
        objectsToSwap[index].SetActive(true);
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i != index)
            {
                objectsToSwap[i].SetActive(false);
            }
        }

    }
    public void ResetTabs()
    {
        foreach (var tab in tabButtons)
        {
            if(selectedTab!=null && tab == selectedTab){continue;}
            tab.background.sprite = tabIdle;
        }
    }
}
