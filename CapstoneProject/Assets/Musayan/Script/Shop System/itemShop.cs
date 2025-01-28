using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class itemShop : MonoBehaviour
{
    [Header("item")]
    [SerializeField]
    private Item itemInContainer;

    private StoreSystem storeSystem;

    private void Awake()
    {
        storeSystem = FindObjectOfType<StoreSystem>(true);

        Button itemButton = GetComponent<Button>();
        itemButton.onClick.AddListener(() => DisplayInShop());
    }

    private void Start()
    {
        Image itemSprite = gameObject.GetComponent<Image>();

        itemSprite.sprite = itemInContainer.itemIcon;
    }

    private void DisplayInShop()
    {
        storeSystem.SelectedItem(itemInContainer, this.gameObject);
    }

    public void SetItem(Item item)
    {
        itemInContainer = item;
    }
}
