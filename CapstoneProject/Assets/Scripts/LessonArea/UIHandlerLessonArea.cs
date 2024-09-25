using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class UIHandlerLessonArea : MonoBehaviour
{
    private VisualElement LessonContainer;
    private VisualElement ExperimentContainer;
    private Vector2 originalMousePosition;
    private float originalElementXPosition;

    private const float minX = -1080f;
    private const float maxX = 1080f;
    private readonly float[] snapPositions = { -1080f, 0f, 1080f };

    #region ExperimentButtonsContainer
    VisualElement ExperimentButtonsContainer;
    Button[] ExperimentButtons;
    // VisualElement mix;
    // VisualElement heat;
    // VisualElement cold;
    // VisualElement serve;
    #endregion


    void OnEnable()
    {
        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;
        LessonContainer = root.Q<VisualElement>("LessonContainer");
        ExperimentContainer = LessonContainer.Q<VisualElement>("ExperimentContainer");
        ExperimentButtonsContainer = root.Q<VisualElement>("ExperimentButtonsContainer");

        if (ExperimentButtonsContainer != null)
        {
            print("ExperimentButtonsContainer found");

            ExperimentButtons = ExperimentButtonsContainer.Children().OfType<Button>().ToArray();
            foreach (Button ExperimentButton in ExperimentButtons)
            {
                ExperimentButton.RegisterCallback<ClickEvent>(evt =>
                {
                    print(ExperimentButton.name + "Button clicked");
                });
            }

            RegisterMouseEvents();
        }
        else
        {
            print("ExperimentButtonsContainer not found");
        }
    }

    private void RegisterMouseEvents()
    {
        ExperimentContainer.RegisterCallback<MouseDownEvent>(evt =>
        {
            originalMousePosition = evt.mousePosition;
            originalElementXPosition = ExperimentContainer.transform.position.x;
            ExperimentContainer.CaptureMouse(); // Capture the mouse to continue receiving events
        });

        ExperimentContainer.RegisterCallback<MouseMoveEvent>(evt =>
        {
            if (ExperimentContainer.HasMouseCapture())
            {
                // Calculate the difference in the X-axis mouse movement
                float mouseDeltaX = evt.mousePosition.x - originalMousePosition.x;

                // Calculate the new X position
                float newX = originalElementXPosition + mouseDeltaX;

                // Clamp the X position within the min and max limits
                newX = Mathf.Clamp(newX, minX, maxX);

                // Update the element's position
                ExperimentContainer.transform.position = new Vector3(
                    newX,
                    ExperimentContainer.transform.position.y,
                    ExperimentContainer.transform.position.z
                );
            }
        });

        ExperimentContainer.RegisterCallback<MouseUpEvent>(evt =>
        {
            // Release the mouse when dragging ends
            if (ExperimentContainer.HasMouseCapture())
            {
                ExperimentContainer.ReleaseMouse();

                float closestPosition = FindClosestSnapPosition(
                    ExperimentContainer.transform.position.x
                );
                ExperimentContainer.transform.position = new Vector3(
                    closestPosition,
                    ExperimentContainer.transform.position.y,
                    ExperimentContainer.transform.position.z
                );
            }
        });
    }

    private float FindClosestSnapPosition(float currentX)
    {
        float closest = snapPositions[0];
        float minDistance = Mathf.Abs(currentX - closest);

        foreach (float snapPosition in snapPositions)
        {
            float distance = Mathf.Abs(currentX - snapPosition);
            if (distance < minDistance)
            {
                closest = snapPosition;
                minDistance = distance;
            }
        }

        return closest;
    }
}
