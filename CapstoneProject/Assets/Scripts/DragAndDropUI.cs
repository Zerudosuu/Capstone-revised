using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    private Vector3 originalPosition; // Store the original position
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public ItemContainerManager itemContainerManager;

    public string destinationTag = "Table"; // Tag of the drop area
    public float longPressThreshold = 1f; // Time before long press is detected (in seconds)
    public float movementThreshold = 1f; // Minimum movement threshold to reset long press

    private float pressTime = 0f; // Time tracking for long press
    private bool isDragging = false; // To track whether we are currently dragging
    private bool isLongPressing = false; // To track long press
    private Vector3 lastMousePosition; // To track mouse movement

    public Item currenItem;

    public TextMeshProUGUI itemName;

    private void Awake()
    {
        // Ensure the CanvasGroup is added
        if (gameObject.GetComponent<CanvasGroup>() == null)
            gameObject.AddComponent<CanvasGroup>();

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        // Find the ItemContainerManager script
    }

    void Start()
    {
        itemContainerManager = GetComponentInParent<ItemContainerManager>();
    }

    public void SetItem(Item newItem)
    {
        currenItem = newItem;

        if (currenItem.itemIcon == null)
        {
            // Show the item name if there's no icon
            itemName.text = currenItem.itemName;
            itemName.gameObject.SetActive(true); // Ensure the name is visible
        }
        else
        {
            // Hide the item name if the icon is present
            itemName.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Store the original position when the drag starts
        originalPosition = transform.position;

        // Calculate the offset between the object's position and the mouse position
        offset = transform.position - Input.mousePosition;

        // Set dragging state
        isDragging = true;

        // Reset the long press tracking
        pressTime = 0f;
        isLongPressing = false;
        lastMousePosition = Input.mousePosition;

        // Make the object semi-transparent and allow it to ignore raycasts
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        itemContainerManager.OnItemClick(currenItem);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / CanvasScalerFactor();

        // Reset long press tracking on significant movement
        if (Vector3.Distance(lastMousePosition, Input.mousePosition) > movementThreshold)
        {
            pressTime = 0f;
            isLongPressing = false;
            lastMousePosition = Input.mousePosition;
        }
    }

    private float CanvasScalerFactor()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas != null ? canvas.scaleFactor : 1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset the raycasting ability and opacity
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Check if the object is dropped on a valid target
        if (
            eventData.pointerCurrentRaycast.gameObject != null
            && eventData.pointerCurrentRaycast.gameObject.CompareTag(destinationTag)
        )
        {
            var dropArea = eventData.pointerCurrentRaycast.gameObject.GetComponent<BagDropArea>();
            if (dropArea != null)
            {
                dropArea.AddedToInventory(currenItem);
            }
            else
            {
                Debug.LogWarning("No BagDropArea found on drop target.");
            }
        }
        else
        {
            transform.position = originalPosition; // Return to original position
        }

        transform.position = originalPosition;
        isDragging = false;
    }


    public Vector3 GetOriginalPosition()
    {
        // Return the original position of the object
        return this.transform.position;
    }
}