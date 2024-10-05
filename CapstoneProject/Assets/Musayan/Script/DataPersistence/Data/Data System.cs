using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class DataSystem : MonoBehaviour
{
    private FileDataHandler dataHandler; // To access the data load

    private List<string> dataString; // This where the Data name will be stored

    private string dirPath; // assign the Directory Path of the Data Files

    [SerializeField] private Dictionary<GameObject, bool> haveData = new Dictionary<GameObject, bool>();    

    private void Awake()
    {
        dirPath = "Assets//PlayerData";
    }

    private void Start()
    {
        //TODO - Get file name in Data folder
        if (Directory.Exists(dirPath))
        {
            string[] fileNames = Directory.GetFiles(dirPath);

            foreach (string filePath in fileNames)
            {
                string fileName = Path.GetFileName(filePath);
                dataString.Add(fileName);
            }
        }     
    }

    private void ShowDatas()
    {

    }


    //TODO - Add event Listner to buttons


}
