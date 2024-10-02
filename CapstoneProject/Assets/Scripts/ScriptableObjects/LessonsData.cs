using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLessonsData", menuName = "Lesson Data", order = 1)]
public class LessonsData : ScriptableObject
{
    public List<Lesson> lessons = new List<Lesson>();
}

// Custom class to replace Dictionary functionality
[System.Serializable]
public class MaterialEntry
{
    public string materialName;
    public bool isCollected = false;
}

[System.Serializable]
public class Lesson
{
    public string chapterName;
    public int chapterNumber;

    public string chapterSummaryDescription;
    public string fullDescription;

    // Use List to mimic Dictionary functionality
    public List<MaterialEntry> materials = new List<MaterialEntry>();
    public bool isCompleted;

    public int Coins;
    public int Experience;
}
