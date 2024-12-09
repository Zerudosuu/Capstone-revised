using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Main Menu Navigation")]
    [SerializeField] private MainMenu menu;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMix;

    [Header("Volume Slider")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider SFXVolSlider;

    [Header("Button")]
    [SerializeField] private Button backButton;

    private void Awake()
    {
        if (menu != null)
        {
            backButton.onClick.AddListener(() => DisplaySetting(false));
        }
        else
        {
            backButton.gameObject.SetActive(false);
        }

        Checker();
    }
    private void Start()
    {
        
    }

    public void SetMasterVolume()
    {
        float volume = masterVolSlider.value;
        audioMix.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicVolSlider.value;
        audioMix.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXVolSlider.value;
        audioMix.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        masterVolSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXVolSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void DisplaySetting(bool active)
    {
        Debug.Log("Volume Setting Display: " + active);
        this.gameObject.SetActive(active);

        if (!active)
            menu.ActivateMenu();
    }

    private void Checker()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }
}
