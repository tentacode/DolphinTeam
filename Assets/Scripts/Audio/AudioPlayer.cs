using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void PlayClip()
    {
        audioSource.Play();
    }
}
