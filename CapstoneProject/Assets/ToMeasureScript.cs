using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMeasureScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
// }
// using TMPro;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// public class ScrollViewSlot : MonoBehaviour, IDropHandler
// {
//     public TextMeshProUGUI itemQuantitytext;
//
//     public void OnDrop(PointerEventData eventData)
//     {
//         GameObject droppedItem = eventData.pointerDrag;
//         DragableItem draggable = droppedItem.GetComponent<DragableItem>();
//
//         if (draggable == null)
//             return;
//
//         if (transform.childCount > 0)
//         {
//             // Slot already has a child, check if the dropped item name matches the existing item
//             DragableItem existingItem = transform.GetChild(0).GetComponent<DragableItem>();
//
//             if (existingItem != null && existingItem.gameObject.name == draggable.gameObject.name)
//             {
//                 // Allow placing only if it's the same item name
//                 draggable.parentAfterDrag = transform;
//                 droppedItem.transform.SetParent(transform);
//                 droppedItem.transform.localPosition = Vector3.zero;
//                 Debug.Log("Dropped in slot: " + eventData.pointerCurrentRaycast.gameObject.name);
//
//                 UpdateQuantityText();
//             }
//             else
//             {
//                 Debug.Log("Cannot place different item in this slot!");
//                 // Return to original position if it's not the same item
//                 draggable.transform.SetParent(draggable.parentAfterDrag);
//                 draggable.transform.position = draggable.originalPosition;
//             }
//         }
//         else
//         {
//             // If slot is empty, allow placing
//             draggable.parentAfterDrag = transform;
//             droppedItem.transform.SetParent(transform);
//             droppedItem.transform.localPosition = Vector3.zero;
//             Debug.Log("Dropped in empty slot: " + eventData.pointerCurrentRaycast.gameObject.name);
//
//             UpdateQuantityText();
//         }
//     }
//
//     private void UpdateQuantityText()
//     {
//         itemQuantitytext.text = transform.childCount > 0 ? transform.childCount.ToString() : "";
//     }
//
//     public void ResizeChild()
//     {
//         foreach (Transform child in transform)
//         {
//             RectTransform childRect = child.GetComponent<RectTransform>();
//             if (childRect != null)
//             {
//                 Vector2 parentSize = GetComponent<RectTransform>().rect.size;
//                 childRect.sizeDelta = parentSize; // Match the size of the parent
//                 childRect.localScale = Vector3.one; // Reset scale to avoid distortions
//                 childRect.anchoredPosition = Vector2.zero; // Center the child
//             }
//         }
//     }
//
//     private void OnTransformChildrenChanged()
//     {
//         ResizeChild();
//         UpdateQuantityText();
//     }
// }