using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sampleDrag : MonoBehaviour
{
    private Vector3 offset; // Offset between the mouse position and object position
    private Camera mainCamera; // Reference to the main camera
    private Collider2D objectCollider; // Collider of this object

    private void Start()
    {
        // Cache the main camera and this object's collider reference
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider2D>();

        if (objectCollider == null)
        {
            Debug.LogError("Collider2D is required for this script to work.");
        }
    }

    private void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the object's position
        Vector3 mousePosition = GetMouseWorldPosition();
        offset = transform.position - mousePosition;
    }

    private void OnMouseDrag()
    {
        // Update the object's position as the mouse moves
        Vector3 mousePosition = GetMouseWorldPosition();
        transform.position = mousePosition + offset;
    }

    private void OnMouseUp()
    {
        // On mouse release, check for overlapping objects
        CheckInteractions();
    }

    private void CheckInteractions()
    {
        // Use OverlapPoint to detect objects at the dragged object's position
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject != gameObject) // Exclude itself
            {
                Debug.Log($"Interacted with: {collider.gameObject.name}");

                // Perform specific logic based on the interacted object
                InteractWithObject(collider.gameObject);
            }
        }
    }

    private void InteractWithObject(GameObject otherObject)
    {
        // Example: React to a specific tag or component
        if (otherObject.CompareTag("Target"))
        {
            Debug.Log($"Interacted with a Target object: {otherObject.name}");
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Convert the mouse position from screen space to world space
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z =
            Mathf.Abs(mainCamera.transform.position.z - transform.position.z); // Set the correct z-depth
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}