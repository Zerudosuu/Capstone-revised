using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Saveslot : MonoBehaviour
{
    [Header("Profile")] [SerializeField] private string profileId = "";

    [Header("UI Elements")] [SerializeField]
    private GameObject noDataContent;

    [SerializeField] private GameObject hasDataContent;

    [SerializeField] private TextMeshProUGUI ChapterTitle;

    [SerializeField] private TextMeshProUGUI Level;

    [SerializeField] private TextMeshProUGUI PlayTime;

    [SerializeField] private TextMeshProUGUI Percentage;
    private Button saveSlotButton;

    [Header("Clear Data Button")] [SerializeField]
    private Button clearDataButton;

    public bool hasData { get; private set; } = false;

    void Awake()
    {
        saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearDataButton.gameObject.SetActive(false);
        }
        else if (data.LessonMode == GameMode.Lesson)
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearDataButton.gameObject.SetActive(true);
            ChapterTitle.text = data.GetLessonTitle();
            Level.text = "Level: " + data.Level;
            PlayTime.text = "Play Time: " + data.timestamp;
            Percentage.text = data.GetCompletedLessonPercentage().ToString() + "%";
        }
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearDataButton.gameObject.SetActive(true);
            ChapterTitle.text = "Creative Mode";
            Level.text = "";
            Percentage.text = "";
            PlayTime.text = "Play Time: " + data.timestamp;
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearDataButton.interactable = interactable;
    }
}