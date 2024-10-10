using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [Header("Debugging")]
    // [SerializeField]
    // private bool disableDataPersistence = false;

    [SerializeField]
    private bool initializeDataIfNull = false;

    [SerializeField]
    private bool diableDataPersistence = false;

    [SerializeField]
    private bool overrideSelectedProfileId = false;

    [SerializeField]
    private string testSelectedProfileId = "test";

    [Header("File Storage")]
    [SerializeField]
    private string fileName;

    [SerializeField]
    private bool useEncryption;
    public GameData gameData { get; private set; } // Make sure this is publicly accessible

    private List<IData> DataObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";

    public static DataManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            print("There is more than one dataManager existing");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(
            Application.persistentDataPath,
            fileName,
            useEncryption
        );

        if (diableDataPersistence)
        {
            Debug.Log("Data persistence disabled");
            return;
        }

        InitializedSelectedProfile();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded");
        this.DataObjects = FindAllDataObject();
        LoadGame();
    }

    public void ChangedSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void DeleteProfile(string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        dataHandler.Delete(profileId);
        InitializedSelectedProfile();
        LoadGame();
    }

    private void InitializedSelectedProfile()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.Log($"Overriding selected profile id to {testSelectedProfileId}");
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (diableDataPersistence)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if (this.gameData == null)
        {
            print("No game Data was found");
            return;
        }

        foreach (IData dataObj in DataObjects)
        {
            dataObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (diableDataPersistence)
        {
            return;
        }
        if (this.gameData == null)
        {
            Debug.LogError("No game data found to save.");
            return;
        }

        foreach (IData dataObj in DataObjects)
        {
            dataObj.SavedData(gameData);
        }

        //timestamp
        gameData.lastUpdate = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
    }

    // Function to ensure saving on application quit
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IData> FindAllDataObject()
    {
        // by adding true to this code, the data manager will find all the objects including the one that is not active.
        IEnumerable<IData> dataObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IData>();
        return new List<IData>(dataObjects);
    }

    public bool HasData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public void Register() { }
}
