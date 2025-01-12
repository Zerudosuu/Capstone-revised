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
    [Header("Item Properties")]
    public GameObject itemPrefab;
    public string itemName = "";
    public string itemDescription = "";
    public Sprite itemIcon;
    public ItemType itemType;
    public bool isUnlock;
    public bool isCollected;

    [Header("Compatibility and Conditions")]
    public string tagName;
    public List<string> compatibleTags;

    [Header("State Management")]
    public List<ItemState> states = new List<ItemState>();
    public int currentStateIndex = 0;
    public ItemState CurrentState => states[currentStateIndex];

    [Header("Temperature")]
    public bool hasTemperature;
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
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].conditions.itemNameRequirement == itemRequirement)
            {
                currentStateIndex = i;
                Debug.Log("Condition met! Transitioning to the next state.");
                return;
            }

            Debug.LogWarning("Condition not met." + states[i].conditions.itemNameRequirement);
        }

        Debug.LogWarning(
            $"{this.itemName}: No matching state found for item requirement: {itemRequirement}"
        );
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
            hasTemperature = this.hasTemperature,
            currentTemperature = this.currentTemperature,
            minTemperature = this.minTemperature,
            maxTemperature = this.maxTemperature,
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
    public string stateName;
    public Sprite sprite;
    public string description;
    public Conditions conditions;
    public float Temperature;
}

[System.Serializable]
public class Conditions
{
    public string itemNameRequirement; // Item required to trigger this condition
}
