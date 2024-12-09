using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReaction : MonoBehaviour, IDropHandler
{
    DragableItem dragableItem;

    public Item item;
    ExperimentManager experimentManager;

    ExperimentObjectManager experimentObjectManagerManager;

    void Start()
    {
        experimentManager = FindObjectOfType<ExperimentManager>();
        dragableItem = GetComponent<DragableItem>();

        experimentObjectManagerManager = FindObjectOfType<ExperimentObjectManager>();
        item = dragableItem.item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropItem = eventData.pointerDrag; // The item being dropped
        DragableItem draggable = dropItem.GetComponent<DragableItem>();

        if (draggable == null || draggable.item == null || item == null)
        {
            Debug.LogWarning("Invalid draggable item or target item.");
            return;
        }

        // Check if the dropped item is compatible with the slot
        if (
            item.compatibleTags.Contains(draggable.item.tagName)
            && gameObject.GetComponent<DragableItem>().placeInSlot
        )
        {
            // Check if the dropped item satisfies the current state's conditions
            List<Conditions> conditions = item.CurrentState.conditions;

            bool isConditionMet = false;

            string interactionType = "";

            if (conditions.Count > 0)
            {
                foreach (Conditions condition in conditions)
                {
                    if (condition.itemNameRequirements.Contains(draggable.item.itemName))
                    {
                        isConditionMet = true;
                        interactionType = condition.typeOfInteraction;

                        break; // Condition met, no need to check further
                    }
                }
            }

            if (isConditionMet)
            {
                Debug.Log($"Condition met! Transitioning {item.itemName} to the next state.");

                // TODO: Check if the type of interaction is Pouring then to this if not do others

                HandleInteraction(interactionType, draggable);
            }
            else
            {
                Debug.LogWarning(
                    $"{draggable.item.itemName} does not meet the conditions for {item.itemName}."
                );
                draggable.transform.position = draggable.originalPosition; // Reset item position
            }
        }
        else
        {
            Debug.LogWarning("Dropped item is not compatible with this slot.");
            draggable.transform.position = draggable.originalPosition; // Reset item position
        }
        // if (draggable.name == "Water" && item.CurrentState.stateName == "Empty")
        // {

        // }
        // else if (
        //     item.compatibleTags.Contains(draggable.item.tagName)
        //     && gameObject.GetComponent<DragableItem>().placeInSlot
        // )
        // {
        //     print("Set to fire");
        // }

        // Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
    }

    public void SwitchStateAfterMeasurement(DragableItem draggable)
    {
        item.SwitchToNextState();
        experimentManager.UpdateItemPrefab(this.gameObject);
        Debug.Log("Switched to the next state after measurement.");
        Destroy(draggable.gameObject);
    }

    private void HandleInteraction(string interactionType, DragableItem draggable)
    {
        switch (interactionType)
        {
            case "ignition":
                // Switch state first
                item.SwitchToNextState();

                // Ensure the new state's prefab is valid
                if (item.CurrentState.statePrefab == null)
                {
                    Debug.LogError($"State prefab is null for {item.itemName}'s next state.");
                    break;
                }

                // Update the prefab in place
                experimentManager.UpdateItemPrefabInPlace(gameObject);

                // Clean up the draggable object
                Destroy(draggable.gameObject);

                Debug.Log(
                    $"Ignited {item.itemName} and transitioned to {item.CurrentState.stateName}."
                );
                break;

            case "pouring":

                experimentManager.MeterPanel.SetActive(true);

                MixingComponent mixingComponent =
                    experimentManager.MeterPanel.GetComponentInChildren<MixingComponent>();
                mixingComponent.SetItem(item);
                mixingComponent.StartMeasurement();

                // Pass this ItemReaction to the MeterPanelManager
                experimentManager
                    .MeterPanel.GetComponent<MeterPanelManager>()
                    .SetItemReaction(this, draggable);
                break;

            case "heating":
                Debug.Log($"Heating {item.itemName}.");
                // item.HeatItem(draggable.item.measuredValue); // Apply heating
                break;

            case "mixing":
                Debug.Log($"Mixing {draggable.item.itemName} with {item.itemName}.");
                experimentManager.MeterPanel.SetActive(true);
                break;

            case "measuring":
                Debug.Log(message: $"Measuring {item.itemName}.");
                item.measuredValue = draggable.item.measuredValue; // Apply measurement
                SwitchStateAfterMeasurement(draggable);
                break;

            case "cold":
                Debug.Log($"Cooling {item.itemName}.");
                // item.CoolItem(draggable.item.measuredValue); // Apply cooling
                break;

            default:
                Debug.LogWarning($"Unhandled interaction type: {interactionType}.");
                break;
        }
    }
}
