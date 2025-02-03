using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreativeObjectManager : MonoBehaviour, IData
{
    public GameObject EquipmentContainer;
    public GameObject ChemicalContainer;

    public List<Item> ClonedItems = new List<Item>();
    [SerializeField] private Items items; // Reference to the list of items

    void Start()
    {
        CloneItems();
        UnlockAllItems();
        PopulateCreativeItems();
    }

    /// <summary>
    /// Clones all items from the master item list into ClonedItems
    /// </summary>
    void CloneItems()
    {
        ClonedItems.Clear(); // Clear the list to avoid duplication

        foreach (var item in items.items) // Use items.items to access the list
        {
            if (item != null)
            {
                ClonedItems.Add(item.Clone()); // Clone and add to the list
            }
        }

        Debug.Log($"Cloned {ClonedItems.Count} items from the item list.");
    }

    /// <summary>
    /// Unlocks all items, making them available for use in Creative Mode
    /// </summary>
    void UnlockAllItems()
    {
        foreach (var item in ClonedItems)
        {
            item.isUnlock = true; // Unlock the item
        }

        Debug.Log("All items unlocked in Creative Mode.");
    }

    /// <summary>
    /// Populates the Equipment and Chemical containers with cloned items
    /// </summary>
    void PopulateCreativeItems()
    {
        foreach (Item item in ClonedItems)
        {
            GameObject parentContainer =
                (item.itemType == Item.ItemType.Equipment) ? EquipmentContainer : ChemicalContainer;

            if (parentContainer == null)
            {
                Debug.LogWarning($"Parent container is missing for {item.itemName}");
                continue;
            }

            Transform slot = FindEmptySlot(parentContainer);
            if (slot != null && item.itemPrefab != null)
            {
                GameObject itemInstance = Instantiate(item.itemPrefab, slot, false);
                itemInstance.name = item.itemName;
                ItemReaction itemUI = itemInstance.GetComponent<ItemReaction>();

                if (itemUI != null) itemUI.SetItem(item);
                Debug.Log($"Instantiated {item.itemName} in {parentContainer.name}");
            }
            else
            {
                Debug.LogWarning($"No available slots in {parentContainer.name} for {item.itemName}");
            }
        }
    }

    /// <summary>
    /// Finds the first empty slot inside a given container and returns its first child transform.
    /// </summary>
    Transform FindEmptySlot(GameObject container)
    {
        foreach (Transform slot in container.transform) // Loop through all slots in the container
        {
            if (slot.childCount == 0) // If slot itself is empty, skip it
                continue;

            Transform itemParent = slot.GetChild(0); // Get the first child (where the item should be instantiated)

            if (itemParent.childCount == 0) // Check if this itemParent is empty (no instantiated item)
            {
                return itemParent; // Return the correct parent transform for instantiation
            }
        }

        return null; // No empty slots found
    }


    public void UpdateItemPrefab(ItemReaction itemReaction, string ItemInteracted)
    {
        if (itemReaction != null && itemReaction.item.CurrentState != null)
        {
            bool stateChanged = false;

            // Iterate through all possible states
            foreach (var state in itemReaction.item.states)
            {
                if (state.conditions != null && state.conditions.Count > 0)
                {
                    bool allConditionsMet = true;

                    // Check if all conditions are met
                    foreach (var condition in state.conditions)
                    {
                        if (condition.itemNameRequirement != ItemInteracted)
                        {
                            allConditionsMet = false;
                            break; // Exit early if any condition fails
                        }
                    }

                    if (allConditionsMet)
                    {
                        // Update item to new state
                        itemReaction.item.currentStateIndex = itemReaction.item.states.IndexOf(state);
                        itemReaction.GetComponent<Image>().sprite = state.sprite;
                        itemReaction.transform.name = state.stateName;
                        itemReaction.item.currentTemperature = state.Temperature;
                        stateChanged = true;

                        Debug.Log($"✅ Item updated to state: {state.stateName}");
                        break; // Stop checking further states once changed
                    }
                }
            }

            if (!stateChanged)
            {
                Debug.LogWarning(
                    $"⚠️ No matching state found for item: {itemReaction.item.itemName} with interaction: {ItemInteracted}");
            }
        }
        else
        {
            Debug.LogError("❌ ItemReaction or CurrentState is null. Cannot update prefab.");
        }
    }

    public void LoadData(GameData gameData)
    {
        if (ClonedItems.Count > 0)
        {
            Debug.Log("Loading creative mode items...");
            UnlockAllItems();
            PopulateCreativeItems();
        }
    }

    public void SavedData(GameData gameData)
    {
        Debug.LogWarning("SaveData not implemented.");
    }
}