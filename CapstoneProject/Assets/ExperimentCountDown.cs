using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentCountDown : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Button SkipButton;

    public TextMeshProUGUI countdownDisplay;
    private Coroutine countdownCoroutine;
    [SerializeField] private StepManager _stepManager;

    private void Start()
    {
        SkipButton.onClick.AddListener(CompleteStep);
    }

    public void SetTime(float time)
    {
        timeRemaining = time;
        StartCountdown();
    }

    private void StartCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        timerIsRunning = true;

        while (timeRemaining > 0)
        {
            countdownDisplay.text = $"{Mathf.FloorToInt(timeRemaining / 60)}:{Mathf.FloorToInt(timeRemaining % 60):00}";
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
        }

        timerIsRunning = false;
        CompleteStep();
    }

    private void CompleteStep()
    {
        countdownDisplay.text = "00:00";
        gameObject.SetActive(false);
        timerIsRunning = false;
        Debug.Log("Completing the wait step...");
        _stepManager.ValidateAndCompleteSubStep("wait");
    }
}