using UnityEngine;

public class MainHolderManager : MonoBehaviour
{
    public void ValidatePlacement(string itemName)
    {
        StepManager stepManager = FindObjectOfType<StepManager>();

        if (stepManager == null)
        {
            Debug.LogError("StepManager is not found in the scene.");
            return;
        }

        // var currentStep = stepManager.steps[stepManager.currentStepIndex];
        // if (currentStep.requiredObjects.Contains(itemName))
        // {
        //     stepManager.CompleteCurrentStep();
        // }
        // else
        // {
        //     Debug.LogWarning($"Item '{itemName}' is not valid for this step.");
        // }
    }
}
