using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeatReceiver : MonoBehaviour
{
    public UnityEvent OnBoilingPointReached;
    private ItemReaction itemReaction;

    public void Start()
    {
        itemReaction = GetComponent<ItemReaction>();
    }

    public void ReceiveHeat(float heatAmount)
    {
        float oldTemperature = itemReaction.item.currentTemperature;
        itemReaction.item.currentTemperature = Mathf.Min(itemReaction.item.currentTemperature + heatAmount,
            itemReaction.item.maxTemperature);

        // Trigger event if boiling point is reached
        if (itemReaction.item.currentTemperature >= 100f && oldTemperature < 100f)
        {
            OnBoilingPointReached?.Invoke();
            Debug.Log($"{gameObject.name} has reached boiling point!");
        }
    }
}