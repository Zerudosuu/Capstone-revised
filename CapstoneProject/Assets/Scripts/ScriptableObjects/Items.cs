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
    public float temperature;
    public bool hasTemperature;
    public float currentTemperature;

    public enum ItemType
    {
        Equipment,
        Chemical,
    }

    // [Header("Reactions")]
    // public UnityEvent OnReact; // Triggered when the item reacts to external stimuli

    // public void ApplyReaction(float temperatureChange)
    // {
    //     if (hasTemperature)
    //     {
    //         currentTemperature += temperatureChange;
    //         OnReact?.Invoke();
    //     }
    // }

    // public void TriggerReaction(Item otherItem, Transform parentTransform)
    // {
    //     foreach (Reaction reaction in reactions)
    //     {
    //         if (reaction.triggers.Contains(otherItem.itemName))
    //         {
    //             // Apply temperature change
    //             currentTemperature += reaction.temperatureChange;

    //             // Instantiate resulting item if applicable
    //             if (reaction.resultingItemPrefab != null)
    //             {
    //                 UnityEngine.Object.Instantiate(
    //                     reaction.resultingItemPrefab,
    //                     parentTransform.position,
    //                     Quaternion.identity
    //                 );
    //             }

    //             // Trigger visual effects
    //             if (reaction.visualEffectPrefab != null)
    //             {
    //                 UnityEngine.Object.Instantiate(
    //                     reaction.visualEffectPrefab,
    //                     parentTransform.position,
    //                     Quaternion.identity
    //                 );
    //             }

    //             // Play animation
    //             if (reaction.animationClip != null)
    //             {
    //                 Animator animator = parentTransform.GetComponent<Animator>();
    //                 if (animator != null)
    //                 {
    //                     animator.Play(reaction.animationClip.name);
    //                 }
    //             }

    //             OnReact?.Invoke(); // Broadcast reaction event
    //             Debug.Log(
    //                 $"{itemName} reacted with {otherItem.itemName} via {reaction.reactionName}"
    //             );
    //             return; // Stop after first matching reaction
    //         }
    //     }

    //     Debug.LogWarning($"No matching reaction for {otherItem.itemName} with {itemName}");
    // }

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
            temperature = this.temperature,
            hasTemperature = this.hasTemperature,
            currentTemperature = this.currentTemperature,
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
    public string stateName; // Name of the state (e.g., "Lamp with Alcohol")
    public Sprite sprite;
    public string description; // Description of the state
    public Conditions conditions; // Conditions to transition to this state
}

[System.Serializable]
public class Conditions
{
    public string itemNameRequirement; // Item required to trigger this condition
}
