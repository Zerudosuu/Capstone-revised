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

    public void Start()
    {
        GetAllProfileAchievements();
        filterAchievements(0);
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
                if (!profileAchievement.isCompleted)
                    profileAchievement.gameObject.SetActive(true);
                else
                    profileAchievement.gameObject.SetActive(false);
            }
        }
        else if (number == 1)
        {
            foreach (ProfileAchievement profileAchievement in profileAchievements)
            {
                if (profileAchievement.isCompleted)
                    profileAchievement.gameObject.SetActive(true);
                else
                    profileAchievement.gameObject.SetActive(false);
            }
        }
    }
}