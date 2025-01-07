using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public StepManager stepManager;

    public void HandleAction(Item item, string actionType)
    {
        if (stepManager == null)
        {
            Debug.LogError("StepManager is not assigned to ItemInteraction.");
            return;
        }

        // bool success = stepManager.ValidateAction(item, actionType);
        // if (success)
        // {
        //     Debug.Log($"Action '{actionType}' with item '{item.itemName}' completed the step.");
        // }
        // else
        // {
        //     Debug.LogWarning(
        //         $"Action '{actionType}' with item '{item.itemName}' did not meet the current step requirements."
        //     );
        // }
    }
}
