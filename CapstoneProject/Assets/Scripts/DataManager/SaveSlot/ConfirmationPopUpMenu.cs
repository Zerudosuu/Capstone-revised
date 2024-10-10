using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationPopUpMenu : Menu
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI displayText;

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;

    public void ActivateMenu(
        string displayText,
        UnityAction confirmAction,
        UnityAction cancelAction
    )
    {
        this.gameObject.SetActive(true);

        this.displayText.text = displayText;
        this.yesButton.onClick.RemoveAllListeners();
        this.noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            confirmAction();
        });

        noButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cancelAction();
        });
    }

    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
