using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField]
    private Button firstSelectedButton;

    SceneLoader loader;
    private void Awake()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }
    protected virtual void OnEnable()
    {
        SetFirstSelected(firstSelectedButton);
    }

    public void SetFirstSelected(Button firsSelectebButton)
    {
        firsSelectebButton.Select();
    }


}
