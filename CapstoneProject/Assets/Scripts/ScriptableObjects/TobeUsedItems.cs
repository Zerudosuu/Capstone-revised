using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TobeUsedItems : MonoBehaviour
{
    public Items items;

    public List<Item> clonedItems = new List<Item>();

    void Start()
    {
        getItems();
    }

    void getItems()
    {
        foreach (var item in items.items)
        {
            clonedItems.Add(item.Clone());
        }
    }
}