using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSystem : MonoBehaviour
{
    [Header("Display")] [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemPrice;
    [SerializeField] private Image itemImage;

    [Header("Windows")] [SerializeField] private GameObject itemPurchaseWindow;
    [SerializeField] private GameObject containers;

    [Header("Button")] [SerializeField] private Button purchaseBtn;
    [SerializeField] private Button cancelBtn;
    private GameObject selectedBtn;


    [Header("Selected Item")] [SerializeField]
    private Item selectedItem;

    [Header("Prefab")] [SerializeField] private GameObject invetoryPrefab;

    private ItemManager itemManage;

    [Header("Coins")] [SerializeField] private int coins;
    [SerializeField] private TMP_Text coinsText;

    [SerializeField] TMP_Text itemPriceText;

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Animator animforBuy;

    private void Awake()
    {
        purchaseBtn.onClick.AddListener(() => OnPurchaseBtnClicked());
        cancelBtn.onClick.AddListener(hideSelected);
        itemManage = FindAnyObjectByType<ItemManager>();
        coinsText.text = playerStats.coins.ToString();
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
        itemPriceText.text = selectedItem.itemPrice.ToString();

        showSelected();
    }

    public void OnPurchaseBtnClicked()
    {
        if (playerStats != null && playerStats.coins >= selectedItem.itemPrice)
        {
            playerStats.coins -= selectedItem.itemPrice;
            coinsText.text = playerStats.coins.ToString();
            selectedItem.isUnlock = true;

            itemManage.InstantiateInInventory(selectedItem);
            Destroy(selectedBtn);

            clearSelection();
            hideSelected();
        }
        else
        {
            if (animforBuy != null)
                animforBuy.Play("boughtSucess");
            Debug.Log("Not enough coins");
            return;
        }
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