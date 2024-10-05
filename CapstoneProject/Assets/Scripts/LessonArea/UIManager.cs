using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("ChemicalContainer")]
    [SerializeField]
    private GameObject chemicalContainer;

    [SerializeField]
    private Button ChemicalContainerMoreButton;

    [SerializeField]
    private GameObject ChemicalAreaItems;

    [Header("EquipmentContainer")]
    [SerializeField]
    private GameObject equipmentContainer;

    [SerializeField]
    private Button EquipmentContainerMoreButton;

    [SerializeField]
    private GameObject EquipmentAreaItems;

    [Header("PlayerUI")]
    [SerializeField]
    private GameObject playerUI;

    [SerializeField]
    private TMP_Text PlayerLevel;

    [SerializeField]
    private TMP_Text PlayerTitle;

    [Header("MainScreenScrollView")]
    [SerializeField]
    private GameObject mainScreenScrollView;

    void Start()
    {
        playerUI.SetActive(true);
        mainScreenScrollView.SetActive(true);
        chemicalContainer.SetActive(true);
        equipmentContainer.SetActive(true);
        ChemicalContainerMoreButton.gameObject.SetActive(true);
        EquipmentContainerMoreButton.gameObject.SetActive(true);
        ChemicalAreaItems.SetActive(false);
        EquipmentAreaItems.SetActive(false);
    }

    void Update() { }

    public void OnChemicalMoreButtonClick()
    {
        mainScreenScrollView.SetActive(false);
        playerUI.SetActive(false);
        chemicalContainer.SetActive(false);
        ChemicalAreaItems.SetActive(true);
    }

    public void OnEquipmentMoreButtonClick()
    {
        mainScreenScrollView.SetActive(false);
        playerUI.SetActive(false);
        equipmentContainer.SetActive(false);
        EquipmentAreaItems.SetActive(true);
    }

    public void OnBackButtonClick()
    {
        mainScreenScrollView.SetActive(true);
        playerUI.SetActive(true);
        equipmentContainer.SetActive(true);
        chemicalContainer.SetActive(true);
        ChemicalAreaItems.SetActive(false);
        EquipmentAreaItems.SetActive(false);
    }

    //Update UI of player
    public void PlayerUpdateUI(string level, string title)
    {
        PlayerLevel.text = level;
        PlayerTitle.text = title;
    }
}
