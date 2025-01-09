using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioInstance { get; private set; }

    [Header("Audio Background Music")]
    [SerializeField] private List<AudioClip> bgMusics;
    private int currentClipIndex = 0;

    [Header("Audio Sorce")]
    [SerializeField] private AudioSource bgAudioSorce;
    [SerializeField] private AudioSource audioClipSorce;

    [Header("Audio Sorce")]
    [SerializeField] private AudioClip clipBag;
    [SerializeField] private AudioClip clipPickUp;
    [SerializeField] private AudioClip clipMixGlass;
    [SerializeField] private AudioClip clipBubble;

    [HideInInspector] public string audioTitle;

 
    private bool skipRequested = false;
    private bool showTitle = false;

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
        if (bgAudioSorce != null)
            StartCoroutine(PlayAudioLoop());
    }

    //This will loop the audio continousely...
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

                yield return null; // Wait for the next frame
                elapsedTime += Time.deltaTime;
            }

            showTitle = false;
            currentClipIndex = (currentClipIndex + 1) % bgMusics.Count;
        }
    }

    private IEnumerator audioClipPlay(string audClip)
    {
        audioClipSorce.Stop();

        switch (audClip)
        {
            case "Bag":
                audioClipSorce.clip = clipBag;
                break;
            case "PickUp":
                audioClipSorce.clip = clipPickUp;
                break;
            case "MixGlass":
                audioClipSorce.clip = clipMixGlass;
                break;
            case "Bubble":
                audioClipSorce.clip = clipBubble;
                break;
            default:
                Debug.LogError("Invalid clip name: " + audioClipSorce);
                yield break; // Exit the coroutine if clipAud is invalid
        }

        audioClipSorce.Play();
        yield return new WaitForSeconds(audioClipSorce.clip.length);
    }

    public void nextAudio()
    {
        skipRequested = true;
    }

    public IEnumerator ShowMusicTItle(System.Action<string> callback)
    {
        yield return new WaitUntil(() => showTitle);

        callback?.Invoke(audioTitle);
    }
}
