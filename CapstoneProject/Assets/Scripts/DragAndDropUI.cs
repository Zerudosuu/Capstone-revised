using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    private Vector3 originalPosition; // Store the original position
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public string destinationTag = "Table"; // Tag of the drop area
    public float longPressThreshold = 1f; // Time before long press is detected (in seconds)
    public float movementThreshold = 1f; // Minimum movement threshold to reset long press

    private float pressTime = 0f; // Time tracking for long press
    private bool isDragging = false; // To track whether we are currently dragging
    private bool isLongPressing = false; // To track long press
    private Vector3 lastMousePosition; // To track mouse movement

    public ItemContainerManager itemContainerManager;

    public Item item;

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
        item = newItem;

        if (itemName != null)
            itemName.text = item.itemName;
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

        itemContainerManager.OnItemClick(item);
        itemContainerManager.PopUpButtons.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        // Update the object's position to follow the mouse, considering the offset
        transform.position = Input.mousePosition + offset;

        // If the mouse moved beyond the threshold, reset long press time
        if (Vector3.Distance(lastMousePosition, Input.mousePosition) > movementThreshold)
        {
            pressTime = 0f; // Reset the long press timer if there's significant movement
            isLongPressing = false;
            lastMousePosition = Input.mousePosition; //
            itemContainerManager.PopUpButtons.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset the raycasting ability and opacity
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Check if the object is dropped on a valid target
        RaycastResult raycastResult = eventData.pointerCurrentRaycast;

        // If the object was dropped on a valid destination
        if (raycastResult.gameObject != null && raycastResult.gameObject.CompareTag(destinationTag))
        {
            // Snap the object to the destination
            transform.position = raycastResult.gameObject.transform.position;
            BagDropArea dropArea = raycastResult.gameObject.GetComponent<BagDropArea>();

            dropArea.AddedToInventory(item);
            transform.position = originalPosition;
        }
        else
        {
            // If not dropped on the correct area, return to the original position
            transform.position = originalPosition;
        }

        // Reset dragging state
        isDragging = false;
    }

    private void Update()
    {
        // If dragging, increase the pressTime to track how long we've been in the same spot
        if (isDragging)
        {
            pressTime += Time.deltaTime;

            // If the press time exceeds the long press threshold, trigger the long press action
            if (pressTime >= longPressThreshold && !isLongPressing)
            {
                // Long press detected
                isLongPressing = true;
                Debug.Log("Long press detected on: " + item.itemName);

                itemContainerManager.PopUP(transform.position);
            }
        }
    }

    public Vector3 GetOriginalPosition()
    {
        // Return the original position of the object
        return this.transform.position;
    }
}
