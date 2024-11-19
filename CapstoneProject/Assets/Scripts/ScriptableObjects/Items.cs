using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items", order = 0)]
public class Items : ScriptableObject
{
    public List<Item> items = new List<Item>();
}

[System.Serializable]
public class Item
{
    public string itemName = "";
    public string itemDescription = "";
    public Sprite itemIcon; // Add a field for the item icon
    public ItemType itemType;
    public bool isUnlock; // checking if the item is unlock

    public enum ItemType
    {
        Equipment,

        Chemical,
    }

    public Item Clone()
    {
        return new Item
        {
            itemName = this.itemName,
            itemDescription = this.itemDescription,
            itemIcon = this.itemIcon,
            itemType = this.itemType,
            isUnlock = this.isUnlock
        };
    }
}