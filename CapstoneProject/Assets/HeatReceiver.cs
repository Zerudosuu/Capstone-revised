using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeatReceiver : MonoBehaviour
{
    public UnityEvent OnBoilingPointReached;
    private ItemReaction itemReaction;

    [Header("Particle System")]
    [SerializeField] private GameObject boilParticle;

    public void Start()
    {
        boilParticle.SetActive(false);  
        itemReaction = GetComponent<ItemReaction>();
    }

    public void ReceiveHeat(float heatAmount)
    {
        boilParticle.SetActive(true);

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