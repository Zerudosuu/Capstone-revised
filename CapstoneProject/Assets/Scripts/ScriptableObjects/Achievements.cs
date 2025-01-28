using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "CreteAchievementData", order = 0)]
public class Achievements : ScriptableObject
{
    public List<Achievement> achievements;
}

[System.Serializable]
public class Achievement
{
    public string id; // Unique ID for the achievement
    public string description; // Achievement description
    public int target; // Target value to unlock
    public bool isUnlocked; // Status of the achievement
    public Sprite AchivementIcon;
    public string InGameMessage;
    public static event Action OnAchievementReached; // Optional: Events triggered upon unlocking
    public int progress; // Current progress (not saved directly in GameData)
    public string QuestIdRequirement;


    public Achievement Clone()
    {
        Achievement clone = new Achievement
        {
            id = this.id,
            description = this.description,
            target = this.target,
            isUnlocked = this.isUnlocked,
            AchivementIcon = this.AchivementIcon,
            InGameMessage = this.InGameMessage,
            QuestIdRequirement = this.QuestIdRequirement,
            progress = this.progress
        };
        return clone;
    }

    public bool CheckCondition()
    {
        return progress >= target;
    }

    public void ResetProgress()
    {
        progress = 0;
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
        OnAchievementReached?.Invoke(); // Trigger any additional events
    }
}