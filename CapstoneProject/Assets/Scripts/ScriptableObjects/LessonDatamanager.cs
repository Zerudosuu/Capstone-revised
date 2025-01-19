using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Sprite ItemIcon;
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
            ItemIcon = this.ItemIcon,
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

    public List<MaterialEntry> materials = new List<MaterialEntry>();
    public List<LessonSteps> steps = new List<LessonSteps>();

    public bool isCompleted;

    [Header("FirstRewards")] public int Coins;
    public int Experience;

    [Header("SecondRewards")] public int SecondCoins;
    public int SecondExperience;

    [Header("Lesson Item Rewards")] public bool isItemRewardCollected;
    public List<MaterialEntry> ItemRewards = new List<MaterialEntry>();

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
            SecondCoins = this.SecondCoins,
            SecondExperience = this.SecondExperience,
            materials = new List<MaterialEntry>(),
            steps = new List<LessonSteps>(),
            isItemRewardCollected = this.isItemRewardCollected,
            ItemRewards = new List<MaterialEntry>(),
        };

        // Deep clone each MaterialEntry
        foreach (MaterialEntry material in this.materials)
        {
            clonedLesson.materials.Add(material.Clone());
        }

        // Deep clone each ItemReward
        foreach (MaterialEntry itemReward in this.ItemRewards)
        {
            clonedLesson.ItemRewards.Add(itemReward.Clone());
        }

        // Deep clone each LessonSteps
        foreach (LessonSteps step in this.steps)
        {
            clonedLesson.steps.Add(step.Clone());
        }

        return clonedLesson;
    }

    public bool AreAllStepsCompleted()
    {
        foreach (LessonSteps step in steps)
        {
            if (!step.isCompleted)
            {
                return false;
            }
        }

        return true;
    }
}

[System.Serializable]
public class LessonSteps
{
    public string stepDescription; // Text for the step
    public bool isCompleted = false; // Check if the step is completed
    public List<string> requiredObjectNames = new List<string>(); // Objects required for this step
    public string actionRequired; // Specific action (e.g., "Measure", "Heat", "Mix")

    // Method to mark the step as completed
    public void CompleteStep()
    {
        isCompleted = true;
    }

    // Deep clone method
    public LessonSteps Clone()
    {
        return new LessonSteps
        {
            stepDescription = this.stepDescription,
            isCompleted = this.isCompleted,
            requiredObjectNames = new List<string>(this.requiredObjectNames), // Properly clone the list
            actionRequired = this.actionRequired,
        };
    }
}