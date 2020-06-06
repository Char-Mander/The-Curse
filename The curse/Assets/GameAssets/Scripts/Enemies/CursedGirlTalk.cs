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
    }

    // Update is called once per frame
    void Update()
    {
        if(!cursedGirl.locked && cursedGirl.activation && cursedGirl.cursedGirlState == CursedGirlStates.TALKING)
        {
            if (canTalk)
            {
                cursedGirl.AimPlayer();
                cursedGirl.anim.SetLayerWeight(0, 0);
                cursedGirl.anim.SetLayerWeight(1, 1);
                cursedGirl.anim.SetLayerWeight(2, 0);
                if (cursedGirl.enemyCanvas.activeInHierarchy) cursedGirl.enemyCanvas.SetActive(false);
                canTalk = false;
                SetDialogueMode(true);
                if (dialogueQueue.Count > 0)
                {
                    currentDialogue = dialogueQueue.Dequeue();
                    FindObjectOfType<DialogueManager>().StartDialogue(currentDialogue);
                }
                else { print("No hay más diálogos"); }
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

    void ChangeTalkAnim(bool value)
    {
        if (value)
        {
            cursedGirl.anim.SetLayerWeight(0, 0);
            cursedGirl.anim.SetLayerWeight(1, 1);
            cursedGirl.anim.SetLayerWeight(2, 0);
        }
        else
        {
            cursedGirl.anim.SetLayerWeight(0, 1);
            cursedGirl.anim.SetLayerWeight(1, 0);
            cursedGirl.anim.SetLayerWeight(2, 0);
        }
    }

    public bool IsCinematicPlaying() { return cinematicIsPlaying; }

    public int DialogueCount() { return dialogueQueue.Count; }

    public void SetCanTalk(bool value) { canTalk = value; }
}
