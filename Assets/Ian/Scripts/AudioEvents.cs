using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents : MonoBehaviour
{
    //all audio functions to be called

    //Audio
    private AudioSource audioSource;
    public AudioSO walkingSound;
    public AudioSO fireSound;
    public AudioSO burstRunSound;
    public AudioSO /*Thats How You Get*/ tinnitus;
    public AudioSO /*Thats How You Get*/ hacking;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Step()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            walkingSound.SetSourceProperties(audioSource);
            audioSource.Play();

            PlayerStats.Instance.Steps++;
        }
    }

    private void Run()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            burstRunSound.SetSourceProperties(audioSource);
            audioSource.Play();

            PlayerStats.Instance.Steps++;
        }
    }

    public void PlayFireSound()
    {
        fireSound.SetSourceProperties(audioSource);
        audioSource.Play();
    }

    public void PlayHackingSound()
    {
        if (!audioSource.isPlaying)
        {
            hacking.SetSourceProperties(audioSource);
            audioSource.Play();
        }
    }

    public void StopHackingSound()
    {
        audioSource.Stop();
    }
}
