using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSource : MonoBehaviour
{
    public float heatEmissionRate = 1f; // Degrees per second emitted by the lamp
    public float range = 2f; // Effective range of the heat source
    public Transform heatcenterPosition;
    private bool isLit = false;
    public Collider2D[] collidersCollected;
    private CircleCollider2D heatCollider;

    private void Awake()
    {
        // Ensure a CircleCollider2D is present
        heatCollider = GetComponent<CircleCollider2D>();
        if (heatCollider == null)
        {
            heatCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        heatCollider.offset = heatcenterPosition.localPosition;
        heatCollider.isTrigger = true; // Make sure it's a trigger
        heatCollider.radius = range; // Set the range
    }

    public void Ignite()
    {
        if (!isLit)
        {
            isLit = true;
            Debug.Log($"{gameObject.name} is now lit and emitting heat.");
            StartCoroutine(EmitHeat());
        }
    }

    public void Extinguish()
    {
        if (isLit)
        {
            isLit = false;
            Debug.Log($"{gameObject.name} has been extinguished.");
            StopAllCoroutines();
        }
    }

    private IEnumerator EmitHeat()
    {
        while (isLit)
        {
            // Collect all objects overlapping with the collider
            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(heatcenterPosition.position, range);

            foreach (Collider2D col in nearbyObjects)
            {
                HeatReceiver receiver = col.GetComponent<HeatReceiver>();

                if (receiver != null)
                {
                    receiver.ReceiveHeat(heatEmissionRate * Time.deltaTime);
                }
            }

            yield return null; // Wait for the next frame
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a visual representation of the heat range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(heatcenterPosition.position, range);
    }
}