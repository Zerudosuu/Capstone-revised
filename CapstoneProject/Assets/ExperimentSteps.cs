using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ExperimentStep
{
    public string stepDescription; // Description of the step
    public List<RequiredSteps> requiredSteps = new List<RequiredSteps>(); // List of required sub-steps
    public bool isCompleted = false; // Track completion status

    public void CompleteStep()
    {
        if (AreAllRequiredStepsCompleted())
        {
            isCompleted = true;
            Debug.Log($"Step completed: {stepDescription}");
        }
        else
        {
            Debug.LogWarning("Cannot complete the step. Not all sub-steps are completed.");
        }
    }

    public bool AreAllRequiredStepsCompleted()
    {
        return requiredSteps.All(step => step.isCompleted);
    }

    public void ValidateSubStep(string subStepName)
    {
        foreach (var requiredStep in requiredSteps)
        {
            requiredStep.MarkSubStepCompleted(subStepName);
        }
    }
}

[System.Serializable]
public class RequiredSteps
{
    public List<string> StepNames = new List<string>(); // Sub-step names
    public bool isCompleted = false; // Track if all required sub-steps are completed

    // Method to validate if a specific sub-step is completed
    public void MarkSubStepCompleted(string subStepName)
    {
        if (StepNames.Contains(subStepName))
        {
            StepNames.Remove(subStepName); // Remove from the list when completed
            Debug.Log($"Sub-step completed: {subStepName}");

            // Check if all sub-steps are now completed
            if (StepNames.Count == 0)
            {
                isCompleted = true;
                Debug.Log("All sub-steps are completed!");
            }
        }
        else
        {
            Debug.LogWarning($"Sub-step '{subStepName}' is not part of the required steps!");
        }
    }
}
