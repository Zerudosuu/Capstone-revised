using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSensitiveComponent : MonoBehaviour
{
    public void Heat(float intensity)
    {
        Debug.Log($"Object heated with intensity {intensity}.");
        // Implement heat sensitivity logic here, e.g.,
        // - Change material color or texture
        // - Play sizzling sound effect
        // - Emit smoke particles
    }
}
