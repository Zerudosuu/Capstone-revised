using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the Text UI component
    public float timeRemaining = 150f; // Example: 150 seconds (2 minutes 30 seconds)
    private bool timerIsRunning = false;
    private bool isPaused = false;

    public static event Action OnTimerEndAction;

    private void Awake()
    {
        UpdateTimerText(); // Initialize the timer text
    }

    private void OnEnable()
    {
        ExperimentManager.OnGameStart += StartTimer;
        SeeWhatsHappeningPanel.OnPanelOpened += PauseTimer;
        SeeWhatsHappeningPanel.OnPanelClosed += ResumeTimer;
    }

    private void OnDisable()
    {
        ExperimentManager.OnGameStart -= StartTimer;
        SeeWhatsHappeningPanel.OnPanelOpened -= PauseTimer;
        SeeWhatsHappeningPanel.OnPanelClosed -= ResumeTimer;
    }

    public void StartTimer()
    {
        if (!timerIsRunning)
        {
            timerIsRunning = true;
            StartCoroutine(TimerCoroutine());
        }
    }


    public void PauseTimer()
    {
        isPaused = true;
        Debug.Log("Timer paused because panel is open.");
    }

    public void ResumeTimer()
    {
        if (isPaused)
        {
            isPaused = false;
            Debug.Log("Timer resumed because panel is closed.");
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for one second

            if (!isPaused)
            {
                timeRemaining--;
                UpdateTimerText();
            }
        }

        if (!isPaused)
        {
            timerIsRunning = false;
            OnTimerEnd();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:D2}:{seconds:D2}"; // Format: MM:SS
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer has ended!");
        OnTimerEndAction?.Invoke();
    }
}