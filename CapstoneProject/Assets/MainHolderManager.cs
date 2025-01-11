using System.Collections.Generic;
using UnityEngine;

public class MainHolderManager : MonoBehaviour
{
    private Dictionary<Slot, ItemReaction> registeredItems = new Dictionary<Slot, ItemReaction>();

    public void RegisterSlot(Slot slot, ItemReaction item)
    {
        if (!registeredItems.ContainsKey(slot))
        {
            registeredItems.Add(slot, item);
            Debug.Log($"Registered {item.name} in {slot.name}");
        }
    }

    public void UnregisterSlot(Slot slot)
    {
        if (registeredItems.ContainsKey(slot))
        {
            registeredItems.Remove(slot);
            Debug.Log($"Unregistered slot {slot.name}");
        }
    }

    public void OnHeatSourceActivated(ItemReaction heatSource)
    {
        foreach (var pair in registeredItems)
        {
            var item = pair.Value;

            if (item != null && item != heatSource && item.item.hasTemperature)
            {
                // heatSource.item.EmitHeat(item.item, Time.deltaTime);
                Debug.Log($"{heatSource.item.itemName} is heating {item.item.itemName}");
            }
        }
    }
}
