using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoItemManager : MonoBehaviour
{
    public Button[] itemButtons; // Renamed to avoid conflict

    [SerializeField]
    private Items itemList; // Renamed to avoid conflict with itemButtons

    [SerializeField]
    private GameObject InfoItem;

    [SerializeField]
    private GameObject InfoItemContainer;

    public Item CurrentSelectedItem;

    [Header("Displaying Information")]
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI itemNameText;

    [SerializeField]
    private TextMeshProUGUI itemDescriptionText;

    // [SerializeField]
    // private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemClassification;

    [SerializeField]
    private Button AddToInvetoryButton;

    private BagManager bag;
    private void Awake()
    {
        bag = FindAnyObjectByType<BagManager>();    
    }

    void Start()
    {
        itemButtons = GetComponentsInChildren<Button>();
        PopulateItems();
    }

    void PopulateItems()
    {
        foreach (Item item in itemList.items) // Assuming itemList contains a list of Item
        {
            GameObject itemInfo = Instantiate(InfoItem, InfoItemContainer.gameObject.transform);

            Button itemButton = itemInfo.GetComponent<Button>();
            itemButton.image.sprite = item.itemIcon;
            itemButton.onClick.AddListener(() => SelectItem(item));
        }
    }

    public void SelectItem(Item selectedItem)
    {
        CurrentSelectedItem = selectedItem;
        Debug.Log("Selected Item: " + CurrentSelectedItem.itemName);
        SetDisplayedItem(CurrentSelectedItem);
    }

    public void SetDisplayedItem(Item CurrentSelectedItem)
    {
        itemImage.sprite = CurrentSelectedItem.itemIcon;
        itemNameText.text = CurrentSelectedItem.itemName;
        itemDescriptionText.text = CurrentSelectedItem.itemDescription;
        itemClassification.text = CurrentSelectedItem.itemType.ToString();
    }

    public void AddToInventory()
    {
        //TO DO - Add item to inventory
        //bag.AddItemInBag(CurrentSelectedItem);
    }
}
