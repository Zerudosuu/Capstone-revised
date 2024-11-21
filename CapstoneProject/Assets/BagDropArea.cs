using UnityEngine;

public class BagDropArea : MonoBehaviour
{
    BagManager _bagManager;
    LessonManager _lessonManager;
    CurrentLessonToDisplay _currentLessonToDisplay;
    public GameObject lessonContainer;

    void Start()
    {
        _currentLessonToDisplay = FindObjectOfType<CurrentLessonToDisplay>(true);
        _lessonManager = FindObjectOfType<LessonManager>(true);
        _bagManager = FindObjectOfType<BagManager>(true);

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
        }
        else
        {
            lessonContainer.SetActive(true);
        }
    }
}
