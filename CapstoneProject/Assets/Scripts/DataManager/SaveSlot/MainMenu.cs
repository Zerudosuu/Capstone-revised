using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField]
    private SaveSlotMenu saveSlotMenu;

    [Header("Menu Buttons")]
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Button ExitButton;

    public Animator animator;

    void Start()
    {
        DisableButtonDependingOnTheData();
    }

    public void DisableButtonDependingOnTheData()
    {
        if (!DataManager.Instance.HasData())
        {
            continueButton.interactable = false;
            loadGameButton.interactable = false;
        }
        else
        {
            // Check if the button is active and play the animation
            PlayButtonAnimationIfActive(continueButton);
            PlayButtonAnimationIfActive(loadGameButton);
        }
    }

    // Method to play animation if the button is active
    private void PlayButtonAnimationIfActive(Button button)
    {
        animator = button.GetComponent<Animator>();

        if (button.gameObject.activeSelf && button.interactable)
        {
            // Trigger an animation by setting a parameter or using a trigger
            animator.SetTrigger("Hover");
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonDependingOnTheData();
    }

    public void OnContinueGameClicked()
    {
        DisbaleMenuButton();
        DataManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(DataManager.Instance.gameData.SceneName);
    }

    public void OnLoadClick()
    {
        saveSlotMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    private void DisbaleMenuButton()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void ApplicationExit()
    {
        // quit application
        Application.Quit();
    }
}
