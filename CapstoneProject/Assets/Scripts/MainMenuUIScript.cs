using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class MainMenuUIScript : MonoBehaviour
{
    VisualElement StartButton_HomeScreen;

    void Awake()
    {
        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;
        StartButton_HomeScreen = root.Q<VisualElement>("StartButton_HS");
        StartButton_HomeScreen.RegisterCallback<ClickEvent>(ev => StartGame());
    }

    private void StartGame()
    {
        print("hotdog");
    }

    void Start() { }

    // Update is called once per frame
    void Update() { }
}
