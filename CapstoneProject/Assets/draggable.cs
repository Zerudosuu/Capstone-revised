using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("Dragging");
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //  Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Debug.Log("Dropped on: " + eventData.pointerCurrentRaycast.gameObject.name);

            // This is where you would call the InteractWith method on the object you dropped the draggable on
            gameObject
                .GetComponent<FireComponent>()?
                .InteractWith(eventData.pointerCurrentRaycast.gameObject);
        }
        else
        {
            Debug.Log("Dropped on nothing");
        }
    }
}
