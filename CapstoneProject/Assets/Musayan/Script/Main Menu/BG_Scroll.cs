using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class BG_Scroll : MonoBehaviour
{
    [SerializeField] private RawImage bgImage;
    [SerializeField] private float imgX, imgY;

    private void Update()
    {
        bgImage.uvRect = new Rect(bgImage.uvRect.position + new Vector2(imgX,imgY) * Time.deltaTime, bgImage.uvRect.size);
    }
}
