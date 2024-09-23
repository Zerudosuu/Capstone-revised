using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CreateAssetMenu to allow creation of LessonsData from the Unity Editor
[CreateAssetMenu(fileName = "NewLessonsData", menuName = "Lesson Data", order = 1)]
public class LessonsData : ScriptableObject
{
    public List<Lesson> lessons = new List<Lesson>();
}

// Mark Lesson as [System.Serializable] so it shows up in the Inspector
[System.Serializable]
public class Lesson
{
    public string chapterName;
    public int chapterNumber;

    public string chapterSummaryDescription;
    public string fullDescription;

    public bool isCompleted;
}
