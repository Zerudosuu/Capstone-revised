using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioInstance { get; private set; }

    [Header("Audio Background Music")]
    [SerializeField]
    private List<AudioClip> bgMusics;
    private int currentClipIndex = 0;

    [Header("Audio Sorce")]
    [SerializeField]
    private AudioSource bgAudioSorce;

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

            yield return new WaitForSeconds(bgAudioSorce.clip.length);

            currentClipIndex = (currentClipIndex + 1) % bgMusics.Count;
        }
    }
}
