using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class ButtomSheetHolder : MonoBehaviour
{
    [Header("Placeholder")]
    [SerializeField] private TMP_Text questionHolder;

    [Header("Question")]
    [TextArea(3,5)]
    [SerializeField] private List<string> questions;
    private int _questionIndex;
   
    [Header("Input")]
    [SerializeField] private TMP_InputField answerInput;
    private List<string> _asnwers;


    private bool _canSubmit = false;
    
    public void ShowQuestion(int questionId)
    {
        questionHolder.text = questions[questionId];
    }

    private void SubmitAnswer()
    {
        if (_canSubmit)
        {
            _asnwers[_questionIndex] = answerInput.text;
        }
    }

    private void ShowAnswer()
    {

    }
}
