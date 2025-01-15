using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEditor.Progress;

public class DebuggingItems : MonoBehaviour
{
    [SerializeField] private Items items;
    [SerializeField] private Item cloneItem = new Item();

    [Header("Selected Item To Instantiate")]
    [SerializeField] private List<Item> clonedItems = new List<Item>();
    [SerializeField] private List<String> itemNeed = new List<string>();

    [Header("GameObjects")]
    [SerializeField] private GameObject itemHolderPref;
    [SerializeField] private GameObject itemContainer;

    private void Start()
    {
        CloneItems();
    }
        
    private void CloneItems()
    {
        clonedItems.Clear(); 

        foreach (var needItem in itemNeed)
        {
            var matchItem = items.items.Find(item => item.itemName == needItem);

            if (matchItem != null)
            {
                clonedItems.Add(matchItem.Clone()); // Clone and add to the list
            }

            Instantiate(matchItem.itemPrefab, gameObject.transform);
        }

    }

   

}
