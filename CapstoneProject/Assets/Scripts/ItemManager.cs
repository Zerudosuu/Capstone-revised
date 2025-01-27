using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [Header("Shop Container")] [SerializeField]
    private GameObject ShopEquipmentArea;

    [SerializeField] private GameObject ShopChemicalArea;

    [Header("Prefab")] public GameObject InventoryPrefabItem; // this prefab contains drag and drop

    public GameObject ShopPrefabItem; // this prefab contains shop

    [SerializeField] private Items items;
    public List<Item> clonedItems = new List<Item>();

    public GameObject ContainerManagerForEquipments;
    public GameObject ContainerManagerForChemicals;

    // Start is called before the first frame update
    void Start()
    {
        CloneItems();
        UncheckMaterials();
        InstantiateItems();
    }

    void CloneItems()
    {
        foreach (var item in items.items)
        {
            clonedItems.Add(item.Clone());
        }
    }

    void UncheckMaterials()
    {
        foreach (var item in clonedItems)
        {
            item.isCollected = false;
        }
    }

    // Instantiate cloned items in Inventory or Shop
    public void InstantiateItems()
    {
        foreach (Item item in clonedItems)
        {
            if (item.isUnlock)
            {
                InstantiateInInventory(item);
            }
            else
            {
                 // InstantiateInShop(item);
            }
        }
    }

    // if the item is unlock it will instantiate in the Inventory
    public void InstantiateInInventory(Item item)
    {
        GameObject itemObject = Instantiate(InventoryPrefabItem);
        itemObject.name = item.itemName;
        itemObject.GetComponent<Image>().sprite = item.itemIcon;

        DragAndDropUI itemDisplay = itemObject.GetComponent<DragAndDropUI>();
        if (itemDisplay != null)
        {
            itemDisplay.SetItem(item); // Ensure the item is set here
        }
        else
        {
            Debug.LogError("DragAndDropUI component is missing on the InventoryPrefabItem.");
        }

        // Determine which container to search in
        ContainerManager containerManager = null;

        if (item.itemType == Item.ItemType.Equipment)
        {
            containerManager = ContainerManagerForEquipments.GetComponent<ContainerManager>();
        }
        else if (item.itemType == Item.ItemType.Chemical)
        {
            containerManager = ContainerManagerForChemicals.GetComponent<ContainerManager>();
        }

        if (containerManager == null)
        {
            Debug.LogError("ContainerManager not found!");
            return;
        }

        GameObject targetContainer = containerManager.SearchForAvailableContainer();

        if (targetContainer != null)
        {
            itemObject.transform.SetParent(targetContainer.transform, false);
            Debug.Log($"Item {item.itemName} added to {targetContainer.name}");
        }
        else
        {
            Debug.LogWarning($"No available container found for item: {item.itemName}");
            Destroy(itemObject); // Prevent orphaned GameObjects if no container found
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
}