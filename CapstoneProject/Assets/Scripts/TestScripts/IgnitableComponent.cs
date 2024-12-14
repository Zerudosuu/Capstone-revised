using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IgnitableComponent : MonoBehaviour, IDropHandler
{
    public void Ignite(float intensity)
    {
        Debug.Log($"Object ignited with intensity {intensity}.");
        // Add visual and sound effects here
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped!");
        // Perform the raycast
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject target = hit.collider.gameObject;
            Debug.Log($"Hit object: {target.name}");
        }
    }
}
