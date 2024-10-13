using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    public Item item;

    public Button DeleteButton;
    BagManager bagManager;

    void Start()
    {
        bagManager = FindAnyObjectByType<BagManager>();
        if (bagManager != null)
            print("bagManager is found");
    }

    public void SetBagItem(Item item)
    {
        this.item = item;
        DeleteButton.onClick.AddListener(DeleteItem);
    }

    public void DeleteItem()
    {
        bagManager.RemoveItem(this);
    }
}
