using UnityEngine;
using Yarn.Unity;

public class BagDropArea : MonoBehaviour
{
    BagManager _bagManager;
    LessonManager _lessonManager;
    CurrentLessonToDisplay _currentLessonToDisplay;
    UIManager _UIManager;
    public GameObject lessonContainer;

    void Start()
    {
        _currentLessonToDisplay = FindObjectOfType<CurrentLessonToDisplay>(true);
        _lessonManager = FindObjectOfType<LessonManager>(true);
        _bagManager = FindObjectOfType<BagManager>(true);
        _UIManager = FindObjectOfType<UIManager>(true);

        if (_bagManager == null)
            print("bagManager is null");
    }

    public void AddedToInventory(Item item)
    {
        if (item != null && _lessonManager != null && _lessonManager.questAsLesson.isActive == true)
        {
            print("Added to inventory: " + item);
            _bagManager.AddItemInBag(item);
            _currentLessonToDisplay.CheckItem(item);


            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>(true);
            if (tutorialManager != null && !tutorialManager.isTutorialComplete && tutorialManager.currentIndex == 19)
            {
                DialogueRunner dialogueRunner = tutorialManager.dialogueRunner.GetComponent<DialogueRunner>();

                if (dialogueRunner != null)
                {
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("hotdogExperiment2");
                }
            }
        }
        else
        {
            _UIManager.LessonState("isLessonContainer");
            _UIManager.OnLessonClick();
        }
    }
}