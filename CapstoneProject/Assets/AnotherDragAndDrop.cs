using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnotherDragAndDrop : MonoBehaviour
{
    Vector3 offset;
    Collider2D collider2d;
    itemTransform itemTransform;
    Vector3 originalPosition;
    public string destinationTag = "DropArea";

    void Awake()
    {
        originalPosition = transform.position;
        collider2d = GetComponent<Collider2D>();
        itemTransform = GetComponent<itemTransform>();
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    // void OnMouseUp()
    // {
    //     collider2d.enabled = false;
    //     var rayOrigin = Camera.main.transform.position;
    //     var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
    //     RaycastHit2D hitInfo;
    //     if (hitInfo = Physics2D.Raycast(rayOrigin, rayDirection))
    //     {
    //         Item.ItemTag hitTag;

    //         if (
    //             System.Enum.TryParse(hitInfo.transform.tag, out hitTag)
    //                 && itemTransform.item.compatibleTags.Contains(hitTag)
    //             || hitInfo.transform.tag == destinationTag
    //         )
    //         {
    //             transform.position = hitInfo.transform.position + new Vector3(0, 0, -0.01f);
    //             gameObject.transform.SetParent(hitInfo.transform);
    //         }
    //         else
    //         {
    //             Debug.Log("Not a valid drop area" + hitInfo.transform.tag);
    //             transform.position = originalPosition;
    //         }
    //     }

    //     collider2d.enabled = true;
    // }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
