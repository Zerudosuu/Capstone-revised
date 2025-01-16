using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name + "Triggered");
    }
}