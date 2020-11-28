using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image background;

    public UnityEvent ONTabSelected;
    public UnityEvent ONTabDeSelected;

    public bool isDefault = false;
    private void Start()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }    

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabHover(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        ONTabSelected?.Invoke();
    }
    public void DeSelect()
    {
        ONTabDeSelected?.Invoke();

    }
}
