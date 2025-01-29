using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TabContainerManager : MonoBehaviour, IData
{
    [SerializeField] private TMP_Dropdown dropdown;
    public GameObject achivementsContainer;
    public List<ProfileAchievement> profileAchievements;

    public List<Achievement> achievementsData;
    public GameObject AchievementPrefab;

    private AchieveManager achieveManager;


    private void OnEnable()
    {
        if (AchieveManager.Instance != null)
        {
            achievementsData = AchieveManager.Instance.ClonedAchievements;
            AchieveManager.OnCompleteAchievement += UpdateAchievements;
        }

        UpdateAchievements(); // Refresh UI on enable
    }

    private void OnDisable()
    {
        if (AchieveManager.Instance != null)
        {
            AchieveManager.OnCompleteAchievement -= UpdateAchievements;
        }
    }


    void PopulateAchievements()
    {
        profileAchievements.Clear(); // Clear list to prevent duplicates

        foreach (Transform child in achivementsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Achievement achievement in achievementsData)
        {
            GameObject newAchievement = Instantiate(AchievementPrefab, achivementsContainer.transform);
            ProfileAchievement profileAchievement = newAchievement.GetComponent<ProfileAchievement>();
            profileAchievement.SetAchievement(achievement);

            profileAchievements.Add(profileAchievement); // Add to list after instantiation
        }
    }

    public void GetValueOnDropDown()
    {
        int index = dropdown.value;
        filterAchievements(index);
    }

    private void filterAchievements(int number)
    {
        foreach (ProfileAchievement profileAchievement in profileAchievements)
        {
            bool unlocked = profileAchievement.Achievement.isUnlocked;

            if (number == 0) // Show only locked achievements
            {
                profileAchievement.gameObject.SetActive(!unlocked);
            }
            else if (number == 1) // Show only unlocked achievements
            {
                profileAchievement.gameObject.SetActive(unlocked);
            }
        }
    }

    private void UpdateAchievements()
    {
        // Clear existing UI elements
        foreach (Transform child in achivementsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        profileAchievements.Clear();

        // Re-populate achievements UI
        PopulateAchievements();
        filterAchievements(dropdown.value); // Apply the current filter
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.Achievements != null && gameData.Achievements.Count > 0)
        {
            achievementsData = gameData.Achievements;
        }
    }

    public void SavedData(GameData gameData)
    {
        gameData.Achievements = this.achievementsData;
    }
}