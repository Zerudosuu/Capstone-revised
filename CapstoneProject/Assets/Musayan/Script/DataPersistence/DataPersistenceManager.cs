using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private string dirPath = "Assets/PlayerData";

    public DataPersistenceManager instance { get; private set; }

    private GameData gameData;

    private List<IDataPersistence> dataPersistencesObject;

    private FileDataHandler dataHandler;


    private void Awake()
    {
        //Only one Data Persistence shall be in the game
        if (instance != null)
        {
           Destroy(instance);
        }
        instance = this;    
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(dirPath, fileName);
        this.dataPersistencesObject = FindDataPersistenceObjects();
        LoadData();
    }

    //This will create new data for the game
    private void NewData()
    {
        this.gameData = new GameData();
    }

    //This will Load new data for the game
    private void LoadData()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            NewData();
        }
        foreach (IDataPersistence d in dataPersistencesObject)
        {
            d.LoadData(gameData);   
        }
    }

    //This will Save data of the game
    private void SaveData()
    {
        foreach (IDataPersistence d in dataPersistencesObject)
        {
            d.SaveData( ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    //This will find any type of MonoBehaviour with an IDataPersistence type
    private List <IDataPersistence> FindDataPersistenceObjects()
    {
        IEnumerable <IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence> (dataPersistenceObjects);
    }
}
