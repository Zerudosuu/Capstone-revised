using UnityEngine;
using UnityEngine.Events;

public class HeatReceiver : MonoBehaviour
{
    public UnityEvent OnBoilingPointReached;
    private ItemReaction itemReaction;

    [Header("Particle System")] [SerializeField]
    private GameObject boilParticle;

    [SerializeField] private GameObject steamParticle;

    private bool hasStartedBoiling = false;
    private bool hasStartedSteaming = false;

    public void Start()
    {
        itemReaction = GetComponent<ItemReaction>();
    }

    public void ReceiveHeat(float heatAmount)
    {
        float oldTemperature = itemReaction.item.currentTemperature;
        itemReaction.item.currentTemperature = Mathf.Min(
            itemReaction.item.currentTemperature + heatAmount,
            itemReaction.item.maxTemperature
        );

        // Trigger steam at boiling point if not already triggered
        if (itemReaction.item.currentTemperature >= 100f && !hasStartedSteaming)
        {
            hasStartedSteaming = true;
            OnBoilingPointReached?.Invoke();
            Debug.Log($"{gameObject.name} has reached boiling point!");

            steamParticle.GetComponent<ParticleSystem>().Play();
        }
        // Trigger boiling effect if not already triggered and temp is above 60
        else if (itemReaction.item.currentTemperature >= 60f && !hasStartedBoiling)
        {
            hasStartedBoiling = true;
            boilParticle.GetComponent<ParticleSystem>().Play();
        }
    }
}