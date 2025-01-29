using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Import DOTween namespace

public class AddedActionContainerManager : MonoBehaviour
{
    [SerializeField] private RectTransform otherActionRectTransform;

    private float offscreenPosition;
    private bool isVisible = false; // Track visibility state
    private float initialYPosition; // Store Y position from inspector


    private void OnEnable()
    {
        // Subscribe to the event
        StepManager.OnStepBroadCastAnotherActionPanel += ToggleVisibility;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        StepManager.OnStepBroadCastAnotherActionPanel -= ToggleVisibility;
    }

    void Start()
    {
        float screenWidth = Screen.width;
        offscreenPosition = -screenWidth + 130f; // Start offscreen

        initialYPosition = otherActionRectTransform.anchoredPosition.y; // Get Y from Inspector

        // Set initial position offscreen while keeping Y from inspector
        otherActionRectTransform.anchoredPosition = new Vector2(offscreenPosition, initialYPosition);
    }

    public void ToggleVisibility()
    {
        float targetX = isVisible ? offscreenPosition : 0; // Toggle X position

        otherActionRectTransform.DOAnchorPosX(targetX, 0.5f)
            .SetEase(Ease.InOutQuad);

        isVisible = !isVisible; // Toggle state
    }
}