using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    [Header("Gameobject")]
    [SerializeField] private GameObject sunRay;
    [SerializeField] private Image imagePlaceHolder;
    [SerializeField] private GameObject panel;

    [Header("Item")]
    [SerializeField] private bool show = true;
    [SerializeField] private Items items;
    [SerializeField] private List<Item> itemList;


    [Header("Debugging")]
    [SerializeField] private bool debugging;
    [SerializeField] private List<string> itemName;

    private UIManager uiManage;
    private SnaptoItem snapItem;
    private ItemManager itemManager;

    private void Awake()
    {
        uiManage = FindAnyObjectByType<UIManager>();
        snapItem = FindAnyObjectByType<SnaptoItem>();
        itemManager = FindAnyObjectByType<ItemManager>();
    }

    private void Start()
    {
        itemList.Clear();

        if (debugging)
            DebuggingSpawn();
    }

    private void DebuggingSpawn()
    {
        foreach (var item in items.items)
        {
            foreach (string name in itemName)
            {
                if (item.itemName == name)
                {
                    itemList.Add(item);
                }
            }
        }

        StartCoroutine(RewardSequence());
    }

    public void GiveRewardItem(List<Item> newItem)
    {
        foreach (var item in newItem)
        {
            var _itemReward = item.Clone();
            _itemReward.isUnlock = true;
            itemManager.clonedItems.Add(_itemReward);

            itemList.Add(_itemReward);
        }

        StartCoroutine(RewardSequence());
    }

    private IEnumerator RewardSequence()
    {
        yield return displayFunction();

        yield return placeFunction();
    } // this function will make items to show first before placing

    private IEnumerator displayFunction()
    {
        foreach (var item in itemList)
        {
            imagePlaceHolder.gameObject.SetActive(true);
            yield return StartCoroutine(ShowItem(item));
            
        }

        imagePlaceHolder.gameObject.SetActive(false);
        yield return null;
    } // this function will work to show item rewards 1 by 1

    private IEnumerator placeFunction()
    {
        foreach (var item in itemList   )
        {
           yield return StartCoroutine(PlaceItem(item));
        }

        yield return null;
    } // this function will work to place item rewards 1 by 1

    private IEnumerator ShowItem(Item newItem)
    {
        // Set the item's icon and activate panel
        panel.SetActive(true);
        imagePlaceHolder.sprite = newItem.itemIcon;
        imagePlaceHolder.transform.localScale = Vector3.zero;

        // Instantiate and destroy the sunray effect
        GameObject sunShine = Instantiate(sunRay, gameObject.transform);
        Destroy(sunShine, 5f);

        // Animate the scale of the placeholder
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(0f, 2f, elapsed / duration);
            imagePlaceHolder.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        Debug.Log($"Showing item: {newItem.itemName}");

        // Wait for the user to observe the item
        yield return new WaitForSeconds(2f);
    }  // this function will be the animation showing item;

    private IEnumerator PlaceItem(Item newItem)
    {
        // Deactivate panel after showing the item
        panel.SetActive(false);

        // Simulate navigation to holder
        uiManage.OnHomeButtonClick();
        yield return new WaitForSeconds(2f);

        // Move item to the correct side based on type
        if (newItem.itemType == Item.ItemType.Equipment)
        {
            snapItem.MoveToRight();
        }
        else
        {
            snapItem.MoveToLeft();
        }

        yield return new WaitForSeconds(1f);

        // Instantiate the item in the inventory
        itemManager.InstantiateInInventory(newItem);

        Debug.Log($"Placed item: {newItem.itemName}");

        yield return new WaitForSeconds(1f);
    } // this function will be the controller for spawning new items to the place holder;
}
