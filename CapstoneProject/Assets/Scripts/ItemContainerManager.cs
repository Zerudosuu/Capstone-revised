using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemContainerManager : MonoBehaviour
{
    [SerializeField] private Item CurrentSelectedItem;
    public bool isUnlocked;
    public int priceToUnlock = 300;
    public int maxItem = 16;

    public GameObject NotUnlockPanel;


    public void Start()
    {
        if (isUnlocked)
        {
            NotUnlockPanel.SetActive(false);
        }
    }

    public void OnItemClick(Item item)
    {
        CurrentSelectedItem = item;
        Debug.Log(item.itemName + " is selected currently.");
    }

    public bool CanAcceptMoreItems()
    {
        return transform.childCount < maxItem;
    }
}