using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardDistributor : MonoBehaviour
{
    [SerializeField] private GameObject collectRewardsButton;

    [SerializeField] private GameObject rewardItemPrefab;

    [SerializeField] private GameObject rewardItemContainer;

    [SerializeField] private TextMeshProUGUI rewardCoinsText;

    [SerializeField] private TextMeshProUGUI rewardExperienceText;

    [SerializeField] private GameObject itemRewardPanel;

    public void SetRewards(QuestAsLesson questAsLesson)
    {
        // Update coin and experience text
        rewardCoinsText.text = "Coins: " + questAsLesson.RewardCoins.ToString();
        rewardExperienceText.text = "Experience: " + questAsLesson.RewardExperience.ToString();

        // Check if the reward is already collected
        if (questAsLesson.isRewardCollected)
        {
            itemRewardPanel.SetActive(false); // Hide the reward panel
            Debug.Log("Rewards already collected for this lesson.");
        }
        else
        {
            itemRewardPanel.SetActive(true); // Show the reward panel

            // Clear any existing items in the container to avoid duplication
            foreach (Transform child in rewardItemContainer.transform)
            {
                Destroy(child.gameObject);
            }

            // Populate rewards into the reward panel
            foreach (var material in questAsLesson.itemRewards)
            {
                GameObject rewardItem = Instantiate(rewardItemPrefab, rewardItemContainer.transform);
                rewardItem.name = material.materialName;

                // Set item icon and name
                Image itemIcon = rewardItem.GetComponentInChildren<Image>();
                itemIcon.sprite = material.ItemIcon;

                TextMeshProUGUI itemNameText = rewardItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
                itemNameText.text = material.materialName;
                itemNameText.color = Color.white;

                // Set item quantity
                TextMeshProUGUI itemQuantityText =
                    rewardItem.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
                itemQuantityText.text = "";
            }
        }
    }
}