using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AchieveManager : MonoBehaviour, IData
{
    public static AchieveManager Instance { get; private set; }
    public Achievements achievementData;

    public List<Achievement> ClonedAchievements = new List<Achievement>();
    public GameObject AchievementPrefab;

    public static event Action OnCompleteAchievement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        CloneAchievements(); // Ensure achievements are cloned on initialization
    }


    void CloneAchievements()
    {
        ClonedAchievements.Clear();

        foreach (Achievement achievement in achievementData.achievements)
        {
            ClonedAchievements.Add(achievement.Clone());
        }
    }

    public void AddAchievementProgress(string achievementID, int value)
    {
        Achievement achievement = ClonedAchievements.Find(a => a.id == achievementID);
        if (achievement != null)
        {
            achievement.AddProgress(value);
            if (achievement.isUnlocked)
            {
                Debug.Log($"Achievement {achievement.id} unlocked!");

                // Instantiate achievement popup
                GameObject popup = Instantiate(AchievementPrefab, FindObjectOfType<Canvas>().transform);
                popup.GetComponent<AchievementPopUp>().SetAchievement(achievement);

                // Notify listeners about the achievement update
                OnCompleteAchievement?.Invoke();
            }
        }
    }


    public void LoadData(GameData gameData)
    {
        if (gameData.Achievements != null && gameData.Achievements.Count > 0)
        {
            ClonedAchievements = gameData.Achievements;
        }
        else
        {
            CloneAchievements();
        }
    }

    public void SavedData(GameData gameData)
    {
        if (gameData.Achievements != null && gameData.Achievements.Count == 0)
        {
            gameData.Achievements = ClonedAchievements;
        }

        gameData.Achievements = ClonedAchievements;
    }
}