using UnityEngine;
using UnityEngine.Events;

public class HeatReceiver : MonoBehaviour
{
    public UnityEvent OnBoilingPointReached;
    public ItemReaction itemReaction;

    [Header("Particle System")] [SerializeField]
    private GameObject boilParticle;

    [SerializeField] private GameObject steamParticle;


    private bool hasStartedBoiling = false;
    private bool hasStartedSteaming = false;

    public bool isReceivingHeat = false;

    private AudioManager audioManage;

    public void Start()
    {
        itemReaction = GetComponent<ItemReaction>();
        audioManage = FindObjectOfType<AudioManager>(true);
    }

    public void ReceiveHeat(float heatAmount)
    {
        float newTemperature = Mathf.Min(
            itemReaction.item.currentTemperature + heatAmount,
            itemReaction.item.maxTemperature
        );

        itemReaction.SetTemperature(newTemperature);


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
            audioManage.PlaySFX("Boil");
            boilParticle.GetComponent<ParticleSystem>().Play();
        }
    }

}