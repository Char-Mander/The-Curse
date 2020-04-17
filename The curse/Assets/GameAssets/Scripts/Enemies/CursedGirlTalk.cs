using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CursedGirlTalk : MonoBehaviour
{
    [SerializeField]
    List<Dialogue> dialogues = new List<Dialogue>();
    [SerializeField]
    List<PlayableAsset> playables = new List<PlayableAsset>();
    Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();
    Queue<PlayableAsset> playablesQueue = new Queue<PlayableAsset>();
    [HideInInspector]
    public PlayableDirector director;
    Dialogue currentDialogue;
    CursedGirlEnemy cursedGirl;
    bool cinematicIsPlaying = false;
    bool canTalk = true;
    // Start is called before the first frame update
    void Start()
    {
        cursedGirl = GetComponent<CursedGirlEnemy>();
        foreach (Dialogue dialogue in dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
        foreach (PlayableAsset playable in playables)
        {
            playablesQueue.Enqueue(playable);
        }
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!cursedGirl.locked && cursedGirl.activation && cursedGirl.cursedGirlState == CursedGirlStates.TALKING)
        {
            if(cursedGirl.enemyCanvas.activeInHierarchy) cursedGirl.enemyCanvas.SetActive(false);
            FindObjectOfType<PlayerController>().SetIsLocked(true);
            FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(false);
            if (canTalk)
            {
                canTalk = false;
                SetDialogueMode(true);
                director.playableAsset = playablesQueue.Dequeue();
                PlayDirector();
            }
            cursedGirl.anim.SetFloat("Speed", 0);
        }
    }

    public void SetDialogueMode(bool value)
    {
        GetComponent<Health>().SetGodMode(value);
        cursedGirl.enemyCanvas.SetActive(!value);
        if(value) GetComponent<Health>().StopReceivingConstantDamage();
    }

    void PlayDirector()
    {
        director.stopped += OnPlayableDirectorStopped;
        director.Play();
        if (dialogueQueue.Count > 0)
        {
            currentDialogue = dialogueQueue.Dequeue();
            FindObjectOfType<DialogueManager>().StartDialogue(dialogueQueue.Dequeue());
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            director.Stop();
            GetComponent<Health>().SetGodMode(false);
            if (!currentDialogue.CanPlayerChoose(0))
            {
                cursedGirl.enemyCanvas.SetActive(true);
                FindObjectOfType<PlayerController>().SetIsLocked(false);
                FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(true);
            }
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
