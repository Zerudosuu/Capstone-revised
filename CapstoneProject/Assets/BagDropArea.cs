using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagDropArea : MonoBehaviour
{
    BagManager bagManager;

    void Start()
    {
        bagManager = FindAnyObjectByType<BagManager>();
    }

    public void AddedToInventory(Item item)
    {
        print("Added to inventory: " + item.itemName);
        bagManager.AddItemInBag(item);
        bagManager.UpdateItemCount();
    }
}
