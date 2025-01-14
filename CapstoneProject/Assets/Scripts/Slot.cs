using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    private GridLayoutGroup gridLayout;
    private Image image;
    public bool FixChildSize = false;
    public List<string> CompatibleNames;

    public string RequireName;
    public string ItemAddedName;

    public bool isOccupied = false;

    private MainHolderManager mainSlotManager;

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        mainSlotManager = FindObjectOfType<MainHolderManager>();
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

            if (draggable != null)
            {
                if (CompatibleNames.Contains(draggable.TagName))
                {
                    draggable.parentAfterDrag = transform;
                    draggable.placeInSlot = true;
                    ItemAddedName = draggable.gameObject.name;
                    isOccupied = true;

                    // Notify StepManager
                    StepManager stepManager = FindObjectOfType<StepManager>();
                    stepManager.ValidateAndCompleteSubStep(ItemAddedName);
                }
                else
                {
                    draggable.transform.position = draggable.originalPosition;
                    Debug.Log("Dropped item is not compatible with this slot.");
                }
            }
            else
            {
                draggable.transform.position = draggable.originalPosition;
            }
        }
    }

    private void OnTransformChildrenChanged()
    {
        ResizeChild();

        if (transform.childCount > 0)
        {
            var item = GetComponentInChildren<ItemReaction>();
            if (item != null)
            {
                mainSlotManager.RegisterSlot(this, item);
            }
        }
        else
        {
            mainSlotManager.UnregisterSlot(this);
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
                childRect.sizeDelta = parentSize;
                childRect.localScale = Vector3.one;
                childRect.anchoredPosition = Vector2.zero;
            }
        }
    }
}