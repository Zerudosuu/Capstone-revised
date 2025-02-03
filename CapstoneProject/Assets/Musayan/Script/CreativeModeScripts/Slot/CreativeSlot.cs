using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreativeSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerClickHandler
{
    private GridLayoutGroup gridLayout;
    private Image image;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent;

    public bool FixChildSize = false;
    public string RequireName;
    public string ItemAddedName;
    public bool isOccupied = false;

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
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
        if (eventData.pointerDrag == null)
        {
            Debug.LogWarning("Dropped item is null.");
            return;
        }

        if (transform.childCount == 0)
        {
            GameObject dropItem = eventData.pointerDrag;
            DragableItem draggable = dropItem.GetComponent<DragableItem>();

            draggable.parentAfterDrag = transform;
            draggable.placeInSlot = true;
            ItemAddedName = draggable.gameObject.name;
            isOccupied = true;
        }
    }

    private void OnTransformChildrenChanged()
    {
        ResizeChild();
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

    //! DRAG EVENTS FOR MOVING THE SLOT ITSELF
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;

        // Disable layout group updates to prevent reordering
        if (originalParent.TryGetComponent(out LayoutGroup layoutGroup))
        {
            layoutGroup.enabled = false;
        }

        transform.SetAsLastSibling(); // Bring to front
        canvasGroup.blocksRaycasts = false; // Allow raycasts to pass through while dragging
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out var globalMousePos
        );
        rectTransform.position = globalMousePos; // Update slot position to follow mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // Restore raycasts when dropped

        // Restore layout group updates
        if (originalParent.TryGetComponent(out LayoutGroup layoutGroup))
        {
            layoutGroup.enabled = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Add any behavior when clicking a slot (if needed)
    }
}