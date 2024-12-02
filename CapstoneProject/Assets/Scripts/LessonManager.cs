using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonManager : MonoBehaviour, IData
{
    //setActive(true) to the CurrentLessonWindow
    [SerializeField]
    private GameObject CurrentLessonWindow;

    // QuestGiver
    public QuestAsLesson questAsLesson;

    [Header("LessonManager")]
    [SerializeField]
    private LessonsDataManager lessonsData;

    [SerializeField]
    private LessonComponentButton[] lessonComponentButton;

    [SerializeField]
    private GameObject LessonButtonPrefab;

    [SerializeField]
    private GameObject LessonButtonContainer;
    private LessonContainer lessonContainer;

    private string CurrentButtonID = "";

    CurrentLessonToDisplay currentLessonToDisplay;

    [SerializeField]
    private List<Lesson> Clonedlessons = new List<Lesson>();

    [SerializeField]
    private GameObject RewardContainer;

    void Awake()
    {
        currentLessonToDisplay = CurrentLessonWindow.GetComponent<CurrentLessonToDisplay>();
        lessonComponentButton = GetComponentsInChildren<LessonComponentButton>();
        lessonContainer = FindObjectOfType<LessonContainer>();
    }

    void Start()
    {
        Populatebuttons();
        PickFirstAvailableLesson();
    }

    void CloneLessons()
    {
        foreach (Lesson lesson in lessonsData.lessons)
        {
            Clonedlessons.Add(lesson.Clone());
        }
    }

    void Populatebuttons()
    {
        foreach (Lesson lesson in Clonedlessons)
        {
            GameObject lessonButton = Instantiate(
                LessonButtonPrefab,
                LessonButtonContainer.transform
            );

            LessonComponentButton buttonScript = lessonButton.GetComponent<LessonComponentButton>();

            buttonScript.ButtonID = lesson.LessonID;
            buttonScript.ChapterNumber.text = lesson.LessonID;

            if (lesson.isCompleted)
            {
                buttonScript.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void OnButtonClick(LessonComponentButton lessonComponentButton)
    {
        Debug.Log("Button clicked: " + lessonComponentButton.GetButtonID());

        // Find the corresponding lesson based on the button ID
        Lesson selectedLesson = FindLessonById(lessonComponentButton.GetButtonID());

        // If the lesson is found, update the LessonContainer
        if (selectedLesson != null)
        {
            UpdateLessonContainer(selectedLesson);
        }
    }

    private Lesson FindLessonById(string lessonID)
    {
        foreach (Lesson lesson in Clonedlessons)
        {
            if (lesson.LessonID == lessonID)
            {
                CurrentButtonID = lessonID;
                return lesson;
            }
        }
        return null; // Return null if not found
    }

    private void UpdateLessonContainer(Lesson lesson)
    {
        if (lessonContainer != null)
        {
            // Update the lesson details in the container
            lessonContainer.ChapterNumberAndTitle.text = $"{lesson.LessonID}: {lesson.chapterName}";
            lessonContainer.ChapterDescription.text = lesson.fullDescription;
            lessonContainer.Coin.text = "Coin: " + lesson.Coins.ToString();
            lessonContainer.Experience.text = "Experience: " + lesson.Experience.ToString();

            // Clear any existing materials before adding new ones
            foreach (Transform child in lessonContainer.MaterialContainer.transform)
            {
                Destroy(child.gameObject);
            }

            // Loop through lesson materials and add them to the container
            foreach (MaterialEntry material in lesson.materials)
            {
                GameObject materialObject = Instantiate(
                    lessonContainer.Material,
                    lessonContainer.MaterialContainer.transform
                );

                materialObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    material.materialName;
            }

            // Enable or disable the buttons as necessary
            lessonContainer.AcceptButton.interactable = true; // Example
            lessonContainer.CancelButton.interactable = true; // Example

            // Do not update quest here
        }
    }

    public void OnAcceptButtonClick()
    {
        CurrentLessonWindow.SetActive(true);

        print("ButtonWasClicked");
        questAsLesson.isActive = true;

        // Update the quest based on the currently selected lesson
        Lesson selectedLesson = FindLessonById(CurrentButtonID);
        if (selectedLesson != null)
        {
            UpdateQuest(selectedLesson);
        }

        if (currentLessonToDisplay != null)
        {
            currentLessonToDisplay.SetCurrentLesson(questAsLesson);
            currentLessonToDisplay.UpdateLessonDisplay();
            DataManager.Instance.gameData.currentQuestIndex = int.Parse(CurrentButtonID);
        }

        DataManager.Instance.gameData.quest = questAsLesson;
    }

    public void OnBackButtonClick()
    {
        // Handle back button click
        Debug.Log("Back button clicked");
    }

    public void UpdateQuest(Lesson lesson)
    {
        // Update the quest based on the current button ID
        questAsLesson.LessonID = CurrentButtonID;
        questAsLesson.chapterName = lesson.chapterName;
        questAsLesson.chapterNumber = lesson.chapterNumber;
        questAsLesson.fullDescription = lesson.fullDescription;
        questAsLesson.RewardCoins = lesson.Coins;
        questAsLesson.RewardExperience = lesson.Experience;
        questAsLesson.materials = new List<MaterialEntry>();

        foreach (MaterialEntry material in lesson.materials)
        {
            questAsLesson.materials.Add(material);
        }
    }

    public void PickFirstAvailableLesson()
    {
        foreach (Lesson lesson in Clonedlessons)
        {
            if (lesson.isCompleted == false)
            {
                UpdateLessonContainer(lesson);
                break;
            }
        }
    }

    public void OnRewardsCollected()
    {
        questAsLesson = null;
        RewardContainer.SetActive(false);
        DataManager.Instance.SaveGame();
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.quest == null)
        {
            print("No quest data found");
        }

        if (gameData.quest != null)
        {
            questAsLesson = gameData.quest;

            if (gameData.quest.isCompleted == true)
            {
                Debug.Log("This lesson is  Completed");
                RewardContainer.SetActive(true);
            }
        }

        if (gameData.lessons != null && gameData.lessons.Count > 0)
        {
            Clonedlessons = gameData.lessons;
            Debug.Log("Loaded lessons from GameData: " + Clonedlessons.Count);
        }
        else
        {
            CloneLessons();
            Debug.Log("No lessons in GameData. Cloning from source.");
        }

        if (gameData.quest != null)
        {
            questAsLesson = gameData.quest;
        }
    }

    public void SavedData(GameData gameData)
    {
        if (gameData.lessons == null || gameData.lessons.Count == 0)
        {
            gameData.lessons = Clonedlessons;
            Debug.Log("Saved lessons to GameData: " + gameData.lessons.Count);
        }

        // Always update the quest data
        gameData.quest = questAsLesson;
    }
}
