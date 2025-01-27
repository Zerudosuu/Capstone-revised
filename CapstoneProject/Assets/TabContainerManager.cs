using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TabContainerManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public GameObject achivementsContainer;
    public List<ProfileAchievement> profileAchievements;

    public List<Achievement> achievementsData;
    public GameObject AchievementPrefab;

    public void Start()
    {
        AchieveManager achieveManager = FindObjectOfType<AchieveManager>(true);
        achievementsData = achieveManager.ClonedAchievements;

        // Subscribe to achievement completion event
        AchieveManager.OnCompleteAchievement += UpdateAchievements;

        PopulateAchievements();
        filterAchievements(0); // Ensure correct filtering after population
    }

    private void OnEnable()
    {
        AchieveManager achieveManager = FindObjectOfType<AchieveManager>();
        if (achieveManager != null)
        {
            achievementsData = achieveManager.ClonedAchievements;
        }

        UpdateAchievements(); // Refresh UI on scene reload
    }


    private void OnDestroy()
    {
        AchieveManager.OnCompleteAchievement -= UpdateAchievements;
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

        PopulateAchievements(); // Re-populate achievements UI
        filterAchievements(dropdown.value); // Apply the current filter
    }
}