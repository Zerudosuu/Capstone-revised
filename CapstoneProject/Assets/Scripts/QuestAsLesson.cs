using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestAsLesson
{
    public string LessonID = "";
    public string chapterName;
    public int chapterNumber;

    public string chapterSummaryDescription;
    public string fullDescription;

    // Use List to mimic Dictionary functionality
    public List<MaterialEntry> materials = new List<MaterialEntry>();
    public bool isCompleted;

    public int RewardCoins;
    public int RewardExperience;

    public bool isActive;
}
