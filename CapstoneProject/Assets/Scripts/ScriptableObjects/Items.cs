using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Items", menuName = "Items", order = 0)]
public class Items : ScriptableObject
{
    public List<Item> items = new List<Item>();
}

[System.Serializable]
public class Item
{
    [Header("Item Properties")] public GameObject itemPrefab;
    public string itemName = "";
    public string itemDescription = "";
    public Sprite itemIcon;
    public ItemType itemType;
    public bool isUnlock;
    public bool isCollected;

    [Header("Compatibility and Conditions")]
    public string tagName;

    public List<string> compatibleTags;

    [Header("State Management")] public List<ItemState> states = new List<ItemState>();
    public int currentStateIndex = 0;
    public ItemState CurrentState => states[currentStateIndex];

    [Header("Temperature")] public bool hasTemperature;
    public float currentTemperature;
    public float minTemperature = 0;
    public float maxTemperature = 100;

    public enum ItemType
    {
        Equipment,
        Chemical,
    }

    public void SwitchToState(string itemRequirement)
    {
        if (states == null || states.Count == 0)
        {
            Debug.LogWarning($"{itemName}: No states available for switching.");
            return;
        }

        for (int i = 0; i < states.Count; i++)
        {
            bool allConditionsMet = true;

            foreach (var condition in states[i].conditions)
            {
                if (condition.itemNameRequirement != itemRequirement)
                {
                    allConditionsMet = false;
                    break; // Exit early if any condition is not met
                }
            }

            if (allConditionsMet)
            {
                currentStateIndex = i;
                Debug.Log($"{itemName}: Condition met! Transitioning to state '{states[i].stateName}'.");
                return;
            }
        }

        Debug.LogWarning($"{itemName}: No matching state found for item requirement: {itemRequirement}");
    }


    public Item Clone()
    {
        Item clonedItem = new Item
        {
            itemPrefab = this.itemPrefab,
            itemName = this.itemName,
            itemDescription = this.itemDescription,
            itemIcon = this.itemIcon,
            itemType = this.itemType,
            isUnlock = this.isUnlock,
            isCollected = this.isCollected,
            hasTemperature = this.hasTemperature,
            currentTemperature = this.currentTemperature,
            minTemperature = this.minTemperature,
            maxTemperature = this.maxTemperature,
            tagName = this.tagName,
            compatibleTags = new List<string>(this.compatibleTags),
            states = new List<ItemState>(), // Create a new list for states
            currentStateIndex = this.currentStateIndex
        };

        // Deep clone the states
        foreach (var state in this.states)
        {
            ItemState clonedState = new ItemState
            {
                stateName = state.stateName,
                willChangeSprite = state.willChangeSprite,
                sprite = state.sprite,
                description = state.description,
                Temperature = state.Temperature,
                conditions = new List<Conditions>() // Properly create a new list for conditions
            };

            // Deep clone the conditions list
            foreach (var condition in state.conditions)
            {
                Conditions clonedCondition = new Conditions
                {
                    itemNameRequirement = condition.itemNameRequirement,
                    isMet = condition.isMet
                };
                clonedState.conditions.Add(clonedCondition);
            }

            clonedItem.states.Add(clonedState);
        }

        return clonedItem;
    }
}


[System.Serializable]
public class ItemState
{
    public string stateName;
    public bool willChangeSprite;
    public Sprite sprite;
    public string description;
    public List<Conditions> conditions;
    public float Temperature;
}

[System.Serializable]
public class Conditions
{
    public string itemNameRequirement; // Item required to trigger this condition
    public bool isMet; // Flag to check if the condition is met
}