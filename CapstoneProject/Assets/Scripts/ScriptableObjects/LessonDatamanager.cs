using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLessonsData", menuName = "Lesson Data", order = 1)]
public class LessonsDataManager : ScriptableObject
{
    public List<Lesson> lessons = new List<Lesson>();
}

// Custom class to replace Dictionary functionality
[System.Serializable]
public class MaterialEntry
{
    public string materialName;
    public bool isCollected = false;
    public int Quantity = 1;
    public bool needToMeasure = false;
    public float measuredValue = 0f;

    // Method to mark material as collected
    public void CollectMaterial()
    {
        isCollected = true;
    }

    // Method to update measured value
    public void MeasureMaterial(float value)
    {
        if (needToMeasure)
        {
            measuredValue = value;
        }
    }

    // Deep clone method
    public MaterialEntry Clone()
    {
        return new MaterialEntry
        {
            materialName = this.materialName,
            isCollected = this.isCollected,
            Quantity = this.Quantity,
            needToMeasure = this.needToMeasure,
            measuredValue = this.measuredValue,
        };
    }
}

[System.Serializable]
public class Lesson
{
    public string LessonID = "";
    public string chapterName;
    public int chapterNumber;

    public string chapterSummaryDescription;
    public string fullDescription;

    // Use List to mimic Dictionary functionality
    public List<MaterialEntry> materials = new List<MaterialEntry>();
    public bool isCompleted;

    public int Coins;
    public int Experience;

    public Lesson Clone()
    {
        Lesson clonedLesson = new Lesson
        {
            LessonID = this.LessonID,
            chapterName = this.chapterName,
            chapterNumber = this.chapterNumber,
            chapterSummaryDescription = this.chapterSummaryDescription,
            fullDescription = this.fullDescription,
            isCompleted = this.isCompleted,
            Coins = this.Coins,
            Experience = this.Experience,
            materials = new List<MaterialEntry>(), // Create a new list
        };

        // Deep clone each MaterialEntry
        foreach (MaterialEntry material in this.materials)
        {
            clonedLesson.materials.Add(
                new MaterialEntry
                {
                    materialName = material.materialName,
                    isCollected = material.isCollected,
                    Quantity = material.Quantity,
                    needToMeasure = material.needToMeasure,
                    measuredValue = material.measuredValue,
                }
            );
        }

        return clonedLesson;
    }
}
