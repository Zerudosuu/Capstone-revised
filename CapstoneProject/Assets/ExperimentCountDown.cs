using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentCountDown : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Button SkipButton;

    public TextMeshProUGUI countdownDisplay;
    private Coroutine countdownCoroutine;
    [SerializeField] private StepManager _stepManager;

    public string currentActionType = "wait"; // Default to wait
    public string currentItemName = ""; // The item used for the action
    public GameObject SlotToGetItem;
    public GameObject SlotChild;

    public float TargetTemperature = 0;
    public static event Action onTimerisRunning;
    private ItemReaction itemReaction;

    private void Start()
    {
        SkipButton.onClick.AddListener(() => CompleteStep());
    }

    public void SetTime(float time, string itemName, string actionType, float targetTemperature = 0)
    {
        timeRemaining = time;
        currentActionType = actionType; // Set the action type (shake, stir, wait)
        currentItemName = itemName; // Set the item being 
        TargetTemperature = targetTemperature;
        onTimerisRunning?.Invoke();
        StartCountdown();
    }

    public GameObject GetSlotToGetItem()
    {
        SlotChild = SlotToGetItem.transform.GetChild(0).gameObject;
        return SlotChild;
    }

    private void StartCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        itemReaction = GetSlotToGetItem()?.GetComponent<ItemReaction>();
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        timerIsRunning = true;

        while (timeRemaining > 0)
        {
            countdownDisplay.text = $"{Mathf.FloorToInt(timeRemaining / 60)}:{Mathf.FloorToInt(timeRemaining % 60):00}";
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
        }

        timerIsRunning = false;
        CompleteStep();
    }

    private void CompleteStep()
    {
        onTimerisRunning?.Invoke();
        countdownDisplay.text = "00:00";
        gameObject.SetActive(false);
        timerIsRunning = false;
        Debug.Log($"Completing the {currentActionType} step with item {currentItemName}...");
        _stepManager.ValidateAndCompleteSubStep(currentItemName, currentActionType); // Pass item name for validation

        if (itemReaction != null && itemReaction.item.hasTemperature)
        {
            itemReaction.item.currentTemperature = TargetTemperature;

            Animator anim = itemReaction.gameObject.GetComponent<Animator>();

            if (anim != null)
                anim.Play("Idle");
        }
    }
}