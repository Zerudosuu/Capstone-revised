using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Fire,
    Water,
    Acid,
    Base,

    Flammable,
}

public enum InteractableObjectType
{
    Flammable,
    HeatSensitive,
    Acidic,
    Basic,
}

public abstract class ReactionBaseClass : MonoBehaviour
{
    public InteractionType InteractionType;

    public List<InteractableObjectType> InteractableObjects;

    public abstract void InteractWith(GameObject other);
}
