using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // For UI Image, if using UI components

public class ExperimentItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Canvas")]
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [Header("Drag handler")]
    private Vector3 offset;
    private Vector3 originalPosition;
    public string destinationTag = "Table"; // Tag of the drop area
    public float longPressThreshold = 1f; // Time before long press is detected (in seconds)
    public float movementThreshold = 1f; // Minimum movement threshold to reset long press

    private float pressTime = 0f; // Time tracking for long press
    private bool isDragging = false; // To track whether we are currently dragging
    private bool isLongPressing = false; // To track long press
    private Vector3 lastMousePosition; // To track mouse movement

    public Item item; // Reference to the item data

    DropImageSetter dropImageSetter;

    public void Awake()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
            gameObject.AddComponent<CanvasGroup>();

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        dropImageSetter = FindObjectOfType<DropImageSetter>();
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

        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        dropImageSetter.SetImage(item.itemIcon);

        Mask mask = GetComponentInParent<Mask>();
        if (mask != null)
        {
            mask.enabled = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Mask mask = GetComponentInParent<Mask>();
        if (mask != null)
        {
            mask.enabled = true;
            dropImageSetter.SetImage(null);
        }

        // Enable raycasting and reset transparency
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Check if dropped on a valid target with the tag "Table"
        if (
            eventData.pointerCurrentRaycast.gameObject != null
            && (
                eventData.pointerCurrentRaycast.gameObject.CompareTag(destinationTag)
                || eventData.pointerCurrentRaycast.gameObject.CompareTag("Container")
            )
        )
        {
            GameObject target = eventData.pointerCurrentRaycast.gameObject;
            transform.SetParent(target.transform);
            RectTransform draggableRect = GetComponent<RectTransform>();

            // Set anchors to center
            draggableRect.anchorMin = new Vector2(0.5f, 0.5f);
            draggableRect.anchorMax = new Vector2(0.5f, 0.5f);

            // Reset pivot to center if necessary (optional but usually required)
            draggableRect.pivot = new Vector2(0.5f, 0.5f);

            // Set anchored position to zero to center the object
            draggableRect.anchoredPosition = Vector2.zero;
            originalPosition = transform.position;
        }
        else
        {
            // Return to the original position if dropped outside
            gameObject.transform.position = originalPosition;
        }

        // Reset dragging state
        isDragging = false;
    }

    public void SetItem(Item item)
    {
        this.item = item;

        gameObject.GetComponent<Image>().sprite = item.itemIcon; // Assuming you have an Image component in the prefab
    }

    void Update()
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
            }
        }
    }

    public Vector3 GetOriginalPosition()
    {
        // Return the original position of the object
        return this.transform.position;
    }
}
