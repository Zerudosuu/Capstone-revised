using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObjectManager : MonoBehaviour, IData
{
    [SerializeField]
    private GameObject experimentModal;

    [SerializeField]
    private QuestAsLesson currentLesson;

    [SerializeField]
    private Items items;

    [SerializeField]
    private List<Item> clonedItems = new List<Item>();

    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private GameObject ItemContainer;

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

        // Populate each cloned item into the scene
        foreach (var clonedItem in clonedItems)
        {
            // Instantiate a new item prefab as a child of the ItemContainer
            GameObject newItem = Instantiate(itemPrefab, ItemContainer.transform);

            // Example: Assuming your item prefab has a script to set its properties
            ExperimentItem itemUI = newItem.GetComponent<ExperimentItem>();

            if (itemUI != null)
            {
                // Set the item data to display in the UI
                itemUI.SetItem(clonedItem);
            }
            else
            {
                Debug.LogError("Item prefab does not have an ItemUI component.");
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
            currentLesson.isCompleted = true; // Mark the lesson as completed
            DataManager.Instance.SaveGame(); // Save the updated state

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
