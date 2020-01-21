using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsManager : MonoBehaviour
{
    private AudioSource aSource;
    [SerializeField]
    private List<AudioClip> generalAudioClips = new List<AudioClip>();

    private void Start()
    {
        aSource = GetComponentInParent<AudioSource>();
    }

    public void ManageWalkSound()
    {
        if (aSource.isPlaying && aSource.name != generalAudioClips[0].name) aSource.Stop();

        if (!aSource.isPlaying)
        {
            aSource.clip = generalAudioClips[0];
            aSource.loop = true;
            aSource.Play();
        }
    }

    public void ManageJumpSound()
    {
        aSource.PlayOneShot(generalAudioClips[1]);
    }

    public void ManageRunSound()
    {
        if (aSource.isPlaying && aSource.name != generalAudioClips[2].name)  aSource.Stop();

        if (!aSource.isPlaying)
        {
            aSource.clip = generalAudioClips[2];
            aSource.loop = true;
            aSource.Play();
        }
    }

    public void ManageMountSound()
    {
        if (aSource.isPlaying && aSource.name != generalAudioClips[3].name) aSource.Stop();

        if (!aSource.isPlaying)
        {
            aSource.clip = generalAudioClips[3];
            aSource.loop = true;
            aSource.Play();
        }
    }

    public void ManageWhistleSound()
    {
        aSource.PlayOneShot(generalAudioClips[4]);
    }

    public void ManageMountSnort()
    {
        aSource.PlayOneShot(generalAudioClips[5]);
    }

    public void StopSound()
    {
        aSource.Stop();
    }
}
