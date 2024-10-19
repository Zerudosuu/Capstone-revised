using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    public Item item;

    public Button DeleteButton;
    BagManager bagManager;

    [SerializeField]
    private TextMeshProUGUI itemNameText;

    void Start()
    {
        bagManager = FindAnyObjectByType<BagManager>();
        if (bagManager != null)
            print("bagManager is found");
    }

    public void SetBagItem(Item item)
    {
        this.item = item;
        // DeleteButton.onClick.AddListener(DeleteItem);

        itemNameText.text = item.itemName;
    }

    // public void DeleteItem()
    // {
    //     bagManager.RemoveItem(this);
    // }
}
