using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private GameObject ProceedWindow;

    [SerializeField]
    private GameObject ItemRewardContainer;

    private bool HasActiveQuest => CurrentLesson != null;

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
        UpdateLessonDisplay();

        ChapterNumberAndTitle.text = CurrentLesson.chapterNumber + ". " + CurrentLesson.chapterName;
        ChapterDescription.text = CurrentLesson.fullDescription;
        RewardCoin.text = "Coin: " + CurrentLesson.RewardCoins.ToString();
        RewardExperience.text = "Experience: " + CurrentLesson.RewardExperience.ToString();

        // Clear any existing materials before adding new ones
        foreach (Transform child in MaterialContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in ItemRewardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (MaterialEntry materialEntry in lesson.materials)
        {
            GameObject materialObject = Instantiate(Material, MaterialContainer.transform);

            TextMeshProUGUI title = materialObject
                .transform.Find("ItemName")
                .GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI quantity = materialObject
                .transform.Find("ItemQuantity")
                .GetComponent<TextMeshProUGUI>();

            Image itemIcon = materialObject.GetComponentInChildren<Image>();

            itemIcon.sprite = materialEntry.ItemIcon;
            title.text = materialEntry.materialName;
            quantity.text = materialEntry.Quantity.ToString();
        }

        foreach (MaterialEntry itemReward in lesson.itemRewards)
        {
            GameObject itemRewardObject = Instantiate(Material, ItemRewardContainer.transform);

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

        // Update the BagManager's item limit
        BagManager bagManager = FindObjectOfType<BagManager>(true);
        if (bagManager != null)
        {
            bagManager.itemLimit = lesson.materials.Count; // Set item limit to the number of required materials
            bagManager.UpdateItemCount(); // Update the UI for item count and limit
        }
    }

    public void UpdateLessonDisplay()
    {
        bool hasLesson = CurrentLesson != null;

        // Set panels active or inactive based on whether a lesson is active
        noCurrentWindow.SetActive(!hasLesson);
        hasCurrentLessonWindow.SetActive(hasLesson);
    }

    public void OnGotoLessonButtonClick()
    {
        // Open Lesson Window
        this.gameObject.SetActive(false);
        LessonWindow.SetActive(true);
    }

    public void AbandonLesson()
    {
        if (CurrentLesson != null && CurrentLesson.isActive)
        {
            CurrentLesson.isActive = false;
            CurrentLesson.isCompleted = false;

            ChapterNumberAndTitle.text = "";
            ChapterDescription.text = "";
            RewardCoin.text = "0";
            RewardExperience.text = "0";

            foreach (Transform child in MaterialContainer.transform)
            {
                Destroy(child.gameObject);
            }

            UpdateLessonDisplay();

            DataManager.Instance.gameData.quest = null;
            Debug.Log("Current lesson abandoned.");

            // Reset the BagManager's item limit
            BagManager bagManager = FindObjectOfType<BagManager>();
            if (bagManager != null)
            {
                bagManager.itemLimit = 0;
                bagManager.UpdateItemCount();
            }
        }
    }

    public void CheckItem(Item item)
    {
        foreach (var currentItemQuest in CurrentLesson.materials)
        {
            if (item.itemName == currentItemQuest.materialName)
            {
                // Reduce quantity if it's greater than 1
                if (currentItemQuest.Quantity > 1)
                {
                    currentItemQuest.Quantity--;
                    Debug.Log(
                        $"Reduced quantity for {item.itemName}: {currentItemQuest.Quantity} remaining."
                    );
                }
                else
                {
                    // Mark as collected if quantity reaches 0
                    currentItemQuest.Quantity = 0;
                    currentItemQuest.isCollected = true;
                    Debug.Log($"Item fully collected: {item.itemName}");
                }

                // Update the MaterialContainer UI
                foreach (Transform child in MaterialContainer.transform)
                {
                    TextMeshProUGUI title = child.Find("ItemName").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI quantity = child
                        .Find("ItemQuantity")
                        .GetComponent<TextMeshProUGUI>();

                    if (title.text == item.itemName)
                    {
                        quantity.text = currentItemQuest.Quantity.ToString();

                        // Apply strikethrough if fully collected
                        if (currentItemQuest.isCollected)
                        {
                            title.fontStyle = FontStyles.Strikethrough;
                            quantity.fontStyle = FontStyles.Strikethrough;
                        }
                    }
                }

                // Check if all items are collected
                if (CurrentLesson.materials.All(m => m.isCollected))
                {
                    Debug.Log(
                        "All items collected! Lesson completed: " + CurrentLesson.chapterName
                    );
                    ProceedWindow.SetActive(true);
                }

                return; // Exit loop after finding and processing the item
            }
        }
    }

    public void ClearCurrentLesson()
    {
        if (CurrentLesson != null)
        {
            CurrentLesson.isActive = false;
            CurrentLesson.isCompleted = false;

            // Clear UI fields
            ChapterNumberAndTitle.text = "";
            ChapterDescription.text = "";
            RewardCoin.text = "0";
            RewardExperience.text = "0";

            // Clear materials
            foreach (Transform child in MaterialContainer.transform)
            {
                Destroy(child.gameObject);
            }

            // Clear saved data
            DataManager.Instance.gameData.quest = null;
            CurrentLesson = null;

            Debug.Log("Current lesson and quest data cleared.");
        }
    }

    public void UpdateMaterialStates()
    {
        if (CurrentLesson == null)
            return;

        BagManager bagManager = FindObjectOfType<BagManager>(true);
        if (bagManager == null)
            return;

        foreach (MaterialEntry materialEntry in CurrentLesson.materials)
        {
            // Check if the item exists in the bag
            bool isInBag = bagManager.bagItems.Any(bagItem =>
                bagItem.item.itemName == materialEntry.materialName
            );

            materialEntry.isCollected = isInBag;

            MaterialContainer
                .GetComponentsInChildren<TextMeshProUGUI>()
                .Where(material => material.text == materialEntry.materialName)
                .ToList()
                .ForEach(material =>
                {
                    material.fontStyle = isInBag ? FontStyles.Strikethrough : FontStyles.Normal;
                });
        }
    }
}
