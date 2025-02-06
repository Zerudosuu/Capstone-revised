using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IData
{
    [Header("Player Stats")] public int playerLvl;
    public float expGained;
    public int coins;
    public string playerTitle;

    [Header("Statistic Base")] public float maxEXP;

    [Header("UI Elements")] [SerializeField]
    private TMP_Text coinText;

    [SerializeField] private TMP_Text lvlTxt;
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text expTxt;
    [SerializeField] private Slider expSlider;

    // Event to notify UI updates
    public static event Action<int, string, float, float> OnPlayerStatsUpdated;


    private readonly (string Title, float MaxExp)[] levelData =
    {
        ("Noob", 100),
        ("Newbie", 210),
        ("Scientist", 400)
    };

    private void Start()
    {
        UpdatePlayerStats();
        UpdateUI();
    }

    public void AddExp(int exp)
    {
        expGained += exp;

        while (expGained >= maxEXP && playerLvl < levelData.Length)
        {
            expGained -= maxEXP; // Carry over extra EXP
            playerLvl++;
            UpdatePlayerStats();
        }

        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    private void UpdatePlayerStats()
    {
        if (playerLvl >= 1 && playerLvl <= levelData.Length)
        {
            playerTitle = levelData[playerLvl - 1].Title;
            maxEXP = levelData[playerLvl - 1].MaxExp;
        }

        NotifyUIUpdate();
    }

    private void UpdateUI()
    {
        coinText.text = $"{coins}";
        lvlTxt.text = playerLvl.ToString();
        titleTxt.text = playerTitle;
        expTxt.text = $"{expGained} / {maxEXP}";

        expSlider.maxValue = maxEXP;
        expSlider.value = expGained;

        NotifyUIUpdate();
    }

    private void NotifyUIUpdate()
    {
        Debug.Log("Notifying UI Update");
        OnPlayerStatsUpdated?.Invoke(playerLvl, playerTitle, expGained, maxEXP);
    }

    public void LoadData(GameData gameData)
    {
        playerLvl = gameData.Level;
        expGained = gameData.currentExperience;
        maxEXP = gameData.currentMaxExperience;
        coins = gameData.playerCoins;

        UpdatePlayerStats();
        UpdateUI();
    }

    public void SavedData(GameData gameData)
    {
        gameData.Level = playerLvl;
        gameData.currentExperience = (int)expGained;
        gameData.currentMaxExperience = (int)maxEXP;
        gameData.playerCoins = coins;
    }
}