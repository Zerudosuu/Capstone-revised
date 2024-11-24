using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    public Item item;

    public Button DeleteButton;
    BagManager bagManager;
    public TextMeshProUGUI itemCountText;

    [SerializeField]
    private TextMeshProUGUI itemNameText;

    public int count = 1; // Default count is 1

    void Start()
    {
        bagManager = FindObjectOfType<BagManager>(true);
        if (bagManager != null)
            print("bagManager is found");
    }

    public void SetBagItem(Item item, int quantity)
    {
        this.item = item;

        itemNameText.text = item.itemName;
        count = quantity;
        UpdateItemCountText();
    }

    public void IncrementCount()
    {
        count++;
        UpdateItemCountText();
    }

    private void UpdateItemCountText()
    {
        if (itemCountText != null)
        {
            itemCountText.text = count > 1 ? $"x{count}" : ""; // Show count only if > 1
        }
    }

    public void DeleteItem()
    {
        bagManager.RemoveItem(this);
    }
}
