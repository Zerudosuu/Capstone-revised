using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField]
    private MainMenu menu;
    private Saveslot[] saveslots;

    [SerializeField]
    private Button backButton;

    private bool isLoadingGame = false;

    private bool hasSavedData = false;

    [Header("Confirmation Popup")]
    [SerializeField]
    private ConfirmationPopUpMenu confirmationPopup;

    void Awake()
    {
        saveslots = GetComponentsInChildren<Saveslot>();
    }

    public void OnSaveClicked(Saveslot saveslot)
    {
        DisbleMenuButtons();

        if (isLoadingGame)
        {
            DataManager.Instance.ChangedSelectedProfileId(saveslot.GetProfileId());
            SaveGameAndLoadGame();
        }
        else if (saveslot.hasData)
        {
            confirmationPopup.ActivateMenu(
                "Starting a new game with this slot will override the currently saved data. and will forever be lost. Are you sure?",
                //function if we select yes
                () =>
                {
                    DataManager.Instance.ChangedSelectedProfileId(saveslot.GetProfileId());
                    DataManager.Instance.NewGame();
                    SaveGameAndLoadGame();
                },
                //function if we select no
                () =>
                {
                    this.ActivateMenu(isLoadingGame);
                }
            );
        }
        else
        {
            DataManager.Instance.ChangedSelectedProfileId(saveslot.GetProfileId());
            DataManager.Instance.NewGame();
            SaveGameAndLoadGame();
        }
    }

    private void SaveGameAndLoadGame()
    {
        // Save the current game data before loading a new scene
        DataManager.Instance.SaveGame();

        // Load the game scene

        print("Loading game scene" + DataManager.Instance.gameData.SceneName);

        SceneManager.LoadSceneAsync(DataManager.Instance.gameData.SceneName);
    }

    public void OnBackClick()
    {
        menu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnClearClick(Saveslot saveslot)
    {
        DisbleMenuButtons();

        confirmationPopup.ActivateMenu(
            " Are you sure you want to delete this saved data?",
            //function if we select yes
            () =>
            {
                DataManager.Instance.DeleteProfile(saveslot.GetProfileId());
                ActivateMenu(isLoadingGame);
            },
            //function if we select no
            () =>
            {
                this.ActivateMenu(isLoadingGame);
            }
        );
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData =
            DataManager.Instance.GetAllProfilesGameData();

        backButton.interactable = true;
        GameObject firstSelected = backButton.gameObject;
        foreach (Saveslot saveslot in saveslots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveslot.GetProfileId(), out profileData);

            saveslot.SetData(profileData);

            if (profileData == null && isLoadingGame)
            {
                saveslot.SetInteractable(false);
            }
            else
            {
                saveslot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveslot.gameObject;
                }
            }
        }

        Button firstSelectedbutton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedbutton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisbleMenuButtons()
    {
        foreach (Saveslot saveslot in saveslots)
        {
            saveslot.SetInteractable(false);
        }

        backButton.interactable = false;
    }
}
