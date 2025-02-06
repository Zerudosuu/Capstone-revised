using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string playerName; // Player name
    public string PicturePath; // Path to the player's profile picture
    public string playerLRN;
    public long lastUpdate; // Timestamp of the last save
    public string SceneName; // Current scene name
    public string ChapterTitle; // Current chapter title
    public int Level; // Current level
    public string timestamp; // Readable timestamp for when the data was created/saved
    public float Percentage; // Progress percentage
    public GameMode LessonMode; // Game mode (Lesson, Creative, etc.)
    public List<SerializableBagItem> BagItems; // Inventory data
    public List<Achievement> Achievements; // List of achievements
    public QuestAsLesson quest;
    public List<Lesson> lessons;
    public int currentQuestIndex;
    public bool isTutorialCompleted = false;
    public bool isLessonTutorialCompleted = false;


    public int currentExperience;
    public int currentMaxExperience;
    public int playerCoins; // Player's currency


    // Default constructor initializes some default values
    public GameData()
    {
        this.playerName = "Ronald Salvador";
        this.PicturePath = "";
        this.playerLRN = "123456789";
        this.currentExperience = 0;
        this.currentMaxExperience = 100;
        this.playerCoins = 0;
        this.Achievements = new List<Achievement>();
        this.SceneName = "GameModeSelection";
        this.ChapterTitle = "Chapter 1";
        this.Level = 1;
        this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.Percentage = 0;
        this.LessonMode = GameMode.None;
        this.BagItems = new List<SerializableBagItem>();
        this.quest = new QuestAsLesson();
        this.lessons = new List<Lesson>();
        this.currentQuestIndex = 0; // Default to the first quest in the list
        this.isTutorialCompleted = false;
        this.isLessonTutorialCompleted = false;
    }


    public float GetCompletedLessonPercentage()
    {
        if (lessons == null || lessons.Count == 0)
            return 0f;

        int completedCount = 0;
        foreach (Lesson lesson in lessons)
        {
            // Replace "IsCompleted" with your actual completion property name
            if (lesson.isCompleted)
                completedCount++;
        }

        return (float)completedCount / lessons.Count * 100f;
    }


    public string GetLessonTitle()
    {
        foreach (Lesson lesson in lessons)
        {
            if (lesson.isCompleted == false)
            {
                return lesson.chapterName;
            }
        }

        return "Finished";
    }
}

[System.Serializable]
public class SerializableBagItem
{
    public string itemName;
    public int count;

    // Constructor to initialize a SerializableBagItem
    public SerializableBagItem(string name, int quantity)
    {
        itemName = name;
        count = quantity;
    }
}

public enum GameMode
{
    None,
    Lesson,
    Creative,
}