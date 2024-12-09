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
    [Header("Item Properties")]
    public GameObject itemPrefab;
    public string itemName = "";
    public string itemDescription = "";
    public Sprite itemIcon;
    public ItemType itemType;
    public bool isUnlock;
    public bool isCollected;

    // Temperature properties
    [Header("Temperature Properties")]
    public bool hasTemperature;
    public float currentTemperature;
    public float maxTemperature = 100f;
    public float coolingRate = 5f;
    public float heatingRate = 10f;

    [Header("Measurement Properties")]
    public float measuredValue = 0;
    public bool needMeasurement = false;
    public string measurementUnit = ""; // e.g., "ml", "g"

    [Header("Compatibility and Conditions")]
    public string tagName;
    public List<string> compatibleTags;

    // State management
    [Header("State Management")]
    public List<ItemState> states = new List<ItemState>();
    public int currentStateIndex = 0;
    public ItemState CurrentState => states[currentStateIndex];

    public enum ItemType
    {
        Equipment,
        Chemical,
    }

    public void SwitchToNextState()
    {
        currentStateIndex = (currentStateIndex + 1) % states.Count;
    }

    public void SwitchToState(int index)
    {
        if (index >= 0 && index < states.Count)
        {
            currentStateIndex = index;
        }
        else
        {
            Debug.LogWarning($"{itemName}: Invalid state index {index}. Keeping current state.");
        }
    }

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
            needMeasurement = this.needMeasurement,
            measurementUnit = this.measurementUnit,
            tagName = this.tagName,
            compatibleTags = new List<string>(this.compatibleTags),
            states = new List<ItemState>(this.states),
            currentStateIndex = this.currentStateIndex,
            hasTemperature = this.hasTemperature,
            currentTemperature = this.currentTemperature,
            maxTemperature = this.maxTemperature,
            coolingRate = this.coolingRate,
            heatingRate = this.heatingRate,
        };
    }
}

[System.Serializable]
public class ItemState
{
    public string stateName; // Name of the state (e.g., "Empty", "Filled", "Measured")
    public GameObject statePrefab; // Prefab associated with this state
    public string description; // Description or additional behavior for the state
    public bool isDefaultState; // Is this the default state for the item?
    public List<Conditions> conditions; // Conditions required to switch to this state
}

[System.Serializable]
public class Conditions
{
    public string conditionName;
    public List<string> itemNameRequirements;
    public string DisplayErrorMessage;
    public string typeOfInteraction;

    public bool CheckConditions(List<string> tags)
    {
        return itemNameRequirements.TrueForAll(tag => tags.Contains(tag));
    }
}
