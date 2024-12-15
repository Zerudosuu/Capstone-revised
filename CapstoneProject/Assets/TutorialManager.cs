using System.Collections.Generic;
using DG.Tweening; // Add DOTween namespace
using UnityEngine;
using Yarn.Unity;

public class TutorialManager : MonoBehaviour, IData
{
    [Header("Tutorial Manager")]
    [SerializeField]
    private GameObject highlightObject; // Highlight object

    [SerializeField]
    private GameObject dialogueBoxPosition; // Dialogue box object

    public GameObject HighLightParent;

    public List<HighlightAndDialoguePosition> highLightPositions =
        new List<HighlightAndDialoguePosition>();
    private int currentIndex = 0;

    private DialogueRunner dialogueRunner; // Reference to Dialogue Runner

    public bool isTutorialComplete = false; // Flag to check if tutorial is complete

    // DOTween settings
    [Header("Tween Settings")]
    public float transitionDuration = 0.5f; // Duration of the transition

    void Start()
    {
        highlightObject.SetActive(false); // Hide the highlight initially

        // Find and reference the Dialogue Runner
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (dialogueRunner != null)
        {
            // Register "set_highlight" command manually
            dialogueRunner.AddCommandHandler<string>("set_highlight", SetHighlight);
            dialogueRunner.AddCommandHandler("hide_highlight", HideHighlight);
        }
        else
        {
            Debug.LogError("Dialogue Runner not found in the scene.");
        }

        HighlightAndDialoguePosition initialPosition = highLightPositions[currentIndex];
        highlightObject.transform.localPosition = initialPosition.highlightPosition;
        highlightObject.transform.localScale = initialPosition.highlightScale;
        dialogueBoxPosition.transform.localPosition = initialPosition.dialoguePosition;
    }

    // Command to move the highlight (smooth transition)
    public void SetHighlight(string direction)
    {
        if (direction == "next")
        {
            currentIndex++;
        }
        else if (direction == "back")
        {
            currentIndex--;
        }

        // Clamp index within bounds
        currentIndex = Mathf.Clamp(currentIndex, 0, highLightPositions.Count - 1);

        HighlightAndDialoguePosition positionData = highLightPositions[currentIndex];

        // Smoothly move and scale the highlight using DOTween
        highlightObject.SetActive(true);

        highlightObject
            .transform.DOLocalMove(positionData.highlightPosition, transitionDuration)
            .SetEase(Ease.OutQuad);

        highlightObject
            .transform.DOScale(positionData.highlightScale, transitionDuration)
            .SetEase(Ease.OutQuad);

        // Smoothly move the dialogue box position
        dialogueBoxPosition
            .transform.DOLocalMove(positionData.dialoguePosition, transitionDuration)
            .SetEase(Ease.OutQuad);
    }

    public void HideHighlight()
    {
        highlightObject
            .transform.DOScale(Vector3.zero, transitionDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => highlightObject.SetActive(false));
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.isTutorialCompleted)
        {
            isTutorialComplete = gameData.isTutorialCompleted;
            gameObject.SetActive(false);
            dialogueBoxPosition.SetActive(false);
        }
    }

    public void SavedData(GameData gameData)
    {
        gameData.isTutorialCompleted = isTutorialComplete;
    }
}

[System.Serializable]
public class HighlightAndDialoguePosition
{
    public Vector3 highlightPosition;

    public Vector3 dialoguePosition;
    public Vector3 highlightScale;

    [Header("Extra Highlight Position")]
    public GameObject AnotherHighlightObject;

    public Vector3 extraHighlightPosition;
    public Vector3 extraHighlightScale;
}
