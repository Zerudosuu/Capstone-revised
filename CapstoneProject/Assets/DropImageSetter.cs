using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropImageSetter : MonoBehaviour
{
    public void SetImage(Sprite sprite)
    {
        Image image = GetComponent<Image>();

        image.sprite = sprite;
    }
}
