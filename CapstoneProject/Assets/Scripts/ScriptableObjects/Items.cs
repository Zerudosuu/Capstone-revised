using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items", order = 0)]
public class Items : ScriptableObject
{
    public List<Item> items = new List<Item>();
}

[System.Serializable]
public class Item
{
    public GameObject itemPrefab; // Prefab of the item
    public string itemName = ""; // Name of the item
    public string itemDescription = ""; // Description of the item
    public Sprite itemIcon; // Icon representing the item
    public ItemType itemType; // Type of the item (Equipment or Chemical)
    public bool isUnlock; // Whether the item is unlocked for use
    public bool isCollected; // Whether the item has been collected
    public float measuredValue = 0; // Measured value associated with the item
    public bool needToMeasure = false; // Whether the item needs to be measured

    // New properties for tags and combinations
    public string tagName;
    public List<string> compatibleTags;

    // **New properties for state management**
    public List<ItemState> states = new List<ItemState>(); // List of states for the item
    public int currentStateIndex = 0; // Track the current state

    public ItemState CurrentState => states[currentStateIndex]; // Get the current state

    public enum ItemType
    {
        Equipment,
        Chemical,
    }

    public void SwitchToNextState()
    {
        currentStateIndex = (currentStateIndex + 1) % states.Count; // Loop through states
    }

    public void SwitchToState(int index)
    {
        if (index >= 0 && index < states.Count)
        {
            currentStateIndex = index;
        }
    }

    // Deep clone method
    public Item Clone()
    {
        return new Item
        {
            itemPrefab = this.itemPrefab,
            itemName = this.itemName,
            itemDescription = this.itemDescription,
            itemIcon = this.itemIcon,
            itemType = this.itemType,
            isUnlock = this.isUnlock,
            isCollected = this.isCollected,
            measuredValue = this.measuredValue,
            tagName = this.tagName,
            compatibleTags = new List<string>(this.compatibleTags),
            states = new List<ItemState>(this.states),
            currentStateIndex = this.currentStateIndex,
        };
    }
}

[System.Serializable]
public class ItemState
{
    public string stateName; // Name of the state (e.g., "Empty", "Filled", "Measured")
    public GameObject statePrefab; // Prefab associated with this state
    public string description; // Description or additional behavior for the state
}
