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

    public enum ItemType
    {
        Equipment,

        Chemical,
    }
}
