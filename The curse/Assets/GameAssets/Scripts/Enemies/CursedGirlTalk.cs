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
        director = GetComponentInParent<PlayableDirector>();
        foreach (Dialogue dialogue in dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
        print("Dialogues: " + dialogueQueue.Count);
        foreach (PlayableAsset playable in playables)
        {
            playablesQueue.Enqueue(playable);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!cursedGirl.locked && cursedGirl.activation && cursedGirl.cursedGirlState == CursedGirlStates.TALKING)
        {
            print("Está en el talk ahora");
            if (canTalk)
            {
                cursedGirl.AimPlayer();
                if (cursedGirl.enemyCanvas.activeInHierarchy) cursedGirl.enemyCanvas.SetActive(false);
                /*FindObjectOfType<PlayerController>().SetIsLocked(true);
                FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(false);*/
                canTalk = false;
                SetDialogueMode(true);
                if (dialogueQueue.Count > 0)
                {
                    currentDialogue = dialogueQueue.Dequeue();
                    FindObjectOfType<DialogueManager>().StartDialogue(currentDialogue);
                }
                else { print("No hay más diálogos"); }
                //director.playableAsset = playablesQueue.Dequeue();
                //PlayDirector();
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
        cinematicIsPlaying = true;
        print("Empieza la cinemática");
       /* if (dialogueQueue.Count > 0)
        {
            currentDialogue = dialogueQueue.Dequeue();
            FindObjectOfType<DialogueManager>().StartDialogue(dialogueQueue.Dequeue());
        }*/
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            director.Stop();
            cinematicIsPlaying = false;
            print("Termina la cinemática");
            /* if (dialogueQueue.Count > 0)
             {
                 currentDialogue = dialogueQueue.Dequeue();
                 FindObjectOfType<DialogueManager>().StartDialogue(dialogueQueue.Dequeue());
             }*/
            GetComponent<Health>().SetGodMode(false);
             if (!currentDialogue.CanPlayerChoose(0))
             {
                 cursedGirl.enemyCanvas.SetActive(true);
                 FindObjectOfType<PlayerController>().SetIsLocked(false);
                 FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(true);
             }
            //cursedGirl.enemyCanvas.SetActive(true);
            print("Activa el player");
            FindObjectOfType<PlayerController>().SetIsLocked(false);
            print("Activa el cController del player");
            FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(true);
            cursedGirl.cursedGirlState = CursedGirlStates.ATTACKING;
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

    public bool IsCinematicPlaying() { return cinematicIsPlaying; }
}
