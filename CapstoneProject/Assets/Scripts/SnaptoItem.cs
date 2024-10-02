using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SnaptoItem : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListItem;

    public float[] snapPositions;
    public float snapSpeed = 10f; // Controls how fast the snap happens
    private float targetPositionX;

    // Start is called before the first frame update
    void Start()
    {
        targetPositionX = contentPanel.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Find the closest snap position based on current position
        float currentPosition = contentPanel.localPosition.x;
        float closestPosition = FindClosestSnapPosition(currentPosition);

        // Gradually move towards the closest snap position
        targetPositionX = closestPosition;
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
    }

    private float FindClosestSnapPosition(float currentPosition)
    {
        float closest = snapPositions[0];
        float minDistance = Mathf.Abs(currentPosition - closest);

        foreach (float snapPosition in snapPositions)
        {
            float distance = Mathf.Abs(currentPosition - snapPosition);
            if (distance < minDistance)
            {
                closest = snapPosition;
                minDistance = distance;
            }
        }

        return closest;
    }
}
