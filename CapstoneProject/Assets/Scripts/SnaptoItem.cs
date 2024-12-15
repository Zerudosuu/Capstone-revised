// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.Mathematics;
// using UnityEngine;
// using UnityEngine.UI;

// public class SnaptoItem : MonoBehaviour
// {
//     public ScrollRect scrollRect;
//     public RectTransform contentPanel;

//     public float[] snapPositions;
//     public float snapSpeed = 10f; // Controls how fast the snap happens
//     private float targetPositionX;

//     [Header("Snap to Item Buttons")]
//     public Button ToRightButton;
//     public Button ToLeftButton;

//     // Start is called before the first frame update
//     void Start()
//     {
//         targetPositionX = contentPanel.localPosition.x;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Find the closest snap position based on current position
//         float currentPosition = contentPanel.localPosition.x;
//         float closestPosition = FindClosestSnapPosition(currentPosition);

//         // Gradually move towards the closest snap position
//         targetPositionX = closestPosition;
//         float newXPosition = Mathf.Lerp(
//             contentPanel.localPosition.x,
//             targetPositionX,
//             Time.deltaTime * snapSpeed
//         );

//         contentPanel.localPosition = new Vector3(
//             newXPosition,
//             contentPanel.localPosition.y,
//             contentPanel.localPosition.z
//         );
//     }

//     private float FindClosestSnapPosition(float currentPosition)
//     {
//         float closest = snapPositions[0];
//         float minDistance = Mathf.Abs(currentPosition - closest);

//         foreach (float snapPosition in snapPositions)
//         {
//             float distance = Mathf.Abs(currentPosition - snapPosition);
//             if (distance < minDistance)
//             {
//                 closest = snapPosition;
//                 minDistance = distance;
//             }
//         }

//         return closest;
//     }
// }

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SnaptoItem : MonoBehaviour
{
    public RectTransform contentPanel;

    [Header("Snap Positions")]
    public float[] snapPositions = { 0f, -1080f }; // Predefined positions
    public float snapSpeed = 10f; // Controls how fast the snap happens

    private float targetPositionX;

    [Header("Snap to Item Buttons")]
    public Button ToRightButton;
    public Button ToLeftButton;

    void Start()
    {
        targetPositionX = contentPanel.localPosition.x;

        // Assign button click listeners
        ToRightButton.onClick.AddListener(MoveToRight);
        ToLeftButton.onClick.AddListener(MoveToLeft);

        // Initialize button visibility
        UpdateButtonVisibility(0);
    }

    // Method to move to the right position (-1080) and update visibility
    public void MoveToRight()
    {
        targetPositionX = snapPositions[1]; // Move to -1080
        StartCoroutine(SmoothMove());

        UpdateButtonVisibility(targetPositionX);
    }

    // Method to move to the left position (0) and update visibility
    public void MoveToLeft()
    {
        targetPositionX = snapPositions[0]; // Move to 0
        StartCoroutine(SmoothMove());

        UpdateButtonVisibility(targetPositionX);
    }

    // Coroutine for smooth movement
    private IEnumerator SmoothMove()
    {
        while (Mathf.Abs(contentPanel.localPosition.x - targetPositionX) > 0.1f)
        {
            float newXPosition = Mathf.Lerp(
                contentPanel.localPosition.x,
                targetPositionX,
                Time.deltaTime * snapSpeed
            );

            contentPanel.localPosition = new Vector3(
                newXPosition,
                contentPanel.localPosition.y,
                contentPanel.localPosition.z
            );

            yield return null;
        }

        // Snap to exact position
        contentPanel.localPosition = new Vector3(
            targetPositionX,
            contentPanel.localPosition.y,
            contentPanel.localPosition.z
        );
    }

    // Update button visibility based on the current target position
    private void UpdateButtonVisibility(float currentPosition)
    {
        ToRightButton.gameObject.SetActive(currentPosition == snapPositions[0]);
        ToLeftButton.gameObject.SetActive(currentPosition == snapPositions[1]);
    }
}
