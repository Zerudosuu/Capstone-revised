using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreativeSlot : MonoBehaviour, IDropHandler
{
    private GridLayoutGroup gridLayout;
    private Image image;
    public bool FixChildSize = false;
    public string RequireName;
    public string ItemAddedName;
    public bool isOccupied = false;


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
}