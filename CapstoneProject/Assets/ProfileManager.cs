using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    [Header("Player Profile")] [SerializeField]
    private Image PlayerProfilePicture;

    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private TMP_Text playerTitle;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerLrn;
    [SerializeField] private TMP_Text EXPgain;

    private string ProfilePicturePath;
    Achievements achievementData;

    public void SetDetails(GameData gameData)
    {
        playerName.text = gameData.playerName;
        playerLevel.text = gameData.Level.ToString();
        playerLrn.text = gameData.playerLRN;
        EXPgain.text = gameData.currentExperience.ToString() + "/" + gameData.currentMaxExperience.ToString();
        ProfilePicturePath = gameData.PicturePath;
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
}