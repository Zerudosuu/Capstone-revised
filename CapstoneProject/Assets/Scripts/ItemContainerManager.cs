using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ItemContainerManager : MonoBehaviour
{
    [SerializeField] private Item CurrentSelectedItem;
    public bool isUnlocked;
    public int priceToUnlock = 300;
    public int maxItem = 16;

    public GameObject NotUnlockPanel;

    public Button purchaseButton;

    [SerializeField] private PlayerStats playerStats;

    public void Start()
    {
        // Initialize the purchase button's click event
        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(Purchase);
        }

        // Check if the container is unlocked
        if (isUnlocked && NotUnlockPanel != null)
        {
            NotUnlockPanel.SetActive(false);
        }
        else if (NotUnlockPanel != null)
        {
            NotUnlockPanel.SetActive(true);
        }
    }

    public void OnItemClick(Item item)
    {
        CurrentSelectedItem = item;
        Debug.Log(item.itemName + " is selected currently.");
    }

    public bool CanAcceptMoreItems()
    {
        return transform.childCount < maxItem;
    }

    // Purchase method to unlock the container
    public void Purchase()
    {
        if (!isUnlocked)
        {
            // Check if the player has enough currency to unlock
            if (HasEnoughCurrency(priceToUnlock))
            {
                // Deduct the currency
                DeductCurrency(priceToUnlock);

                // Unlock the container
                isUnlocked = true;
                NotUnlockPanel.SetActive(false);

                Debug.Log("Container unlocked!");
            }
            else
            {
                Debug.Log("Not enough currency to unlock the container.");
            }
        }
        else
        {
            Debug.Log("Container is already unlocked.");
        }
    }

    // Check if the player has enough currency
    private bool HasEnoughCurrency(int amount)
    {
        if (playerStats != null)
        {
            return playerStats.coins >= amount;
        }
        else
        {
            print("Canot find player stats or buy");
            return false;
        }
    }

    // Deduct currency from the player
    private void DeductCurrency(int amount)
    {
        if (playerStats != null)
        {
            playerStats.AddCoins(-amount);
        }

        Debug.Log($"Deducted {amount} currency. Remaining: {playerStats.coins}");
    }
}