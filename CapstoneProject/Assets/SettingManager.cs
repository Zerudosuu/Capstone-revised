using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    SceneLoader _loader;

    private void Start()
    {
        _loader = FindObjectOfType<SceneLoader>(true);
    }
    public void OnBackMainMenu()
    {
        StartCoroutine(_loader.loadingNextScene("MainMenu"));
    }
}
