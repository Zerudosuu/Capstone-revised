using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExperiment", menuName = "Experiment System/Experiment Data")]
public class ExperimentFlow : ScriptableObject
{
    public List<ExperimentStep> steps = new List<ExperimentStep>();

    [System.Serializable]
    public class ExperimentStep
    {
        public string description; // Description of the step
        public string requiredTool; // Tool required to complete the step (e.g., thermometer, beaker)
        public string targetObjectTag; // Tag of the target object involved in the step
        public float requiredValue; // Condition (e.g., temperature)
        public StepConditionType conditionType; // Enum to define condition type (temperature check, tool placement, etc.)
        public bool isComplete; // Track if the step is completed
    }

    public enum StepConditionType
    {
        None, // No condition
        MeasureTemperature, // Measure temperature condition
        PlaceObject, // Place object at target location
        Wait, // Wait for a certain time
        CustomCondition // Any custom condition defined by user
        ,
    }
}
