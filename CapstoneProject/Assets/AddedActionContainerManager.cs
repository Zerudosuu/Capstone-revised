using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro; // DOTween for animations

public class AddedActionContainerManager : MonoBehaviour
{
    [SerializeField] private RectTransform otherActionRectTransform;
    [SerializeField] private GameObject objectToGetAction;
    [SerializeField] private StepManager stepManager;

    [SerializeField] private GameObject inputContainer;
    [SerializeField] private TMP_InputField inputFieldMinutes;
    [SerializeField] private TMP_InputField inputFieldSeconds;
    [SerializeField] private ExperimentCountDown experimentCountDown;


    private float offscreenPosition;
    private bool isVisible = false; // Track visibility state
    private float initialYPosition; // Store Y position from inspector
    private AudioManager audioManage;

    private void OnEnable()
    {
        StepManager.OnStepBroadCastAnotherActionPanel += ToggleVisibility;
    }

    private void OnDisable()
    {
        StepManager.OnStepBroadCastAnotherActionPanel -= ToggleVisibility;
    }

    void Start()
    {
        audioManage = FindObjectOfType<AudioManager>(); 
        float screenWidth = Screen.width;
        offscreenPosition = -screenWidth + 130f; // Start offscreen

        initialYPosition = otherActionRectTransform.anchoredPosition.y; // Preserve Y from Inspector

        // Set initial position offscreen while keeping the Y position unchanged
        if (DataManager.Instance.gameData.LessonMode == GameMode.Lesson)
        {
            otherActionRectTransform.anchoredPosition = new Vector2(offscreenPosition, initialYPosition);
        }

        inputContainer.SetActive(true);
    }

    // Get total time in seconds from input fields
    private float GetTotalTimeInSeconds()
    {
        int minutes = int.TryParse(inputFieldMinutes.text, out int min) ? min : 0;
        int seconds = int.TryParse(inputFieldSeconds.text, out int sec) ? sec : 0;
        return (minutes * 60) + seconds;
    }

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        otherActionRectTransform.DOAnchorPosX(isVisible ? 0 : offscreenPosition, 0.5f)
            .SetEase(Ease.InOutQuad);
    }

    // Shake function using inputted time
    public void Shake()
    {
        if (DataManager.Instance.gameData.LessonMode == GameMode.Lesson)
        {
            float duration = GetTotalTimeInSeconds();
            string itemName = GetItemOnSlot()?.name; // Get the item being shaken

            Animator animator = GetItemOnSlot()?.GetComponent<Animator>();
            if (animator != null)
            {
                audioManage.PlaySFX("Shake");
                animator.Play("Shake");
            }

            if (experimentCountDown != null)
            {
                experimentCountDown.gameObject.SetActive(true);
                experimentCountDown.SetTime(duration, itemName, "shake",
                    stepManager.GetTargetTemperature()); // Pass item name
            }

            inputContainer.SetActive(false);

            StartCoroutine(ActionTimer(duration, () =>
            {
                Debug.Log($"Shake action completed with item: {itemName}");
                experimentCountDown.gameObject.SetActive(false);
                inputContainer.SetActive(true);

                stepManager?.ValidateAndCompleteSubStep(itemName, "shake"); // Pass item name

                if (animator != null)
                {
                    animator.Play("Idle");
                    audioManage.StopSFX();
                }
            }));
        }
        else
        {
            ItemReaction itemReaction = GetItemOnSlot().gameObject.GetComponent<ItemReaction>();
            if (itemReaction != null)
            {
                itemReaction.CheckReactions(itemReaction.item.states[itemReaction.item.currentStateIndex].stateName,
                    itemReaction.gameObject.GetComponent<DragableItem>());
            }

            print("Shake action completed with item:");
        }
    }

    public void Stir()
    {
        if (DataManager.Instance.gameData.LessonMode == GameMode.Lesson)
        {
            float duration = GetTotalTimeInSeconds();
            string itemName = GetItemOnSlot()?.name; // Get the item being stirred
            if (experimentCountDown != null)
            {
                experimentCountDown.gameObject.SetActive(true);
                experimentCountDown.SetTime(duration, itemName, "stir",
                    stepManager.GetTargetTemperature()); // Pass item name
            }

            inputContainer.SetActive(false);

            StartCoroutine(ActionTimer(duration, () =>
            {
                Debug.Log($"Stir action completed with item: {itemName}");
                experimentCountDown.gameObject.SetActive(false);
                inputContainer.SetActive(true);
                stepManager?.ValidateAndCompleteSubStep(itemName, "stir"); // Pass item name
            }));
        }
        else
        {
            ItemReaction itemReaction = GetItemOnSlot().gameObject.GetComponent<ItemReaction>();
            if (itemReaction != null)
            {
                itemReaction.CheckReactions(itemReaction.item.states[itemReaction.item.currentStateIndex].stateName,
                    itemReaction.gameObject.GetComponent<DragableItem>(), "stir");
            }

            print("Stir action completed with item: ");
        }
    }

    // Timer function for actions
    private IEnumerator ActionTimer(float duration, Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }

    // Get the item on the slot
    public GameObject GetItemOnSlot()
    {
        if (objectToGetAction == null || objectToGetAction.transform.childCount == 0)
            return null;

        return objectToGetAction.transform.GetChild(0).gameObject;
    }
}