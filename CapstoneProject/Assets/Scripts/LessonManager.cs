using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonManager : MonoBehaviour
{
    //setActive(true) to the CurrentLessonWindow
    [SerializeField]
    private GameObject CurrentLessonWindow;

    [SerializeField]
    private GameObject LessonWindow;

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

    void Awake()
    {
        lessonComponentButton = GetComponentsInChildren<LessonComponentButton>();
        lessonContainer = FindObjectOfType<LessonContainer>();
    }

    void Start()
    {
        Populatebuttons();
        PickFirstAvailableLesson();
    }

    void Populatebuttons()
    {
        foreach (Lesson lesson in lessonsData.lessons)
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
        foreach (Lesson lesson in lessonsData.lessons)
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

            UpdateQuest(lesson);
        }
    }

    public void OnAcceptButtonClick()
    {
        CurrentLessonWindow.SetActive(true);

        print("ButtonWasClicked");
        questAsLesson.isActive = true;

        CurrentLessonToDisplay currentLessonToDisplay =
            GetComponentInChildren<CurrentLessonToDisplay>();

        if (currentLessonToDisplay != null)
        {
            currentLessonToDisplay.SetCurrentLesson(questAsLesson);

            currentLessonToDisplay.UpdateLessonDisplay();
        }
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
        foreach (Lesson lesson in lessonsData.lessons)
        {
            if (lesson.isCompleted == false)
            {
                UpdateLessonContainer(lesson);
                break;
            }
        }
    }
}
