using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemContainerManager : MonoBehaviour
{
    [SerializeField]
    private Item CurrentSelectedItem;

    [SerializeField]
    public GameObject PopUpButtons;

    InfoItemManager infoItemManager;

    void Awake()
    {
        PopUpButtons.SetActive(false);
    }

    void Start()
    {
        infoItemManager = FindObjectOfType<InfoItemManager>(true);

        if (infoItemManager == null)
        {
            Debug.LogError("InfoItemManager is not found in the scene.");
        }
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
        Debug.Log(item.itemName + " is selected currently.");
    }

    public void OpenInfoContainer()
    {
        infoItemManager.gameObject.SetActive(true);
        infoItemManager.SetDisplayedItem(CurrentSelectedItem);
    }

    public void CloseInfoContainer()
    {
        infoItemManager.gameObject.SetActive(false);
    }
}
