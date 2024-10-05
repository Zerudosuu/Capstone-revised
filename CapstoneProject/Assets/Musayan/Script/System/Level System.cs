using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour, IDataPersistence 
{
    private UIManager uiManage;

    private int level;

    private string PlayerTitle;

    private void Awake()
    {
        uiManage = FindAnyObjectByType<UIManager>(); 
    }
    public void LoadData(GameData data)
    {
        this.level = data.playerLevel;
        this.PlayerTitle = data.playerTitle;

        uiManage.PlayerUpdateUI(level.ToString(), PlayerTitle); 
    }

    public void SaveData (ref GameData data)
    {
        data.playerLevel = this.level;
        data.playerTitle = this.PlayerTitle;
    }
}
