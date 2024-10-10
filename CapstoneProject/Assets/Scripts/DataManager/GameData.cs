using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdate;
    public int clickCount;
    public Vector3 playerPosition;

    public string userName;
    public string fullName;
    public DateTime birthday;
    public int age;

    public string SceneName;

    public string[] floorsCleared;

    public string studentID;
    public string currenTitle;

    public string avatarName;

    public GameData()
    {
        this.clickCount = 0;
        this.playerPosition = Vector3.zero;
        this.userName = "";
        this.fullName = "";
        this.birthday = DateTime.Now;
        this.age = 0;
        this.SceneName = "RegistrationScene";
        this.currenTitle = "The Noob";
        this.avatarName = "";
    }
}
