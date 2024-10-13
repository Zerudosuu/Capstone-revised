using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void RemoveItem(BagItem bagItem)
    {
        bagItems.Remove(bagItem); // Remove the item from the list
        Destroy(bagItem.gameObject);
        UpdateItemCount();
    }
}
