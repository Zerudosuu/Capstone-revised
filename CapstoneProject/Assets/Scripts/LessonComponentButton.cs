using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LessonComponentButton : MonoBehaviour
{
    public string ButtonID = "";

    // LessonContainer lessonContainer;
    public TextMeshProUGUI ChapterNumber;

    private LessonManager lessonManager; // Reference to the LessonManager

    void Start()
    {
        // lessonContainer = FindObjectOfType<LessonContainer>();
        lessonManager = FindObjectOfType<LessonManager>();
    }

    public string GetButtonID()
    {
        return this.ButtonID;
    }

    public void OnButtonClick()
    {
        if (lessonManager != null)
        {
            // Notify the LessonManager that this button was clicked
            lessonManager.OnButtonClick(this);
        }
    }
}
