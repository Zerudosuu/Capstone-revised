using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public TextMeshProUGUI lessonStepText;
    private ExperimentObjectManager experimentObjectManager;
    public List<LessonSteps> MainLessonSteps;
    private int mainCurrentLessonIndex = 0;
    public int currentLessonIndex = 0;
    public List<LessonStepsExample> lessonSteps;
    public GameObject Timer;
    public Animator anim;

    public string requiredAction;
    public static event Action OnStepBroadcasted;
    public static event Action OnStepBroadCastAnotherActionPanel;

    private void Start()
    {
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
        var currentSubstep = currentLesson.GetCurrentSubstep();

        if (currentSubstep != null)
        {
            // Explicitly check for "wait" action and force the correct item name
            if (currentSubstep.requiredAction == "wait" && itemName != "wait")
            {
                Debug.LogWarning("Step requires waiting. Action ignored until timer completes.");
                return; // Prevent early completion
            }

            currentLesson.ValidateAndCompleteSubStep(itemName);
        }

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

    public bool RequiredItemForTheStep(string item)
    {
        var currentLesson = lessonSteps[currentLessonIndex];
        var currentSubstep = currentLesson.GetCurrentSubstep();

        if (currentSubstep != null)
            return currentSubstep.requiredItemName == item;
        else
            return false;
    }

    public bool IsStepForTheDroppedObject()
    {
        var currentLesson = lessonSteps[currentLessonIndex];
        var currentSubstep = currentLesson.GetCurrentSubstep();

        return currentSubstep.theDroppedObject;
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
                requiredAction = currentSubstep.requiredAction;

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

            if (currentSubstep != null && currentSubstep.willNeedAnotherAction)
            {
                OnStepBroadCastAnotherActionPanel?.Invoke();
            }
        }
    }
}

[System.Serializable]
public class LessonStepsExample
{
    public List<Step> substeps;
    private int currentSubstepIndex = 0;

    public void ValidateAndCompleteSubStep(string itemName, string targetObject = null)
    {
        if (currentSubstepIndex >= substeps.Count)
            return;

        Step currentStep = substeps[currentSubstepIndex];
        string requiredAction = currentStep.requiredAction;

        switch (requiredAction)
        {
            case "drop":
                if (currentStep.requiredItemName == itemName)
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }

                break;

            case "drag":
                if (currentStep.requiredItemName == itemName)
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
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
                if (itemName == "wait" && !currentStep.isCompleted)
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }

                break;

            case "shake":
                if (currentStep.requiredItemName == itemName)
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }

                break;
            case "stir":
                if (currentStep.requiredItemName == itemName)
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }

                break;
            default:
                if (currentStep.requiredItemName == itemName)
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
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