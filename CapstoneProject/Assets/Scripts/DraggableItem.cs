using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private GraphicRaycaster raycaster; // Reference to the Canvas's GraphicRaycaster

    [SerializeField] private Canvas canvas;
    public Transform parentAfterDrag;
    public Vector3 originalPosition;
    public Vector2 originalSize; // Store the original size
    private RectTransform rectTransform;
    private bool isDragging = false;
    private GameObject lastOverlappedObject = null;
    public bool placeInSlot = false;
    public string TagName;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (raycaster == null)
        {
            raycaster = GetComponentInParent<GraphicRaycaster>();
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        originalSize = rectTransform.sizeDelta;
        // Store the initial size of the item
    }

    //! DRAG EVENTS
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); // Move item to root canvas while dragging
        transform.SetAsLastSibling();
        canvasGroup.alpha = 0.5f; // Make the item semi-transparent
        canvasGroup.blocksRaycasts = false; // Allow raycasts to pass through
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //
        // GameObject overlappedObject = GetOverlappedGameObject(eventData);
        // if (overlappedObject != null)
        // {
        //     Debug.Log($"Overlapped with: {overlappedObject.name} (has DragableItem script)");
        // }
        // else
        // {
        //     Debug.Log("No valid draggable object detected.");
        // }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore the item to its original parent or slot
        canvasGroup.alpha = 1f; // Restore full opacity
        canvasGroup.blocksRaycasts = true; // Block raycasts again

        if (
            eventData.pointerCurrentRaycast.gameObject != null
            && eventData.pointerCurrentRaycast.gameObject.CompareTag("ScrollViewSlot")
        )
        {
            Transform newSlot = eventData.pointerCurrentRaycast.gameObject.transform;
            transform.SetParent(newSlot);

            print("Dropped in slot" + eventData.pointerCurrentRaycast.gameObject.name);
        }
        else
        {
            // Return to original parent or slot if not dropped into a slot

            transform.SetParent(parentAfterDrag);
            transform.position = originalPosition;
        }
    }

    // private GameObject GetOverlappedGameObject(PointerEventData eventData)
    // {
    //     List<RaycastResult> results = new List<RaycastResult>();
    //     raycaster.Raycast(eventData, results);
    //
    //     foreach (RaycastResult result in results)
    //     {
    //         if (result.gameObject != gameObject) // Exclude self
    //         {
    //             ItemReaction overlappedItem = result.gameObject.GetComponent<ItemReaction>();
    //             if (overlappedItem != null && overlappedItem.item.hasTemperature)
    //             {
    //                 Debug.Log(overlappedItem.gameObject.name);
    //                 // Check if the current object has a MeasureScript
    //                 MeasureScript measureScript = GetComponent<MeasureScript>();
    //                 if (measureScript != null)
    //                 {
    //                     // Display the temperature of the overlapped item
    //                     measureScript.DisplayTemperature(overlappedItem.item.currentTemperature);
    //                     StepManager stepManager = FindObjectOfType<StepManager>();
    //                     if (stepManager != null)
    //                     {
    //                         stepManager.ValidateAndCompleteSubStep(overlappedItem.gameObject.name);
    //                     }
    //                 }
    //
    //                 return result.gameObject; // Return the first valid object with DragableItem
    //             }
    //         }
    //     }
    //
    //     // Clear the temperature display if no valid object is found
    //     MeasureScript currentMeasureScript = GetComponent<MeasureScript>();
    //     if (currentMeasureScript != null)
    //     {
    //         currentMeasureScript.ClearTemperature();
    //     }
    //
    //     return null; // No valid object found
    // }

    private void ResizeToFitContainer(RectTransform container)
    {
        Vector2 containerSize = container.rect.size;
        rectTransform.sizeDelta = containerSize; // Match the size of the slot
        rectTransform.localScale = Vector3.one; // Reset scale to avoid distortions
    }

    private void OnTransformParentChanged()
    {
        print("Parent changed" + gameObject.transform.parent);
    }
}