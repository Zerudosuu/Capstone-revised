using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BottomSheetFunction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")] [SerializeField]
    private RectTransform btmSheet;

    [Header("Buttons")] [SerializeField] private Button btnSheetBtn;
    [SerializeField] private Button submitBtn;

    private float initialY; // Bottom edge of the screen
    private float middleY; // Middle position
    public float fullscreenY = 0f; // Fully visible position
    private Vector2 dragStartPosition;
    private Vector2 sheetStartPosition;
    private ButtomSheetHolder bottomSheet;

    private void Start()
    {
        // Calculate dynamic positions
        float screenHeight = Screen.height;
        float sheetHeight = btmSheet.rect.height;

        initialY = -sheetHeight + 130f; // Off-screen position
        middleY = -sheetHeight / 2; // Halfway visible (adjust as needed)

        // Set the initial position of the bottom sheet
        btmSheet.anchoredPosition = new Vector2(0, initialY);

        // Attach button listeners
        btnSheetBtn.onClick.AddListener(ToggleVisibility);
        submitBtn.onClick.AddListener(OnSubmit);

        bottomSheet = FindObjectOfType<ButtomSheetHolder>();
    }

    private void ToggleVisibility()
    {
        if (Mathf.Approximately(btmSheet.anchoredPosition.y, fullscreenY))
        {
            MoveToPosition(initialY);
        }
        else if (Mathf.Approximately(btmSheet.anchoredPosition.y, middleY))
        {
            MoveToPosition(initialY);
        }
        else
        {
            MoveToPosition(middleY);
        }
    }

    public void MoveToPosition(float targetY)
    {
        btmSheet.DOAnchorPosY(targetY, 0.3f);
    }

    private void OnSubmit()
    {
        if (bottomSheet.canSubmit())
        {
            bottomSheet.GetAnswer();
        }
        else
        {
            print("not yet");
        }

        Debug.Log("Submit Answer");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
        sheetStartPosition = btmSheet.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Calculate the new Y position during drag
        float dragDeltaY = eventData.position.y - dragStartPosition.y;
        float newY = Mathf.Clamp(sheetStartPosition.y + dragDeltaY, initialY, fullscreenY);

        // Update the bottom sheet's position
        btmSheet.anchoredPosition = new Vector2(0, newY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float dragEndY = btmSheet.anchoredPosition.y;

        // Snap to the nearest position
        if (dragEndY > middleY + (fullscreenY - middleY) / 2)
        {
            // Snap to fullscreen
            MoveToPosition(fullscreenY);
        }
        else if (dragEndY > initialY + (middleY - initialY) / 2)
        {
            // Snap to middle
            MoveToPosition(middleY);
        }
        else
        {
            // Snap to initial
            MoveToPosition(initialY);
        }
    }
}