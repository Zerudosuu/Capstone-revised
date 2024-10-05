using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    //TODO - ADD VARAIBLES THAT ARE NEEEDED TO STORE AND LOAD

    //TODO - NEEDED TO SAVE DATA
    /*      Player Name
            Player Level
            Player Title
            Finished Experiment
            EXP
            Coins
            Item Unlocked

        */
    public string playerTitle;
    public int playerLevel;


    public GameData()
    {
        this.playerTitle = "";
        this.playerLevel = 1;
    }
}
