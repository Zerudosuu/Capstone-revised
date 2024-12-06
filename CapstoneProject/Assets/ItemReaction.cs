using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReaction : MonoBehaviour, IDropHandler
{
    DragableItem dragableItem;

    public Item item;
    ExperimentManager experimentManager;

    ExperimentObjectManager experimentObjectManagerManager;

    void Start()
    {
        experimentManager = FindObjectOfType<ExperimentManager>();
        dragableItem = GetComponent<DragableItem>();

        experimentObjectManagerManager = FindObjectOfType<ExperimentObjectManager>();
        item = dragableItem.item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropItem = eventData.pointerDrag;
        DragableItem draggable = dropItem.GetComponent<DragableItem>();

        if (
            item.compatibleTags.Contains(draggable.item.tagName)
            && gameObject.GetComponent<DragableItem>().placeInSlot
        )
        {
            if (draggable.name == "Water" && item.CurrentState.stateName == "Empty")
            {
                print("Need to measure");

                experimentManager.MeterPanel.SetActive(true);

                MixingComponent mixingComponent =
                    experimentManager.MeterPanel.GetComponentInChildren<MixingComponent>();
                mixingComponent.SetItem(item);
                mixingComponent.StartMeasurement();

                // Pass this ItemReaction to the MeterPanelManager
                experimentManager
                    .MeterPanel.GetComponent<MeterPanelManager>()
                    .SetItemReaction(this, draggable);
            }
        }

        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
    }

    public void SwitchStateAfterMeasurement(DragableItem draggable)
    {
        item.SwitchToNextState();
        experimentManager.UpdateItemPrefab(this.gameObject);
        Debug.Log("Switched to the next state after measurement.");
        Destroy(draggable.gameObject);
    }
}
