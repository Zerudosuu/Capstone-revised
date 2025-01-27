using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour, IData
{
    [Header("Player Profile")] [SerializeField]
    private Image PlayerProfilePicture;

    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private TMP_Text playerTitle;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerLrn;
    [SerializeField] private TMP_Text ExperienceGain;
    Achievements achievementData;

    //Values

    public string ProfilePicturePath;
    public int level;
    public string title = "Noob";
    public string name;
    public string lrn;
    public int exp;
    public int maxExperience;


    private void Start()
    {
        playerLevel.text = "Level: " + level.ToString();
        playerTitle.text = title;
        playerName.text = name;
        playerLrn.text = "LRN: " + lrn;
        ExperienceGain.text = exp.ToString() + " / " + maxExperience.ToString();

        LoadImageIntoUI();
    }


    private void LoadImageIntoUI()
    {
        byte[] imageBytes = File.ReadAllBytes(ProfilePicturePath);
        Texture2D texture = new Texture2D(2, 2);

        if (texture.LoadImage(imageBytes)) // Automatically resizes texture
        {
            Sprite imageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            PlayerProfilePicture.sprite = imageSprite;
        }
        else
        {
            Debug.LogError("Failed to load image from path: " + ProfilePicturePath);
        }
    }

    public void LoadData(GameData gameData)
    {
        level = gameData.Level;
        name = gameData.playerName;
        lrn = gameData.playerLRN;
        exp = gameData.currentExperience;
        maxExperience = gameData.currentMaxExperience;
        ProfilePicturePath = gameData.PicturePath;
    }

    public void SavedData(GameData gameData)
    {
        gameData.Level = this.level;
        gameData.playerName = this.name;
        gameData.playerLRN = this.lrn;
        gameData.currentExperience = this.exp;
        gameData.currentMaxExperience = this.maxExperience;
    }
}