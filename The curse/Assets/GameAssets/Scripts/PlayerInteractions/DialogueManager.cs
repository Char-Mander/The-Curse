using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    FixedElementCanvasController canvasC;
    Dialogue dialogue;
    bool isOnADialogue = false;
    int index;

    private Queue<Sentence> sentences = new Queue<Sentence>();
    

    public void StartDialogue(Dialogue dialogue)
    {
        if (!isOnADialogue)
        {
            print("Empieza el diálogo");
            FindObjectOfType<PlayerController>().SetIsLocked(true);
            print("Bloquea al player y la montura");
            FindObjectOfType<Mount>().SetIsLocked(true);
            print("Pone el modo diálogo");
            isOnADialogue = true;
            print("Limpia las frases");
            sentences.Clear();
            this.dialogue = dialogue;
            print("Encola las frases");
            foreach (Sentence sentence in dialogue.GetSentences())
            {
                sentences.Enqueue(sentence);
            }
            print("Antes de entrar al displaynextSentence");
            DisplayNextSentence(0);
        }
    }

    public void DisplayNextSentence(int index)
    {
        if(GetComponent<NPC>() != null) dialogue.GetComponent<NPC>().StartTalking();
        StartCoroutine(WaitForDisplay(index));
    }

    IEnumerator WaitForDisplay(int index)
    {   if (index < dialogue.GetSentences().Count)
        {
            print("Pone el display de la frase " + index);
            this.index = index;
            Sentence s = sentences.Dequeue();
            float textTime = s.sentence.Length > 30 ? (float)s.sentence.Length / 20 : 1f;
            canvasC.UpdateTextPanel(s.sentence, true, dialogue.GetName());
            if (dialogue.CanPlayerChoose(index))
            {
                canvasC.UpdateSentenceOptionsPanel(s, index);
            }
            else {
                yield return new WaitForSeconds(canvasC.GetTextTime());
                DisplayNextSentence(index + 1);
            }
        }
        else
        {
            EndDialogue();
            yield return null;
        }
    }

    private void EndDialogue()
    {
        print("Termina el diálogo");
        isOnADialogue = false;
        FindObjectOfType<PlayerController>().SetIsLocked(false);
        FindObjectOfType<Mount>().SetIsLocked(false);
        if (FindObjectOfType<PlayerController>().IsOnAMount()) FindObjectOfType<Mount>().PlayerClimbsOn();

        if (Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
        
        if (dialogue.gameObject.GetComponent<CursedGirlEnemy>())
        {
            FindObjectOfType<PlayerController>().SetIsLocked(false);
            FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(true);
            FindObjectOfType<CursedGirlEnemy>().cursedGirlState = CursedGirlStates.ATTACKING;
            //dialogue.gameObject.GetComponent<CursedGirlEnemy>().StartAttackingMode();
        }
        else if (dialogue.gameObject.GetComponentInChildren<Quest>() != null && !dialogue.gameObject.GetComponentInChildren<Quest>().HasBeenTriggered())
        {
            dialogue.gameObject.GetComponentInChildren<Quest>().ActivateQuest();
        }
        if (dialogue.gameObject.GetComponent<CursedGirlEnemy>() == null || dialogue.CanPlayerChoose(index)) FindObjectOfType<PlayerController>().EnableOrDisableCharacterController(true);

    }

    public bool IsOnADialogue() { return isOnADialogue; }
}
