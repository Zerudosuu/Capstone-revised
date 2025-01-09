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

    // [SerializeField]
    // private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemClassification;

    [SerializeField]
    private Button AddToInvetoryButton;

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

            TextMeshProUGUI itemText = itemInfo.GetComponentInChildren<TextMeshProUGUI>(); // Use TextMeshProUGUI for UI elements
            itemText.text = item.itemName;

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
    }

    public void AddToInventory()
    {
        Debug.Log("AddToInventory: " + CurrentSelectedItem.itemName);
    }
}
