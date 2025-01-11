using TMPro;
using UnityEngine;

public class MeasureScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI temperatureDisplay; // Reference to the TextMeshProUGUI for temperature

    public void Start()
    {
        temperatureDisplay.text = "";
    }

    public void DisplayTemperature(float temperature)
    {
        if (temperatureDisplay != null)
        {
            temperatureDisplay.text = $"{temperature}°C";
        }
    }

    public void ClearTemperature()
    {
        if (temperatureDisplay != null)
        {
            temperatureDisplay.text = "";
        }
    }
}
