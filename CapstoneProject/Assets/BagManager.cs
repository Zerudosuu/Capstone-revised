using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagManager : MonoBehaviour, IData
{
    [SerializeField] private GameObject BagContainer;

    [SerializeField] private GameObject ItemPrefab;

    [SerializeField] private GameObject ItemCountContainer;

    [SerializeField] private GameObject ItemCountText;

    public List<BagItem> bagItems = new List<BagItem>();
    public CurrentLessonToDisplay currentLessonToDisplay;

    public int itemLimit; // Limit the number of items in the bag
    public TextMeshProUGUI itemLimitText;



    private List<string> sceneList = new List<string>()
    {
        "Chapter1Experiment",
        "Chapter2Experiment",
        "Chapter3Experiment"
    };


    private void Awake()
    {
        
    }
    void Start()
    {
        ItemCountContainer.gameObject.SetActive(false);
        UpdateItemCount();
    }

    public void AddItemInBag(Item item)
    {
        BagItem existingBagItem = bagItems.Find(b => b.item.itemName == item.itemName);

      

        if (existingBagItem != null)
        {
            existingBagItem.IncrementCount();
        }
        else
        {
            // Check if bag limit is enforced
            if (currentLessonToDisplay.CurrentLesson != null)
            {
                // Enforce bag limit
                if (bagItems.Count >= itemLimit)
                {
                    Debug.Log("Bag is full");
                    return;
                }
            }

            GameObject newItem = Instantiate(ItemPrefab, BagContainer.transform);
            BagItem bagItem = newItem.GetComponent<BagItem>();
            bagItem.SetBagItem(item, 1);
            bagItems.Add(bagItem);

            newItem.transform.SetAsFirstSibling();
        }

        UpdateItemCount();
    }

    public void UpdateItemCount()
    {
        ItemCountText.GetComponent<TMPro.TextMeshProUGUI>().text = bagItems.Count.ToString();
        itemLimitText.text = bagItems.Count.ToString() + "/" + itemLimit.ToString();

        if (bagItems.Count > 0)
        {
            ItemCountContainer.gameObject.SetActive(true);
        }
        else
        {
            ItemCountContainer.gameObject.SetActive(false);
        }
    }

    public void RemoveItem(BagItem bagItem)
    {
        bagItems.Remove(bagItem);

        Destroy(bagItem.gameObject);
        UpdateItemCount();

        // Update lesson display when an item is removed
        if (currentLessonToDisplay != null)
        {
            currentLessonToDisplay.UpdateMaterialStates();
        }
    }

    public void ClearBag()
    {
        bagItems.Clear();

        foreach (Transform child in BagContainer.transform)
        {
            Destroy(child.gameObject);
        }

        itemLimit = 0;
        UpdateItemCount();
    }

    public void ProceedToExperiment()
    {
        DataManager.Instance.SaveGame(); // Save bag contents before moving to the experiment
        // Load the next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            sceneList[DataManager.Instance.gameData.currentQuestIndex - 1]);
        ;
    }

    // IData Implementation
    public void LoadData(GameData data)
    {
        if (data.BagItems == null)
            return;

        // Clear current items
        bagItems.Clear();

        foreach (var savedItem in data.BagItems)
        {
            Item item = new Item { itemName = savedItem.itemName }; // Replace with your item initialization logic
            GameObject newItem = Instantiate(ItemPrefab, BagContainer.transform);
            BagItem bagItem = newItem.GetComponent<BagItem>();
            bagItem.SetBagItem(item, savedItem.count); // Assuming `SetBagItem` can handle setting count

            bagItems.Add(bagItem);
        }

        UpdateItemCount();
    }

    public void SavedData(GameData data)
    {
        data.BagItems = new List<SerializableBagItem>();
        foreach (BagItem bagItem in bagItems)
        {
            SerializableBagItem savedItem = new SerializableBagItem(
                bagItem.item.itemName,
                bagItem.count
            );

            data.BagItems.Add(savedItem);
        }
    }
}