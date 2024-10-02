using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() { }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        var rayHit = Physics2D.GetRayIntersection(
            _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue())
        );
        if (!rayHit.collider)
            return;

        Debug.Log("Clicked on: " + rayHit.collider.gameObject.name);
    }
}
