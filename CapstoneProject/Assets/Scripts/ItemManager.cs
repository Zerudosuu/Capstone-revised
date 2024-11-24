using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Item Container")]
    [SerializeField]
    private GameObject EquipmentArea;

    [SerializeField]
    private GameObject ChemicalArea;

    [Header("Shop Container")]
    [SerializeField]
    private GameObject ShopEquipmentArea;

    [SerializeField]
    private GameObject ShopChemicalArea;

    [Header("Prefab")]
    public GameObject InventoryPrefabItem; // this prefab contains drag and drop

    public GameObject ShopPrefabItem; // this prefab contains shop

    [SerializeField]
    private Items items;
    private List<Item> clonedItems = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        CloneItems();
        InstantiateItems();
    }

    void CloneItems()
    {
        foreach (var item in items.items)
        {
            clonedItems.Add(item.Clone());
        }
    }

    // Instantiate cloned items in Inventory or Shop
    void InstantiateItems()
    {
        foreach (Item item in clonedItems)
        {
            if (item.isUnlock)
            {
                InstantiateInInventory(item);
            }
            else
            {
                InstantiateInShop(item);
            }
        }
    }

    // if the item is unlock it will instantiate in the Inventory
    public void InstantiateInInventory(Item item)
    {
        GameObject itemObject = Instantiate(InventoryPrefabItem);
        itemObject.name = item.itemName;

        DragAndDropUI itemDisplay = itemObject.GetComponent<DragAndDropUI>();
        if (itemDisplay != null)
        {
            itemDisplay.SetItem(item); // Ensure the item is set here
        }
        else
        {
            Debug.LogError("DragAndDropUI component is missing on the InventoryPrefabItem.");
        }

        if (item.itemType == Item.ItemType.Equipment)
        {
            itemObject.transform.SetParent(EquipmentArea.transform, false);
        }
        else if (item.itemType == Item.ItemType.Chemical)
        {
            itemObject.transform.SetParent(ChemicalArea.transform, false);
        }
    }

    //if the item in lock it will instantitate in the shop
    public void InstantiateInShop(Item item)
    {
        GameObject itemObject = Instantiate(ShopPrefabItem);
        itemObject.name = item.itemName;

        itemShop shopItem = itemObject.GetComponent<itemShop>();
        shopItem.SetItem(item);

        //add here where if the item is unlock
        if (item.itemType == Item.ItemType.Equipment)
        {
            // Place the object in the EquipmentArea
            itemObject.transform.SetParent(ShopEquipmentArea.transform, false);
        }
        else if (item.itemType == Item.ItemType.Chemical)
        {
            // Place the object in the ChemicalArea
            itemObject.transform.SetParent(ShopChemicalArea.transform, false);
        }
    }

    // Update is called once per frame
    void Update() { }
}
