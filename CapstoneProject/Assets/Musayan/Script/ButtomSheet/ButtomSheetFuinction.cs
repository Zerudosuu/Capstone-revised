using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtomSheetFuinction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RectTransform btmSheet;

    [Header("Button")]
    [SerializeField] private Button btnSheetBtn;
    [SerializeField] private Button SubmitBtn;

    [Header("Reference")]
    private ButtomSheetHolder bottomSheet;

    private Animator btmSheetAnim;

    private bool isVisible;


    private void Awake()
    {
        btnSheetBtn.onClick.AddListener(OnPress);
        SubmitBtn.onClick.AddListener(OnSubmit);
    }

    private void Start()
    {
        btmSheetAnim = GetComponent<Animator>();
        bottomSheet = FindAnyObjectByType<ButtomSheetHolder>();

    }

    private void Update()
    {
        if (bottomSheet.canSubmit())
            SubmitBtn.interactable = true;
        else
            SubmitBtn.interactable = false;
    }
    private void OnPress()
    {
        if (isVisible)
        {
            Debug.Log("Show");
            btmSheetAnim.SetBool("isVisible", false);
            isVisible = false;
        }
        else
        {
            Debug.Log("Hide");
            btmSheetAnim.SetBool("isVisible", true);
            isVisible = true ;
        }
    }

    private void OnSubmit()
    {
        Debug.Log("Submit Answer");

        bottomSheet.GetAnswer();
    }
}
