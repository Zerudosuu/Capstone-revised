using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public TextMeshProUGUI lessonStepText; // UI to display current lesson and step
    private ExperimentObjectManager experimentObjectManager;
    public List<LessonSteps> MainLessonSteps;
    private int mainCurrentLessonIndex = 0;
    public int currentLessonIndex = 0;
    public List<LessonStepsExample> lessonSteps;
    public GameObject Timer;
    public Animator anim;


    public string requiredAction;
    public static event Action OnStepBroadcasted; // Event for step broadcast

    private void Start()
    {
        // Get the experiment object manager and populate lesson steps
        experimentObjectManager = FindObjectOfType<ExperimentObjectManager>(true);
        MainLessonSteps = experimentObjectManager.currentLesson.steps;
        Timer.SetActive(false);
        DisplayCurrentStep();
    }

    public void ValidateAndCompleteSubStep(string itemName)
    {
        if (currentLessonIndex >= lessonSteps.Count)
            return;

        var currentLesson = lessonSteps[currentLessonIndex];

        // Validate and complete the substep
        currentLesson.ValidateAndCompleteSubStep(itemName);

        // Check if the current lesson is completed
        if (currentLesson.IsTaskCompleted())
        {
            Debug.Log($"Lesson {currentLessonIndex + 1} completed!");
            currentLessonIndex++;
            anim.Play("DoneStep");

            if (currentLessonIndex < lessonSteps.Count)
            {
                DisplayCurrentStep();
            }
            else
            {
                Debug.Log("All lessons completed!");
                lessonStepText.text = "All tasks completed!";
                experimentObjectManager.currentLesson.isCompleted = true;
            }
        }
        else
        {
            DisplayCurrentStep();
        }
    }

    private void DisplayCurrentStep()
    {
        anim.Play("SpawnSteps");
        if (currentLessonIndex < lessonSteps.Count)
        {
            var currentLesson = lessonSteps[currentLessonIndex];
            var currentSubstep = currentLesson.GetCurrentSubstep();


            if (currentSubstep != null)
            {
                requiredAction = currentSubstep.requiredAction; // Get the required action of the current substep
                if (requiredAction == "wait")
                {
                    Timer.SetActive(true);
                    ExperimentCountDown countdown = Timer.GetComponent<ExperimentCountDown>();
                    countdown.SetTime(currentSubstep.waitTime);
                }
                else
                {
                    Timer.SetActive(false);
                }

                lessonStepText.text = MainLessonSteps[currentLessonIndex].stepDescription;
            }

            if (currentSubstep != null && currentSubstep.WillBroadCastChange)
            {
                OnStepBroadcasted?.Invoke();
                print("Broadcasted");
            }
        }
    }
}

[System.Serializable]
public class LessonStepsExample
{
    public List<Step> substeps; // Steps for the task
    private int currentSubstepIndex = 0;

    public void ValidateAndCompleteSubStep(string itemName, string targetObject = null)
    {
        if (currentSubstepIndex >= substeps.Count)
            return;

        Step currentStep = substeps[currentSubstepIndex];
        string requiredAction = currentStep.requiredAction; // Get the required action for this step

        switch (requiredAction)
        {
            case "drop":
                if (currentStep.requiredItemName == itemName)
                {
                    Debug.Log($"Step {currentStep.stepName} completed with drag-and-drop!");
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }
                else
                {
                    Debug.LogWarning("Incorrect item for drag-and-drop.");
                }

                break;

            case "drag":
                if (currentStep.requiredItemName == itemName)
                {
                    Debug.Log($"Step {currentStep.stepName} completed with drag!");
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }
                else
                {
                    Debug.LogWarning("Incorrect item for drag.");
                }

                break;

            case "assemble":
                foreach (var substep in currentStep.substeps)
                {
                    if (!substep.isCompleted && substep.targetObject == itemName)
                    {
                        substep.isCompleted = true;
                        Debug.Log($"Substep completed: {itemName} on {targetObject}");

                        if (currentStep.substeps.TrueForAll(s => s.isCompleted))
                        {
                            Debug.Log($"Step {currentStep.stepName} fully assembled!");
                            currentStep.isCompleted = true;
                            currentSubstepIndex++;
                        }

                        return;
                    }
                }

                Debug.LogWarning("Incorrect assembly action or target object.");
                break;

            case "wait":
                Debug.Log($"Step {currentStep.stepName} is waiting... Before: {currentStep.isCompleted}");

                if (!currentStep.isCompleted) // Prevent duplicate completion
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                    Debug.Log($"Step {currentStep.stepName} marked complete. New index: {currentSubstepIndex}");
                }
                else
                {
                    Debug.LogWarning("Step was already completed.");
                }

                break;
            default:
                if (currentStep.requiredItemName == itemName)
                {
                    Debug.Log(
                        $"Step {currentStep.stepName} completed with action {requiredAction}!"
                    );
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }
                else
                {
                    Debug.LogWarning("Incorrect item or action.");
                }

                break;
        }
    }

    public bool IsTaskCompleted()
    {
        return currentSubstepIndex >= substeps.Count;
    }

    public Step GetCurrentSubstep()
    {
        if (currentSubstepIndex < substeps.Count)
        {
            return substeps[currentSubstepIndex];
        }

        return null;
    }
}