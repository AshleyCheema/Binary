using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioSO")]
public class AudioSO : ScriptableObject
{
    public string AudioName = "";
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1f;

    [Range(0, 1)]
    public float pitch = 1f;

    public bool loop = false;

    [Range(0f, 1f)]
    public float spatialBlend = 1f;

    public void SetSourceProperties(AudioSource _audioSource)
    {
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.pitch = pitch;
        _audioSource.loop = loop;
        _audioSource.spatialBlend = spatialBlend;
    }
}
