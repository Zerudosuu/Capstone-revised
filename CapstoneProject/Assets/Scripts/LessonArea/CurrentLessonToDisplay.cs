using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CurrentLessonToDisplay : MonoBehaviour
{
    public QuestAsLesson CurrentLesson;

    // Current Lesson
    [Header("Lesson Window")]
    public GameObject LessonWindow;
    public bool isLessonCurrentWindowActive = false;

    [SerializeField]
    private Animator animator;

    [Header("No Current Window")]
    [SerializeField]
    private GameObject noCurrentWindow;

    [Header("HasCurrentLessonWindow")]
    [SerializeField]
    private GameObject hasCurrentLessonWindow;

    [SerializeField]
    private TextMeshProUGUI ChapterNumberAndTitle;

    [SerializeField]
    private TextMeshProUGUI ChapterDescription;

    [SerializeField]
    private TextMeshProUGUI RewardCoin;

    [SerializeField]
    private TextMeshProUGUI RewardExperience;

    [SerializeField]
    private GameObject MaterialContainer;

    [SerializeField]
    private Button Continue;

    [SerializeField]
    private Button Abandon;

    [SerializeField]
    private GameObject Material;

    void Start()
    {
        animator = LessonWindow.GetComponent<Animator>();
        UpdateLessonDisplay();
    }

    public void OnclickOpenClose()
    {
        isLessonCurrentWindowActive = !isLessonCurrentWindowActive;
        animator.SetBool("isCurrentLessonWindowIsOpen?", isLessonCurrentWindowActive);
        Debug.Log("Lesson Window Open/Close: " + isLessonCurrentWindowActive);
    }

    public void SetCurrentLesson(QuestAsLesson lesson)
    {
        CurrentLesson = lesson;

        ChapterNumberAndTitle.text = CurrentLesson.chapterNumber + ". " + CurrentLesson.chapterName;
        ChapterDescription.text = CurrentLesson.fullDescription;
        RewardCoin.text = "Coin: " + CurrentLesson.RewardCoins.ToString();
        RewardExperience.text = "Experience: " + CurrentLesson.RewardExperience.ToString();

        // Clear any existing materials before adding new ones
        foreach (Transform child in MaterialContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (MaterialEntry materialEntry in lesson.materials)
        {
            GameObject materialObject = Instantiate(Material, MaterialContainer.transform);

            materialObject.GetComponentInChildren<TextMeshProUGUI>().text =
                materialEntry.materialName;
        }
    }

    public void UpdateLessonDisplay()
    {
        if (CurrentLesson.isActive == false)
        {
            noCurrentWindow.gameObject.SetActive(true);
            hasCurrentLessonWindow.gameObject.SetActive(false);
        }
        else
        {
            noCurrentWindow.gameObject.SetActive(false);
            hasCurrentLessonWindow.gameObject.SetActive(true);
        }
    }

    public void onGotoLessonButtonClick()
    {
        // Open Lesson Window
        this.gameObject.SetActive(false);
        LessonWindow.SetActive(true);
    }

    public void AbandonLesson()
    {
        if (CurrentLesson != null && CurrentLesson.isActive)
        {
            // Clear current lesson data
            CurrentLesson.isActive = false;
            CurrentLesson.isCompleted = false;

            // Clear UI fields
            ChapterNumberAndTitle.text = "";
            ChapterDescription.text = "";
            RewardCoin.text = "0";
            RewardExperience.text = "0";

            // Clear the materials
            foreach (Transform child in MaterialContainer.transform)
            {
                Destroy(child.gameObject);
            }

            // Update the UI to show no current lesson
            UpdateLessonDisplay();

            Debug.Log("Current lesson abandoned.");
        }
    }
}
