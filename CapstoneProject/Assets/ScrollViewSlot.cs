using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        DragableItem draggable = droppedItem.GetComponent<DragableItem>();

        draggable.parentAfterDrag = transform;
    }

    public void checkSlot()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("Slot is empty.");
            GetComponent<Image>().color = Color.green; // Indicates an empty slot
        }
        else
        {
            GetComponent<Image>().color = Color.red; // Indicates an occupied slot
            Debug.Log("Slot is not empty.");
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
        ResizeChild();
    }
}
