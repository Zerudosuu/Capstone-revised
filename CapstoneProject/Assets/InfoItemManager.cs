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
    private TextMeshProUGUI itemNameText;

    [SerializeField]
    private TextMeshProUGUI itemDescriptionText;

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI itemClassification;

    [SerializeField]
    private Button AddToInvetoryButton;

    private BagDropArea _bag;

    private void Awake()
    {
        _bag = FindObjectOfType<BagDropArea>(true);
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

            itemInfo.transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;

            Button itemButton = itemInfo.GetComponent<Button>();
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
        itemNameText.text = CurrentSelectedItem.itemName;
        itemDescriptionText.text = CurrentSelectedItem.itemDescription;
        itemClassification.text = CurrentSelectedItem.itemType.ToString();
        itemImage.sprite = CurrentSelectedItem.itemIcon;
    }

    public void AddToInventory()
    {
        _bag.AddedToInventory(CurrentSelectedItem);
    }
}
