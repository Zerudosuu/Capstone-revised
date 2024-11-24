using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPopUpContainer : MonoBehaviour
{
    Item currentItemSelected;
    InfoItemManager infoItemManager;

    BagDropArea bagDropArea;

    [SerializeField]
    private GameObject PlayerUi;

    void Start()
    {
        bagDropArea = FindObjectOfType<BagDropArea>(true);
        infoItemManager = FindObjectOfType<InfoItemManager>(true);
    }

    public void GetTheCurrentItemToDisaplay(Item item)
    {
        currentItemSelected = item;
    }

    public void SetItemToDisplayInInfo()
    {
        gameObject.SetActive(false);
        PlayerUi.SetActive(false);
        infoItemManager.gameObject.SetActive(true);
        infoItemManager.SelectItem(currentItemSelected);
    }

    public void AddToInventory()
    {
        bagDropArea.AddedToInventory(currentItemSelected);
        gameObject.SetActive(false);
        PlayerUi.SetActive(false);
    }

    public void CloseButtonContainer()
    {
        gameObject.SetActive(false);
    }
}
