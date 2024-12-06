using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    private GridLayoutGroup gridLayout;
    private Image image;
    public bool FixChildSize = false;
    public List<string> compatibleTagsForSlot;
    public bool forItemSlot = false;

    private List<GameObject> overlappedObjects = new List<GameObject>();

    public string requiredTag;
    public string itemTagAdded;

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (FixChildSize)
        {
            float width = image.rectTransform.rect.width;
            float height = image.rectTransform.rect.height;
            gridLayout.cellSize = new Vector2(width, height);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropItem = eventData.pointerDrag;
            DragableItem draggable = dropItem.GetComponent<DragableItem>();

            //! SLOT ONLY WORKS WITH THE COMPATIBLE TAGS
            if (compatibleTagsForSlot.Contains(draggable.item.tagName) && !forItemSlot)
            {
                draggable.parentAfterDrag = transform;
                Debug.Log("Item placed successfully.");

                itemTagAdded = draggable.item.itemName;
            }
            else if (forItemSlot)
            {
                draggable.parentAfterDrag = transform;
                Debug.Log("Item placed successfully.");
            }
            else
            {
                draggable.transform.position = draggable.originalPosition;
                return;
            }

            // // Update the list of overlapping objects after placing the item
            // UpdateOverlappingObjects();

            // // Log all overlapping objects
            // if (overlappedObjects.Count > 0)
            // {
            //     Debug.Log($"Overlapping with {overlappedObjects.Count} objects:");
            //     foreach (GameObject obj in overlappedObjects)
            //     {
            //         Debug.Log($"- {obj.name}");
            //     }
            // }
            // else
            // {
            //     Debug.Log("No overlapping objects detected.");
            // }
        }
    }

    /// <summary>
    /// Updates the list of overlapping UI elements.
    /// </summary>
    private void UpdateOverlappingObjects()
    {
        overlappedObjects.Clear(); // Clear the previous list
        RectTransform slotRect = GetComponent<RectTransform>();

        // Iterate over all RectTransforms to find overlaps
        foreach (RectTransform uiElement in FindObjectsOfType<RectTransform>())
        {
            if (uiElement != slotRect && uiElement.parent != transform) // Exclude self and its own children
            {
                if (IsRectTransformOverlapping(slotRect, uiElement))
                {
                    overlappedObjects.Add(uiElement.gameObject); // Add overlapping object to the list
                }
            }
        }
    }

    /// <summary>
    /// Checks if two RectTransforms overlap.
    /// </summary>
    private bool IsRectTransformOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

        return rect1World.Overlaps(rect2World);
    }

    /// <summary>
    /// Converts a RectTransform's local rectangle to a world rectangle.
    /// </summary>
    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector3 position = corners[0]; // Bottom-left corner
        Vector2 size = new Vector2(
            corners[2].x - corners[0].x, // Width
            corners[2].y - corners[0].y // Height
        );

        return new Rect(position, size);
    }
}
