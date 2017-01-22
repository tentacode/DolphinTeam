using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioClip audioClipIdle;
    private AudioClip audioClipTouched;

    [SerializeField]
    private List<AudioClip> clips;

    [SerializeField]
    private AudioSource audioSource;

    public void SetAudioIdle(AudioClip clip ) { this.audioClipIdle = clip; }
    public void SetAudioTouched(AudioClip clip) { this.audioClipTouched = clip; }

    private void SetAudioClip(AudioClip clip, bool loop)
    {
        this.audioSource.Stop();
        this.audioSource.loop = loop;
        this.audioSource.clip = clip;
        this.audioSource.Play();
    }

    public void PlayIdle()
    {
        if ( this.audioClipIdle != null )
        {
            SetAudioClip(this.audioClipIdle, true);
        }
    }

    public void PlayTouched()
    {
        if (this.audioClipTouched != null)
        {
            SetAudioClip(this.audioClipTouched, false);
        }
    }

    public void PlayClip(string clipname)
    {
        if ( this.clips != null )
        {
            for ( int i = 0; i < this.clips.Count; ++i )
            {
                if (this.clips[i].name == clipname)
                {
                    this.audioSource.PlayOneShot(this.clips[i], 1.0f);
                    return;
                }
            }
        }
    }
}
