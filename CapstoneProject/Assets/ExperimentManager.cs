using UnityEngine;
using UnityEngine.UI;

public class ExperimentManager : MonoBehaviour
{
    public Slider currentScore; // The slider displaying the current score
    public float score = 100f; // Initial score

    public float yellowPenalty = 10f; // Score reduction for Yellow
    public float redPenalty = 20f; // Score reduction for Red
    public float greenBonus = 5f; // Score bonus for Green if score is 100

    public GameObject slotPrefab; // Prefab for the item slot

    public GameObject MeterPanel;

    ExperimentObjectManager experimentObjectManager;

    void Start()
    {
        MeterPanel.SetActive(false);
        currentScore.minValue = 0;
        currentScore.maxValue = 100;
        currentScore.value = score; // Initialize slider value

        experimentObjectManager = FindObjectOfType<ExperimentObjectManager>();
    }

    public void UpdateScore(string zone)
    {
        switch (zone)
        {
            case "Yellow":
                score -= yellowPenalty;
                Debug.Log("Yellow Zone! -10 points");
                break;
            case "Red":
                score -= redPenalty;
                Debug.Log("Red Zone! -20 points");
                break;
            case "Green":
                if (score == 100f)
                {
                    score += greenBonus;
                    Debug.Log("Green Zone! Bonus +5 points");
                }
                else
                {
                    Debug.Log("Green Zone! No penalty");
                }
                break;
        }

        // Clamp score to a minimum of 0 and maximum of 100
        score = Mathf.Clamp(score, 0, 100);
        currentScore.value = score; // Update the score slider
    }

    public void UpdateItemPrefab(GameObject currentItem)
    {
        ItemReaction itemReaction = currentItem.GetComponent<ItemReaction>();

        if (itemReaction != null && itemReaction.item.CurrentState != null)
        {
            // Get the new state prefab
            GameObject statePrefab = itemReaction.item.CurrentState.statePrefab;

            if (statePrefab != null)
            {
                // Instantiate a new slot
                GameObject newSlot = Instantiate(
                    slotPrefab,
                    experimentObjectManager.ItemContainer.transform
                );

                // Instantiate the new prefab within the slot
                GameObject newPrefab = Instantiate(statePrefab, newSlot.transform);
                newPrefab.name = statePrefab.name;
                // Align the new prefab within the slot
                newPrefab.transform.localPosition = Vector3.zero;
                newPrefab.transform.localScale = Vector3.one;

                // Destroy the old item
                Destroy(currentItem);

                Debug.Log("Updated item prefab and added to a new slot.");
            }
            else
            {
                Debug.LogError("State prefab is null for the current item.");
            }
        }
        else
        {
            Debug.LogError("ItemReaction or CurrentState is null.");
        }
    }
}
