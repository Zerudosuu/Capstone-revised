using System.Collections;
using UnityEngine;

public class CoolComponent : MonoBehaviour
{
    public float coolingRate = 1f; // Degrees per second

    public void CoolObject(ItemReaction itemReaction)
    {
        if (itemReaction == null) return;

        // Start a coroutine to gradually reduce the temperature
        itemReaction.StartCoroutine(CoolingProcess(itemReaction));
    }

    private IEnumerator CoolingProcess(ItemReaction itemReaction)
    {
        Item item = itemReaction.item;

        while (item.currentTemperature > 0) // Stop cooling at 0°C or desired minimum temperature
        {
            float newTemp = item.currentTemperature - (coolingRate * Time.deltaTime);
            itemReaction.SetTemperature(newTemp);
            yield return null;
        }
    }
}