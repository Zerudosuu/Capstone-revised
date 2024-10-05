using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Unity.VisualScripting;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFilename = "";

    public FileDataHandler (string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFilename = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFilename);
        GameData loadedData = null;
        
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream strem = new FileStream(fullPath, FileMode.Open) )
                {
                    using(StreamReader sr = new StreamReader(strem))
                    {
                        dataToLoad = sr.ReadToEnd();    
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);    
            }
            catch (Exception e)
            {
                Debug.Log("Error in loading data: " + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFilename);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream (fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error in saving data to file: " + "\n" + e);
        }
    }
}
