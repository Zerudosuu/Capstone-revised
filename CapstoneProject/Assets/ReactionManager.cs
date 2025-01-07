using System;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    public static ReactionManager Instance { get; private set; }

    public event Action OnCool;
    public event Action OnHeat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void TriggerCool()
    {
        Debug.Log("Cooling event triggered.");
        OnCool?.Invoke();
    }

    public void TriggerHeat()
    {
        Debug.Log("Heating event triggered.");
        OnHeat?.Invoke();
    }
}
