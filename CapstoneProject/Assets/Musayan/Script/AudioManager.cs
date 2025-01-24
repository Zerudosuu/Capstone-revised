using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class SFX
    {
        public string triggerString;
        public AudioClip clip;
    }

    public static AudioManager audioInstance { get; private set; }

    [Header("Audio Background Music")]
    [SerializeField] private List<AudioClip> bgMusics;
    [Header("Audio Source")]
    [SerializeField] private AudioSource bgAudioSorce;
    [SerializeField] private AudioSource sfxAudioSorce;

    [Header("Music")]
    [SerializeField] public string audioTitle;

    private int currentClipIndex = 0;
    private bool skipRequested = false;
    private bool showTitle = false;

    [Header("SFX")]
    [SerializeField] private List<SFX> soundEffects;
    private Dictionary<string, AudioClip> sfxDictionary;


    private void Awake()
    {
        if (audioInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        audioInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(PlayAudioLoop());
    }

    private IEnumerator PlayAudioLoop()
    {
        while (true)
        {
            bgAudioSorce.clip = bgMusics[currentClipIndex];
            bgAudioSorce.Play();

            audioTitle = bgAudioSorce.clip.name;
            showTitle = true;

            float elapsedTime = 0f;
            while (elapsedTime < bgAudioSorce.clip.length)
            {
                if (skipRequested)
                {
                    skipRequested = false;
                    break;
                }

                yield return null;
                elapsedTime += Time.deltaTime;
            }

            showTitle = false;
            currentClipIndex = (currentClipIndex + 1) % bgMusics.Count;
        }
    }

    public void nextAudio()
    {
        skipRequested = true;
    }

    public void PlaceSFX()
    {
        foreach (var sfx in soundEffects)
        {
            if (!sfxDictionary.ContainsKey(sfx.triggerString))
            {
                sfxDictionary.Add(sfx.triggerString, sfx.clip);
            }
        }
    }

    public void PlaySFX(string trigger)
    {
        if (sfxDictionary.TryGetValue(trigger, out AudioClip sfx))
        {
            sfxAudioSorce.PlayOneShot(sfx);
        }
        else
        {
            Debug.Log("No Audio SFX");
        }
    }
}
