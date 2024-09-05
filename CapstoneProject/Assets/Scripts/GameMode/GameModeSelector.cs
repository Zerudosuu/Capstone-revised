using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameModeSelector : MonoBehaviour
{
    VisualElement GameModeContainer;
    VisualElement GameModeLessonContainer;
    VisualElement GameModeCreativeContainer; // Corrected variable name

    bool isGameModeLessonActive = false;
    bool isGameModeCreativeisActive = false;

    void OnEnable()
    {
        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;
        GameModeContainer = root.Q<VisualElement>("GameModeContainer");
        GameModeLessonContainer = GameModeContainer.Q<VisualElement>("LessonContainer");
        GameModeCreativeContainer = GameModeContainer.Q<VisualElement>("CreativeContainer");

        RegisterMouseEvents();
    }

    void RegisterMouseEvents()
    {
        foreach (VisualElement panel in GameModeContainer.Children())
        {
            panel.RegisterCallback<MouseDownEvent>(evt => SelectGameMode(evt.target));
        }
    }

    private void SelectGameMode(IEventHandler target)
    {
        var visualElement = target as VisualElement; // Cast to VisualElement
        if (visualElement != null)
        {
            if (visualElement.name == "LessonContainer")
            {
                // Expand LessonContainer and reset CreativeContainer
                visualElement.AddToClassList("SelectedLesson");
                GameModeCreativeContainer.RemoveFromClassList("SelectedCreative");
                isGameModeLessonActive = true;
                isGameModeCreativeisActive = false;

                CloseOpenDetails(isGameModeLessonActive);
            }
            else if (visualElement.name == "CreativeContainer")
            {
                visualElement.AddToClassList("SelectedCreative");
                GameModeLessonContainer.RemoveFromClassList("SelectedLesson");
                isGameModeLessonActive = false;
                isGameModeCreativeisActive = true;

                CloseOpenDetails(isGameModeCreativeisActive);
            }
        }
    }

    private void CloseOpenDetails(bool selected)
    {
        VisualElement lessonButtonContainer = GameModeLessonContainer.Q<VisualElement>(
            "LessonButtonContainer-GM"
        );
        VisualElement creativeButtonContainer = GameModeCreativeContainer.Q<VisualElement>(
            "CreativeButtonContainer-GM"
        );

        if (selected == isGameModeLessonActive)
        {
            lessonButtonContainer.style.display = DisplayStyle.Flex; // Show lesson details
            creativeButtonContainer.style.display = DisplayStyle.None; // Hide creative details
        }
        else
        {
            lessonButtonContainer.style.display = DisplayStyle.None; // Hide lesson details
            creativeButtonContainer.style.display = DisplayStyle.Flex; // Show creative details
        }
    }
}
