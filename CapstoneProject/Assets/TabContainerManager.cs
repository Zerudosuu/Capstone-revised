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

    Achievements achievementData;
    public GameObject AchievementPrefab;

    public void Start()
    {
        GetAllProfileAchievements();
        filterAchievements(0);

        AchieveManager achieveManager = FindObjectOfType<AchieveManager>(true);
        achievementData = achieveManager.achievementData;

        PopulateAchievements();
    }

    void PopulateAchievements()
    {
        foreach (Achievement achievement in achievementData.achievements)
        {
            GameObject newAchievement = Instantiate(AchievementPrefab, achivementsContainer.transform);
            ProfileAchievement profileAchievement = newAchievement.GetComponent<ProfileAchievement>();
            profileAchievement.SetAchievement(achievement);
        }
    }

    public void GetValueOnDropDown()
    {
        int index = dropdown.value;
        filterAchievements(index);
    }

    private void GetAllProfileAchievements()
    {
        // get all profile achievements child in achievements container
        foreach (Transform child in achivementsContainer.transform)
        {
            ProfileAchievement profileAchievement = child.GetComponent<ProfileAchievement>();
            profileAchievements.Add(profileAchievement);
        }
    }

    private void filterAchievements(int number)
    {
        if (number == 0)
        {
            foreach (ProfileAchievement profileAchievement in profileAchievements)
            {
                if (!profileAchievement.Achievement.isUnlocked)
                    profileAchievement.gameObject.SetActive(true);
                else
                    profileAchievement.gameObject.SetActive(false);
            }
        }
        else if (number == 1)
        {
            foreach (ProfileAchievement profileAchievement in profileAchievements)
            {
                if (profileAchievement.Achievement.isUnlocked)
                    profileAchievement.gameObject.SetActive(true);
                else
                    profileAchievement.gameObject.SetActive(false);
            }
        }
    }
}