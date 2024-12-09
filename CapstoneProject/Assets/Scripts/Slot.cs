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

    public string RequireName;
    public string ItemAddedName;

    public bool isOccupied = false;

    public Item parentItem; // The item in this slot (e.g., Beaker)

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

            if (compatibleTagsForSlot.Contains(draggable.item.tagName))
            {
                draggable.parentAfterDrag = transform;
                draggable.placeInSlot = true;

                isOccupied = true;

                // Update the slot item
                parentItem = draggable.item;

                Debug.Log($"Item {parentItem.itemName} placed in the slot.");

                // Check for heating recursively
                // ApplyHeatIfPresent();
            }
            else
            {
                draggable.transform.position = draggable.originalPosition;
                Debug.LogWarning("Dropped item is not compatible with this slot.");
            }
        }
    }

    private void OnTransformChildrenChanged()
    {
        ResizeChild();

        if (transform.childCount > 0)
        {
            isOccupied = true;

            Color currentColor = image.color;
            currentColor.a = 0;
            image.color = currentColor; // Apply the change

            // Check heat when child changes
        }
        else
        {
            isOccupied = false;

            Color currentColor = image.color;
            currentColor.a = 1; // Full opacity
            image.color = currentColor; // Apply the change
        }

        // ApplyHeatIfPresent();
    }

    public void ResizeChild()
    {
        foreach (Transform child in transform)
        {
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null)
            {
                Vector2 parentSize = GetComponent<RectTransform>().rect.size;
                childRect.sizeDelta = parentSize;
                childRect.localScale = Vector3.one;
                childRect.anchoredPosition = Vector2.zero;
            }
        }
    }

    // private void ApplyHeatIfPresent()
    // {
    //     if (parentItem == null)
    //         return;

    //     // Check recursively for heat in children
    //     float cumulativeHeat = CalculateHeatFromDescendants(transform);

    //     if (cumulativeHeat > 0)
    //     {
    //         Debug.Log($"Heating {parentItem.itemName} with cumulative heat: {cumulativeHeat}°C.");
    //         parentItem.HeatItem(cumulativeHeat * Time.deltaTime); // Apply heat
    //     }
    // }

    // private float CalculateHeatFromDescendants(Transform parent)
    // {
    //     float totalHeat = 0f;

    //     foreach (Transform child in parent)
    //     {
    //         DragableItem childDragable = child.GetComponent<DragableItem>();
    //         if (childDragable != null && childDragable.item.hasTemperature)
    //         {
    //             if (childDragable.item.CurrentState.stateName == "Heating")
    //             {
    //                 Debug.Log(
    //                     $"Found heating item: {childDragable.item.itemName} with heat rate {childDragable.item.heatingRate}°C."
    //                 );
    //                 totalHeat += childDragable.item.heatingRate;
    //             }
    //         }

    //         // Recursive call for further descendants
    //         totalHeat += CalculateHeatFromDescendants(child);
    //     }

    //     return totalHeat;
    // }
}
