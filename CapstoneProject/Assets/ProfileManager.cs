using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ProfileManager : MonoBehaviour, IData
{
    [Header("Player Profile")] [SerializeField]
    private Image PlayerProfilePicture;

    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private TMP_Text playerTitle;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerLrn;
    [SerializeField] private TMP_Text ExperienceGain;
    [SerializeField] private Slider expSlider;
    Achievements achievementData;

    //Values

    public string ProfilePicturePath;
    public int level;
    public string title = "Noob";
    public string name;
    public string lrn;
    public int exp;
    public int maxExperience;


    public PlayerStats playerStats;

    private void OnEnable()
    {
        PlayerStats.OnPlayerStatsUpdated += UpdateProfileUI;

        if (playerStats != null)
        {
            UpdateProfileUI(playerStats.playerLvl, playerStats.playerTitle, playerStats.expGained, playerStats.maxEXP);
        }
    }


    private void OnDestroy()
    {
        PlayerStats.OnPlayerStatsUpdated -= UpdateProfileUI;
    }

    private void Awake()
    {
        playerLevel.text = "Level: " + level.ToString();
        playerTitle.text = title;
        playerName.text = name;
        playerLrn.text = "LRN: " + lrn;
        ExperienceGain.text = exp.ToString() + " / " + maxExperience.ToString();

        expSlider.maxValue = maxExperience;
        expSlider.value = exp;
    }

    private void UpdateProfileUI(int newLevel, string newTitle, float newExp, float newMaxExp)
    {
        print("Updating Profile UI" + newLevel + " " + newTitle + " " + newExp + " " + newMaxExp);

        level = newLevel;
        title = newTitle;
        exp = (int)newExp;
        maxExperience = (int)newMaxExp;

        playerLevel.text = "Level: " + level.ToString();
        playerTitle.text = title;
        ExperienceGain.text = $"{exp} / {maxExperience}";

        expSlider.maxValue = maxExperience;
        expSlider.value = exp;
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
        name = gameData.playerName;
        lrn = gameData.playerLRN;
        ProfilePicturePath = gameData.PicturePath;

        playerName.text = name;
        playerLrn.text = lrn;
        LoadImageIntoUI();
    }

    public void SavedData(GameData gameData)
    {
        gameData.playerName = this.name;
        gameData.playerLRN = this.lrn;
    }
}