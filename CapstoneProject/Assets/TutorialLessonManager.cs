using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialLessonManager : MonoBehaviour, IData
{
    public GameObject PanelTutorial; // Tutorial panel
    public bool tutorialComplete = false;
    public Button nextButton; // Next button
    public Button prevButton; // Previous button
    public Button closeButton; // Close button

    private int tutorialIndex = 0; // Current tutorial index
    private Transform[] tutorialSteps; // Array to store the tutorial steps
    private RectTransform panelRectTransform; // RectTransform of PanelTutorial
    private bool TutorialButtonWasPressed = false;
    public GameObject GameStartPanel;

    private void Start()
    {
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        if (!tutorialComplete || TutorialButtonWasPressed)
        {
            // Get the RectTransform of the PanelTutorial
            panelRectTransform = PanelTutorial.GetComponent<RectTransform>();

            // Get all child elements of PanelTutorial
            int childCount = PanelTutorial.transform.childCount;
            tutorialSteps = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                tutorialSteps[i] = PanelTutorial.transform.GetChild(i);
                tutorialSteps[i].gameObject.SetActive(false); // Deactivate all steps initially
            }

            // Activate the first tutorial step
            if (tutorialSteps.Length > 0)
            {
                tutorialSteps[0].gameObject.SetActive(true);
            }


            // Update button states
            UpdateButtonStates();
            SmoothRebuildLayout();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Display the next tutorial step
    public void ShowNextTutorial()
    {
        if (tutorialIndex < tutorialSteps.Length - 1)
        {
            tutorialSteps[tutorialIndex].gameObject.SetActive(false); // Deactivate current step
            tutorialIndex++;
            tutorialSteps[tutorialIndex].gameObject.SetActive(true); // Activate next step
            UpdateButtonStates();
            SmoothRebuildLayout();
        }
    }

    // Display the previous tutorial step
    public void ShowPreviousTutorial()
    {
        if (tutorialIndex > 0)
        {
            tutorialSteps[tutorialIndex].gameObject.SetActive(false); // Deactivate current step
            tutorialIndex--;
            tutorialSteps[tutorialIndex].gameObject.SetActive(true); // Activate previous step
            UpdateButtonStates();
            SmoothRebuildLayout();
        }
    }

    // Update the states of the buttons
    private void UpdateButtonStates()
    {
        prevButton.interactable = tutorialIndex > 0;
        nextButton.interactable = tutorialIndex < tutorialSteps.Length - 1;

        if (nextButton.interactable == false)
        {
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            closeButton.gameObject.SetActive(false);
        }
    }

    public void CloseTutorial()
    {
        tutorialComplete = true;
        TutorialButtonWasPressed = false;
        gameObject.SetActive(false);

        ExperimentManager experimentManager = FindObjectOfType<ExperimentManager>(true);
        if (experimentManager.isGameStarted)
        {
            GameStartPanel.SetActive(false);
        }
        else
        {
            GameStartPanel.SetActive(true);
        }
    }

    // Smoothly rebuild the layout with DOTween
    private void SmoothRebuildLayout()
    {
        // Force rebuild to get the new preferred size
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRectTransform);

        // Get the new preferred size
        float targetHeight = LayoutUtility.GetPreferredHeight(panelRectTransform);

        // Animate the height of the panel
        panelRectTransform.DOSizeDelta(new Vector2(panelRectTransform.sizeDelta.x, targetHeight), 0.3f)
            .SetEase(Ease.InOutQuad);
    }

    public void TutorialButtonPressed()
    {
        this.gameObject.SetActive(true);
        TutorialButtonWasPressed = true;
    }

    public void LoadData(GameData gameData)
    {
        tutorialComplete = gameData.isLessonTutorialCompleted;
    }

    public void SavedData(GameData gameData)
    {
        gameData.isLessonTutorialCompleted = tutorialComplete;
    }
}