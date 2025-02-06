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
    [SerializeField] public List<Item> itemList;


    [Header("Debugging")]
    [SerializeField] private bool debugging;
    [SerializeField] private List<string> itemName;

    private UIManager uiManage;
    private SnaptoItem snapItem;
    private ItemManager itemManager;
    private GameObject shineSpawn;
    private LessonManager lessonManage;
    private ObtainableManager obtainableManager;

    private void Awake()
    {
        obtainableManager = FindObjectOfType<ObtainableManager>(true);
        uiManage = FindAnyObjectByType<UIManager>();
        snapItem = FindAnyObjectByType<SnaptoItem>();
        itemManager = FindAnyObjectByType<ItemManager>();
           

    }

    private void Start()
    {

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
        itemList.Clear();

        foreach (var item in newItem)
        {
            itemList.Add(item);
            Debug.Log($"{item.itemName}");
        }

        gameObject.SetActive(true);

        StartCoroutine(RewardSequence());
    }

    public IEnumerator RewardSequence()
    {
        yield return obtainableManager.StartDistributingReward(); 
        
        yield return displayFunction();
        
        yield return placeFunction();

        gameObject.SetActive(false);

    } // this function will make items to show first before placing

    private IEnumerator displayFunction()
    {
        sunRay.SetActive(true);

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
        panel.SetActive(false);

        Animator sunAnim = sunRay.GetComponent<Animator>();
        sunAnim.SetTrigger("Hide");
        yield return new WaitForSeconds(0.35f);
        sunRay.SetActive(false);

        foreach (var item in itemList)
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
        yield return new WaitForSeconds(1f);
    }  // this function will be the animation showing item;

    private IEnumerator PlaceItem(Item newItem)
    {
        
        // Simulate navigation to holder
        uiManage.OnHomeButtonClick();
        yield return new WaitForSeconds(1f);

        // Move item to the correct side based on type
        if (newItem.itemType == Item.ItemType.Equipment)
        {
            snapItem.MoveToRight();
        }
        else
        {
            snapItem.MoveToLeft();
        }

        yield return new WaitForSeconds(.5f);

        // Instantiate the item in the inventory
        itemManager.InstantiateInInventory(newItem);

        Debug.Log($"Placed item: {newItem.itemName}");

        yield return new WaitForSeconds(1f);
    } // this function will be the controller for spawning new items to the place holder;
}
