using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AchievementData", menuName = "CreteAchievementData", order = 0)]
public class Achievements : ScriptableObject
{
    public List<Achievement> achievements;
}

[System.Serializable]
public class Achievement
{
    public string id; // Unique ID for the achievement
    public string title; // Achievement title
    public string description; // Achievement description
    public int target; // Target value to unlock
    public bool isUnlocked; // Status of the achievement
    public UnityEvent onUnlock; // Optional: Events triggered upon unlocking

    [NonSerialized] public int progress; // Current progress (not saved directly in GameData)

    public bool CheckCondition()
    {
        return progress >= target;
    }

    public void AddProgress(int value)
    {
        if (!isUnlocked)
        {
            progress += value;
            if (CheckCondition())
            {
                Unlock();
            }
        }
    }

    private void Unlock()
    {
        isUnlocked = true;
        Debug.Log($"Achievement Unlocked: {title}");
        onUnlock?.Invoke(); // Trigger any additional events
    }
}