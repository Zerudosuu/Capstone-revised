using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField]
    private List<BagItem> bagItems = new List<BagItem>();

    CurrentLessonToDisplay currentLessonToDisplay;

    void Start()
    {
        currentLessonToDisplay = FindObjectOfType<CurrentLessonToDisplay>(true);
        ItemCountContainer.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void AddItemInBag(Item item)
    {
        // Check if the item already exists in the bag
        BagItem existingBagItem = bagItems.Find(b => b.item.itemName == item.itemName);

        if (existingBagItem != null)
        {
            // If the item exists, increment its count
            existingBagItem.IncrementCount();
        }
        else
        {
            // If the item doesn't exist, add a new entry
            GameObject newItem = Instantiate(ItemPrefab, BagContainer.transform);
            BagItem bagItem = newItem.GetComponent<BagItem>();
            bagItem.SetBagItem(item);
            bagItems.Add(bagItem);

            newItem.transform.SetAsFirstSibling();
        }

        UpdateItemCount();
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

    public void RemoveItem(BagItem bagItem)
    {
        bagItems.Remove(bagItem); // Remove the item from the list
        Destroy(bagItem.gameObject);
        UpdateItemCount();
    }

    public void ClearBag()
    {
        // TODO: Implement functionality for clearing the bag
        bagItems.Clear();
        UpdateItemCount();
    }

    public void ProceedToExperiment()
    {
        // TODO: Implement functionality for proceeding to the experiment
        //  currentLessonToDisplay.ProceedToExperiment();
    }
}
