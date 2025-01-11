using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemReaction : MonoBehaviour, IDropHandler
{
    DragableItem dragableItem;
    public Item item; // Current item (state-based)
    ExperimentManager experimentManager;
    ExperimentObjectManager experimentObjectManagerManager;

    public List<Reaction> reactions; // List of reactions for the item
    public UnityItemReaction OnItemReaction;

    void Start()
    {
        experimentManager = FindObjectOfType<ExperimentManager>();
        dragableItem = GetComponent<DragableItem>();
        experimentObjectManagerManager = FindObjectOfType<ExperimentObjectManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropItem = eventData.pointerDrag;
        DragableItem draggable = dropItem.GetComponent<DragableItem>();

        // Check compatibility
        if (
            item.compatibleTags.Contains(draggable.TagName)
            && gameObject.GetComponent<DragableItem>().placeInSlot
        )
        {
            print($"Dropped item tag: {draggable.TagName}");

            // Switch state
            item.SwitchToState(draggable.name);
            experimentManager.UpdateItemPrefab(this.gameObject, draggable.name);

            // Check for reactions
            CheckReactions(item.CurrentState.stateName, draggable);

            // Step validation
            StepManager stepManager = FindObjectOfType<StepManager>();
            if (stepManager != null)
            {
                stepManager.ValidateAndCompleteSubStep(draggable.name);
            }
        }
    }

    private void CheckReactions(string currentStateName, DragableItem draggable)
    {
        foreach (Reaction reaction in reactions) // Assuming `item.reactions` is a list of Reaction objects1
        {
            // Check if the item's state matches and the trigger is valid
            if (
                reaction.CheckStateName(currentStateName)
                && reaction.triggers.Contains(draggable.name)
            )
            {
                Debug.Log($"Reaction triggered: {reaction.reactionName}");

                // TriggerReaction(reaction, draggable);
                return; // Reaction triggered, exit the loop
            }
        }

        Debug.LogWarning(
            $"No valid reaction found for state: {currentStateName} with dropped item: {draggable.name}"
        );
    }

    private void TriggerReaction(Reaction reaction, DragableItem draggable)
    {
        Debug.Log($"Reaction triggered: {reaction.reactionName}");

        // Apply temperature change
        if (reaction.temperatureChange != 0)
        {
            item.currentTemperature += reaction.temperatureChange;
            Debug.Log($"Temperature changed to: {item.currentTemperature}");
        }

        // Instantiate resulting item prefab
        if (reaction.resultingItemPrefab != null && reaction.changePrefab)
        {
            Instantiate(reaction.resultingItemPrefab, transform.position, Quaternion.identity);
        }

        // Instantiate visual effect
        if (reaction.visualEffectPrefab != null)
        {
            Instantiate(reaction.visualEffectPrefab, transform.position, Quaternion.identity);
        }

        // Play reaction animation
        if (reaction.animationClip != null)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(reaction.animationClip.name);
            }
        }

        // Trigger UnityEvent
        OnItemReaction?.Invoke(item);
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }
}

public class UnityItemReaction : UnityEvent<Item> { }

[System.Serializable]
public class Reaction
{
    public string needStateName;
    public string reactionName;
    public List<string> triggers; // Items or tags that trigger this reaction
    public bool changePrefab;
    public GameObject resultingItemPrefab; // Resulting item (if applicable)
    public float temperatureChange; // Change in temperature
    public GameObject visualEffectPrefab; // Optional visual effect (e.g., steam, frost)
    public AnimationClip animationClip; // Optional animation for the reaction

    public bool CheckStateName(string stateName)
    {
        if (stateName == needStateName)
        {
            return true;
        }
        else
            return false;
    }
}

// TODO: TEST THE SCRIPT



#region RESERVE

// // Check if the dropped item is compatible with the slot
// if (
//     item.compatibleTags.Contains(draggable.tagName)
//     && gameObject.GetComponent<DragableItem>().placeInSlot
// )
// {
//     // Check if the dropped item satisfies the current state's conditions
//     List<Conditions> conditions = item.CurrentState.conditions;

//     bool isConditionMet = false;

//     string interactionType = "";

//     if (conditions.Count > 0)
//     {
//         foreach (Conditions condition in conditions)
//         {
//             if (condition.itemNameRequirements.Contains(draggable.TagName))
//             {
//                 isConditionMet = true;
//                 interactionType = condition.typeOfInteraction;

//                 break; // Condition met, no need to check further
//             }
//         }
//     }

//     if (isConditionMet)
//     {
//         Debug.Log($"Condition met! Transitioning {item.itemName} to the next state.");
//         StepManager stepManager = FindObjectOfType<StepManager>();
//         stepManager.ValidateAndCompleteSubStep(draggable.name);

//
//         HandleInteraction(interactionType, draggable);
//     }
//     else
//     {
//         Debug.LogWarning(
//             $"{draggable.name} does not meet the conditions for {item.itemName}."
//         );
//         draggable.transform.position = draggable.originalPosition; // Reset item position
//     }
// }
// else
// {
//     // Debug.LogWarning("Dropped item is not compatible with this slot.");
//     // draggable.transform.position = draggable.originalPosition; // Reset item position

//     if (transform.parent.GetComponent<ScrollViewSlot>())
//     {
//         ScrollViewSlot scrollViewSlot = transform.parent.GetComponent<ScrollViewSlot>();
//         scrollViewSlot.OnDrop(eventData);
//     }
// }


// public void SwitchStateAfterMeasurement(DragableItem draggable)
// {
//     item.SwitchToNextState();

//     Debug.Log("Switched to the next state after measurement.");
// }

// private void HandleInteraction(string interactionType, DragableItem draggable)
// {
//     switch (interactionType)
//     {
//         case "ignition":
//             // Switch state first
//             item.SwitchToNextState();
//             Destroy(draggable.gameObject);

//             Debug.Log(
//                 $"Ignited {item.itemName} and transitioned to {item.CurrentState.stateName}."
//             );
//             break;

//         case "pouring":

//             experimentManager.MeterPanel.SetActive(true);

//             MixingComponent mixingComponent =
//                 experimentManager.MeterPanel.GetComponentInChildren<MixingComponent>();
//             mixingComponent.SetItem(item);
//             mixingComponent.StartMeasurement();

//             // Pass this ItemReaction to the MeterPanelManager
//             experimentManager
//                 .MeterPanel.GetComponent<MeterPanelManager>()
//                 .SetItemReaction(this, draggable);
//             break;

//         case "heating":
//             Debug.Log($"Heating {item.itemName}.");
//             // item.HeatItem(draggable.item.measuredValue); // Apply heating
//             break;

//         case "mixing":
//             Debug.Log($"Mixing {draggable.name} with {item.itemName}.");
//             experimentManager.MeterPanel.SetActive(true);
//             break;

//         // case "measuring":
//         //     Debug.Log(message: $"Measuring {item.itemName}.");
//         //     item.measuredValue = draggable.item.measuredValue; // Apply measurement
//         //     SwitchStateAfterMeasurement(draggable);
//         //     break;

//         case "cold":
//             Debug.Log($"Cooling {item.itemName}.");
//             // item.CoolItem(draggable.item.measuredValue); // Apply cooling
//             break;

//         default:
//             Debug.LogWarning($"Unhandled interaction type: {interactionType}.");
//             break;
//     }
// }
#endregion
