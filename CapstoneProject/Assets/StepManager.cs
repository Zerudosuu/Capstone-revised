using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    private AudioManager _audioManage;


    [SerializeField] [NotNull] private ParticleSystem confett1;
    [SerializeField] [NotNull] private ParticleSystem confett2;

    [SerializeField] BottomSheetFunction bottomSheetFunction;
    public static event Action OnStepBroadcasted;
    public static event Action OnStepBroadCastAnotherActionPanel;
    public static event Action OnLessonsComplete;

    private void Start()
    {
        _audioManage = FindObjectOfType<AudioManager>(true);
        experimentObjectManager = FindObjectOfType<ExperimentObjectManager>(true);
        MainLessonSteps = experimentObjectManager.currentLesson.steps;
        Timer.SetActive(false);
        DisplayCurrentStep();
    }

    public void ValidateAndCompleteSubStep(string itemName, string actionType = "drop")
    {
        if (currentLessonIndex >= lessonSteps.Count)
            return;

        var currentLesson = lessonSteps[currentLessonIndex];
        var currentSubstep = currentLesson.GetCurrentSubstep();

        if (currentSubstep != null)
        {
            // Check if the action type matches and if the correct item is being used
            if (currentSubstep.requiredAction == actionType && currentSubstep.requiredItemName == itemName)
            {
                currentLesson.ValidateAndCompleteSubStep(itemName);
            }
            else if (currentSubstep.requiredAction == "assemble")
            {
                currentLesson.ValidateAndCompleteSubStep(itemName);
            }
            else
            {
                Debug.LogWarning($"Incorrect item '{itemName}' for action '{actionType}'. Step NOT validated.");
            }
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
                lessonStepText.text =
                    "All tasks completed! Congratulations! Please proceed to the questions below.";

                _audioManage.PlaySFX("Experiment Success");
                confett1.Play();
                confett2.Play();
                bottomSheetFunction.MoveToPosition(bottomSheetFunction.middleY);
                OnLessonsComplete?.Invoke();
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

    public float GetTargetTemperature()
    {
        var currentLesson = lessonSteps[currentLessonIndex];
        var currentSubstep = currentLesson.GetCurrentSubstep();

        if (currentSubstep != null)
            return currentSubstep.targetTemperature;
        else
            return 0;
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
                    countdown.SetTime(currentSubstep.waitTime, currentSubstep.requiredItemName, requiredAction,
                        currentSubstep.targetTemperature);
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
                if (currentStep.requiredItemName == itemName && !currentStep.isCompleted)
                {
                    currentStep.isCompleted = true;
                    currentSubstepIndex++;
                }

                break;

            case "shake":
                if (currentStep.requiredItemName == itemName && !currentStep.isCompleted)
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