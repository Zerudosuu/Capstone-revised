using UnityEngine;

public class BagDropArea : MonoBehaviour
{
    BagManager _bagManager;

    void Start()
    {
        _bagManager = FindObjectOfType<BagManager>(true);

        if (_bagManager == null)
            print("bagManager is null");
    }

    public void AddedToInventory(Item item)
    {
        if (item != null)
        {
            print("Added to inventory: " + item);
            _bagManager.AddItemInBag(item);
            // bagManager.UpdateItemCount();
        }
        else
        {
            print("Item was null!");
        }
    }
}
