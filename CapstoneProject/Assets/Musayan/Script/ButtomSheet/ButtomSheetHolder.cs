using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEditor.VersionControl;
using UnityEngine;

public class ButtomSheetHolder : MonoBehaviour
{
    [Header("Q and A Holder")] [SerializeField]
    private GameObject QuestionHolder;

    [Header("Question")] [TextArea(3, 5)] [SerializeField]
    private List<string> questions;


    [Header("reference")] [SerializeField] private Transform contentHolder;
    [HideInInspector] public List<string> answers;
    private List<GameObject> questionInstiate;

    private bool _canSubmit = false;


    [Header("Summary Panel")] public GameObject summaryPanel;

    private void Start()
    {
        questionInstiate = new List<GameObject>();
        answers = new List<string>();
        ShowQeuestion();
        summaryPanel.SetActive(false);
    }

    private void ShowQeuestion()
    {
        foreach (string question in questions)
        {
            GameObject newQuestion = Instantiate(QuestionHolder, contentHolder);
            questionInstiate
                .Add(newQuestion); // this where the instatiate question will be store and where to access the answer per question
            TMP_Text questionTxt = newQuestion.transform.GetChild(0).GetComponent<TMP_Text>();
            questionTxt.text = question;
        }
    }

    public void GetAnswer()
    {
        // this will get all the answer from the input from each question
        foreach (GameObject answerQuestion in questionInstiate)
        {
            // This will get  the Input from the instatiated gameobject for each question
            TMP_InputField answer = answerQuestion.transform.GetChild(1).GetComponent<TMP_InputField>();

            //Store the answer per index question
            answers.Add(answer.text);
            Debug.Log($"{answer.text}");
        }

        summaryPanel.SetActive(true);
    }


    public bool canSubmit()
    {
        foreach (GameObject answerQuestion in questionInstiate)
        {
            // Get the InputField component from the instantiated gameobject for each question
            TMP_InputField answer = answerQuestion.transform.GetChild(1).GetComponent<TMP_InputField>();

            // If any question does not have an answer or is null, return false
            if (answer == null || string.IsNullOrWhiteSpace(answer.text))
            {
                return false;
            }
        }

        // All questions have answers
        return true;
    }
}