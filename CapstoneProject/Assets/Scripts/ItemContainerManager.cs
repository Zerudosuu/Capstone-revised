using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemContainerManager : MonoBehaviour
{
    [SerializeField] private Item CurrentSelectedItem;

    [SerializeField] public GameObject PopUpButtons;

    void Awake()
    {
        PopUpButtons.SetActive(false);
    }

    // Update is called once per frame
    public void PopUP(Vector3 SelectedPosition)
    {
        PopUpButtons.SetActive(true);
        PopUpButtons.transform.position = SelectedPosition;
    }

    public void OnItemClick(Item item)
    {
        CurrentSelectedItem = item;
        Debug.Log(item.itemName + " is selected");
    }
}