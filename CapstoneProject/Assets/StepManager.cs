using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StepManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stepText;

    [SerializeField]
    private GameObject experimentModal;

    public List<ExperimentStep> experimentSteps = new List<ExperimentStep>();
    private ExperimentObjectManager experimentObjectManager;

    private int currentLessonStepIndex = 0; // Current lesson step index
    private int currentSubStepIndex = 0; // Current sub-step index
    private bool allStepsCompleted = false;

    void Start()
    {
        experimentObjectManager = FindObjectOfType<ExperimentObjectManager>(true);

        // Display the first step when the script starts
        if (experimentObjectManager.currentLesson.steps.Count > 0)
        {
            DisplayCurrentStep();
        }
        else
        {
            Debug.LogError("No steps available in the current lesson.");
        }
    }

    public void ValidateAndCompleteSubStep(string completedSubStep)
    {
        if (allStepsCompleted)
        {
            Debug.Log("All steps and sub-steps are already completed.");
            return;
        }

        var currentLessonStep = experimentObjectManager.currentLesson.steps[currentLessonStepIndex];
        var currentExperimentStep = experimentSteps[currentLessonStepIndex];

        // Validate the sub-step
        currentExperimentStep.ValidateSubStep(completedSubStep);

        // If all sub-steps for the current step are completed, move to the next step
        if (currentExperimentStep.AreAllRequiredStepsCompleted())
        {
            currentLessonStep.CompleteStep(); // Mark lesson step complete
            currentLessonStepIndex++;

            if (currentLessonStepIndex < experimentObjectManager.currentLesson.steps.Count)
            {
                currentExperimentStep.CompleteStep();
                DisplayCurrentStep();
            }
            else
            {
                allStepsCompleted = true;
                stepText.text = "All steps completed!";
                Debug.Log("All steps and sub-steps are completed!");

                // Show the experiment modal
                experimentModal.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Sub-steps remain to be completed.");
        }
    }

    private void DisplayCurrentStep()
    {
        if (
            currentLessonStepIndex >= 0
            && currentLessonStepIndex < experimentObjectManager.currentLesson.steps.Count
        )
        {
            var currentLessonStep = experimentObjectManager.currentLesson.steps[
                currentLessonStepIndex
            ];
            stepText.text = currentLessonStep.stepDescription;
            Debug.Log(
                $"Displaying Step {currentLessonStepIndex + 1}: {currentLessonStep.stepDescription}"
            );
        }
        else
        {
            Debug.LogError("Invalid step index!");
        }
    }
}
