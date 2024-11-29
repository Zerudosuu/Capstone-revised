using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField]
    private SaveSlotMenu saveSlotMenu;

    [SerializeField]
    private VolumeSettings volSetting;

    [Header("Menu Buttons")]
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Button settingButton;

    [SerializeField]
    private Button ExitButton;

    [Header("Animator")]
    [SerializeField] private Animator loadingAnim;

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
    }

    // Method to play animation if the button is active
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

    public void OnSettingClick()
    {
        volSetting.DisplaySetting(true);
        DeactivateMenu();
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


    //FOR DEBUGGING TESTING THE ANIMATION
    public void nextScne()
    {
        StartCoroutine("loadingNextScene");
    }

    private IEnumerator loadingNextScene()
    {
        // Trigger the loading animation
        loadingAnim.SetTrigger("Load");

        // Wait for the duration of the "Load" animation
        yield return new WaitForSeconds(loadingAnim.runtimeAnimatorController.animationClips[1].length);

        // Load the next scene
        SceneManager.LoadSceneAsync("TransitionLoad");
    }
}
