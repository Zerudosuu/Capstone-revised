using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class LessonManager : MonoBehaviour, IData
{
    //setActive(true) to the CurrentLessonWindow
    [SerializeField] private GameObject CurrentLessonWindow;

    // QuestGiver
    public QuestAsLesson questAsLesson;

    [Header("LessonManager")] [SerializeField]
    private LessonsDataManager lessonsData;

    [SerializeField] private LessonComponentButton[] lessonComponentButton;

    [SerializeField] private GameObject LessonButtonPrefab;

    [SerializeField] private GameObject LessonButtonContainer;
    private LessonContainer lessonContainer;

    private string CurrentButtonID = "";

    CurrentLessonToDisplay currentLessonToDisplay;

    [SerializeField] private List<Lesson> Clonedlessons = new List<Lesson>();

    [SerializeField] private GameObject RewardContainer;

    [SerializeField] private GameObject ItemRewardContainer;
    [SerializeField] private GameObject ItemRewardInfo;


    private bool HasActiveQuest => questAsLesson is { isActive: true };

    public GameObject OngoingquestWindow;


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

        OngoingquestWindow.SetActive(questAsLesson != null && questAsLesson.isActive);
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
                lesson.Coins = lesson.SecondCoins;
                lesson.Experience = lesson.SecondExperience;
            }

            // Disable the button if there's an active quest
            buttonScript.GetComponent<Button>().interactable = !HasActiveQuest;
        }
    }

    public void OnButtonClick(LessonComponentButton lessonComponentButton)
    {
        // Find the corresponding lesson based on the button ID
        Lesson selectedLesson = FindLessonById(lessonComponentButton.GetButtonID());

        // If the lesson is found, update the LessonContainer
        if (selectedLesson != null)
        {
            SelectedLesson(selectedLesson);
        }
    }

    public void SelectedLesson(Lesson lesson)
    {
        if (lesson == null)
        {
            Debug.LogError("Selected lesson is null.");
            return;
        }

        questAsLesson = new QuestAsLesson
        {
            LessonID = lesson.LessonID,
            chapterName = lesson.chapterName,
            fullDescription = lesson.fullDescription,
            RewardCoins = lesson.Coins,
            RewardExperience = lesson.Experience,
            materials = new List<MaterialEntry>(lesson.materials),
            itemRewards = new List<MaterialEntry>(lesson.ItemRewards),
            steps = new List<LessonSteps>(lesson.steps),
            isCompleted = lesson.isCompleted,
            isRewardCollected = lesson.isItemRewardCollected,
            isActive = false
        };

        UpdateLessonContainer(lesson);

        // Pass the OngoingQuestWindow reference to the CurrentLessonToDisplay
        if (currentLessonToDisplay != null)
        {
            currentLessonToDisplay.OngoingQuestWindow = OngoingquestWindow;
        }
    }


    private Lesson FindLessonById(string lessonID)
    {
        foreach (Lesson lesson in Clonedlessons)
        {
            if (lesson.LessonID == lessonID)
            {
                Debug.Log("Found lesson: " + lessonID);
                CurrentButtonID = lessonID;
                return lesson;
            }
        }

        Debug.LogWarning("Lesson not found: " + lessonID);
        return null;
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

            foreach (Transform child in lessonContainer.ItemsToUnlockContainer.transform)
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

                TextMeshProUGUI title = materialObject
                    .transform.Find("ItemName")
                    .GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI quantity = materialObject
                    .transform.Find("ItemQuantity")
                    .GetComponent<TextMeshProUGUI>();
                Image itemIcon = materialObject.GetComponentInChildren<Image>();

                itemIcon.sprite = material.ItemIcon;
                title.text = material.materialName;
                quantity.text = material.Quantity.ToString();
            }

            if (lesson.isItemRewardCollected)
            {
                ItemRewardContainer.SetActive(false);
                ItemRewardInfo.SetActive(true);
            }
            else
            {
                ItemRewardContainer.SetActive(true);
                ItemRewardInfo.SetActive(false);
                foreach (MaterialEntry itemReward in lesson.ItemRewards)
                {
                    GameObject itemRewardObject = Instantiate(
                        lessonContainer.Material,
                        lessonContainer.ItemsToUnlockContainer.transform
                    );

                    TextMeshProUGUI title = itemRewardObject
                        .transform.Find("ItemName")
                        .GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI quantity = itemRewardObject
                        .transform.Find("ItemQuantity")
                        .GetComponent<TextMeshProUGUI>();
                    Image itemIcon = itemRewardObject.GetComponentInChildren<Image>();

                    itemIcon.sprite = itemReward.ItemIcon;
                    title.text = itemReward.materialName;
                    quantity.text = "";
                }
            }

            // Enable or disable the buttons as necessary
            lessonContainer.AcceptButton.interactable = true; // Example

            // Do not update quest here
        }
    }

    public void OnAcceptButtonClick()
    {
        if (HasActiveQuest)
        {
            Debug.LogError("Cannot start a new lesson while another quest is active.");
            return;
        }

        if (questAsLesson == null)
        {
            Debug.LogError("No lesson is currently selected. Please select a lesson before starting.");
            return;
        }

        CurrentLessonWindow.SetActive(true);
        questAsLesson.isActive = true;

        // Simplify the activation logic
        OngoingquestWindow.SetActive(true);

        Lesson selectedLesson = FindLessonById(CurrentButtonID);
        if (selectedLesson != null)
        {
            UpdateQuest(selectedLesson);
        }
        else
        {
            Debug.LogError("No lesson found for the current button ID. Make sure the lesson exists.");
            return;
        }

        if (currentLessonToDisplay != null)
        {
            currentLessonToDisplay.SetCurrentLesson(questAsLesson);
            currentLessonToDisplay.UpdateLessonDisplay();
            DataManager.Instance.gameData.currentQuestIndex = int.Parse(CurrentButtonID);
        }

        DataManager.Instance.gameData.quest = questAsLesson;

        TutorialManager tutorialManager = FindObjectOfType<TutorialManager>(true);
        if (tutorialManager != null && !tutorialManager.isTutorialComplete)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>(true);
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("currentExperiment");
        }
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
        questAsLesson.steps = new List<LessonSteps>();
        questAsLesson.itemRewards = new List<MaterialEntry>();


        foreach (MaterialEntry material in lesson.ItemRewards)
        {
            questAsLesson.itemRewards.Add(material);
        }

        foreach (MaterialEntry material in lesson.materials)
        {
            questAsLesson.materials.Add(material);
        }

        foreach (LessonSteps step in lesson.steps)
        {
            questAsLesson.steps.Add(step);
        }
    }

    public void PickFirstAvailableLesson()
    {
        foreach (Lesson lesson in Clonedlessons)
        {
            if (!lesson.isCompleted) // Check for the first incomplete lesson
            {
                CurrentButtonID = lesson.LessonID; // Set the CurrentButtonID
                SelectedLesson(lesson); // Pass the lesson to the selection method
                UpdateLessonContainer(lesson); // Update the display
                break;
            }
        }
    }

    public void OnRewardsCollected()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>(true);
        ItemManager itemManager = FindObjectOfType<ItemManager>(true);
        RewardItem itemToReward = FindObjectOfType<RewardItem>(true);

        // Add experience points to the player
        playerStats.AddCoins(questAsLesson.RewardCoins);
        playerStats.AddExp(questAsLesson.RewardExperience);

        List<Item> _itemList = new List<Item>();

        foreach (MaterialEntry itemReward in questAsLesson.itemRewards)
        {
            // Find the matching item in the clonedItems list
            Item matchingItem = itemManager.clonedItems.Find(item =>
                item.itemName == itemReward.materialName
            );

            if (matchingItem != null)
            {
                // Mark the item as unlocked and instantiate it in the inventory
                if (!matchingItem.isUnlock)
                {
                    matchingItem.isUnlock = true;
                    _itemList.Add(matchingItem);
                    Debug.Log($"{matchingItem.itemName}");
                }
                else
                {
                    Debug.Log("Item already unlocked.");
                }
            }

            itemReward.isCollected = true;
        }

        itemToReward.GiveRewardItem(_itemList);

        // Flag the current lesson as completed
        questAsLesson.isCompleted = true;
        questAsLesson.isRewardCollected = true;


        // Find the lesson in Clonedlessons and update its state
        Lesson lessonToUpdate = Clonedlessons.Find(lesson => lesson.LessonID == questAsLesson.LessonID);
        if (lessonToUpdate != null)
        {
            lessonToUpdate.isCompleted = true;
            lessonToUpdate.isItemRewardCollected = true;

            foreach (var material in lessonToUpdate.ItemRewards)
            {
                material.isCollected = true;
            }
        }

        questAsLesson = null;
        RefreshLesson();
        RewardContainer.SetActive(false);
        DataManager.Instance.SaveGame();
        Debug.Log("Rewards collected, lesson and items flagged as completed.");

        AchieveManager.Instance.AddAchievementProgress("NoviceChemist", 1);


        ObtainableManager obtainableManager = FindObjectOfType<ObtainableManager>(true);
        obtainableManager.StartDistributingReward();
    }

    public void RefreshLesson()
    {
        foreach (Lesson lesson in Clonedlessons)
        {
            foreach (MaterialEntry material in lesson.materials)
            {
                material.isCollected = false;
            }
        }
    }


    public void LoadData(GameData gameData)
    {
        // Check if the quest data exists
        if (gameData.quest == null)
        {
            Debug.Log("No quest data found");
        }
        else
        {
            questAsLesson = gameData.quest;

            if (questAsLesson.isCompleted)
            {
                RewardContainer.SetActive(true);
                RewardContainer.GetComponent<RewardDistributor>().SetRewards(questAsLesson);
            }
            else if (!questAsLesson.isCompleted)
            {
                questAsLesson.isActive = false;
                gameData.quest = null;
                BagManager bagManager = FindObjectOfType<BagManager>(true);
                bagManager.ClearBag();

                Debug.Log("Quest was active but not completed. Resetting quest.");
            }
        }

        // Load lessons if available; otherwise, clone from source
        if (gameData.lessons != null && gameData.lessons.Count > 0)
        {
            Clonedlessons = gameData.lessons;
            RefreshLesson();
            Debug.Log("Loaded lessons from GameData. Lesson count: " + Clonedlessons.Count);
        }
        else
        {
            CloneLessons();
            Debug.Log("No lessons found in GameData. Creating new lessons from source.");
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