using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeterPanelManager : MonoBehaviour
{
    private MixingComponent mixingComponent; // Reference to the MixingComponent
    public TextMeshProUGUI messageText; // Reference to the UI Text to display the result message

    private ItemReaction itemReaction; // Reference to ItemReaction
    private DragableItem draggableItem; // Reference to the dragged item

    private void Start()
    {
        mixingComponent = GetComponentInChildren<MixingComponent>(); // Get the MixingComponent in the children
        messageText.gameObject.SetActive(false); // Hide the message text initially
    }

    public void SetItemReaction(ItemReaction reaction, DragableItem draggable)
    {
        itemReaction = reaction;
        draggableItem = draggable;
    }

    public void ShowMeterResult()
    {
        StartCoroutine(DisplayResultCoroutine()); // Start the coroutine to display the result
    }

    private IEnumerator DisplayResultCoroutine()
    {
        yield return new WaitForSeconds(1.0f); // Simulate an animation length

        string result = mixingComponent.getValue;

        messageText.gameObject.SetActive(true);

        switch (result)
        {
            case "Green":
                messageText.text = "You perfectly poured the water into the beaker!";
                break;
            case "Yellow":
                messageText.text = "You poured too low!";
                break;
            case "Red":
                messageText.text = "You poured too much!";
                break;
            default:
                messageText.text = "Error: Unknown result!";
                break;
        }

        yield return new WaitForSeconds(2.0f);

        messageText.gameObject.SetActive(false);
        gameObject.SetActive(false);

        // // Switch state after coroutine
        // if (itemReaction != null && draggableItem != null)
        // {
        //     itemReaction.?SwitchStateAfterMeasurement(draggableItem);
        // }
    }
}
