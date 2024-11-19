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
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = itemInContainer.itemName;
    }

    private void DisplayInShop()
    {
        if (storeSystem != null)
        {
            storeSystem.SelectedItem(itemInContainer, this.gameObject);
        }
        else
        {
            Debug.Log("StoreSystem is not found");
        }
    }

    public void SetItem(Item item)
    {
        itemInContainer = item;
    }
}
