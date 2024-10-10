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
    private TextMeshProUGUI FullName;

    [SerializeField]
    private TextMeshProUGUI Title;

    [SerializeField]
    private TextMeshProUGUI playerUsername;

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

            FullName.text = data.fullName;
            Title.text = data.currenTitle;
            playerUsername.text = data.userName;

            string savedAvatarName = data.avatarName;
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
