using System;
using System.Collections;
using UnityEngine;
using DG.Tweening; // DOTween for animations

public class AddedActionContainerManager : MonoBehaviour
{
    [SerializeField] private RectTransform otherActionRectTransform;
    [SerializeField] private GameObject objectToGetAction;
    [SerializeField] private StepManager stepManager;

    private float offscreenPosition;
    private bool isVisible = false; // Track visibility state
    private float initialYPosition; // Store Y position from inspector

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
        float screenWidth = Screen.width;
        offscreenPosition = -screenWidth + 130f; // Start offscreen

        initialYPosition = otherActionRectTransform.anchoredPosition.y; // Preserve Y from Inspector

        // Set initial position offscreen while keeping the Y position unchanged
        otherActionRectTransform.anchoredPosition = new Vector2(offscreenPosition, initialYPosition);
    }

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        otherActionRectTransform.DOAnchorPosX(isVisible ? 0 : offscreenPosition, 0.5f)
            .SetEase(Ease.InOutQuad);
    }

    // Shake function with optional delay
    public void Shake(float duration = 2.0f)
    {
        StartCoroutine(ActionTimer(duration, () =>
        {
            Debug.Log("Shake action completed!");
            Debug.Log(GetItemOnSlot().name);
            stepManager?.ValidateAndCompleteSubStep(GetItemOnSlot()?.name);
        }));
    }

    // Stir function with optional delay
    public void Stir(float duration = 2.0f)
    {
        StartCoroutine(ActionTimer(duration, () =>
        {
            Debug.Log("Stir action completed!");
            Debug.Log(GetItemOnSlot().name);
            stepManager?.ValidateAndCompleteSubStep(GetItemOnSlot()?.name);
        }));
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