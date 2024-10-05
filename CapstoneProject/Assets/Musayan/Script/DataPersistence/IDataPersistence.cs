using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    //This will only read the data
    void LoadData(GameData data);

    //This will allow to modify the script
    void SaveData(ref GameData data);
}
