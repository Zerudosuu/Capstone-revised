using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSystem : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemPrice;
    [SerializeField] private Image itemImage;

    [Header("Windows")]
    [SerializeField] private GameObject itemPurchaseWindow;
    [SerializeField] private GameObject containers;

    [Header("Button")]
    [SerializeField] private Button purchaseBtn;
    [SerializeField] private Button cancelBtn;
    private GameObject selectedBtn;
    

    [Header("Selected Item")]
    [SerializeField]private Item selectedItem;

    [Header("Prefab")]
    [SerializeField]private GameObject invetoryPrefab;

    private ItemManager itemManage;

    private void Awake()
    {
        purchaseBtn.onClick.AddListener(() => OnPurchaseBtnClicked());
        cancelBtn.onClick.AddListener(hideSelected);
        itemManage = FindAnyObjectByType<ItemManager>();
    }

    private void Start()
    {
        if (selectedItem == null)
            purchaseBtn.interactable = false; //check if there is seleceted item
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

        showSelected();
    }

    public void OnPurchaseBtnClicked()
    {
        selectedItem.isUnlock = true;

        itemManage.InstantiateInInventory(selectedItem);
        Destroy(selectedBtn);

        clearSelection();
        hideSelected();
    }

    private void clearSelection()
    {
        purchaseBtn.interactable = false;
        itemName.text = null;
        itemDescription.text = null;
        itemImage.sprite = null;
    }

    public void showSelected()
    {
        itemPurchaseWindow.SetActive(true);
        containers.SetActive(false);
    }

    public void hideSelected()
    {
        itemPurchaseWindow.SetActive(false);
        containers.SetActive(true);

    }
}
