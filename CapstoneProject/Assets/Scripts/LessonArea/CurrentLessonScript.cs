using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLessonScript : MonoBehaviour
{
    // Current Lesson
    [Header("Lesson Window")]
    public GameObject LessonWindow;
    public bool isLessonCurrentWindowActive = false;

    [SerializeField]
    private Animator animator;

    void Start()
    {
        animator = LessonWindow.GetComponent<Animator>();
    }

    public void OnclickOpenClose()
    {
        isLessonCurrentWindowActive = !isLessonCurrentWindowActive;
        animator.SetBool("isCurrentLessonWindowIsOpen?", isLessonCurrentWindowActive);
        Debug.Log("Lesson Window Open/Close: " + isLessonCurrentWindowActive);
    }
}
