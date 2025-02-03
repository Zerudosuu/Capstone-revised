using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemReaction : MonoBehaviour, IDropHandler
{
    DragableItem dragableItem;
    public Item item = new Item(); // Current item (state-based)
    ExperimentManager experimentManager;
    ExperimentObjectManager experimentObjectManagerManager;
    private DragableItem ownDraggableItem;
    public List<Reaction> reactions; // List of reactions for the item


    public delegate void TemperatureChanged(float newTemperature);

    public event TemperatureChanged OnTemperatureChanged;


    public void SetTemperature(float newTemp)
    {
        if (Mathf.Abs(item.currentTemperature - newTemp) > 0.01f) // Avoid frequent tiny updates
        {
            item.currentTemperature = newTemp;
            OnTemperatureChanged?.Invoke(newTemp);
        }
    }

    void Start()
    {
        experimentManager = FindObjectOfType<ExperimentManager>();
        experimentObjectManagerManager = FindObjectOfType<ExperimentObjectManager>();
        ownDraggableItem = GetComponent<DragableItem>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            Debug.LogWarning("Dropped item is null.");
            return;
        }

        GameObject dropItem = eventData.pointerDrag;
        DragableItem draggable = dropItem.GetComponent<DragableItem>();
        ItemReaction droppedItem = dropItem.GetComponent<ItemReaction>();


        // Check if the current item is compatible with the dropped item
        if (
            item.compatibleTags.Contains(draggable.TagName) &&
            gameObject.GetComponent<DragableItem>().placeInSlot && item.states.Count > 0
        )
        {
            if (experimentObjectManagerManager != null && experimentObjectManagerManager.gameMode == GameMode.Lesson)
            {
                StepManager stepManager = FindObjectOfType<StepManager>();
                if (stepManager != null)
                {
                    if (stepManager.RequiredItemForTheStep(draggable.name))
                    {
                        stepManager.ValidateAndCompleteSubStep(draggable.name);
                        experimentManager.UpdateItemPrefab(this, draggable.name);
                        CheckReactions(item.CurrentState.stateName, draggable);
                    }
                    else
                    {
                        draggable.transform.position = draggable.originalPosition;
                    }
                }
            }
            else
            {
                experimentManager?.UpdateItemPrefab(this, draggable.name);

                CreativeObjectManager creativeObjectManager = FindObjectOfType<CreativeObjectManager>(true);
                creativeObjectManager?.UpdateItemPrefab(this, draggable.name);

                CheckReactions(item.CurrentState.stateName, draggable);
            }
        }
        else if (droppedItem.item.compatibleTags.Contains(gameObject.GetComponent<DragableItem>().TagName) &&
                 droppedItem != null && droppedItem.item.states.Count > 0)

        {
            if (experimentObjectManagerManager != null && experimentObjectManagerManager.gameMode == GameMode.Lesson)
            {
                StepManager stepManager = FindObjectOfType<StepManager>();
                if (stepManager != null)
                {
                    if (stepManager.RequiredItemForTheStep(draggable.name))
                    {
                        stepManager.ValidateAndCompleteSubStep(draggable.name);

                        experimentManager.UpdateItemPrefab(droppedItem, gameObject.GetComponent<DragableItem>().name);
                        CheckReactions(item.CurrentState.stateName, draggable);
                    }
                    else
                    {
                        draggable.transform.position = draggable.originalPosition;
                    }
                }
            }
            else
            {
                experimentManager?.UpdateItemPrefab(droppedItem, draggable.name);
                CheckReactions(item.CurrentState.stateName, draggable);
            }
        }
        else
        {
            ownDraggableItem.PopUpOnItemInvalid();
            draggable.transform.position = draggable.originalPosition;
            Debug.LogWarning("Dropped item is not compatible with this slot.");
        }
    }

    private void CheckReactions(string currentStateName, DragableItem draggable)
    {
        foreach (Reaction reaction in reactions) // Assuming `item.reactions` is a list of Reaction objects1
        {
            if (
                reaction.CheckStateName(currentStateName)
                && reaction.triggers.Contains(draggable.name) || reaction.triggers.Contains("stir") ||
                reaction.triggers.Contains("shake")
            )

            {
                Debug.Log($"Reaction triggered: {reaction.reactionName}");
                TriggerReaction(reaction, draggable);
                return;
            }
        }

        Debug.LogWarning($"No valid reaction found for state: {currentStateName} with dropped item: {draggable.name}");
    }

    private void TriggerReaction(Reaction reaction, DragableItem draggable)
    {
        Debug.Log($"Reaction triggered: {reaction.reactionName}");

        if (draggable.GetComponent<CoolComponent>())
        {
            CoolComponent coolComponent = draggable.GetComponent<CoolComponent>();
            coolComponent.CoolObject(this);
        }
        else if (draggable.GetComponent<Igniter>())
        {
            HeatSource heatSource = transform.GetComponent<HeatSource>();
            if (heatSource)
            {
                heatSource.Ignite();
                if (!reaction.changePrefab && reaction.ReactionSprite != null)
                {
                    gameObject.GetComponent<Image>().sprite = reaction.ReactionSprite;
                }
            }
        }

        if (reaction.changePrefab && reaction.resultingItemPrefab != null)
        {
            GameObject resultingItem =
                Instantiate(reaction.resultingItemPrefab, transform.position, Quaternion.identity);
            resultingItem.name = reaction.resultingItemPrefab.name;
            // Set parent correctly within UI
            resultingItem.transform.SetParent(transform.parent, false); // 'false' maintains local scale

            // Ensure it is at the correct hierarchy level
            resultingItem.transform.SetSiblingIndex(transform.GetSiblingIndex());

            // Get and initialize draggable component
            DragableItem draggableItem = resultingItem.GetComponent<DragableItem>();
            if (draggableItem != null)
            {
                draggableItem.originalPosition = transform.position;
                draggableItem.parentAfterDrag = transform.parent;
                draggableItem.canvas = GetComponentInParent<Canvas>();
            }

            Destroy(draggable.gameObject);
            Destroy(gameObject);
        }

        else if (reaction.ReactionSprite != null)
        {
            gameObject.GetComponent<Image>().sprite = reaction.ReactionSprite;
        }

        if (reaction.visualEffectPrefab)
        {
            Instantiate(reaction.visualEffectPrefab, transform.position, Quaternion.identity);
        }

        if (reaction.Animator)
        {
            reaction.Animator?.SetTrigger(reaction.TriggerAnimationName);
        }
    }

    public void SetItem(Item item)
    {
        this.item = item.Clone();
    }
}

public class UnityItemReaction : UnityEvent<Item>
{
}

[System.Serializable]
public class Reaction
{
    // Basic Information
    [Header("Basic Information")] public string needStateName;
    public string reactionName;
    public List<string> triggers; // Items or tags that trigger this reaction


    [Header("Prefab and Visuals")] public bool changePrefab;
    public Sprite ReactionSprite;
    public GameObject resultingItemPrefab; // Resulting item (if applicable)
    public GameObject visualEffectPrefab; // Optional visual effect (e.g., steam, frost)

    [Header("Animation")]
    // Animation
    public Animator Animator; // Optional animation for the reaction

    public string TriggerAnimationName;


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