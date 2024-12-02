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
    public string itemName = "";
    public string itemDescription = "";
    public Sprite itemIcon;
    public ItemType itemType;
    public bool isUnlock;

    public bool isCollected;
    public float measuredValue = 0;

    // States
    public List<ItemState> itemStates = new List<ItemState>();
    public ItemState currentState;

    public enum ItemType
    {
        Equipment,
        Chemical,
    }

    public Item Clone()
    {
        return new Item
        {
            itemName = this.itemName,
            itemDescription = this.itemDescription,
            itemIcon = this.itemIcon,
            itemType = this.itemType,
            isUnlock = this.isUnlock,
            itemStates = new List<ItemState>(this.itemStates),
        };
    }
}

[System.Serializable]
public class ItemState
{
    public string stateName;
    public Sprite stateIcon; // Icon for this state
    public List<StateCondition> conditions; // Conditions to transition to next state
}

[System.Serializable]
public class StateCondition
{
    public string targetItemName; // The item it needs to interact with
    public InteractionType interactionType; // Type of interaction required

    public enum InteractionType
    {
        Collision,
        Mixing,
        Heating,
        Cooling,
        Measurement,
    }
}
