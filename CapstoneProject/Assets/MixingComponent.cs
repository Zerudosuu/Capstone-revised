using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; // Required for Pointer events
using UnityEngine.UI;

public class MixingComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Item ItemToMeasure; // The item to measure (optional)

    [SerializeField]
    private GameObject ItemObject; // Visual representation of the item (optional)

    [SerializeField]
    private float MeasuredValue = 0f; // Current measured value

    [SerializeField]
    private float TargetValue = 60f; // Target value the player needs to hit

    [SerializeField]
    private float Tolerance = 5f; // Allowable range for a correct measurement

    [SerializeField]
    private Slider Slider; // The UI Slider representing the meter

    [SerializeField]
    private float FillRate = 20f; // Base fill rate for the meter

    [SerializeField]
    private float MaxFillRate = 100f; // Maximum fill rate when holding longer

    [SerializeField]
    private float AccelerationRate = 10f; // Acceleration of the fill rate over time

    private bool IsMeasuring = false; // Flag to determine if the player is measuring
    private float CurrentFillRate = 0f; // Current rate at which the meter is filling

    private void Start()
    {
        ResetMeasurement();
    }

    private void Update()
    {
        if (IsMeasuring)
        {
            // Increase the fill rate over time (simulate acceleration)
            CurrentFillRate = Mathf.Min(
                CurrentFillRate + AccelerationRate * Time.deltaTime,
                MaxFillRate
            );

            // Increase the measured value
            MeasuredValue += CurrentFillRate * Time.deltaTime;

            // Clamp the measured value within bounds (e.g., 0 to 100)
            MeasuredValue = Mathf.Clamp(MeasuredValue, 0f, 100f);

            // Update the slider to reflect the measured value
            UpdateSlider();
        }
    }

    private void UpdateSlider()
    {
        if (Slider != null)
        {
            Slider.value = MeasuredValue / 100f; // Normalize value for slider (0 to 1)
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start measuring when the button is pressed
        IsMeasuring = true;
        CurrentFillRate = FillRate; // Reset fill rate to base value
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop measuring when the button is released
        IsMeasuring = false;

        // Provide feedback based on the measured value
        CheckMeasurement();
    }

    private void CheckMeasurement()
    {
        if (Mathf.Abs(MeasuredValue - TargetValue) <= Tolerance)
        {
            Debug.Log("Perfect measurement!");
            // Trigger success logic
        }
        else if (MeasuredValue < TargetValue)
        {
            Debug.Log("Too little! Try again.");
        }
        else
        {
            Debug.Log("Too much! Try again.");
        }

        // Reset for retry
        ResetMeasurement();
    }

    private void ResetMeasurement()
    {
        MeasuredValue = 0f;
        CurrentFillRate = FillRate; // Reset fill rate
        UpdateSlider();
    }
}
