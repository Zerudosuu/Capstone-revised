using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameModeSelector : MonoBehaviour, IData
{
    [Header("Game Mode Selector")]
    VisualElement GameModeContainer;
    VisualElement GameModeLessonContainer;
    VisualElement GameModeCreativeContainer; // Corrected variable name

    bool isGameModeLessonActive = false;
    bool isGameModeCreativeisActive = false;

    Button LessonPlayButton;

    Button CreativePlayButton;

    public GameModeSelected gameModeSelected;

    void OnEnable()
    {
        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;
        GameModeContainer = root.Q<VisualElement>("GameModeContainer");
        GameModeLessonContainer = GameModeContainer.Q<VisualElement>("LessonContainer");
        GameModeCreativeContainer = GameModeContainer.Q<VisualElement>("CreativeContainer");

        LessonPlayButton = GameModeLessonContainer.Q<Button>("LessonPlayButton");
        CreativePlayButton = GameModeCreativeContainer.Q<Button>("CreativePlayButton");

        LessonPlayButton.RegisterCallback<ClickEvent>(evt => LessonPlay());
        CreativePlayButton.RegisterCallback<ClickEvent>(evt => CreativePlay());

        RegisterMouseEvents();

        SelectGameMode(GameModeLessonContainer);
    }

    private void CreativePlay()
    {
        print("Creative Play started");
        gameModeSelected = GameModeSelected.Creative;
        DataManager.Instance.gameData.SceneName = "LessonMode";
        DataManager.Instance.SaveGame();
        DataManager.Instance.LoadGame();
        SceneManager.LoadSceneAsync(DataManager.Instance.gameData.SceneName);
    }

    void LessonPlay()
    {
        gameModeSelected = GameModeSelected.Lesson;
        DataManager.Instance.gameData.SceneName = "LessonMode";
        DataManager.Instance.SaveGame();
        DataManager.Instance.LoadGame();
        SceneManager.LoadSceneAsync(DataManager.Instance.gameData.SceneName);
        print("Lesson Play started");
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

    public void LoadData(GameData gameData)
    {
        this.gameModeSelected = (GameModeSelected)gameData.LessonMode;
    }

    public void SavedData(GameData gameData)
    {
        gameData.LessonMode = (GameMode)this.gameModeSelected;
    }
}

public enum GameModeSelected
{
    None,
    Lesson,
    Creative,
}
