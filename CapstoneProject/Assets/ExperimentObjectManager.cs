using System.Collections;
using UnityEngine;

public class ExperimentObjectManager : MonoBehaviour, IData
{
    [SerializeField]
    private GameObject experimentModal;

    [SerializeField]
    private QuestAsLesson currentLesson;

    void Start()
    {
        if (DataManager.Instance.gameData.quest != null)
        {
            currentLesson = DataManager.Instance.gameData.quest;
        }
        else
        {
            Debug.LogWarning("No quest is currently active.");
        }
    }

    public void BackToLesson()
    {
        if (currentLesson != null)
        {
            currentLesson.isCompleted = true; // Mark the lesson as completed
            DataManager.Instance.SaveGame(); // Save the updated state
            UnityEngine.SceneManagement.SceneManager.LoadScene("LessonMode"); // Ensure scene name is correct
        }
        else
        {
            Debug.LogError("No current lesson to complete.");
        }
    }

    public void LoadData(GameData data)
    {
        if (data.quest != null)
        {
            currentLesson = data.quest;
        }
    }

    public void SavedData(GameData data)
    {
        if (data.quest != null && currentLesson != null)
        {
            data.quest.isCompleted = currentLesson.isCompleted; // flag the current lesson as completed
            data.lessons[data.currentQuestIndex - 1].isCompleted = currentLesson.isCompleted; // Update the lesson status -1 cause index starts from 0
            data.BagItems.Clear(); // Clear the bag items
            data.currentQuestIndex = 0; // Reset the current quest index
        }
    }
}
