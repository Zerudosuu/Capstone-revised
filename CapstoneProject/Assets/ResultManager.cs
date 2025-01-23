using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text Score;
    [SerializeField] private TMP_Text lrn;
    [SerializeField] private TMP_Text playerName;

    private Animator _animation;

    private void Start()
    {
        _animation = GetComponent<Animator>();
    }



}
