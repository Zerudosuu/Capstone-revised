using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IData
{
    [Header("Player Stats")] [SerializeField]
    private int playerLvl;

    [SerializeField] private float playerEXP;

    [SerializeField] private string playerTitle;

    [Header("Statistic Base")] [SerializeField]
    private float expGained;

    [SerializeField] private float maxEXP;

    [Header("Display")] [SerializeField] private TMP_Text lvlTxt;

    [SerializeField] private TMP_Text titleTxt;

    [SerializeField] private TMP_Text expTxt;

    [SerializeField] private Slider expSlider;


    // Event to notify UI updates
    public static event Action<int, string, float, float> OnPlayerStatsUpdated;

    private void Start()
    {
        PlayerStatsUpdate(playerLvl);
        NotifyUIUpdate();
    }

    //TO DO - needed to load and save
    //              * playerLvl
    //              * expGained


    public void AddExp(int exp)
    {
        expGained += exp;

        if (expGained >= maxEXP)
        {
            playerLvl++;
            PlayerStatsUpdate(playerLvl);
        }

        playerUpdateUI();
        NotifyUIUpdate();
    }

    #region Update UI

    private void PlayerStatsUpdate(int level)
    {
        switch (level)
        {
            case 1:
                playerTitle = "Noob";
                maxEXP = 100;
                LevelUpUI();
                return;
            case 2:
                playerTitle = "Newbie";
                maxEXP = 210;
                expGained = 0;
                LevelUpUI();
                return;
            case 3:
                playerTitle = "Scientist";
                maxEXP = 400;
                expGained = 0;
                LevelUpUI();
                return;
        }
    }

    private void playerUpdateUI()
    {
        //update UI Slider
        expSlider.value = expGained;
        expTxt.text = $"{expGained.ToString()} / {maxEXP.ToString()}";
    }

    private void LevelUpUI()
    {
        lvlTxt.text = playerLvl.ToString();
        titleTxt.text = playerTitle;
        expTxt.text = $"{expGained.ToString()} / {maxEXP.ToString()}";

        expSlider.value = expGained;
        expSlider.maxValue = maxEXP;
    }

    private void NotifyUIUpdate()
    {
        OnPlayerStatsUpdated?.Invoke(playerLvl, playerTitle, expGained, maxEXP);
    }

    #endregion

    public void LoadData(GameData gameData)
    {
        playerLvl = gameData.Level;
        expGained = gameData.currentExperience;
        maxEXP = gameData.currentMaxExperience;
    }

    public void SavedData(GameData gameData)
    {
        gameData.Level = playerLvl;
        gameData.currentExperience = (int)expGained;
        gameData.currentMaxExperience = (int)maxEXP;
    }
}