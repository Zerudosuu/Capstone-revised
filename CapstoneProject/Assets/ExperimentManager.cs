using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentManager : MonoBehaviour
{
    
    public GameObject slotPrefab; // Prefab for the item slot

    public GameObject MeterPanel;

    ExperimentObjectManager experimentObjectManager;

    ScrollViewSlot[] slots;

    public static event Action OnGameStart; 

    void Start()
    {
        MeterPanel.SetActive(false);
        currentScore.minValue = 0;
        currentScore.maxValue = 100;
        currentScore.value = score; // Initialize slider value

        experimentObjectManager = FindObjectOfType<ExperimentObjectManager>();

        // Get all slots as children of the item container
        slots = experimentObjectManager.ItemContainer.GetComponentsInChildren<ScrollViewSlot>();
    }

    public void UpdateScore(string zone)
    {
        switch (zone)
        {
            case "Yellow":
                score -= yellowPenalty;
                Debug.Log("Yellow Zone! -10 points");
                break;
            case "Red":
                score -= redPenalty;
                Debug.Log("Red Zone! -20 points");
                break;
            case "Green":
                if (score == 100f)
                {
                    score += greenBonus;
                    Debug.Log("Green Zone! Bonus +5 points");
                }
                else
                {
                    Debug.Log("Green Zone! No penalty");
                }

                break;
        }

        // Clamp score to a minimum of 0 and maximum of 100
        score = Mathf.Clamp(score, 0, 100);
        currentScore.value = score; // Update the score slider
    }
    
    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    public void UpdateItemPrefab(ItemReaction itemReaction, string ItemInteracted)
    {
        Debug.LogWarning(itemReaction.GetInstanceID());

        if (itemReaction != null && itemReaction.item.CurrentState != null)
        {
            bool conditionMet = false;

            // Iterate through all states of the item
            foreach (var state in itemReaction.item.states)
            {
                if (state.conditions.itemNameRequirement == ItemInteracted)
                {
                    // Condition met, switch to this state
                    itemReaction.item.currentStateIndex = itemReaction.item.states.IndexOf(state);
                    itemReaction.GetComponent<Image>().sprite = state.sprite;
                    itemReaction.transform.name = state.stateName;
                    conditionMet = true;
                    itemReaction.item.currentTemperature = state.Temperature;


                    Debug.Log($"Item updated to state: {state.stateName}");

                    itemReaction.PlayShake();
                    break;
                }
            }

            if (!conditionMet)
            {
                Debug.LogWarning(
                    $"Item {ItemInteracted} not compatible with any state conditions."
                );


                itemReaction.PlayPopUp();
            }
        }
        else
        {
            Debug.Log("ItemReaction or CurrentState is null. Cannot update prefab.");
        }
    }

    public ScrollViewSlot FindEmptySlot()
    {
        foreach (ScrollViewSlot slot in slots)
        {
            if (slot.transform.childCount == 0) // Check if slot is empty
            {
                return slot;
            }
        }

        return null; // No empty slots found
    }

    // public void TriggerAction()
    // {
    //     ReactionManager.Instance.TriggerHeat();
    // }
}