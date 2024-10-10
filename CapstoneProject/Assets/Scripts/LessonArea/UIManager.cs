using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("MainMenuButtons")]
    [SerializeField]
    private Button ProfileButton;

    [SerializeField]
    private Button CurrentLessonQuestButton;

    [SerializeField]
    private Button Bag;

    [SerializeField]
    private Button LessonContainer;

    [SerializeField]
    private Button Settings;

    [Header("Windows")]
    [SerializeField]
    private GameObject ProfileWindow;

    [SerializeField]
    private GameObject CurrentLessonWindow;

    [SerializeField]
    private GameObject BagWindow;

    [SerializeField]
    private GameObject LessonContainerWindow;

    [SerializeField]
    private GameObject SettingsWindow;

    [Header("Others")]
    [SerializeField]
    private GameObject PlayerUI;

    void Start()
    {
        BagWindow.SetActive(true);
        ProfileWindow.SetActive(false);

        LessonContainerWindow.SetActive(false);
        SettingsWindow.SetActive(false);
    }

    public void OnBagButtonClick()
    {
        BagWindow.SetActive(true);
        ProfileWindow.SetActive(false);
        CurrentLessonWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        PlayerUI.SetActive(true);
    }

    public void OnProfileButtonClick()
    {
        ProfileWindow.SetActive(true);
        BagWindow.SetActive(false);
        CurrentLessonWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        PlayerUI.SetActive(false);
    }

    public void OnCurrentLessonQuestButtonClick()
    {
        CurrentLessonWindow.SetActive(true);
        BagWindow.SetActive(false);
        ProfileWindow.SetActive(false);
        SettingsWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        PlayerUI.SetActive(false);
    }

    public void OnLessonContainerButtonClick()
    {
        LessonContainerWindow.SetActive(true);
        BagWindow.SetActive(false);
        ProfileWindow.SetActive(false);

        SettingsWindow.SetActive(false);
        PlayerUI.SetActive(false);
    }

    public void OnSettingsButtonClick()
    {
        SettingsWindow.SetActive(true);
        BagWindow.SetActive(false);
        ProfileWindow.SetActive(false);
        CurrentLessonWindow.SetActive(false);
        LessonContainerWindow.SetActive(false);
        PlayerUI.SetActive(false);
    }
}
