using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdate;

    public string SceneName;

    public string ChapterTitle;

    public string Level;

    public string timestamp;

    public float Percentage;

    public GameData()
    {
        this.SceneName = "Lesson";
        this.ChapterTitle = "Chapter 1";
        this.Level = "1";
        this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.Percentage = 0;
    }
}
