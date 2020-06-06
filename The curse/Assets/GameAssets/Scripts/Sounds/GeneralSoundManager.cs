using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSoundManager : MonoBehaviour
{
    private AudioSource aSource;
    [SerializeField]
    private List<AudioClip> generalAudioClips = new List<AudioClip>();

    private void Start()
    {
        aSource = GetComponentInParent<AudioSource>();
        ManageGeneralSound();
    }

    public void ManageGeneralSound()
    {
        if (aSource.isPlaying && aSource.name != generalAudioClips[0].name) aSource.Stop();

        if (!aSource.isPlaying)
        {
            aSource.clip = generalAudioClips[0];
            aSource.loop = true;
            aSource.Play();
        }
    }

    public void ManageSadSound()
    {
        if (aSource.isPlaying && aSource.name != generalAudioClips[0].name) aSource.Stop();

        if (!aSource.isPlaying)
        {
            aSource.clip = generalAudioClips[1];
            aSource.loop = true;
            aSource.Play();
        }
    }

    public void ManageActionSound()
    {
        if (aSource.isPlaying && aSource.name != generalAudioClips[0].name) aSource.Stop();

        if (!aSource.isPlaying)
        {
            aSource.clip = generalAudioClips[2];
            aSource.loop = true;
            aSource.Play();
        }
    }

    public void ManageWinSound()
    {
        aSource.clip = null;
        aSource.PlayOneShot(generalAudioClips[3]);
    }

    public void ManageLoseSound()
    {
        aSource.clip = null;
        aSource.PlayOneShot(generalAudioClips[4]);
    }

    public void StopSound()
    {
        if (aSource.isPlaying) aSource.Stop();
    }
}
