using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Saveslot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField]
    private string profileId = "";

    [Header("UI Elements")]
    [SerializeField]
    private GameObject noDataContent;

    [SerializeField]
    private GameObject hasDataContent;

    [SerializeField]
    private TextMeshProUGUI ChapterTitle;

    [SerializeField]
    private TextMeshProUGUI Level;

    [SerializeField]
    private TextMeshProUGUI PlayTime;

    [SerializeField]
    private TextMeshProUGUI Percentage;
    private Button saveSlotButton;

    [Header("Clear Data Button")]
    [SerializeField]
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
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearDataButton.gameObject.SetActive(true);

            ChapterTitle.text = data.ChapterTitle;
            Level.text = "Level: " + data.Level;
            PlayTime.text = "Play Time: " + data.timestamp;
            Percentage.text = data.Percentage + "%";
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
