using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private GraphicRaycaster raycaster; // Reference to the Canvas's GraphicRaycaster

    [SerializeField] public Canvas canvas;
    public Transform parentAfterDrag;
    public Vector3 originalPosition;
    public Vector2 originalSize; // Store the original size
    private RectTransform rectTransform;
    private bool isDragging = false;
    private GameObject lastOverlappedObject = null;
    public bool placeInSlot = false;
    public string TagName;
    public int itemVariantID;
    public Animator anim;
    public GameObject Popup;

    public static event Action OnItemDragged;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        if (Popup != null)
        {
            Popup.SetActive(false);
        }
    }

    public void PopUpOnItemValid()
    {
        if (anim != null)
        {
            anim.Play("Sucess"); // Ensure the animation name is correct
        }
        else
        {
            Debug.LogWarning("Animator component is not assigned.");
        }
    }

    public void PopUpOnItemInvalid()
    {
        if (Popup != null)
        {
            Popup.SetActive(true);
            Animator popupAnimator = Popup.GetComponent<Animator>();
            if (popupAnimator != null)
            {
                anim.Play("Shake");
                popupAnimator.Play("Pop");
            }
            else
            {
                Debug.LogWarning("Animator component not found on Popup.");
            }
        }
        else
        {
            Debug.LogWarning("Popup GameObject is not assigned.");
        }
    }

    //! DRAG EVENTS
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnItemDragged != null)
            OnItemDragged.Invoke();

        originalPosition = transform.position;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); // Move item to root canvas while dragging
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false; // Allow raycasts to pass through
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out var globalMousePos
        );
        transform.position = globalMousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore the item to its original parent or slot

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (HasAnimationTrigger(anim, "Shake"))
        {
            anim.Play("Shake");

            StepManager stepManager = FindObjectOfType<StepManager>();
            if (stepManager != null)
            {
                stepManager.ValidateAndCompleteSubStep(gameObject.name);
            }
        }
    }


    // Helper method to check if the trigger exists in the Animator
    private bool HasAnimationTrigger(Animator animator, string triggerName)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
                {
                    return true;
                }
            }
        }

        return false;
    }
}