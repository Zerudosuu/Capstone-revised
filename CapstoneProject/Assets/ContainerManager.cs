using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerManager : MonoBehaviour
{
    public List<ItemContainerManager> itemContainers;
    public ItemContainerManager currentContainer;

    public Button NextButton;
    public Button PreviousButton;

    private int currentIndex = 0;

    private void Start()
    {
        InitializeItemContainerManager();
        SetupButtons();
        ShowContainer(currentIndex);
    }


    private void InitializeItemContainerManager()
    {
        itemContainers.Clear();

        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                ItemContainerManager itemContainer = child.GetComponent<ItemContainerManager>();
                if (itemContainer != null)
                {
                    itemContainers.Add(itemContainer);
                }
            }
        }

        if (itemContainers.Count > 0)
        {
            currentContainer = itemContainers[0];
        }
    }

    public GameObject SearchForAvailableContainer()
    {
        // Ensure the latest state of containers is considered
        InitializeItemContainerManager();

        foreach (ItemContainerManager itemContainer in itemContainers)
        {
            Debug.Log($"Checking container: {itemContainer.gameObject.name}, isUnlocked: {itemContainer.isUnlocked}");
            if (itemContainer.isUnlocked)
            {
                Debug.Log($"Container {itemContainer.gameObject.name} selected for item placement.");
                return itemContainer.gameObject; // Return the first available container
            }
        }

        Debug.LogWarning("No available unlocked containers found.");
        return null; // Return null if no container is available
    }


    private void SetupButtons()
    {
        if (NextButton != null)
        {
            NextButton.onClick.AddListener(ShowNextContainer);
        }

        if (PreviousButton != null)
        {
            PreviousButton.onClick.AddListener(ShowPreviousContainer);
        }

        UpdateButtonInteractivity();
    }

    private void ShowNextContainer()
    {
        if (currentIndex < itemContainers.Count - 1)
        {
            currentIndex++;
            ShowContainer(currentIndex);
        }
    }

    private void ShowPreviousContainer()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowContainer(currentIndex);
        }
    }

    private void ShowContainer(int index)
    {
        foreach (var container in itemContainers)
        {
            container.gameObject.SetActive(false);
        }

        currentContainer = itemContainers[index];
        currentContainer.gameObject.SetActive(true);

        UpdateButtonInteractivity();
    }

    private void UpdateButtonInteractivity()
    {
        if (NextButton != null)
        {
            NextButton.interactable = currentIndex < itemContainers.Count - 1;
        }

        if (PreviousButton != null)
        {
            PreviousButton.interactable = currentIndex > 0;
        }
    }
}