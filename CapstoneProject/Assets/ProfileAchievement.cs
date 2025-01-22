using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Correct UI namespace

public class ProfileAchievement : MonoBehaviour
{
    public Slider sliderProgress;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Target;


    public Achievement Achievement;

    private void Start()
    {
        if (Achievement != null)
        {
            Title.text = Achievement.id;
            Description.text = Achievement.description;
            Target.text = Achievement.progress.ToString() + " / " + Achievement.target.ToString();
            sliderProgress.maxValue = Achievement.target;
            sliderProgress.value = Achievement.progress;
        }
    }

    public void SetAchievement(Achievement achievement)
    {
        Achievement = achievement;
    }
}