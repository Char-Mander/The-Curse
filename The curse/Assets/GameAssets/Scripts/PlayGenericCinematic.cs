using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayGenericCinematic : MonoBehaviour
{
    public bool activate = false;
    bool played = false;
    bool started = false;
    PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        if (this.gameObject.CompareTag("InitialCinematic"))
        {
            played = GameManager.instance.data.LoadInitialCinematic();
            started = played;
        }
    }


    void Update()
    {
        if(activate && !started)
        {
            started = true;
            if (this.gameObject.CompareTag("InitialCinematic")) GameManager.instance.data.SaveInitialCinematic();
            PlayDirector();
        }
    }

    void PlayDirector()
    {
        //Configura la culling Mask y desactiva el player
        if (FindObjectOfType<ManageCameraCullingMask>().cm == CameraCullingMask.PLAYER) FindObjectOfType<ManageCameraCullingMask>().ChangeCullingMask();
        FindObjectOfType<PlayerController>().soundsManager.StopSound();
        FindObjectOfType<GeneralSoundManager>().StopSound();
        FindObjectOfType<PlayerController>().SetIsLocked(true);
        FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(false);
        director.stopped += OnPlayableDirectorStopped;
        director.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            director.Stop();
            played = true;
            //Configura la culling Mask y activa al player
            if (FindObjectOfType<ManageCameraCullingMask>().cm == CameraCullingMask.EVERYTHING) FindObjectOfType<ManageCameraCullingMask>().ChangeCullingMask();
            FindObjectOfType<GeneralSoundManager>().ManageGeneralSound();
            FindObjectOfType<PlayerController>().SetIsLocked(false);
            FindObjectOfType<Mount>().SetIsLocked(false);
            FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(true);
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

    public bool HasBeenPlayed() { return played; }

    public bool HasStarted() { return started; }

    public bool IsNotPlaying() { return (played && started) || (!played&&!started); }
}
