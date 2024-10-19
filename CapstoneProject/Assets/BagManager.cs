using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagManager : MonoBehaviour
{
    [SerializeField]
    private GameObject BagContainer;

    [SerializeField]
    private GameObject ItemPrefab;

    [SerializeField]
    private GameObject ItemCountContainer;

    [SerializeField]
    private GameObject ItemCountText;

    private List<BagItem> bagItems = new List<BagItem>();

    void Start()
    {
        ItemCountContainer.gameObject.SetActive(false);
    }

    public void AddItemInBag(Item item)
    {
        GameObject newItem = Instantiate(ItemPrefab, BagContainer.transform);
        BagItem bagItem = newItem.GetComponent<BagItem>();
        bagItem.SetBagItem(item);
        bagItems.Add(bagItem);

        newItem.GetComponent<BagItem>().SetBagItem(item);
        newItem.transform.SetAsFirstSibling();
        UpdateItemCount();

        Button itemButton = newItem.GetComponent<Button>();
        itemButton.onClick.AddListener(() => SelectItem(item));
    }

    private void SelectItem(Item item)
    {
        // TODO: Implement functionality for selecting an item
        Debug.Log("Selected Item: " + item.itemName);
    }

    public void UpdateItemCount()
    {
        ItemCountText.GetComponent<TMPro.TextMeshProUGUI>().text = bagItems.Count.ToString();

        if (bagItems.Count > 0)
        {
            ItemCountContainer.gameObject.SetActive(true);
        }
        else
        {
            ItemCountContainer.gameObject.SetActive(false);
        }
    }

    // public void RemoveItem(BagItem bagItem)
    // {
    //     bagItems.Remove(bagItem); // Remove the item from the list
    //     Destroy(bagItem.gameObject);
    //     UpdateItemCount();
    // }
}


