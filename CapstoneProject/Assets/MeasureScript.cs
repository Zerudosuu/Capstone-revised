using System;
using TMPro;
using UnityEngine;

public class MeasureScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI temperatureDisplay; // Reference to the TextMeshProUGUI for temperature
    private ItemReaction currentItemReaction;

    private void Start()
    {
        temperatureDisplay.gameObject.SetActive(false);
    }

    private void DisplayTemperature(float temperature)
    {
        if (temperatureDisplay != null)
        {
            temperatureDisplay.gameObject.SetActive(true);
            temperatureDisplay.text = $"{temperature}°C";
        }
    }

    private void ClearTemperature()
    {
        if (temperatureDisplay != null)
        {
            temperatureDisplay.gameObject.SetActive(false);
            temperatureDisplay.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentItemReaction = other.GetComponent<ItemReaction>();
        if (currentItemReaction != null && currentItemReaction.item.hasTemperature)
        {
            currentItemReaction.OnTemperatureChanged += DisplayTemperature;
            DisplayTemperature(currentItemReaction.item.currentTemperature);


            // Step validation
            StepManager stepManager = FindObjectOfType<StepManager>();
            if (stepManager != null)
            {
                stepManager.ValidateAndCompleteSubStep(other.gameObject.name);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentItemReaction != null && other.gameObject == currentItemReaction.gameObject)
        {
            currentItemReaction.OnTemperatureChanged -= DisplayTemperature;
            ClearTemperature();
            currentItemReaction = null;
        }
    }
}