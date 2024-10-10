using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemContainerManager : MonoBehaviour
{
    // Start is called before the first frame update
    private DragAndDropUI[] dragAndDropItems;

    [SerializeField]
    public GameObject PopUpButtons;

    void Awake()
    {
        PopUpButtons.SetActive(false);
        dragAndDropItems = GetComponentsInChildren<DragAndDropUI>();
    }

    // Update is called once per frame
    public void PopUP(Vector3 SelectedPosition)
    {
        PopUpButtons.SetActive(true);
        PopUpButtons.transform.position = SelectedPosition;
    }
}
