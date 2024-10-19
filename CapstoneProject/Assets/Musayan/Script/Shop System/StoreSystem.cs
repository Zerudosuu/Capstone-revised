using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;
using System;

public class StoreSystem : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemPrice;

    [SerializeField] private Image itemImage;

    [Header("Button")]
    [SerializeField] private Button purchaseBtn;
    private GameObject selectedBtn;

    [Header("Selected Item")]
    [SerializeField] private Item selectedItem;

    [Header("Prefab")]
    [SerializeField] private GameObject invetoryPrefab;

    private ItemManager itemManage;

    private void Awake()
    {
        purchaseBtn.onClick.AddListener(() => OnPurchaseBtnClicked());
        itemManage = FindAnyObjectByType<ItemManager>();
    }
    private void Start()
    {
        if (selectedItem == null)
            purchaseBtn.interactable = false;   //check if there is seleceted item
    }
    public void SelectedItem(Item item, GameObject btnSelected)
    {
        selectedItem = item;
        selectedBtn = btnSelected;

        purchaseBtn.interactable = true;
        itemName.text = selectedItem.itemName;
        itemDescription.text = selectedItem.itemDescription;
        itemImage.sprite = selectedItem.itemIcon;
        //ItemPrice.text = selectedItem.itemPrice;
    }

    public void OnPurchaseBtnClicked()
    {
        selectedItem.isUnlock = true;

        itemManage.InstantiateInInventory(selectedItem);
        Destroy(selectedBtn);

        clearSelection();
    }

    private void clearSelection()
    {
        purchaseBtn.interactable = false;
        itemName.text = null;
        itemDescription.text = null;
        itemImage.sprite = null;
    }
}
