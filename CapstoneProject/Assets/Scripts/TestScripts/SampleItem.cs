using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireComponent : ReactionBaseClass
{

    public float HeatIntensity;

    private void Awake()
    {
        InteractionType = InteractionType.Fire;
        InteractableObjects = new List<InteractableObjectType>
        {
            InteractableObjectType.Flammable,
            InteractableObjectType.HeatSensitive,
        };
    }

    public override void InteractWith(GameObject other)
    {
        if (other.TryGetComponent(out IgnitableComponent ignitableComponent))
        {
            ignitableComponent.Ignite(HeatIntensity);
        }

        if (other.TryGetComponent(out HeatSensitiveComponent heatSensitiveComponent))
        {
            heatSensitiveComponent.Heat(HeatIntensity);
        }
    }


}
