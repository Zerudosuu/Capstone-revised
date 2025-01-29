using System.Collections.Generic;

[System.Serializable]
public class Step
{
    public string stepName;
    public string requiredItemName; // For drag-and-drop
    public string requiredAction; // For other actions (e.g., "click", "stir")
    public bool isCompleted;
    public bool theDroppedObject;
    public bool WillBroadCastChange;
    public bool willNeedAnotherAction;

    // Substeps for assemble actions
    public List<SubStep> substeps;

    public float waitTime;
}

[System.Serializable]
public class SubStep
{
    public string targetObject; // Object to interact with
    public string action; // Action to perform (e.g., "PlaceOn", "AttachTo")
    public bool isCompleted;
}