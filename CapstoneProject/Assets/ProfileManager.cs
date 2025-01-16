using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    [Header("Player Profile")]
    [SerializeField] private Image playerBadge;
    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private TMP_Text playerTitle;
    [SerializeField] private TMP_Text EXPgain;

    //private achivement[] achivementObtain;
    //private GameObject achiveDisplay;
    //private Transform achivementHolder


    //To display the achivement of the player
    /*
    private void showAchivement()
    {
        foreach(achivement achive in achivementObtain)
        {
            GameObject achive = Instantiate(achiveDisplay, achivementHolder)
            
            Image achiveImage = achive.transform.getChild(0).GetComponent<Image>();
            TMP_Text AchiveTitle = achive.transform.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text AchiveDescrip = achive.transform.GetChild(2).GetComponent<TMP_Text>();

            achiveImage.Sprite = achive.sprite;
            achiveTitle.text = achive.title;
            achiveDescrip.text = achive.descript;
        }
    }
    */
}
