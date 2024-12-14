using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StepManager : MonoBehaviour
{
    [SerializeField]
    private Button completeButton;

    [SerializeField]
    private TextMeshProUGUI stepText;

    [SerializeField]
    private GameObject experimentModal;

    ExperimentObjectManager experimentObjectManager;
    private int currentStepIndex = 0; // Track the current step index
    private bool allStepsCompleted = false;

    void Start()
    {
        experimentObjectManager = FindObjectOfType<ExperimentObjectManager>(true);
        completeButton.onClick.AddListener(OnCompleteButtonClicked);
    }

    void OnCompleteButtonClicked()
    {
        if (allStepsCompleted)
        {
            Debug.Log("All steps are already completed.");
            return;
        }

        // Mark the current step as completed
        experimentObjectManager.currentLesson.steps[currentStepIndex].CompleteStep();

        Debug.Log(
            $"Step {currentStepIndex + 1} completed: {experimentObjectManager.currentLesson.steps[currentStepIndex].stepDescription}"
        );

        // Advance to the next step
        currentStepIndex++;

        if (currentStepIndex < experimentObjectManager.currentLesson.steps.Count)
        {
            DisplayCurrentStep();
        }
        else
        {
            allStepsCompleted = true;
            stepText.text = "All steps completed!";
            Debug.Log("All steps are completed!");

            // Show the experiment modal

            experimentModal.SetActive(true);
        }
    }

    private void DisplayCurrentStep()
    {
        if (
            currentStepIndex >= 0
            && currentStepIndex < experimentObjectManager.currentLesson.steps.Count
        )
        {
            stepText.text = experimentObjectManager
                .currentLesson
                .steps[currentStepIndex]
                .stepDescription;
            Debug.Log(
                $"Displaying Step {currentStepIndex + 1}: {experimentObjectManager.currentLesson.steps[currentStepIndex].stepDescription}"
            );
        }
        else
        {
            Debug.LogError("Invalid step index!");
        }
    }
}
