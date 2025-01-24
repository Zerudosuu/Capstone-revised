using TMPro;
using UnityEngine;

public class MeasureScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI temperatureDisplay;
    private ItemReaction currentItemReaction;
    private StepManager stepManager;

    private void Start()
    {
        temperatureDisplay.gameObject.SetActive(false);
        stepManager = FindObjectOfType<StepManager>();
    }

    private void DisplayTemperature(float temperature)
    {
        if (temperatureDisplay != null)
        {
            temperatureDisplay.gameObject.SetActive(true);
            temperatureDisplay.text = $"{Mathf.RoundToInt(temperature)}°C";
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
        if (currentItemReaction == null)
        {
            ItemReaction itemReaction = other.GetComponent<ItemReaction>();
            if (itemReaction != null && itemReaction.item.hasTemperature)
            {
                currentItemReaction = itemReaction;
                currentItemReaction.OnTemperatureChanged += DisplayTemperature;
                DisplayTemperature(Mathf.RoundToInt(currentItemReaction.item.currentTemperature));

                // Check if this is the correct step
                if (stepManager != null && stepManager.requiredAction == "drag")
                {
                    stepManager.ValidateAndCompleteSubStep(other.gameObject.name);
                }
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