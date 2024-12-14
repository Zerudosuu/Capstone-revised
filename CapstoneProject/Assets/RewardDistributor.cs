using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardDistributor : MonoBehaviour
{
    [SerializeField]
    private GameObject collectRewardsButton;

    [SerializeField]
    private GameObject rewardItemPrefab;

    [SerializeField]
    private GameObject rewardItemContainer;

    [SerializeField]
    private TextMeshProUGUI rewardCoinsText;

    [SerializeField]
    private TextMeshProUGUI rewardExperienceText;

    public void SetRewards(QuestAsLesson questAsLesson)
    {
        rewardCoinsText.text = "Coins: " + questAsLesson.RewardCoins.ToString();
        rewardExperienceText.text = "Experience: " + questAsLesson.RewardExperience.ToString();

        foreach (var material in questAsLesson.itemRewards)
        {
            GameObject rewardItem = Instantiate(rewardItemPrefab, rewardItemContainer.transform);
            rewardItem.name = material.materialName;

            rewardItem.GetComponentInChildren<TextMeshProUGUI>().text = material.materialName;
        }
    }
}
