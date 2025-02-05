using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class ExperimentManager : MonoBehaviour, IData
{
    // public Slider currentScore; // The slider displaying the current score
    // public float score = 100f; // Initial score
    //
    // public float yellowPenalty = 10f; // Score reduction for Yellow
    // public float redPenalty = 20f; // Score reduction for Red
    // public float greenBonus = 5f; // Score bonus for Green if score is 100

    public GameObject slotPrefab; // Prefab for the item slot

    public GameObject GameStartPanel;
    public GameObject GameOverPanel;

    ExperimentObjectManager experimentObjectManager;

    ScrollViewSlot[] slots;

    public bool isGameStarted = false;

    #region ExperimentData

    public StudentExperimentRecord studentExperimentRecord = new StudentExperimentRecord();

    #endregion


    public static event Action OnGameStart;


    public void OnEnable()
    {
        LessonTimer.OnTimerEndAction += GameOver;
    }

    public void OnDisable()
    {
        LessonTimer.OnTimerEndAction -= GameOver;
    }


    void Start()
    {
        // MeterPanel.SetActive(false);
        // currentScore.minValue = 0;
        // currentScore.maxValue = 100;
        // currentScore.value = score; // Initialize slider value

        experimentObjectManager = FindObjectOfType<ExperimentObjectManager>();
        GameOverPanel.SetActive(false);
        // Get all slots as children of the item container
        slots = experimentObjectManager.ItemContainer.GetComponentsInChildren<ScrollViewSlot>();
    }

    // public void UpdateScore(string zone)
    // {
    //     switch (zone)
    //     {
    //         case "Yellow":
    //             score -= yellowPenalty;
    //             Debug.Log("Yellow Zone! -10 points");
    //             break;
    //         case "Red":
    //             score -= redPenalty;
    //             Debug.Log("Red Zone! -20 points");
    //             break;
    //         case "Green":
    //             if (score == 100f)
    //             {
    //                 score += greenBonus;
    //                 Debug.Log("Green Zone! Bonus +5 points");
    //             }
    //             else
    //             {
    //                 Debug.Log("Green Zone! No penalty");
    //             }
    //
    //             break;
    //     }
    //
    //     // Clamp score to a minimum of 0 and maximum of 100
    //     score = Mathf.Clamp(score, 0, 100);
    //     currentScore.value = score; // Update the score slider
    // }
    //
    public void StartGame()
    {
        GameStartPanel.SetActive(false);
        isGameStarted = true;
        OnGameStart?.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateItemPrefab(ItemReaction itemReaction, string ItemInteracted)
    {
        if (itemReaction != null && itemReaction.item.CurrentState != null)
        {
            bool stateChanged = false;

            // Iterate through all possible states
            foreach (var state in itemReaction.item.states)
            {
                if (state.conditions != null && state.conditions.Count > 0)
                {
                    bool allConditionsMet = true;

                    // Check if all conditions are met
                    foreach (var condition in state.conditions)
                    {
                        if (condition.itemNameRequirement != ItemInteracted)
                        {
                            allConditionsMet = false;
                            break; // Exit early if any condition fails
                        }
                    }

                    if (allConditionsMet)
                    {
                        // Update item to new state
                        itemReaction.item.currentStateIndex = itemReaction.item.states.IndexOf(state);
                        itemReaction.GetComponent<Image>().sprite = state.sprite;
                        itemReaction.transform.name = state.stateName;
                        itemReaction.item.currentTemperature = state.Temperature;
                        stateChanged = true;

                        Debug.Log($"✅ Item updated to state: {state.stateName}");
                        break; // Stop checking further states once changed
                    }
                }
            }

            if (!stateChanged)
            {
                Debug.LogWarning(
                    $"⚠️ No matching state found for item: {itemReaction.item.itemName} with interaction: {ItemInteracted}");
            }
        }
        else
        {
            Debug.LogError("❌ ItemReaction or CurrentState is null. Cannot update prefab.");
        }
    }


    public ScrollViewSlot FindEmptySlot()
    {
        foreach (ScrollViewSlot slot in slots)
        {
            if (slot.transform.childCount == 0) // Check if slot is empty
            {
                return slot;
            }
        }

        return null; // No empty slots found
    }


    public void GetQuestionAndAnswer(List<string> Question, List<string> Answer)
    {
        studentExperimentRecord.questionsAndAnswers.Clear();

        for (int i = 0; i < Question.Count; i++)
        {
            studentExperimentRecord.questionsAndAnswers.Add(new StudentExperimentRecord.QuestionAndAnswer
            {
                question = Question[i],
                answer = Answer[i]
            });
        }
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void LoadData(GameData gameData)
    {
        LoadDataFromGameData(gameData);
    }

    public void SavedData(GameData gameData)
    {
        gameData.lessons[gameData.currentQuestIndex].studentRecords.Add(studentExperimentRecord);
    }


    //LOAD DATA FUNCTIONS

    public void LoadDataFromGameData(GameData data)
    {
        // Ensure lessons list is not null
        if (data.lessons == null || data.lessons.Count == 0)
        {
            Debug.LogError("❌ Error: Lessons list is null or empty.");
            return;
        }

        // Ensure currentQuestIndex is within valid range
        if (data.currentQuestIndex < 0 || data.currentQuestIndex >= data.lessons.Count)
        {
            Debug.LogError(
                $"❌ Error: currentQuestIndex ({data.currentQuestIndex}) is out of range. Lessons count: {data.lessons.Count}");
            return;
        }

        Lesson currentLesson = data.lessons[data.currentQuestIndex];

        // Ensure studentRecords is not null
        if (currentLesson.studentRecords == null)
        {
            Debug.LogWarning("⚠️ studentRecords list is null. Initializing a new list.");
            currentLesson.studentRecords = new List<StudentExperimentRecord>();
        }

        // Find the highest existing attemptId and assign a new unique one
        int maxAttemptId = 0;
        foreach (var record in currentLesson.studentRecords)
        {
            if (record.attemptId > maxAttemptId)
            {
                maxAttemptId = record.attemptId;
            }
        }

        // Assign a new unique attempt ID
        studentExperimentRecord.attemptId = maxAttemptId + 1;
        studentExperimentRecord.studentName = data.playerName;
        studentExperimentRecord.lessonId = currentLesson.LessonID;

        Debug.Log($"✅ Data loaded successfully. New attempt ID: {studentExperimentRecord.attemptId}");
    }
}