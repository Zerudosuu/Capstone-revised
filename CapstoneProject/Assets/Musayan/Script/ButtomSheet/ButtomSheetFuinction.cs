using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtomSheetFuinction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RectTransform btmSheet;
    [SerializeField] private float slideDuration = 0.5f;

    [Header("Positions")]
    [SerializeField] private Vector2 hiddenPosition;
    [SerializeField] private Vector2 visiblePosition;

    [Header("Buton")]
    [SerializeField] private Button btnSheetBtn;

    private bool isVisible;

    private void Awake()
    {
        btnSheetBtn.onClick.AddListener(OnPress);
    }
    private void Start()
    {
        HideSheet();
    }


    private void HideSheet()
    {
        isVisible = false;
        btmSheet.anchoredPosition = hiddenPosition;
    }

    private void OnPress()
    {
        if (isVisible)
        {
            btmSheet.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.InOutSine);
            isVisible = false;
        }
        else if (!isVisible)
        {
            btmSheet.DOAnchorPos(visiblePosition, slideDuration).SetEase(Ease.InOutSine);
            isVisible = true;
        }
    }

    
}
