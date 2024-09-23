using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Reference to the ScriptableObject, type it as LessonsData (your custom ScriptableObject class)
    public LessonsData lessonsData;

    void Start()
    {
        // Loop through the lessons in the lessonsData ScriptableObject
        foreach (Lesson lesson in lessonsData.lessons)
        {
            Debug.Log(
                "Chapter Name: "
                    + lesson.chapterName
                    + ", Chapter Number: "
                    + lesson.chapterNumber
                    + ", Chapter Description"
                    + lesson.fullDescription
            );
        }
    }
}
