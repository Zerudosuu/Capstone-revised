using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LessonTimer : MonoBehaviour
{
    private Slider slider;
    public float timeRemaining = 10f;
    private bool timerIsRunning = false;

    public static event Action OnTimerEndAction;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        if (slider != null)
        {
            slider.maxValue = timeRemaining;
            slider.value = timeRemaining;
        }
    }

    private void OnEnable()
    {
        ExperimentManager.OnGameStart += StartTimer;
    }

    private void OnDisable()
    {
        ExperimentManager.OnGameStart -= StartTimer;
    }

    public void StartTimer()
    {
        if (!timerIsRunning)
        {
            timerIsRunning = true;
            StartCoroutine(TimerCoroutine());
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f); // Decrease timer every second
            timeRemaining--;
            slider.value = timeRemaining;
        }

        timerIsRunning = false;
        OnTimerEnd();
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer has ended!");
        OnTimerEndAction?.Invoke();
    }
}