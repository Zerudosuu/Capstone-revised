using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewSlot : MonoBehaviour, IDropHandler
{
    public TextMeshProUGUI itemQuantitytext;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        DragableItem draggable = droppedItem.GetComponent<DragableItem>();

        if (draggable == null)
            return;

        if (transform.childCount > 0)
        {
            //Check the first child of the slot if the same as the dropped item
            DragableItem existingItem = transform.GetChild(0).GetComponent<DragableItem>();

            if (existingItem != null && existingItem.gameObject.name == draggable.gameObject.name)
            {
                // Allow placing only if it's the same item name
                draggable.parentAfterDrag = transform;
                droppedItem.transform.SetParent(transform);
                droppedItem.transform.localPosition = Vector3.zero;
                Debug.Log("Dropped in slot: " + eventData.pointerCurrentRaycast.gameObject.name);
            }
            else
            {
                Debug.Log("Cannot place different item in this slot!");
                // Return to original position if it's not the same item
                draggable.transform.SetParent(draggable.parentAfterDrag);
                draggable.transform.position = draggable.originalPosition;
            }
        }
        else
        {
            draggable.parentAfterDrag = transform;
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector3.zero;
            Debug.Log("Dropped in empty slot: " + eventData.pointerCurrentRaycast.gameObject.name);
        }
    }

    public void ResizeChild()
    {
        foreach (Transform child in transform)
        {
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null)
            {
                Vector2 parentSize = GetComponent<RectTransform>().rect.size;
                childRect.sizeDelta = parentSize; // Match the size of the parent
                childRect.localScale = Vector3.one; // Reset scale to avoid distortions
                childRect.anchoredPosition = Vector2.zero; // Center the child
            }
        }
    }

    private void OnTransformChildrenChanged()
    {
        // Resize all child elements to fit the slot
        ResizeChild();

        // Check if the slot has any children
        if (transform.childCount > 0)
        {
            // Update the quantity text to reflect the number of children
            itemQuantitytext.text = transform.childCount.ToString();

            // Iterate through all children
            for (int i = 0; i < transform.childCount; i++)
            {
                // Set the first child as active and the rest as inactive
                transform.GetChild(i).gameObject.SetActive(i == 0);
            }
        }
        else
        {
            // Clear the quantity text if no children are present
            itemQuantitytext.text = "";
        }
    }
}