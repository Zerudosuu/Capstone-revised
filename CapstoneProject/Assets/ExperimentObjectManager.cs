using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperimentObjectManager : MonoBehaviour, IData
{
    public QuestAsLesson currentLesson;

    [SerializeField] private Items items;

    [SerializeField] private List<Item> clonedItems = new List<Item>();

    [SerializeField] private GameObject itemPrefab;

    public GameObject ItemContainer;

    [SerializeField] private GameObject itemSlot;

    void Start()
    {
        if (DataManager.Instance.gameData.quest != null)
        {
            currentLesson = DataManager.Instance.gameData.quest;
        }
        else
        {
            Debug.LogWarning("No quest is currently active.");
        }

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
                DragableItem draggable = newItem.GetComponent<DragableItem>();


                if (itemUI != null) itemUI.SetItem(clonedItem);
                if (draggable != null) draggable.itemVariantID = i;
            }
        }
    }

    void CloneItems()
    {
        clonedItems.Clear(); // Clear the list to avoid duplication

        foreach (var bagItem in DataManager.Instance.gameData.BagItems)
        {
            // Find the matching item in the items list
            var matchingItem = items.items.Find(item => item.itemName == bagItem.itemName);

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
            UnityEngine.SceneManagement.SceneManager.LoadScene("LessonMode"); // Load lesson scene
        }
        else
        {
            Debug.LogError("No current lesson to complete.");
        }
    }

    public void LoadData(GameData data)
    {
        if (data.quest != null)
        {
            currentLesson = data.quest;
            CloneItems(); // Clone items when loading the data
        }
    }

    public void SavedData(GameData data)
    {
        if (currentLesson != null)
        {
            // Update quest completion state
            data.quest.isCompleted = currentLesson.isCompleted;

            if (data.currentQuestIndex > 0 && data.currentQuestIndex <= data.lessons.Count)
            {
                // Update the specific lesson's completion state in the lessons list
                data.lessons[data.currentQuestIndex - 1].isCompleted = currentLesson.isCompleted;
            }
            else
            {
                Debug.LogWarning("Invalid currentQuestIndex: " + data.currentQuestIndex);
            }

            // Clear BagItems after the experiment is done
            data.BagItems.Clear();

            // Reset the current quest index
            data.currentQuestIndex = 0;
        }
    }
}