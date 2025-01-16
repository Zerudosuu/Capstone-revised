using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuggingItems : MonoBehaviour
{
    public QuestAsLesson currentLesson;

    [SerializeField] private Items items;

    [SerializeField] private List<Item> clonedItems = new List<Item>();

    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private List<string> itemNeed = new List<string>();

    public GameObject ItemContainer;

    [SerializeField] private GameObject itemSlot;

    void Start()
    {
       
        PopulateExperimentItem();
    }

    void PopulateExperimentItem()
    {
        // Clear any existing items from the container before populating new ones
        foreach (Transform child in ItemContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Loop through cloned items
        foreach (var clonedItem in clonedItems)
        {
            int quantity = 0;

            foreach (MaterialEntry material in currentLesson.materials)
            {
                if (clonedItem.itemName == material.materialName)
                {
                    quantity = material.Quantity;
                }
            }

            GameObject newSlot = Instantiate(itemSlot, ItemContainer.transform);
            GameObject newSlotContainer = newSlot.transform.GetChild(0).gameObject;

            for (int i = 0; i < quantity; i++)
            {
                GameObject newItem = Instantiate(clonedItem.itemPrefab, newSlotContainer.transform);
                newItem.name = clonedItem.itemName; // Name for debugging
                ItemReaction itemUI = newItem.GetComponent<ItemReaction>();

                if (itemUI != null)
                {
                    itemUI.SetItem(clonedItem);
                }
            }
        }
    }

    void CloneItems()
    {
        clonedItems.Clear(); // Clear the list to avoid duplication

        foreach (var bagItem in itemNeed)
        {
            // Find the matching item in the items list
            var matchingItem = items.items.Find(item => item.itemName == bagItem);

            if (matchingItem != null)
            {
                clonedItems.Add(matchingItem.Clone()); // Clone and add to the list
            }
        }

        Debug.Log($"Cloned {clonedItems.Count} items from the bag.");
    }

    public void BackToLesson()
    {
        if (currentLesson != null)
        {
            currentLesson.isCompleted = true; // Mark the lesson as completed
            DataManager.Instance.SaveGame(); // Save the updated state

            UnityEngine.SceneManagement.SceneManager.LoadScene("LessonMode"); // Load lesson scene
        }
        else
        {
            Debug.LogError("No current lesson to complete.");
        }
    }
}
