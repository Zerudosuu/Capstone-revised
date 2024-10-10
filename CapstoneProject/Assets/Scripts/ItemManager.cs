using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Container")]
    [SerializeField]
    private GameObject EquipmentArea;

    [SerializeField]
    private GameObject ChemicalArea;

    [Header("Prefab")]
    public GameObject PrefabItem;

    [SerializeField]
    private Items items;

    // Start is called before the first frame update
    void Start()
    {
        // Iterate through the list of items in the Items ScriptableObject
        foreach (Item item in items.items)
        {
            // Check the item type and instantiate the prefab in the appropriate area
            GameObject itemObject = Instantiate(PrefabItem);
            itemObject.name = item.itemName;

            if (item.itemType == Item.ItemType.Equipment)
            {
                // Place the object in the EquipmentArea
                itemObject.transform.SetParent(EquipmentArea.transform, false);
            }
            else if (item.itemType == Item.ItemType.Chemical)
            {
                // Place the object in the ChemicalArea
                itemObject.transform.SetParent(ChemicalArea.transform, false);
            }
        }
    }

    // Update is called once per frame
    void Update() { }
}
