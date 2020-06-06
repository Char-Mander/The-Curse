using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    GameObject targetObj;
    [SerializeField]
    Transform targetDestPos;
    [SerializeField]
    Transform targetIniPos;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    private int neededObjects;
    [SerializeField]
    List<PlayableAsset> cinematics = new List<PlayableAsset>();

    private int activeObj = 0;
    [HideInInspector]
    public Vector3 dir;
    private bool activated = false;
    [HideInInspector]
    public float distance;
    //[HideInInspector]
    public bool reached = false;
    //[HideInInspector]
    public bool restarted = false;
    PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }
    
    void Update()
    {
        if (!activated && activeObj == neededObjects)
        {
            activated = true;
            PlayDoorCinematic();
            dir = targetDestPos.position - targetObj.transform.position;
            distance = dir.magnitude;
        }
        else if (activated && !reached)
        {
            Move();
        }
    }

    public void PlayDoorCinematic()
    {
        director.playableAsset = cinematics[!restarted ? 0 : 1];
        PlayDirector();
    }


    void PlayDirector()
    {
        if (FindObjectOfType<ManageCameraCullingMask>().cm == CameraCullingMask.PLAYER) FindObjectOfType<ManageCameraCullingMask>().ChangeCullingMask();
        FindObjectOfType<PlayerController>().soundsManager.StopSound();
        FindObjectOfType<PlayerController>().SetIsLocked(true);
        FindObjectOfType<Mount>().SetIsLocked(true);
        FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(false);
        director.stopped += OnPlayableDirectorStopped;
        director.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            director.Stop();
            if (FindObjectOfType<ManageCameraCullingMask>().cm == CameraCullingMask.EVERYTHING) FindObjectOfType<ManageCameraCullingMask>().ChangeCullingMask();
            director.playableAsset = null;
            FindObjectOfType<PlayerController>().SetIsLocked(false);
            FindObjectOfType<Mount>().SetIsLocked(false);
            FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(true);
        }
    }

    private void Move()
    {
        targetObj.transform.position += dir * moveSpeed * Time.deltaTime;
        distance -= dir.magnitude * moveSpeed * Time.deltaTime;
        if (distance <= 0) reached = true;
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

    public bool HasBeenActivated() { return activated; }

    public void SetActivated(bool value) { activated = value; }

    public int GetActiveObj() { return activeObj; }

    public void SetActiveObj(int num)
    {
        activeObj = num;
    }

    public Transform GetTargetIniPos() { return targetIniPos; }

    public GameObject GetTargetObj() { return targetObj; }

}
